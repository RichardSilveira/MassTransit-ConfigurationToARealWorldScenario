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

        private Guid _correlationId;
        private DateTime _timeStamp;
        private int _orderID;
        private string _customerName;
        private string _customerPhone;
        private int _pizzaID;

        public Guid CorrelationId
        {
            get { return _correlationId; }
        }

        public DateTime Timestamp { get { return _timeStamp; } }

        public int OrderID
        {
            get { return _orderID; }

        }
        public string CustomerName
        {
            get { return _customerName; }
        }

        public string CustomerPhone
        {
            get { return _customerPhone; }
        }

        public int PizzaID
        {
            get { return _pizzaID; }
        }

        public OrderRegisteredEvent(Order orderInstance)
        {
            _orderInstance = orderInstance;

            _correlationId = _orderInstance.CorrelationId;
            _timeStamp = _orderInstance.Updated;

            _orderID = _orderInstance.OrderID.Value;
            _customerName = _orderInstance.CustomerName;
            _customerPhone = _orderInstance.CustomerPhone;
            _pizzaID = _orderInstance.PizzaID;
        }
    }
}
