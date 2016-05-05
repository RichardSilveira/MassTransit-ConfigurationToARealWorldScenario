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
using Microsoft.Owin.Hosting;

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
                //cfg.UseRetry(Retry.Except(typeof(ArgumentException),
                //    typeof(NotAcceptedStateMachineException)).Immediate(10));

                cfg.ReceiveEndpoint(host, RabbitMqConstants.SagaQueue, e =>
                {
                    cfg.EnablePerformanceCounters();

                    e.UseRetry(Retry.Interval(10, TimeSpan.FromSeconds(10)));

                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TripThreshold = 15;
                        cb.ResetInterval(TimeSpan.FromMinutes(5));
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.ActiveThreshold = 10;
                    });

                    e.UseRetry(Retry.Except(typeof(ArgumentException),
                        typeof(NotAcceptedStateMachineException)).Interval(10, TimeSpan.FromSeconds(5)));

                    e.StateMachineSaga(saga, repo);
                });
            });

            var consumeObserver = new ConsoleLogConsumeObserver();

            bus.ConnectConsumeObserver(consumeObserver);

            //TODO: See how to do versioning of messages (best practices)
            //http://masstransit.readthedocs.io/en/master/overview/versioning.html

            try
            {
                bus.Start();
                Console.WriteLine("Saga active.. Press enter to exit");

                GlobalConfiguration.Configuration
                .UseSqlServerStorage(@"Data Source=.\SQLEXPRESS;Initial Catalog=hangfire-masstransit;Integrated Security=True");

                hangfireServer = new BackgroundJobServer();
                Console.WriteLine("Hangfire Server started. Press any key to exit...");

                WebApp.Start<Startup>("http://localhost:1235");

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
