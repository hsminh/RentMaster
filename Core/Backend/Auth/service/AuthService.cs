using System.Text;
using Microsoft.EntityFrameworkCore;
using RentMaster.Core.Auth.Interface;
using RentMaster.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using RentMaster.Core.Auth.Types;

namespace RentMaster.Core.Auth.service;

public class AuthService : IAuthService
{
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(string gmail, string password, UserTypes type)
        {
            if (string.IsNullOrEmpty(gmail) || string.IsNullOrEmpty(password))
                return null;

            var user = await GetUserByTypeAsync(gmail, type);
            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return GenerateJwtToken(user, type);
        }

        private string GenerateJwtToken(BaseAuth user, UserTypes type)
        {
            var keyString = Environment.GetEnvironmentVariable("JWT_KEY") 
                            ?? _configuration["Jwt:Key"];

            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                         ?? _configuration["Jwt:Issuer"];

            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                           ?? _configuration["Jwt:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Gmail),
                new Claim("uid", user.Uid.ToString()),
                new Claim("name", $"{user.FirstName} {user.LastName}"),
                new Claim("role", type.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private async Task<BaseAuth?> GetUserByTypeAsync(string gmail, UserTypes type)
        {
            return type switch
            {
                UserTypes.Consumer => await _context.Consumers.FirstOrDefaultAsync(u => u.Gmail == gmail),
                UserTypes.Admin => await _context.Admins.FirstOrDefaultAsync(u => u.Gmail == gmail),
                UserTypes.LandLord => await _context.LandLords.FirstOrDefaultAsync(u => u.Gmail == gmail),
                _ => null
            };
        }
}
