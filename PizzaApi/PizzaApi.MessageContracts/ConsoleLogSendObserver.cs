using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public class ConsoleLogSendObserver : ISendObserver
    {
        public async Task PostSend<T>(SendContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            await Console.Out.WriteLineAsync("SendObserver - PostSend Observed with context: " + messageContext);
        }

        public async Task PreSend<T>(SendContext<T> context) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            await Console.Out.WriteLineAsync("SendObserver - PreSend Observed with context: " + messageContext);
        }

        public async Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            await Console.Out.WriteLineAsync("Error on SEND: " + messageContext + "with exception: " + exception.Message);
        }
    }
}
