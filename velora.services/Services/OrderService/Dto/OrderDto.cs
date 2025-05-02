using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;

namespace velora.services.Services.OrderService.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } 
        public AddressDto ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } 
        public OrderPaymentStatus OrderPaymentStatus { get; set; } 
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
        public string? CartId { get; set; }
        public string? PaymentIntentId { get; set; }

    }
}
