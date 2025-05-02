using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.repository.Cart.Interface;
using velora.repository.Cart.Models;
using velora.services.Services.CartService.Dto;

namespace velora.services.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddItemToCartAsync(string cartId, CartItemDto itemDto)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null) return false;

            var item = _mapper.Map<CartItem>(itemDto);
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.CartItems.Add(item);
            }

            await _cartRepository.UpdateCartAsync(cart);
            return true;
        }
        public async Task<bool> ClearCartAsync(string cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null) return false;

            cart.CartItems.Clear();
            var updatedCart = await _cartRepository.UpdateCartAsync(cart);
            return updatedCart != null;
        }

        public async Task<bool> DeleteCartAsync(string cartId)
        {
            return await _cartRepository.DeleteCartAsync(cartId);
        }
        public async Task AddAsync(CustomerCartDto cartDto)
        {
            var cart = _mapper.Map<CustomerCart>(cartDto);
            await _cartRepository.AddAsync(cart);
        }

        public async Task<CustomerCartDto> GetCartAsync(string cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart == null)
                return new CustomerCartDto();

            var mappedBaskets = _mapper.Map<CustomerCartDto>(cart);
            return mappedBaskets;
        }

        public async Task<bool> RemoveItemFromCartAsync(string cartId, string productId)
        {
            if (!int.TryParse(productId, out int parsedProductId))
                throw new ArgumentException("Invalid product ID format", nameof(productId));

            var cart = await _cartRepository.GetCartAsync(cartId);
            if (cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(i => i.ProductId == parsedProductId);
                if (item != null)
                {
                    cart.CartItems.Remove(item);
                    await _cartRepository.UpdateCartAsync(cart);
                    return true;
                }
            }

            return false;
        }

        public async Task<CustomerCartDto> UpdateCartAsync(CustomerCartDto cartDto)
        {
            if (cartDto.Id is null) //generate baseKey id
                cartDto.Id = GenerateRandomCartId();

            var cart = _mapper.Map<CustomerCart>(cartDto);
            var updatedCart = await _cartRepository.UpdateCartAsync(cart);
            if (updatedCart == null) return null;

            return _mapper.Map<CustomerCartDto>(updatedCart);
        }

        private string GenerateRandomCartId()
        {
            Random random = new Random();
            int randomDigits = random.Next(1000, 10000);

            return $"BS-{randomDigits}";

        }
    }
}
