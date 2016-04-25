using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDesktopApp.Attendant
{
    public static class AttendantApplicationActions
    {
        public static async Task<HttpResponseMessage> ApproveOrder(dynamic orderData)
        {
            using (var client = new HttpClient())
            {
                string uri = "api//order//" + orderData.OrderID + "//approve";
                client.BaseAddress = new Uri("http://localhost:1234");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(orderData.EstimatedTime, Encoding.UTF8, "application/json");

                return await client.PostAsync(uri, content);
            }
        }

        public static async Task<HttpResponseMessage> RejectOrder(dynamic orderData)
        {
            using (var client = new HttpClient())
            {
                string uri = "api//order//" + orderData.OrderID + "//reject";
                client.BaseAddress = new Uri("http://localhost:1234");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(orderData.ReasonPhrase, Encoding.UTF8, "application/json");

                return await client.PostAsync(uri, content);
            }
        }
    }
}