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
        Pending = 2,
        Running = 3,
        Shipped = 4,
        Delivered = 5,
        Cancelled = 6
    }
}
