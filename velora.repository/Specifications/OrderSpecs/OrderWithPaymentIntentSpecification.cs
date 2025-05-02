using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;

namespace velora.repository.Specifications.OrderSpecs
{
    public class OrderWithPaymentIntentSpecification : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecification(string? paymentIntentId) 
            : base(order => order.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
