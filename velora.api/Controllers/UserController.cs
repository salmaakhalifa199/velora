using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using velora.services.Services.UserService.Dto;
using velora.services.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using velora.core.Entities.IdentityEntities;
using velora.services.Services.FeedbackService;
using System.Threading.Tasks;

namespace velora.api.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
	public class UserController : APIBaseController
	{
		private readonly IUserService _userService; private readonly IFeedbackService _feedbackService;

		public UserController(IUserService userService, IFeedbackService feedbackService)
		{
			_userService = userService;
			_feedbackService = feedbackService;
		}

		[HttpGet("profile")]
		public async Task<IActionResult> GetProfile()
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userIdClaim))
			{
				return BadRequest("Token is missing the UserId claim.");
			}

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

			var updatedUserProfile = await _userService.UpdateProfileAsync(userIdClaim, updateProfileDto);

			if (updatedUserProfile == null)
				return NotFound();

			return Ok(updatedUserProfile);
		}

		
	}

}