using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public interface IClosedOrderEvent
    {
        Guid CorrelationId { get; }
        DateTime Timestamp { get; }

        int OrderID { get; }
        int Status { get; }
    }
}
