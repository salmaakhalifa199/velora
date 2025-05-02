using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.ContactsService.Dto
{
    public class ContactsDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [MaxLength(500, ErrorMessage = "Message must not exceed 500 characters.")]
        public string Message { get; set; }
    }
}
