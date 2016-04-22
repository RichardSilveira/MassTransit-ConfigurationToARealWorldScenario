using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "rabbitmq://localhost/pizzaapi/";
        public const string UserName = "guest";
        public const string Password = "guest";
        //public const string RegisterOrderServiceQueue = "registerorder.service";
        //public const string NotificationServiceQueue = "notification.service";
        public const string SagaQueue = "saga.service";
    }
}
