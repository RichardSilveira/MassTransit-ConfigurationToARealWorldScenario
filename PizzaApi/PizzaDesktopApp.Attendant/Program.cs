using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace PizzaDesktopApp.Attendant
{
    class Program
    {
        static void Main(string[] args)
        {
            BusConfigurationForAttendanteApp.Configure();
        }
    }

    public static class BusConfigurationForAttendanteApp
    {
        public static void Configure()
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.RegisterOrderServiceQueue, e =>
                {
                    e.UseRateLimit(100, TimeSpan.FromSeconds(1));

                    //e.UseRetry(Retry.Immediate(3)); //For tests
                    e.UseRetry(Retry.Except<ArgumentException>().Interval(10, TimeSpan.FromSeconds(5)));

                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TripThreshold = 15;
                        cb.ResetInterval(TimeSpan.FromMinutes(5));
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.ActiveThreshold = 10;
                    });

                    e.Consumer<OrderRegisteredConsumer>();
                    //x.UseLog(ConsoleOut, async context => "Consumer created");
                });
            });

            var consumeObserver = new ConsoleLogConsumeObserver();
            bus.ConnectConsumeObserver(consumeObserver);

            bus.Start();

            Console.WriteLine("Listening for Register order commands.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
//TODO: add middleware to the consumer pipeline