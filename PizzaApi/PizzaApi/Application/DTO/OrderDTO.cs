
namespace PizzaApi.Application.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public int? EstimatedTime { get; set; }

        public string StatusDescription { get; set; }

        public string RejectedReasonPhrase { get; set; }

        public int PizzaID { get; set; }
    }
}
