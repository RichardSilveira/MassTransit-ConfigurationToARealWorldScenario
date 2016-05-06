using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.StateMachines
{
    public class OrderMaxTimeExpiredEvent : IOrderMaxTimeExpiredEvent
    {
        private readonly Order _orderInstance;

        private Guid _correlationId;
        private DateTime _timeStamp;
        private int _orderID;
        private int? _estimatedTime;
        private string _customerName;
        private string _customerPhone;

        public Guid CorrelationId
        {
            get { return _correlationId; }
        }

        public DateTime Timestamp { get { return _timeStamp; } }

        public int OrderID
        {
            get { return _orderID; }
        }
        public int? EstimatedTime
        {
            get { return _estimatedTime; }
        }

        public string CustomerName
        {
            get { return _customerName; }
        }
        public string CustomerPhone
        {
            get { return _customerPhone; }
        }

        public OrderMaxTimeExpiredEvent(Order orderInstance)
        {
            _orderInstance = orderInstance;

            _correlationId = _orderInstance.CorrelationId;
            _timeStamp = _orderInstance.Updated;

            _orderID = _orderInstance.OrderID.Value;
            _estimatedTime = _orderInstance.EstimatedTime;
            _customerName = _orderInstance.CustomerName;
            _customerPhone = _orderInstance.CustomerPhone;
        }
    }
}
