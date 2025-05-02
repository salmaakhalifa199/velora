using velora.repository.Cart.Models;
using velora.services.Services.CartService.Dto;

namespace velora.services.Services.CartService
{
    public interface ICartService
    {
        Task<CustomerCartDto> GetCartAsync(string cartId);
        Task<CustomerCartDto> UpdateCartAsync(CustomerCartDto cartDto);
        Task<bool> DeleteCartAsync(string cartId);
        Task<bool> AddItemToCartAsync(string cartId, CartItemDto itemDto);
        Task<bool> RemoveItemFromCartAsync(string cartId, string productId);
        Task<bool> ClearCartAsync(string cartId);
    }
}