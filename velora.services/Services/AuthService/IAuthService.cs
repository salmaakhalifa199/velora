using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.services.Services.AuthService.Dto;

namespace velora.services.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto, Role role);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    }
}
