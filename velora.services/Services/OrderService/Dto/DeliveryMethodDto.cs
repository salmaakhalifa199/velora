using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.OrderService.Dto
{
    public class DeliveryMethodDto
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
