using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.UserService.Dto
{
	public class CreateFeedbackDto
	{
		public string? Name { get; set; } 
		public string? Email { get; set; }
		public string Comment { get; set; }
	}
}
