using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace PizzaApi.WindowsService
{
    public class SagaService 
    {

        public bool Start()
        {
            Console.WriteLine("Start...");
            return true;
        }

        public bool Stop()
        {
            Console.WriteLine("Stop...");
            return true;
        }
    }
}
