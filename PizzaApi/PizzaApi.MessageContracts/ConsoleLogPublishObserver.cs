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
    public class ConsoleLogPublishObserver : IPublishObserver
    {
        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            return Task.Run(() => Logger.Get("mongoCustomLog").InfoFormat("PublishObserver - PostPublish Observed with context: " + messageContext));
        }

        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            return Task.Run(() => Logger.Get("mongoCustomLog").InfoFormat("PrePublish - PostPublish Observed with context: " + messageContext));
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            return Task.Run(() => Logger.Get("mongoCustomLog").ErrorFormat("Error on PUBLISH: " + messageContext + "with exception: " + exception.Message));
        }
    }
}
