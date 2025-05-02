using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using velora.core.Entities.IdentityEntities;
using velora.services.Helper;
using velora.services.Services.AuthService.Dto;
using velora.services.Services.TokenService;

namespace velora.services.Services.AuthService
{
    public enum Role
    {
        User = 0,
        Admin = 1,
        Guest = 2
    }
    public class AuthService : IAuthService
    {
        private readonly SignInManager<Person> _signInManager;
        private readonly UserManager<Person> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IOptions<AuthSettings> _authSettings;

        public AuthService(SignInManager<Person> signInManager,
                           UserManager<Person> userManager,
                           ITokenService tokenService,
                              IOptions<AuthSettings> authSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _authSettings = authSettings;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto , Role role)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                throw new Exception("Login failed");

            var userRoles = await _userManager.GetRolesAsync(user);
            var actualRole = userRoles.FirstOrDefault();

            if (actualRole == null || actualRole != role.ToString())
                throw new Exception($"Access denied. You're not a {role}");


            var token = await _tokenService.GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Token = token,
                Role = actualRole
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto )
        {
            var existing = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existing != null)
                return null;

            if (registerDto.Role == Role.Admin)
            {
                if (string.IsNullOrEmpty(registerDto.SecretCode) || registerDto.SecretCode != _authSettings.Value.AdminSecretCode)
                {
                    throw new Exception("Invalid admin registration code.");
                }
            }


            var person = new Person
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(person, registerDto.Password);
            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            var roleName = registerDto.Role.ToString(); 
            await _userManager.AddToRoleAsync(person, roleName);

            return new AuthResponseDto
            {
                Token = await _tokenService.GenerateTokenAsync(person),
                Role = roleName
            };
        }

    }
}

