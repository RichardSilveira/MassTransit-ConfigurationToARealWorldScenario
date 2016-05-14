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
using Hangfire;
using Microsoft.Owin.Hosting;
using MassTransit.NLogIntegration;
using MassTransit.Logging;
using NLog;
using PizzaApi.MessageContracts;
using MassTransit.BusConfigurators;
using Hangfire.Mongo;
using Topshelf;
using Topshelf.Runtime;
using Topshelf.Logging;

namespace PizzaApi.WindowsService
{
    public class SagaService : ServiceControl
    {
        private IBusControl bus;//_busControl;
        private BusHandle _busHandle;

        private IBusObserver busObserver;

        private BackgroundJobServer hangfireServer;

        public bool Start(HostControl hostControl)
        {
            var saga = new OrderStateMachine();
            var repo = new InMemorySagaRepository<Order>();

            hangfireServer = null;
            var busObserver = new BusObserver();

            bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.AddBusFactorySpecification(new BusObserverSpecification(() => busObserver));

                cfg.ReceiveEndpoint(host, RabbitMqConstants.SagaQueue, e =>
                {
                    cfg.UseNLog(new LogFactory());

                    cfg.EnablePerformanceCounters();

                    e.UseRetry(Retry.Interval(5, TimeSpan.FromSeconds(5)));


                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TripThreshold = 15;
                        cb.ResetInterval(TimeSpan.FromMinutes(5));
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.ActiveThreshold = 10;
                    });

                    //e.UseRetry(Retry.Except(typeof(ArgumentException),
                    //    typeof(NotAcceptedStateMachineException)).Interval(10, TimeSpan.FromSeconds(5)));
                    //TODO: Create a custom filter policy for inner exceptions on Sagas: http://stackoverflow.com/questions/37041293/how-to-use-masstransits-retry-policy-with-sagas

                    e.StateMachineSaga(saga, repo);
                });
            });

            var consumeObserver = new ConsoleLogConsumeObserver();

            bus.ConnectConsumeObserver(consumeObserver);

            //TODO: See how to do versioning of messages (best practices)
            //http://masstransit.readthedocs.io/en/master/overview/versioning.html

            try
            {
                _busHandle = bus.Start();
                Console.WriteLine("Saga active.. Press enter to exit");

                //GlobalConfiguration.Configuration
                //.UseSqlServerStorage(@"Data Source=.\SQLEXPRESS;Initial Catalog=hangfire-masstransit;Integrated Security=True;User ID=sa;Password=123456");
                GlobalConfiguration.Configuration.UseMongoStorage("mongodb://localhost:27017", "hangfire-masstransit");

                hangfireServer = new BackgroundJobServer();
                Console.WriteLine("Hangfire Server started. Press any key to exit...");

                WebApp.Start<Startup>("http://localhost:1235");
            }
            catch (Exception exc)
            {
                hangfireServer.Dispose();
                bus.Stop();

                throw;
            }

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            if (_busHandle != null)
                _busHandle.Stop();

            if(hangfireServer != null)
                hangfireServer.Dispose();

            return true;
        }
    }
}
