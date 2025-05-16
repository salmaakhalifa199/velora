using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities;

namespace velora.services.Services.UserService.Dto
{
	public class FeedbackProfile : Profile
	{
		public FeedbackProfile()
		{
			CreateMap<Feedback, FeedbackDto>();
			CreateMap<CreateFeedbackDto, Feedback>();
		}
	}
}
