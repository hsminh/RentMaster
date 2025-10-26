using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RentMaster.Core.Auth.Types;
using RentMaster.Data;

namespace RentMaster.Core.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        // ✅ 1. Lấy endpoint đang được gọi
        var endpoint = context.GetEndpoint();
        var hasAdminScope = endpoint?.Metadata.GetMetadata<Attributes.AdminScopeAttribute>() != null;
        var hasUserScope = endpoint?.Metadata.GetMetadata<Attributes.UserScopeAttribute>() != null;

        // ✅ 2. Lấy token nếu có
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        // Nếu không yêu cầu xác thực (public API)
        if (!hasAdminScope && !hasUserScope)
        {
            await _next(context);
            return;
        }

        // Nếu API yêu cầu xác thực nhưng không có token → 401
        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing or invalid token");
            return;
        }

        try
        {
            // ✅ 3. Validate token
            var jwt = ValidateJwtToken(token);
            if (jwt == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token");
                return;
            }

            // ✅ 4. Đọc role
            var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            if (!Guid.TryParse(userId, out var uid))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid user id");
                return;
            }

            // ✅ 5. Check scope
            if (hasAdminScope && role != nameof(UserTypes.Admin) ||
                hasUserScope && role != nameof(UserTypes.Consumer))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden: role not allowed");
                return;
            }

            // ✅ 6. Attach current user
            var user = await GetUserByRoleAsync(db, uid, role);
            if (user == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("User not found");
                return;
            }

            context.Items["CurrentUser"] = user;
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JWT Error: {ex.Message}");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Token validation failed");
        }
    }

    private JwtSecurityToken? ValidateJwtToken(string token)
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.UTF8.GetBytes(key);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        }, out SecurityToken validatedToken);

        return (JwtSecurityToken)validatedToken;
    }

    private async Task<object?> GetUserByRoleAsync(AppDbContext db, Guid uid, string? role)
    {
        return role switch
        {
            nameof(UserTypes.Admin) => await db.Admins.FirstOrDefaultAsync(u => u.Uid == uid),
            nameof(UserTypes.LandLord) => await db.LandLords.FirstOrDefaultAsync(u => u.Uid == uid),
            nameof(UserTypes.Consumer) => await db.Consumers.FirstOrDefaultAsync(u => u.Uid == uid),
            _ => null
        };
    }
}
