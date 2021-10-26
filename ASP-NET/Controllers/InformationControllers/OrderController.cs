using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.InformationControllers
{
    [ApiController]
    [Route("api/orders")]
    [Produces("application/json")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("{OrderId}")]
        [SwaggerOperation(
            Summary = "Finds order by id",
            Description = "Gets order of current user by specified id",
            OperationId = "GetOrderById",
            Tags = new[] { "Search", "Order" })]
        [SwaggerResponse(200, "Returns order")]
        [SwaggerResponse(400, "No orders were found")]
        public async Task<IActionResult> GetOrderById(int OrderId)
        {
            var result = await _orderService.GetOrderInfoByIdAsync(OrderId);
            if (result == null)
                return BadRequest("There are no products in the database");
            return Ok(result);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("")]
        [SwaggerOperation(
            Summary = "Creates Order",
            OperationId = "CreateOrder",
            Tags = new[] { "Order" })]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Unable to create order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreationDto info)
        {
            info.UserId = int.Parse(HttpContext.User.Claims.First().Value);
            var result = await _orderService.CreateOrderAsync(info);
            if (result == null)
                return BadRequest("Unable to create order");
            return Ok(result);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("{OrderId}")]
        [SwaggerOperation(
            Summary = "Changes the amount of items in order list",
            OperationId = "ChangeAmount",
            Tags = new[] { "Order" })]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "There are no products in the database")]
        public async Task<IActionResult> ChangeAmount(int OrderId, [FromBody] OrderAmountChangeDto info)
        {
            info.UserId = int.Parse(HttpContext.User.Claims.First().Value);
            if (info.UserId != (await _orderService.GetOrderByIdAsync(OrderId)).UserId)
                return BadRequest("There are no such orders in the database");
            var result = await _orderService.ChangeOrderAmountAsync(OrderId, info);
            if (result == null)
                return BadRequest("There are no products in the database");
            return Ok(result);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("{OrderId}")]
        [SwaggerOperation(
            Summary = "Deletes specified items in list",
            OperationId = "DeleteItems",
            Tags = new[] { "Order" })]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "There are no such orders in the database")]
        public async Task<IActionResult> DeleteItems(int OrderId, [FromBody] OrderItemsDeletionDto info)
        {
            info.UserId = int.Parse(HttpContext.User.Claims.First().Value);
            if (info.UserId != (await _orderService.GetOrderByIdAsync(OrderId)).UserId)
                return BadRequest("There are no such orders in the database");
            var result = await _orderService.DeleteItems(OrderId, info);
            if (result == null)
                return BadRequest("There are no such orders in the database");
            return Ok(result);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("buy/{OrderId}")]
        [SwaggerOperation(
            Summary = "Buys items in order",
            OperationId = "Buy",
            Tags = new[] { "Order" })]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "There are no such orders in the database")]
        public async Task<IActionResult> Buy(int OrderId)
        {
            var userId = int.Parse(HttpContext.User.Claims.First().Value);
            var result = await _orderService.BuyAsync(OrderId, userId);
            if (result == null)
                return BadRequest("There are no such orders in the database");
            return Ok(result);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
