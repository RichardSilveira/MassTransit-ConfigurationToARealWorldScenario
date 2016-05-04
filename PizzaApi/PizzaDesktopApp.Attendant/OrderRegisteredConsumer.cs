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
            try
            {
                //throw new Exception("Test for monitoring consume observer on fault method");

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
                        Console.Write("Why do you want do reject this order? :");
                        string reasonPhrase = "\"" + Console.ReadLine() + "\"";

                        var responseToReject = await AttendantApplicationActions.RejectOrder(new { OrderID = context.Message.OrderID, ReasonPhrase = reasonPhrase });

                        var responseToRejectContent = await responseToReject.Content.ReadAsStringAsync();
                        Console.WriteLine(string.Format("PizzaApi server status code {0}. \n Content: {1}", responseToReject.StatusCode, responseToRejectContent));

                        break;
                    default:
                        Console.WriteLine("Your awnser is invalid!");
                        break;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw exc;
            }
            finally
            {
                Console.WriteLine("Press enter to exit (and finalize the current queue consumer");
                Console.ReadLine();
            }
            //BusConfigurationForAttendanteApp.Configure();
        }
    }
}