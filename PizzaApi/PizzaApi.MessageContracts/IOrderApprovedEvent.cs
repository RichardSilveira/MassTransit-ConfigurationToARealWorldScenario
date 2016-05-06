using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    //TODO: Remove it later 
    public interface IOrderApprovedEvent
    {
        Guid CorrelationId { get; }
        DateTime Timestamp { get; }

        int OrderID { get; }
        int? EstimatedTime { get; }

        int Status { get; }
    }
}