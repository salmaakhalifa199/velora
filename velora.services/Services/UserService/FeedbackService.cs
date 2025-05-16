// Services/Services/FeedbackService/FeedbackService.cs
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using velora.core.Data.Contexts;
using velora.core.Entities;
using velora.core.Entities.IdentityEntities;

using velora.services.Services.UserService.Dto;

namespace velora.services.Services.FeedbackService
{
	public class FeedbackService : IFeedbackService
	{
		private readonly StoreContext _context;
		private readonly UserManager<Person> _userManager;
		private readonly IMapper _mapper;

		public FeedbackService(
			StoreContext context,
			UserManager<Person> userManager,
			IMapper mapper)
		{
			_context = context;
			_userManager = userManager;
			_mapper = mapper;
		}

		//public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto, string? userId = null)
		//{
		//	var feedback = new Feedback
		//	{
		//		UserId = userId,
		//		Comment = dto.Comment
		//	};

		//	if (userId != null)
		//	{
		//		var user = await _userManager.FindByIdAsync(userId);
		//		feedback.Name = $"{user.FirstName} {user.LastName}";
		//		feedback.Email = user.Email;
		//	}
		//	else
		//	{
		//		feedback.Name = dto.Name;
		//		feedback.Email = dto.Email;
		//	}

		//	await _context.Feedbacks.AddAsync(feedback);
		//	await _context.SaveChangesAsync();

		//	return _mapper.Map<FeedbackDto>(feedback);
		//}

		// في FeedbackService.cs
		// FeedbackService.cs
		public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto, string? userId = null)
		{
			var feedback = new Feedback
			{
				Comment = dto.Comment,
				CreatedAt = DateTime.UtcNow,
				IsApproved = false
			};

			if (!string.IsNullOrEmpty(userId))
			{
				feedback.UserId = userId; // تخزين الـ ID فقط بدون علاقة
				var user = await _userManager.FindByIdAsync(userId);
				feedback.Name = user != null ? $"{user.FirstName} {user.LastName}" : "Registered User";
				feedback.Email = user?.Email ?? "no-email@example.com";
			}
			else
			{
				feedback.Name = dto.Name ?? "Anonymous";
				feedback.Email = dto.Email ?? "no-email@example.com";
			}

			_context.Feedbacks.Add(feedback);
			await _context.SaveChangesAsync();

			return _mapper.Map<FeedbackDto>(feedback);
		}
		public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync()
		{
			var feedbacks = _context.Feedbacks
				.OrderByDescending(f => f.CreatedAt)
				.ToList();

			return _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
		}

		public async Task<FeedbackDto> ApproveFeedbackAsync(int id)
		{
			var feedback = await _context.Feedbacks.FindAsync(id);
			if (feedback == null) return null;

			feedback.IsApproved = true;
			await _context.SaveChangesAsync();

			return _mapper.Map<FeedbackDto>(feedback);
		}
	}
}