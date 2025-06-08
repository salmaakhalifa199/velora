using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using velora.services.Services.NotificationService.Dto;
using velora.services.Services.NotificationService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace velora.api.Controllers
{
    public class NotificationsController : APIBaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> SendNotification([FromBody] AdminNotificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _notificationService.SendNotificationAsync(dto);
                return Ok("Notification sent successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
