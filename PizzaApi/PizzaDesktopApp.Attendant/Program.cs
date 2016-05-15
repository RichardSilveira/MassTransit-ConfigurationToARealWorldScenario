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
    class Program
    {
        static int Main(string[] args)
        {
            return (int)HostFactory.Run(x => x.Service<AttendantService>());
        }
    }
}