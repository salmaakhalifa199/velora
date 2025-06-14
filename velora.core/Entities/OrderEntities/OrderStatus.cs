using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.core.Entities.OrderEntities
{
    public enum OrderStatus
    {
        Placed = 1,
        Confirmed = 2,
        Pending = 3,
        Shipped = 4,
        Delivered = 5,
        Cancelled = 6,
       
    }
}
