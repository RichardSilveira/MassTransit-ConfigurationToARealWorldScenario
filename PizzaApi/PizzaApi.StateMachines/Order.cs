﻿using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.StateMachines
{
    public class Order : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public State CurrentState { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public int? OrderID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int? EstimatedTime { get; set; }
        public int Status { get; set; }
        public string RejectedReasonPhrase { get; set; }
        public int PizzaID { get; set; }
    }
}
