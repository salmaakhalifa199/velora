using System.ComponentModel.DataAnnotations;
using velora.core.Entities.OrderEntities;

namespace velora.services.Services.OrderService.Dto
{
    public class CreateOrderDto
    {
        public string CartId { get; set; }
        [EmailAddress]
        public string BuyerEmail { get; set; }
        public string? UserId { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        public OrderPaymentStatus? OrderPaymentStatus { get; set; } 
    }
}
