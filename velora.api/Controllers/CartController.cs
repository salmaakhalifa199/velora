using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using velora.services.Services.CartService;
using velora.services.Services.CartService.Dto;

namespace velora.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartController : APIBaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<CustomerCartDto>> GetCart(string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart == null) return NotFound("Cart not found.");
            return Ok(cart);
        }

        [HttpPost("Update-Cart")]
        public async Task<ActionResult<CustomerCartDto>> UpdateCart([FromBody] CustomerCartDto cartDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedCart = await _cartService.UpdateCartAsync(cartDto);

            if (updatedCart == null) return BadRequest("Failed to update cart.");
            return Ok(updatedCart);
        }



        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCart(string cartId)
        {
            var deleted = await _cartService.DeleteCartAsync(cartId);
            if (!deleted) return NotFound("Cart not found.");
            return Ok("Cart deleted successfully.");
        }

        [HttpPost("Add-To-Cart/{cartId}")]
        public async Task<ActionResult> AddItem(string cartId, [FromBody] CartItemDto item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var added = await _cartService.AddItemToCartAsync(cartId, item);
            if (!added) return BadRequest("Failed to add item.");
            return Ok("Item added successfully.");
        }

        [HttpDelete("Remove-From-Cart/{productId}")]
        public async Task<ActionResult> RemoveItem(string cartId, string productId)
        {
            var removed = await _cartService.RemoveItemFromCartAsync(cartId, productId);
            if (!removed) return NotFound("Item not found.");
            return Ok("Item removed successfully.");
        }

        [HttpDelete("clear/{cartId}")]
        public async Task<ActionResult> ClearCart(string cartId)
        {
            var cleared = await _cartService.ClearCartAsync(cartId);
            if (!cleared) return BadRequest("Failed to clear cart.");
            return Ok("Cart cleared successfully.");
        }
    }
}
