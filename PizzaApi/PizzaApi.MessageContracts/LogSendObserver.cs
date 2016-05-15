using MassTransit;
using MassTransit.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public class LogSendObserver : ISendObserver
    {
        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            return Task.Run(() => Logger.Get("mongoCustomLog").InfoFormat("SendObserver - PostSend Observed with context: " + messageContext));
        }

        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            return Task.Run(() => Logger.Get("mongoCustomLog").InfoFormat("SendObserver - PreSend Observed with context: " + messageContext));
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            return Task.Run(() => Logger.Get("mongoCustomLog").ErrorFormat("Error on SEND: " + messageContext + "with exception: " + exception.Message));
        }
    }
}
