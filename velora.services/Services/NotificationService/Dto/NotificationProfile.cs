using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities;

namespace velora.services.Services.NotificationService.Dto
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationDto, Notification>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.EmailOrUsername))
                .ForMember(dest => dest.IsGuestOnly, opt => opt.MapFrom(src => false));

            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.EmailOrUsername, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.IsGuestOnly, opt => opt.MapFrom(src => src.IsGuestOnly));
        }
    }
}
