﻿using MassTransit.Saga;
using PizzaApi.MessageContracts;
using PizzaApi.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;

namespace PizzaApi.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Saga";
            var saga = new OrderStateMachine();
            var repo = new InMemorySagaRepository<Order>();

            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.SagaQueue, e =>
                {
                    e.StateMachineSaga(saga, repo);
                    e.UseMessageScheduler(new Uri("rabbitmq://localhost/quartz"));
                });
            });
            bus.Start();
            Console.WriteLine("Saga active.. Press enter to exit");
            Console.ReadLine();
            bus.Stop();
        }
    }
}
