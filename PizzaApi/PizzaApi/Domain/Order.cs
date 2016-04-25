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

        public int? EstimatedTime { get; protected set; }

        [Required]
        public int Status { get; protected set; }

        public string RejectedReasonPhrase { get; set; }

        [Required]
        public int PizzaID { get; protected set; }

        [Required]
        public Guid CorrelationId { get; set; }

        public Order(string customerName, string customerPhone, int pizzaID, Guid correlationId)
        {
            if (string.IsNullOrEmpty(customerName))
                throw new ArgumentNullException("customerName");

            if (string.IsNullOrEmpty(customerPhone))
                throw new ArgumentNullException("customerPhone");

            CustomerName = customerName;
            CustomerPhone = customerPhone;
            PizzaID = pizzaID;

            CorrelationId = correlationId;

            Status = (int)OrderStatus.WaitingAttendance;
        }

        public Order()
        {
            //For EF Only
        }

        public void Approve(int estimatedTimeInMinutes)
        {
            Status = (int)OrderStatus.Approved;
            EstimatedTime = estimatedTimeInMinutes;
        }

        public void Reject(string reasonPhrase)
        {
            Status = (int)OrderStatus.Rejected;
            RejectedReasonPhrase = reasonPhrase;
        }

        public void Close()
        {
            Status = (int)OrderStatus.Closed;
            EstimatedTime = null;
        }
    }
}
