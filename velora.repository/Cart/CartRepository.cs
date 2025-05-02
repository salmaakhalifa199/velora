using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using velora.repository.Cart.Interface;
using velora.repository.Cart.Models;
using IDatabase = StackExchange.Redis.IDatabase;

namespace velora.repository.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;
        public CartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteCartAsync(string cartId)
        => await _database.KeyDeleteAsync(cartId.ToString());

        public async Task<CustomerCart> GetCartAsync(string CartId)

        {
            var cart = await _database.StringGetAsync(CartId);
            return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(cart);
        }
        public async Task<CustomerCart> UpdateCartAsync(CustomerCart cart)
        {
            var isCreated = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            if (!isCreated)
                return null;

            return await GetCartAsync(cart.Id);
        }
        public async Task AddAsync(CustomerCart cart)
        {
            var isCreated = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            if (!isCreated)
                throw new Exception("Failed to add cart to Redis.");
        }
    }
}
