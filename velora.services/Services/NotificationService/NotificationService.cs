using Microsoft.AspNetCore.Identity;
using Store.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities;
using velora.core.Entities.IdentityEntities;
using velora.repository.Interfaces;
using velora.Repository.Repositories;
using velora.services.Services.NotificationService.Dto;

namespace velora.services.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly UserManager<Person> _userManager;
        private readonly IGenericRepository<Notification, Guid> _notificationRepository;
        private readonly IUnitWork _unitOfWork;

        public NotificationService(
            UserManager<Person> userManager,
            IGenericRepository<Notification, Guid> notificationRepository,
            IUnitWork unitOfWork)
        {
            _userManager = userManager;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        #region Send notification
 public async Task SendToAllUsersAsync(string title, string message)
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var notif = new Notification
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Message = message,
                    UserId = user.Id,
                    IsGuestOnly = false
                };
                await _notificationRepository.AddAsync(notif);
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task SendToUserAsync(string title, string message, string emailOrUsername)
        {
            var user = await _userManager.FindByEmailAsync(emailOrUsername)
                       ?? await _userManager.FindByNameAsync(emailOrUsername);

            if (user == null)
                throw new Exception("User not found");

            var notif = new Notification
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                UserId = user.Id,
                IsGuestOnly = false
            };

            await _notificationRepository.AddAsync(notif);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SendToGuestsAsync(string title, string message)
        {
            var notif = new Notification
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                IsGuestOnly = true
            };

            await _notificationRepository.AddAsync(notif);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SendNotificationAsync(AdminNotificationDto dto)
        {
            switch (dto.Target)
            {
                case "All":
                    await SendToAllUsersAsync(dto.Title, dto.Message);
                    break;

                case "Guest":
                    await SendToGuestsAsync(dto.Title, dto.Message);
                    break;

                case "User":
                    if (string.IsNullOrEmpty(dto.EmailOrUsername))
                        throw new ArgumentException("EmailOrUsername is required when Target is 'User'");
                    await SendToUserAsync(dto.Title, dto.Message, dto.EmailOrUsername);
                    break;

                default:
                    throw new ArgumentException("Invalid Target. Allowed values: All, User, Guest.");
            }
        }
        #endregion

        #region Get notifications
        public async Task<IEnumerable<UserNotificationDto>> GetUserNotificationsAsync(string userId)
        {
            var all = await _notificationRepository.GetAllAsync();

            var userNotifs = all
                .Where(n =>
                    // 1. Notifications sent to all users (IsGuestOnly == false && UserId == null)
                    (n.UserId == null && !n.IsGuestOnly) ||
                    // 2. Notifications sent to this specific user
                    (n.UserId == userId && !n.IsGuestOnly)
                )
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new UserNotificationDto
                {
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt
                });

            return userNotifs;
        }

        public async Task<IEnumerable<UserNotificationDto>> GetGuestNotificationsAsync()
        {
            var all = await _notificationRepository.GetAllAsync();

            var guestNotifs = all
                .Where(n => n.IsGuestOnly)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new UserNotificationDto
                {
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt
                });

            return guestNotifs;
        }
        #endregion

    }
}
