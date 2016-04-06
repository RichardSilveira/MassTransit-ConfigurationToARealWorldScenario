using System;
using System.ComponentModel.DataAnnotations;

namespace PizzaApi.Domain
{
    public class Order
    {
        [Key]
        public int OrderID { get; protected set; }

        [Required]
        public string CustomerName { get; protected set; }

        [Required]
        public string CustomerPhone { get; protected set; }

        public int? SenhaEspera { get; protected set; }//TODO:review

        public decimal? EstimatedTime { get; protected set; }

        [Required]
        public int Status { get; protected set; }

        [Required]
        public int PizzaID { get; protected set; }

        public Order(string customerName, string customerPhone, int pizzaID)
        {
            if (string.IsNullOrEmpty(customerName))
                throw new ArgumentNullException("customerName");

            if (string.IsNullOrEmpty(customerPhone))
                throw new ArgumentNullException("customerPhone");

            CustomerName = customerName;
            CustomerPhone = CustomerPhone;
            PizzaID = pizzaID;

            Status = (int)OrderStatus.WaitingAttendance;
        }

        public Order()
        {
            //For EF Only
        }
    }
}
