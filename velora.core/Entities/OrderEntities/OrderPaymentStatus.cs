using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.core.Entities.OrderEntities
{
    public enum OrderPaymentStatus
    {
        Pending  = 1,
        Paid = 2,
        Received = 3,
        Failed = 4
    }
}
