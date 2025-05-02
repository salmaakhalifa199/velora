using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.AuthService.Dto
{
   
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Format For Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])(?!.*(.).*\1.*\1).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long, contain at least 1 digit, 1 lowercase letter, 1 uppercase letter, 1 special character, and 2 unique characters.")]
        public string Password { get; set; }

        public Role Role { get; set; } = Role.User;
        public string? SecretCode { get; set; }
    }
}
