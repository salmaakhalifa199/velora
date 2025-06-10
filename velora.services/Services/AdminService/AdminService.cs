using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Repository.Interfaces;
using velora.core.Entities;
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
            var products = await _unitWork.Repository<Product, int>().GetAllAsync();

            var topSellingProducts = orderItems
                .Where(item => item.ItemOrdered?.ProductId != null)
                .Join(products,orderItem => orderItem.ItemOrdered.ProductId,product => product.Id,
                    (orderItem, product) => new { orderItem, product })
                .GroupBy(x => new { x.product.Id, x.product.Name })
                .Select(group => new ProductSalesDto
                {
                ProductId = group.Key.Id,
                ProductName = group.Key.Name,
                TotalQuantity = group.Sum(x => x.orderItem.Quantity)
                })
                .OrderByDescending(product => product.TotalQuantity)
                .Take(10)
                .ToList();

            return topSellingProducts;


        }
        #endregion


        //public async Task<List<ProductSalesDto>> GetTopSellingProductsAsync()
        //{
        //    var orderItems = await _unitWork.Repository<OrderItem, Guid>().GetAllAsync();
        //    var products = await _unitWork.Repository<Product, int>().GetAllAsync();

        ////    var topSellingProducts = orderItems
        //        .Where(item => item.ItemOrdered?.ProductId != null)
        //        .GroupBy(item => item.ItemOrdered.ProductId)
        //        .Select(group => new ProductSalesDto
        //        {
        //            ProductId = group.Key,
        //            TotalQuantity = group.Sum(item => item.Quantity)
        //        })
        //        .OrderByDescending(product => product.TotalQuantity)
        //        .Take(10)
        //        .ToList();

        //    return _mapper.Map<List<ProductSalesDto>>(topSellingProducts);
     
        //}

    }
}


