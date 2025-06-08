using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.services.Services.NotificationService.Dto;

namespace velora.services.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendToAllUsersAsync(string title, string message);
        Task SendToUserAsync(string title, string message, string emailOrUsername);
        Task SendToGuestsAsync(string title, string message);
        Task SendNotificationAsync(AdminNotificationDto dto);

    }
}
