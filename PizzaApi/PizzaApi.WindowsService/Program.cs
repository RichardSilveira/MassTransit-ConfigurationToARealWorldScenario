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
using MassTransit.NLogIntegration;
using MassTransit.Logging;
using NLog;
using PizzaApi.MessageContracts;
using MassTransit.BusConfigurators;
using Hangfire.Mongo;
using Topshelf;
using Topshelf.Runtime;

namespace PizzaApi.WindowsService
{
    class Program
    {
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<SagaService>();

                x.UseNLog();
                x.DependsOn("RabbitMQ");

                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Saga Service - Pizza API Order State Machine");
                x.SetDisplayName("Saga Service - Pizza API Order State Machine");
                x.SetServiceName("Saga Service - Pizza API Order State Machine");

                //x.EnablePauseAndContinue();

                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(1);

                    //number of days until the error count resets
                    r.SetResetPeriod(1);
                });
            });

            //TODO:Enable Pause and continue (Stop the bus but don't stop the hangfire server)
            //Console.ReadLine();
        }
    }
}
