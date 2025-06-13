using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using velora.api.Extensions;
using velora.core.Entities.OrderEntities;
using velora.services.HandlerResponses;
using velora.services.Services.OrderService;
using velora.services.Services.OrderService.Dto;

namespace velora.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderSController : APIBaseController
    {
        private readonly IOrderService _orderService;

        public OrderSController(IOrderService orderService )
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderDto orderDto)
        {
            orderDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.CreateOrderAsync(orderDto);

            if (order is null)
                return BadRequest(new ApiResponse<string>(null, false, 400, "Error while creating your order"));
            return Ok(order);
        }

        [HttpGet("myorders")]
        public async Task<IActionResult> GetOrdersForUser()
        {
            var email = User?.RetrieveEmailFromPrincipal();
            var orders = await _orderService.GetAllOrdersForUserAsync(email);

            if (orders == null || !orders.Any())
                return NoContent(); // 204 No Content

            return Ok(orders);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdAsync(id , email);

            return Ok(order);

        }

        [HttpPut("{orderId}/status")]
        public async Task<ActionResult> UpdateOrderStatus(Guid orderId, [FromBody] string status)
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                return BadRequest("Invalid status value");

            var updatedStatus = await _orderService.UpdateOrderStatusAsync(orderId, parsedStatus);
            if (updatedStatus == null)
                return NotFound();

            return Ok(updatedStatus.ToString());
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethods>>> GetAllDeliveryMethodsAsync()
         => Ok(await _orderService.GetAllDeliveryMethodsAsync());
    }
}
