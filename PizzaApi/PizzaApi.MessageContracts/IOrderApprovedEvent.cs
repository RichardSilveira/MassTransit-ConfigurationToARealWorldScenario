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

        int? EstimatedTime { get; set; }

        int Status { get; set; }
    }
}
//public int OrderID { get; set; }

//public string CustomerName { get; set; }

//public string CustomerPhone { get; set; }

//public int? EstimatedTime { get; set; }

//public string StatusDescription { get; set; }

//public string RejectedReasonPhrase { get; set; }

//public int PizzaID { get; set; }