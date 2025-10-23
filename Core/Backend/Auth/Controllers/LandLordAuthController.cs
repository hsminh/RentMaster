using Microsoft.AspNetCore.Mvc;
using RentMaster.Core.Auth.Interface;
using RentMaster.Core.Auth.Models;
using RentMaster.Core.Auth.service;
using RentMaster.Core.Auth.Types;

[ApiController]
[Route("landlord/api/auth")]
public class LandLordAuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public LandLordAuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var token = await _authService.LoginAsync(model.Gmail, model.Password, UserTypes.LandLord);
        if (token == null)
            return Unauthorized(new { message = "Invalid Gmail or Password" });

        return Ok(new { token });
    }
}