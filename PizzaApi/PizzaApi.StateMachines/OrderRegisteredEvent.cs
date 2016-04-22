using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.StateMachines
{
    public class OrderRegisteredEvent : IOrderRegisteredEvent
    {
        private readonly Order _orderInstance;


        private Guid _eventId;

        public Guid EventId
        {
            get { return _eventId; }
        }

        public OrderRegisteredEvent(Order orderInstance)
        {
            _orderInstance = orderInstance;
            _eventId = _orderInstance.CorrelationId;
            //etc...
        }
    }
}
