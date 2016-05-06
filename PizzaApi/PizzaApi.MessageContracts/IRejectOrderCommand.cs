using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public interface IRejectOrderCommand
    {
        Guid CorrelationId { get; }
        DateTime Timestamp { get; }

        int OrderID { get; }
        string RejectedReasonPhrase { get; }
    }
}