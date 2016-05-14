using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Topshelf;

namespace PizzaDesktopApp.Attendant
{
    public class AttendantService : ServiceControl
    {
        private BusHandle _busHandle;

        public bool Start(HostControl hostControl)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.RegisterOrderServiceQueue, e =>
                {
                    e.UseRateLimit(100, TimeSpan.FromSeconds(1));

                    e.UseRetry(Retry.Interval(5, TimeSpan.FromSeconds(5)));

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

            _busHandle = bus.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            if (_busHandle != null)
                _busHandle.Stop();

            return true;
        }
    }
}
