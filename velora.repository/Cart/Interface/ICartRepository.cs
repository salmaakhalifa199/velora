using velora.repository.Cart.Models;

namespace velora.repository.Cart.Interface
{
    public interface ICartRepository
    {
        Task<CustomerCart> GetCartAsync(string CartId);
        Task<CustomerCart> UpdateCartAsync(CustomerCart cart);
        Task<bool> DeleteCartAsync(string cartId);
        Task AddAsync(CustomerCart cart);
    }
}