using AutoMapper;
using Microsoft.AspNetCore.Identity;
using velora.core.Entities.IdentityEntities;
using velora.repository.Interfaces.IdentityInterfaces;
using velora.services.Services.UserService.Dto;

namespace velora.services.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IPersonRepository _personRepository;
        private readonly UserManager<Person> _userManager;
        private readonly IMapper _mapper;
        public UserService(IPersonRepository personRepository, UserManager<Person> userManager, IMapper mapper)
        {
            _personRepository = personRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto)
        {
            var user = await _personRepository.GetByIdAsync(userId);
            if (user == null) return null;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;

            await _personRepository.UpdateAsync(user);

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<UserDto> GetProfileAsync(string userId)
        {
            var person = await _personRepository.GetByIdAsync(userId);
            if (person == null)
                throw new Exception("User not found.");

            var userDto = _mapper.Map<UserDto>(person);
            return userDto;
        }
    }
}
