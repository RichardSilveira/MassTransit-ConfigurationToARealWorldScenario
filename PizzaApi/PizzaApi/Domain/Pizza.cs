using System;
using System.ComponentModel.DataAnnotations;

namespace PizzaApi.Domain
{
    public class Pizza
    {
        [Key]
        public int PizzaID { get; protected set; }

        [Required]
        public string Name { get; protected set; }

        [Required]
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

        public Pizza()
        {
            //For EF Only
        }
    }
}