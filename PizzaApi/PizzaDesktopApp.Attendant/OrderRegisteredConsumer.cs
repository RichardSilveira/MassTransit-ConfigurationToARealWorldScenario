using MassTransit;
using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDesktopApp.Attendant
{
    public class OrderRegisteredConsumer : IConsumer<IOrderRegisteredEvent>
    {
        public Task Consume(ConsumeContext<IOrderRegisteredEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}