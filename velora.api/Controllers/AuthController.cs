using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using velora.services.HandlerResponses;
using velora.core.Entities.IdentityEntities;
using velora.services.Services.AuthService;
using velora.services.Services.AuthService.Dto;
using velora.services.Services.TokenService;

namespace velora.api.Controllers
{
    public class AuthController : APIBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto )
        {
            var personDto = await _authService.RegisterAsync(registerDto );
            if (personDto == null)
                return BadRequest(new CustomException(400, "Email already exists or registration failed"));

            return Ok(personDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, [FromQuery] Role role)
        {
            var personDto = await _authService.LoginAsync(loginDto, role);
            if (personDto == null)
                return BadRequest(new CustomException(400, "Email Does not Exist"));

            return Ok(personDto);
        }
    }
}
