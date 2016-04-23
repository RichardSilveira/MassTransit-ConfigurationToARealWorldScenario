using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public interface IRegisterOrderCommand
    {
        Guid? EventID { get; }

        int OrderID { get; }
        string CustomerName { get; }
        string CustomerPhone { get; }
        int PizzaID { get; }
    }
}