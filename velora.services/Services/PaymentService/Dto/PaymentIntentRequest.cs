using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.PaymentService.Dto
{
    public class PaymentIntentRequest
    {
        public long Amount { get; set; }
        public string Currency { get; set; } = "usd";
    }
}
