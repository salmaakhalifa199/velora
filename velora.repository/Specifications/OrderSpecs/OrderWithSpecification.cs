using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;

namespace velora.repository.Specifications.OrderSpecs
{
    public class OrderWithSpecification : BaseSpecifications<Order>
    {
        public OrderWithSpecification(OrderSpecification specs)
       : base(order =>
           (!specs.Status.HasValue || order.Status == specs.Status) &&  
           (!specs.UserId.HasValue || order.Id == specs.UserId) &&  
           (string.IsNullOrEmpty(specs.Search) || order.OrderItems.Any(oi => oi.ProductName.Contains(specs.Search))) &&  
           (!specs.OrderDate.HasValue || order.OrderDate >= specs.OrderDate.Value)  
       )
        {
            AddInclude(x => x.ShippingAddress); 
            AddInclude(x => x.DeliveryMethod);  

   
            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort.ToLower())
                {
                    case "date":
                        AddOrderBy(o => o.OrderDate);  
                        break;
                    case "dateDesc":
                        AddOrderByDes(o => o.OrderDate);  
                        break;
                    case "amount":
                        AddOrderBy(o => o.Subtotal); 
                        break;
                    case "amountDesc":
                        AddOrderByDes(o => o.Subtotal);  
                        break;
                    default:
                        AddOrderBy(o => o.OrderDate);  
                        break;
                }
            }
            else
            {
                AddOrderBy(o => o.OrderDate); 
            }

  
            ApplyPagination(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);  
        }
    }
}
