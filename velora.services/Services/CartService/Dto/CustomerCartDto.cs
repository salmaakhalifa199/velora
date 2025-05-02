using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.CartService.Dto
{
    public class CustomerCartDto
    {
        public string? Id { get; set; }

        public int? DeliveryMethodId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Shipping price must be zero or greater.")]
        public decimal ShippingPrice { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Cart must contain at least one item.")]
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        //public string? PaymentIntentId { get; set; }
        //public string? ClientSecret { get; set; }
    }
}
