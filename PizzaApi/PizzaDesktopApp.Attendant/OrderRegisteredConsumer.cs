using MassTransit;
using PizzaApi.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDesktopApp.Attendant
{
    public class OrderRegisteredConsumer : IConsumer<IOrderRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<IOrderRegisteredEvent> context)
        {
            //Ask user to verify if order will be approved or reject
            //Send request to Pizza API
            Console.Write(string.Format("The customer {0} made an order (ID: {1}) for pizza ID {2}. Did you want to approve this order? Answer bellow - Y/N",
                                                context.Message.CustomerName, context.Message.OrderID, context.Message.PizzaID));
            var attendantChoice = Console.ReadLine();

            switch (attendantChoice.ToUpper())
            {
                case "Y":
                    Console.Write("What is the estimated time for this order (in minutes)? :");
                    var estimatedTime = Console.ReadLine();

                    var response = await AttendantApplicationActions.ApproveOrder(new { OrderID = context.Message.OrderID, EstimatedTime = estimatedTime });

                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(string.Format("PizzaApi server status code {0}. \n Content: {1}", response.StatusCode, responseContent));

                    break;
                case "N":

                    break;
                default:
                    Console.WriteLine("Your awnser is invalid!");
                    break;
            }

            //BusConfigurationForAttendanteApp.Configure();
        }
    }
}