using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;
using velora.repository.Specifications;

namespace velora.repository.Specification.OrderSpecs
{
    public class OrderWithItemSpecification : BaseSpecifications<Order>
    {
        public OrderWithItemSpecification()
      : base() 
        {
            AddInclude(order => order.OrderItems); //.Select(oi => oi.ItemOrdered)
            AddInclude(order => order.DeliveryMethod); 
            AddOrderByDes(order => order.OrderDate); 
        }
        public OrderWithItemSpecification(string buyerEmail) 
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDes(order => order.OrderDate);
        }

        public OrderWithItemSpecification(Guid id , string buyerEmail )
            : base(order => order.Id == id  && order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }

    }
}
