using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseUri = ConfigurationManager.AppSettings["Root"];

            WebApp.Start<Startup>(baseUri);

            Console.WriteLine("Starting local web server at " + baseUri);
            Console.ReadLine();
        }
    }
}
