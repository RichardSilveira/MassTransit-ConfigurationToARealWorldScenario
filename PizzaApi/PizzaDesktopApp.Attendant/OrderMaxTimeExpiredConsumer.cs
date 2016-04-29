using MassTransit;
using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDesktopApp.Attendant
{
    public class OrderMaxTimeExpiredConsumer : IConsumer<IOrderMaxTimeExpiredEvent>
    {
        public async Task Consume(ConsumeContext<IOrderMaxTimeExpiredEvent> context)
        {
            await Console.Out.WriteLineAsync(string.Format("Send notification to attendant to inform about an order that is taking too long to get ready (orderID: {0}, estimated time: {1}, customerName: {2}.",
                                                                                                context.Message.OrderID, context.Message.EstimatedTime, context.Message.CustomerName));

            Console.ReadLine();
        }
    }
}
