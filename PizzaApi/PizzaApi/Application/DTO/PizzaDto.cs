using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.DTO
{
    public class PizzaDTO
    {
        public int PizzaID { get; set; }

        public string Name { get; set; }

        public string Ingredients { get; set; }
    }
}
