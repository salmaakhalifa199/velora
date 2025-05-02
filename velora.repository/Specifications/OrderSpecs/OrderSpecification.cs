using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;

namespace velora.repository.Specifications.OrderSpecs
{
    public class OrderSpecification
    {
        public OrderStatus? Status { get; set; }  
        public Guid? PersonId { get; set; }  
        public string Search { get; set; }  
        public DateTime? OrderDate { get; set; }  
        public string Sort { get; set; }  
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
