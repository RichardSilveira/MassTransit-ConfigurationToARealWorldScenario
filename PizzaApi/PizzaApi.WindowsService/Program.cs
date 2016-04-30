using MassTransit.Saga;
using PizzaApi.MessageContracts;
using PizzaApi.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;
using Hangfire;

namespace PizzaApi.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Saga";
            var saga = new OrderStateMachine();
            var repo = new InMemorySagaRepository<Order>();

            BackgroundJobServer hangfireServer = null;

            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.SagaQueue, e =>
                {
                    e.StateMachineSaga(saga, repo);
                });
            });

            try
            {
                bus.Start();
                Console.WriteLine("Saga active.. Press enter to exit");

                GlobalConfiguration.Configuration
                .UseSqlServerStorage(@"Data Source=.\SQLEXPRESS;Initial Catalog=hangfiresample;Integrated Security=True");

                hangfireServer = new BackgroundJobServer();
                Console.WriteLine("Hangfire Server started. Press any key to exit...");

                Console.ReadLine();
            }
            catch (Exception exc)
            {
                hangfireServer.Dispose();
                bus.Stop();
            }
        }
    }
}
