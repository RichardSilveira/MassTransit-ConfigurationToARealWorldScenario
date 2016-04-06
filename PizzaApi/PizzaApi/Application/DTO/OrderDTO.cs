
namespace PizzaApi.Application.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public int? SenhaEspera { get; set; }

        public decimal? EstimatedTime { get; set; }

        public int Status { get; set; }

        public int PizzaID { get; set; }
    }
}
