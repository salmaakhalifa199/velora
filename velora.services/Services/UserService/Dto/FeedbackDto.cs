using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.UserService.Dto
{
	public class FeedbackDto
	{
		public int Id { get; set; }
		public string? UserId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsApproved { get; set; }
	}
}
