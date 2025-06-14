using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
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
        private readonly IUnitWork _unitWork;
        private readonly IMapper _mapper;
        public OrderSController(IOrderService orderService , IUnitWork unitWork, IMapper mapper)
        {
            _orderService = orderService;
            _unitWork = unitWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderDto orderDto)
        {
            orderDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
            try
            {
                var order = await _orderService.CreateOrderAsync(orderDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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
