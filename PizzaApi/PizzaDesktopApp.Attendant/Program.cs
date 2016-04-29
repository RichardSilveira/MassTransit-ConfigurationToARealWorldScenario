using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.QuartzIntegration;
using Quartz;
using Quartz.Impl;

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
            IScheduler _scheduler = CreateScheduler();

            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.RegisterOrderServiceQueue, e =>
                {
                    e.UseRateLimit(100, TimeSpan.FromSeconds(1));

                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TripThreshold = 15;
                        cb.ResetInterval(TimeSpan.FromMinutes(5));
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.ActiveThreshold = 10;
                    });

                    e.Consumer<OrderRegisteredConsumer>();
                });

                //cfg.ReceiveEndpoint("quartz", e =>
                //{
                //    cfg.UseMessageScheduler(e.InputAddress);
                //    //e.PrefetchCount = 1;

                //    e.Consumer(() => new ScheduleMessageConsumer(_scheduler));
                //    e.Consumer(() => new CancelScheduledMessageConsumer(_scheduler));
                //});

                //cfg.ReceiveEndpoint(host, "quartz", e =>
                //{
                //    cfg.UseMessageScheduler(e.InputAddress);
                //    //e.PrefetchCount = 1;

                //    e.Consumer(() => new ScheduleMessageConsumer(_scheduler));
                //    e.Consumer(() => new CancelScheduledMessageConsumer(_scheduler));
                //});
            });

            bus.Start();

            Console.WriteLine("Listening for Register order commands.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }

        static IScheduler CreateScheduler()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            IScheduler scheduler = schedulerFactory.GetScheduler();

            return scheduler;
        }
    }
}
