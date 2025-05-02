using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.IdentityEntities;

namespace velora.services.Services.UserService.Dto
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Person, UserDto>();
            CreateMap<UpdateUserProfileDto, Person>();
        }
    }
}
