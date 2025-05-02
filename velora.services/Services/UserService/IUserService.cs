using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.IdentityEntities;
using velora.services.Services.UserService.Dto;

namespace velora.services.Services.UserService
{
    public interface IUserService
    {
        Task <UserDto> UpdateProfileAsync(string userId, UpdateUserProfileDto updateProfileDto);
        Task<UserDto> GetProfileAsync(string userId);
    }
}
