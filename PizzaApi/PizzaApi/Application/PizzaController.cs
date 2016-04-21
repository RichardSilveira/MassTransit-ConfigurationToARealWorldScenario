using PizzaApi.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Omu.ValueInjecter;
using PizzaApi.Application.DTO;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using PizzaApi.DAL;
using System.Data.Entity;

namespace PizzaApi.Application
{
    public class PizzaController : ApiController
    {
        private OrderContext _context;
        private DbSet<Pizza> _dbSet;
        private string _baseUri;

        public PizzaController()
        {
            _context = new OrderContext();
            _dbSet = _context.Set<Pizza>();

            _baseUri = ConfigurationManager.AppSettings["Root"] + @"/api/pizza/";
        }

        /// <summary>
        /// Retrieve all pizzas
        /// </summary>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(PizzaDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var pizzas = await _dbSet.ToListAsync();

            var pizzasDTO = pizzas.Select(p => new PizzaDTO().InjectFrom(p));

            if (!pizzasDTO.Any())
                return NotFound();

            return Ok(pizzasDTO);
        }

        /// <summary>
        /// Fetch a pizza
        /// </summary>
        /// <param name="id">Pizza identifier</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(PizzaDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var pizza = await _dbSet.Where(p => p.PizzaID == id).SingleOrDefaultAsync();

            if (pizza == null)
                return NotFound();

            var pizzaDTO = new PizzaDTO().InjectFrom(pizza);

            return Ok(pizzaDTO);
        }

        /// <summary>
        /// Create new Pizza
        /// </summary>
        /// <param name="pizzaDTO">Pizza</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Created, type: typeof(PizzaDTO))]
        [HttpPost]
        public async Task<IHttpActionResult> Create(PizzaDTO pizzaDTO)
        {
            try
            {
                var pizza = new Pizza(name: pizzaDTO.Name, ingredients: pizzaDTO.Ingredients);

                _dbSet.Add(pizza);
                pizzaDTO.PizzaID = await _context.SaveChangesAsync();

                return Created(new Uri(_baseUri + pizzaDTO.PizzaID), pizzaDTO);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}
