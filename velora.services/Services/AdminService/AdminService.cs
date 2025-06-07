using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Repository.Interfaces;
using velora.core.Entities.IdentityEntities;
using velora.core.Entities.OrderEntities;
using velora.repository.Interfaces.IdentityInterfaces;
using velora.repository.Specification.OrderSpecs;
using velora.repository.Specifications.OrderSpecs;
using velora.repository.Specifications.ProductSpecs;
using velora.services.HandlerResponses;
using velora.services.Services.AdminService.Dto;
using velora.services.Services.OrderService.Dto;
using velora.services.Services.ProductService;
using velora.services.Services.ProductService.Dto;

namespace velora.services.Services.AdminService
{
    public class AdminService : IAdminService

    {

        private readonly IUnitWork _unitWork;
        private readonly IPersonRepository _personRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(IUnitWork unitWork, IPersonRepository personRepository, UserManager<Person> userManager, RoleManager<IdentityRole> roleManager, IProductService productService, IMapper mapper)
        {
            _unitWork = unitWork;
            _personRepository = personRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _productService = productService;
            _mapper = mapper;
        }
		public async Task<int> GetUsersCountAsync()
		{
			return await _userManager.Users.CountAsync();
		}
		#region Crud Products
		public async Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto)
        {
            return await _productService.CreateProductAsync(productDto);
        }

        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            return await _productService.UpdateProductAsync(id, productDto);
        }

        public async Task<bool> UpdateProductStockAsync(int productId, int stockQuantity)
        {
            return await _productService.UpdateProductStockAsync(productId, stockQuantity);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productService.DeleteProductAsync(id);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            return await _productService.GetProductByIdAsync(id);
        }

        public async Task<List<ProductDto>> GetAllProductsAsync(ProductSpecification specParams)
        {
            return await _productService.GetAllProductsAsync(specParams);
        }
        #endregion

        #region Sales
        public async Task<decimal> GetTotalSalesAsync()
        {
            var orders = await _unitWork.Repository<Order, Guid>().GetAllAsync();
            return orders.Sum(order => order.GetTotal()); 
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            var orders = await _unitWork.Repository<Order, Guid>().GetAllAsync();
            return orders.Count;
        }

        public async Task<IReadOnlyList<OrderSummaryDto>> GetRecentOrdersAsync()
        {

            var spec = new OrderWithItemSpecification();

            var order = await _unitWork.Repository<Order, Guid>().GetAllWithSpecAsync(spec);
            if (order == null) return null;

            var orderSummaries = order.OrderByDescending(o => o.OrderDate).Take(10).Select(order => new OrderSummaryDto
            {
                OrderId = order.Id,
                BuyerEmail = order.BuyerEmail,
                TotalAmount = order.GetTotal(),
                OrderDate = order.OrderDate,
                OrderStatus = order.Status.ToString(),
                DeliveryMethod = order.DeliveryMethod.ShortName,
                OrderItems = order.OrderItems?.Select(item => new OrderItemDto
                {
                    OrderId = item.OrderId,
                    ProductId = item.ItemOrdered.ProductId,
                    ProductName = item.ItemOrdered.ProductName,
                    PictureUrl = item.ItemOrdered.PictureUrl,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();

            return orderSummaries;
        }

        public async Task<List<ProductSalesDto>> GetTopSellingProductsAsync()
        {
            var orderItems = await _unitWork.Repository<OrderItem, Guid>().GetAllAsync();

            var topSellingProducts = orderItems
                .Where(item => item.ItemOrdered?.ProductId != null)
                .GroupBy(item => item.ItemOrdered.ProductId)
                .Select(group => new ProductSalesDto
                {
                    ProductId = group.Key,
                    TotalQuantity = group.Sum(item => item.Quantity)
                })
                .OrderByDescending(product => product.TotalQuantity)
                .Take(10)
                .ToList();

            return _mapper.Map<List<ProductSalesDto>>(topSellingProducts);
        }
        #endregion

        #region vbn
        //public async Task<List<OrderSummaryDto>> GetAllOrdersAsync()
        //{
        //    var spec = new OrderWithPersonSpec();
        //    var orders = await _unitWork.Repository<Order, Guid>().GetAllWithSpecAsync(spec);
        //    return _mapper.Map<List<OrderSummaryDto>>(orders);
        //}
        //public async Task<OrderSummaryDto> GetOrderByIdAsync(Guid orderId)
        //{
        //    var spec = new OrderWithPersonByIdSpec(orderId);
        //    var order = await _unitWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(spec);
        //    return _mapper.Map<OrderSummaryDto>(order);
        //}

        //public async Task<UserManagementDto> GetUserByIdAsync(string userId)
        //{
        //    var userRepository = _unitOfWork.PersonRepository();

        //    var user = await userRepository.GetByIdAsync(userId);

        //    if (user == null)
        //    {
        //        throw new Exception($"User with ID {userId} not found.");
        //    }
        //    var userDto = _mapper.Map<UserManagementDto>(user);

        //    return userDto;
        //}


        // public async Task UpdateUserRoleAsync(string userId, string newRole)
        // {
        //     var user = await _userManager.FindByIdAsync(userId);
        //     if (user == null)
        //         throw new Exception("User not found");

        //     var currentRoles = await _userManager.GetRolesAsync(user);

        //     var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        //     if (!removeResult.Succeeded)
        //         throw new Exception("Failed to remove old roles");

        //     var addResult = await _userManager.AddToRoleAsync(user, newRole);
        //     if (!addResult.Succeeded)
        //         throw new Exception("Failed to assign new role");
        // }


        //public async Task DeactivateUserAsync(string userId)
        //{
        // var user = await _userManager.FindByIdAsync(userId);
        // if (user == null)
        //     throw new Exception("User not found");

        // user.IsActive = false;

        // var result = await _userManager.UpdateAsync(user);
        // if (!result.Succeeded)
        //     throw new Exception("Failed to deactivate user");
        //}
        #endregion


    }
}


