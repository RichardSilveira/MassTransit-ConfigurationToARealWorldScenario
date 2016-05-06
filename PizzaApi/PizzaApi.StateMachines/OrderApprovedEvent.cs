using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.StateMachines
{
    public class OrderApprovedEvent : IOrderApprovedEvent
    {
        private readonly Order _orderInstance;
        
        private Guid _correlationId;
        private DateTime _timeStamp;
        private int _orderID;
        private int? _estimatedTime;
        private int _status;

        public Guid CorrelationId
        {
            get
            {
                return _correlationId;
            }
        }

        public DateTime Timestamp { get { return _timeStamp; } }

        public int? EstimatedTime
        {
            get
            {
                return _estimatedTime;
            }
        }

        public int OrderID
        {
            get
            {
                return _orderID;
            }
        }

        public int Status
        {
            get
            {
                return _status;
            }
        }

        public OrderApprovedEvent(Order orderInstance)
        {
            _orderInstance = orderInstance;

            _correlationId = _orderInstance.CorrelationId;
            _timeStamp = _orderInstance.Updated;

            _orderID = _orderInstance.OrderID.Value;
            _estimatedTime = _orderInstance.EstimatedTime;
            _status = _orderInstance.Status;

        }
    }
}
