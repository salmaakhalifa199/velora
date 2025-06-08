using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.NotificationService.Dto
{
    public class NotificationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string? EmailOrUsername { get; set; }
        [Required]
        public string Target { get; set; }
        public bool IsGuestOnly { get; set; } = false;
    }
}
