using System;
using MassTransit.BusConfigurators;
using MassTransit;

namespace PizzaApi.MessageContracts
{
    public static class BusObserverExtensions
    {
        /// <summary>
        /// Connect an observer to the bus, to observe creation, start, stop, fault events.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="observer"></param>
        public static void BusObserver(IBusFactoryConfigurator configurator, IBusObserver observer)
        {
            configurator.AddBusFactorySpecification(new BusObserverSpecification(() => observer));
        }

        /// <summary>
        /// Connect an observer to the bus, to observe creation, start, stop, fault events.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="observerFactory">Factory to create the bus observer</param>
        public static void BusObserver<T>(IBusFactoryConfigurator configurator, Func<T> observerFactory)
            where T : IBusObserver
        {
            configurator.AddBusFactorySpecification(new BusObserverSpecification(() => observerFactory()));
        }
    }
}
