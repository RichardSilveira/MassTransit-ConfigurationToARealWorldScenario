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
using PizzaApi.MessageContracts;
using MassTransit;

namespace PizzaApi.Application
{
    public class OrderController : ApiController
    {
        private OrderContext _context;
        private DbSet<Order> _dbSet;
        private string _baseUri;

        private IBusControl _bus;
        private Uri _sendToUri;

        public OrderController()
        {
            _context = new OrderContext();
            _dbSet = _context.Set<Order>();

            _baseUri = ConfigurationManager.AppSettings["Root"] + @"/api/order/";

            _bus = BusConfigurator.ConfigureBus();

            _sendToUri = new Uri(RabbitMqConstants.RabbitMqUri + RabbitMqConstants.SagaQueue);
        }

        /// <summary>
        /// Retrieve all orders
        /// </summary>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var orders = await _dbSet.ToListAsync();

            var ordersDTO = orders.Select(order =>
            {
                var orderDTO = (OrderDTO)new OrderDTO().InjectFrom(order);
                orderDTO.StatusDescription = ((OrderStatus)order.Status).ToString();
                return orderDTO;
            });

            if (!ordersDTO.Any())
                return NotFound();

            return Ok(ordersDTO);
        }

        /// <summary>
        /// Fetch an order
        /// </summary>
        /// <param name="id">Order identifier</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var order = await _dbSet.Where(p => p.OrderID == id).SingleOrDefaultAsync();

            if (order == null)
                return NotFound();

            var orderDTO = (OrderDTO)new OrderDTO().InjectFrom(order);
            orderDTO.StatusDescription = ((OrderStatus)order.Status).ToString();

            return Ok(orderDTO);
        }

        /// <summary>
        /// Register a new order
        /// </summary>
        /// <remarks> Register a new order with 'CustomerName', 'CustomerPhone' and 'PizzaID' fields</remarks>
        /// <param name="orderDTO">Order</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderDTO))]
        [HttpPost]
        public async Task<IHttpActionResult> RegiterNew(OrderDTO orderDTO)
        {
            var order = new Order(orderDTO.CustomerName, orderDTO.CustomerPhone, orderDTO.PizzaID, Guid.NewGuid());

            _dbSet.Add(order);
            await _context.SaveChangesAsync();

            orderDTO = (OrderDTO)new OrderDTO().InjectFrom(order);
            orderDTO.StatusDescription = ((OrderStatus)order.Status).ToString();


            var endPoint = await _bus.GetSendEndpoint(_sendToUri);

            await endPoint.Send<IRegisterOrderCommand>(new
            {
                OrderID = order.OrderID,
                CustomerName = orderDTO.CustomerName,
                CustomerPhone = orderDTO.CustomerPhone,
                PizzaID = orderDTO.PizzaID,
                CorrelationId = order.CorrelationId,
                Timestamp = DateTime.UtcNow
            });

            return Created(new Uri(_baseUri + orderDTO.OrderID), orderDTO);
        }

        /// <summary>
        /// Approve an order
        /// </summary>
        /// <param name="id">Order identifier</param>
        /// <param name="estimatedTime">estimated time to order be prepared</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderDTO))]
        [HttpPost]
        [Route("~/api/order/{id:int}/approve")]
        public async Task<IHttpActionResult> Approve(int id, [FromBody] int estimatedTime)
        {
            var order = await _dbSet.Where(p => p.OrderID == id).SingleOrDefaultAsync();

            if (order == null)
                return NotFound();

            order.Approve(estimatedTime);

            _dbSet.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var orderDTO = (OrderDTO)new OrderDTO().InjectFrom(order);
            orderDTO.StatusDescription = ((OrderStatus)order.Status).ToString();

            var endPoint = await _bus.GetSendEndpoint(_sendToUri);
            await endPoint.Send<IApproveOrderCommand>(new
            {
                OrderID = order.OrderID,
                EstimatedTime = order.EstimatedTime.Value,
                Status = order.Status,
                CorrelationId = order.CorrelationId,
                Timestamp = DateTime.UtcNow
            });

            return Ok(orderDTO);
        }

        /// <summary>
        /// Close an order
        /// </summary>
        /// <param name="id">Order identifier</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [HttpPost]
        [Route("~/api/order/{id:int}/close")]
        public async Task<IHttpActionResult> Close(int id)
        {
            var order = await _dbSet.Where(p => p.OrderID == id).SingleOrDefaultAsync();

            if (order == null)
                return NotFound();

            order.Close();

            _dbSet.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var endPoint = await _bus.GetSendEndpoint(_sendToUri);

            await endPoint.Send<ICloseOrderCommand>(new
            {
                OrderID = order.OrderID,
                Status = order.Status,
                CorrelationId = order.CorrelationId,
                Timestamp = DateTime.UtcNow
            });
            
            //Just to show the ConsoleLogPublishObserver connected with this '_bus' instance.
            await _bus.Publish<IClosedOrderEvent>(new
            {
                OrderID = order.OrderID,
                Status = order.Status,
                CorrelationId = order.CorrelationId,
                Timestamp = DateTime.UtcNow
            });

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Reject an order
        /// </summary>
        /// <param name="id">Order identifier</param>
        /// <param name="reasonPhrase">Simple phrase explaining why the order was rejected</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [HttpPost]
        [Route("~/api/order/{id:int}/reject")]
        public async Task<IHttpActionResult> Reject(int id, [FromBody] string reasonPhrase)
        {
            if (string.IsNullOrEmpty(reasonPhrase))
                return BadRequest("A reason phrase should be informed.");

            var order = await _dbSet.Where(p => p.OrderID == id).SingleOrDefaultAsync();

            if (order == null)
                return NotFound();

            order.Reject(reasonPhrase);

            _dbSet.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var endPoint = await _bus.GetSendEndpoint(_sendToUri);

            await endPoint.Send<IRejectOrderCommand>(new
            {
                OrderID = order.OrderID,
                RejectedReasonPhrase = order.RejectedReasonPhrase,
                CorrelationId = order.CorrelationId,
                Timestamp = DateTime.UtcNow
            });

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
