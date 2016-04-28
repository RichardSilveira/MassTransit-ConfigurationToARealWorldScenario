﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public interface IOrderMaxTimeExpiredEvent
    {
        Guid CorrelationId { get; }

        int OrderID { get; }
        int? EstimatedTime { get; }

        string CustomerName { get; }
        string CustomerPhone { get; }
    }
}
