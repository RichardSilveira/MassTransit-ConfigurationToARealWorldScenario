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
using Quartz;
using Quartz.Impl;
using MassTransit.QuartzIntegration;

namespace PizzaApi.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Saga";
            var saga = new OrderStateMachine();
            var repo = new InMemorySagaRepository<Order>();

            IScheduler _scheduler = CreateScheduler();


            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {

                cfg.ReceiveEndpoint(host, RabbitMqConstants.SagaQueue, e =>
                {
                    e.StateMachineSaga(saga, repo);
                    //e.UseMessageScheduler(new Uri("rabbitmq://localhost/quartz"));

                });

                cfg.ReceiveEndpoint(host, "quartz", e =>
                {
                    cfg.UseMessageScheduler(e.InputAddress);
                    //e.PrefetchCount = 1;

                    e.Consumer(() => new ScheduleMessageConsumer(_scheduler));
                    e.Consumer(() => new CancelScheduledMessageConsumer(_scheduler));
                });
            });

            try
            {
                bus.Start();

                //_scheduler.JobFactory = new MassTransitJobFactory(bus);

                //_scheduler.Start();
            }
            catch (Exception)
            {
                //_scheduler.Shutdown();
                throw;
            }

            Console.WriteLine("Saga active.. Press enter to exit");
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
