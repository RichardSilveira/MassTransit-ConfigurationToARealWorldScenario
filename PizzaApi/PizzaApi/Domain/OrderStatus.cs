
using System.ComponentModel;
namespace PizzaApi.Domain
{
    public enum OrderStatus
    {
        WaitingAttendance = 1,
        Approved,
        Rejected,
        Closed
    }
}
