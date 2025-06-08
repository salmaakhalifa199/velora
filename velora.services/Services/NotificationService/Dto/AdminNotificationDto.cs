using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.NotificationService.Dto
{
    public class AdminNotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public string? EmailOrUsername { get; set; }

        [Required]
        public string Target { get; set; }
    }
}
