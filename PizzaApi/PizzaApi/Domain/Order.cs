using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Domain
{
    public class Order
    {
        public Guid OrderID { get; protected set; }

        public string CustomerName { get; protected set; }

        public string CustomerPhone { get; protected set; }

        public int? SenhaEspera { get; protected set; }//TODO:review

        public decimal? EstimatedTime { get; protected set; }

        public int Status { get; protected set; }

        public Guid PizzaID { get; protected set; }

        public Order(string customerName, string customerPhone, Guid pizzaID)
        {
            if (string.IsNullOrEmpty(customerName))
                throw new ArgumentNullException("customerName");

            if (string.IsNullOrEmpty(customerPhone))
                throw new ArgumentNullException("customerPhone");

            if (pizzaID == null || pizzaID == Guid.Empty)
                throw new ArgumentNullException("pizzaID");

            CustomerName = customerName;
            CustomerPhone = CustomerPhone;
            PizzaID = pizzaID;
        }
    }
}
