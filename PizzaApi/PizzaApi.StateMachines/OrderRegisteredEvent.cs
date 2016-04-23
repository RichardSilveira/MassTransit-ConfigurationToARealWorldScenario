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


        private Guid _eventID;
        private int _orderID;
        private string _customerName;
        private string _customerPhone;
        private int _pizzaID;

        public Guid EventID
        {
            get { return _eventID; }
        }

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

            _eventID = _orderInstance.CorrelationId;
            _orderID = orderInstance.OrderID;
            _customerName = _orderInstance.CustomerName;
            _customerPhone = _orderInstance.CustomerPhone;
            _pizzaID = _orderInstance.PizzaID;
        }        
    }
}
