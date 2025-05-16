using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using velora.core.Data.Contexts;
using velora.core.Entities;
using velora.repository.Interfaces;

namespace velora.repository.Repositories
{
	public class FeedbackRepository : IFeedbackRepository
	{
		private readonly StoreContext _context;

		public FeedbackRepository(StoreContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Feedback feedback)
		{
			await _context.Feedbacks.AddAsync(feedback);
			await _context.SaveChangesAsync();
		}

		public async Task<Feedback> GetByIdAsync(int id)
		{
			return await _context.Feedbacks
				
				.FirstOrDefaultAsync(f => f.Id == id);
		}

		public async Task<IEnumerable<Feedback>> GetAllAsync()
		{
			return await _context.Feedbacks
		
				.ToListAsync();
		}
	}

}