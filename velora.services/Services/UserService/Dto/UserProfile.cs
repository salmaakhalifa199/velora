using AutoMapper;
using velora.core.Entities;
using velora.core.Entities.IdentityEntities;

namespace velora.services.Services.UserService.Dto {
	public class UserProfile : Profile 
	{
		public UserProfile() 
		{ 
			CreateMap<Person, UserDto>();
			CreateMap<UpdateUserProfileDto, Person>();
			CreateMap<FeedbackDto, Feedback>(); 
		} 
	}
}