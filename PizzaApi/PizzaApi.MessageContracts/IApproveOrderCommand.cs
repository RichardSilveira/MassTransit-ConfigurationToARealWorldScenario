using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public interface IApproveOrderCommand
    {
        Guid CorrelationId { get; }

        int OrderID { get; }
        int? EstimatedTime { get; }

        int Status { get; }
    }
}