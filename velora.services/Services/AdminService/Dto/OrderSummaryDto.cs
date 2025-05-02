using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;
using velora.services.Services.OrderService.Dto;

namespace velora.services.Services.AdminService.Dto
{
    public class OrderSummaryDto
    {
        public Guid OrderId { get; set; }
        [EmailAddress]
        public string BuyerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string DeliveryMethod { get; set; }
    }
}
