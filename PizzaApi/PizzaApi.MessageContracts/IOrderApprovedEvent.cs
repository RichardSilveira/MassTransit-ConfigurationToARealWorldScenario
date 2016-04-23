using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public interface IOrderApprovedEvent
    {
        Guid? EventID { get; }

        int OrderID { get; set; }
        int? EstimatedTime { get; set; }

        int Status { get; set; }
    }
}