using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities;

public interface IFeedbackRepository
{
	Task AddAsync(Feedback feedback);
	Task<Feedback> GetByIdAsync(int id);
	Task<IEnumerable<Feedback>> GetAllAsync();
}