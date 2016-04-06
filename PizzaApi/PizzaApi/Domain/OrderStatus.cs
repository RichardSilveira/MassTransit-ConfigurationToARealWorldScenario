using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Domain
{
    public enum OrderStatus
    {
        WaitingAttendance = 1,
        Approved,
        Rejected
    }
}
