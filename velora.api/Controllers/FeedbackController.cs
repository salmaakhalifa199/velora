// API/Controllers/FeedbackController.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using velora.services.Services.FeedbackService;

using velora.services.Services.UserService.Dto;

namespace velora.api.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class FeedbackController : APIBaseController
	{
		private readonly IFeedbackService _feedbackService;

		public FeedbackController(IFeedbackService feedbackService)
		{
			_feedbackService = feedbackService;
		}


		[Authorize]
		[HttpPost("User")]
		public async Task<ActionResult<FeedbackDto>> CreateUserFeedback(CreateFeedbackDto dto)
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return BadRequest("Invalid user");

			var feedback = await _feedbackService.CreateFeedbackAsync(dto, userId);
			return Ok(feedback);
		}


		[AllowAnonymous]
		[HttpPost("Guest")]
		public async Task<ActionResult<FeedbackDto>> CreateGuestFeedback(CreateFeedbackDto dto)
		{
			if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Email))
				return BadRequest("Name and email are required for guest feedback");

			var feedback = await _feedbackService.CreateFeedbackAsync(dto);
			return Ok(feedback);
		}


		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetAllFeedbacks()
		{
			var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
			return Ok(feedbacks);
		}


		[Authorize(Roles = "Admin")]
		[HttpPatch("{id}/approve")]
		public async Task<ActionResult<FeedbackDto>> ApproveFeedback(int id)
		{
			var feedback = await _feedbackService.ApproveFeedbackAsync(id);
			if (feedback == null)
				return NotFound();

			return Ok(feedback);
		}
	}
}