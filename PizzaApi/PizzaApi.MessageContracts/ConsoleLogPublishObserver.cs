using MassTransit;
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
        public async Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            await Console.Out.WriteLineAsync("PublishObserver - PostPublish Observed with context: " + messageContext);
        }

        public async Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            await Console.Out.WriteLineAsync("PrePublish - PostPublish Observed with context: " + messageContext);
        }

        public async Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            await Console.Out.WriteLineAsync("Error on PUBLISH: " + messageContext + "with exception: " + exception.Message);
        }
    }
}
