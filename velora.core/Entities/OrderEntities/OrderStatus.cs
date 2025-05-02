using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.core.Entities.OrderEntities
{
    public enum OrderStatus
    {
        Placed,
        Pending,
        Running,
        Shipped,
        Delivered,
        Cancelled
    }
}
