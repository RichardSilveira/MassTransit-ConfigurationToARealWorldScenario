using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Domain
{
    public class Pizza
    {
        public Guid PizzaID { get; protected set; }

        public string Name { get; protected set; }

        public string Ingredients { get; set; }

        public Pizza(string name, string ingredients)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(ingredients))
                throw new ArgumentNullException("ingredients");

            Name = name;
            Ingredients = ingredients;
        }
    }
}