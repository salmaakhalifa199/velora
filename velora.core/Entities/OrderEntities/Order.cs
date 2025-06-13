using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities.IdentityEntities;

namespace velora.core.Entities.OrderEntities
{
    public class Order : BaseEntity<Guid>
    {
        public string? UserId { get; set; }
        public string BuyerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethods DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); 
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        public OrderPaymentStatus OrderPaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public string? PaymentIntentId { get; set; } = string.Empty;
        public string? CartId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + (DeliveryMethod?.Price ?? 0);
        }


    }
}
