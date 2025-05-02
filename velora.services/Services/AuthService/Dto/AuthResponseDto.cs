using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.AuthService.Dto
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
