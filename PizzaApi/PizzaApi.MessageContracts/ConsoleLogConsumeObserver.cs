using MassTransit;
using MassTransit.Pipeline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.MessageContracts
{
    public class ConsoleLogConsumeObserver : IConsumeObserver
    {
        public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            var messageContext = JsonConvert.SerializeObject(context.Message);

            await Console.Out.WriteLineAsync("Error on CONSUME: " + messageContext + "with exception: " + exception.Message);
        }

        public async Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            //var messageContext = JsonConvert.SerializeObject(context.Message);

            //await Console.Out.WriteLineAsync("ConsumeObserver - PostConsume Observed with context: " + messageContext);
        }

        public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            //var messageContext = JsonConvert.SerializeObject(context.Message);

            //await Console.Out.WriteLineAsync("ConsumeObserver - PreConsume Observed with context: " + messageContext);
        }
    }
}
