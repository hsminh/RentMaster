using Microsoft.AspNetCore.Mvc;
using RentMaster.Core.Auth.Interface;
using RentMaster.Core.Auth.Models;
using RentMaster.Core.Auth.Types;

namespace RentMaster.Controllers
{
    [ApiController]
    [Route("consumer/api/auth")]
    public class ConsumerAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public ConsumerAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var token = await _authService.LoginAsync(model.Gmail, model.Password, UserTypes.Consumer);
            if (token == null)
                return Unauthorized(new { message = "Invalid Gmail or Password" });

            return Ok(new { token });
        }
    }
}