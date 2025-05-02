using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using velora.services.Services.UserService.Dto;
using velora.services.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using velora.core.Entities.IdentityEntities;

namespace velora.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public class UserController : APIBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return BadRequest("Token is missing the UserId claim.");
            }

            // Directly use the userIdClaim as a string
            var userProfile = await _userService.GetProfileAsync(userIdClaim);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(userProfile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileDto updateProfileDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return BadRequest("UserId claim is missing in the token.");

            var updatedUserProfile = await _userService.UpdateProfileAsync(userId: userIdClaim, updateProfileDto: updateProfileDto);

            if (updatedUserProfile == null) return NotFound();

            return Ok(updatedUserProfile);
        }
    }
}
