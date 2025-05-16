using System.Threading.Tasks;
using velora.services.Services.UserService.Dto;

namespace velora.services.Services.FeedbackService 
{
	public interface IFeedbackService
	{
		Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto, string? userId = null);
		Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync();
		Task<FeedbackDto> ApproveFeedbackAsync(int id);
	}
}