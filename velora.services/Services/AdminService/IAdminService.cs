using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities.IdentityEntities;
using velora.core.Entities.OrderEntities;
using velora.repository.Specifications.ProductSpecs;
using velora.services.HandlerResponses;
using velora.services.Services.AdminService.Dto;
using velora.services.Services.AuthService.Dto;
using velora.services.Services.ProductService.Dto;

namespace velora.services.Services.AdminService
{
    public interface IAdminService
    {
        Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto);
        Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto);
        Task<bool> UpdateProductStockAsync(int productId, int stockQuantity);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetAllProductsAsync(ProductSpecification specParams);

        Task<decimal> GetTotalSalesAsync();
        Task<int> GetTotalOrdersAsync();
        Task<IReadOnlyList<OrderSummaryDto>> GetRecentOrdersAsync();
        Task<List<ProductSalesDto>> GetTopSellingProductsAsync();
		Task<int> GetUsersCountAsync();

		//Task<IEnumerable<UserManagementDto>> GetAllUsersAsync();
		//Task<UserManagementDto> GetUserByIdAsync(string userId);
		//Task UpdateUserRoleAsync(string userId, string newRole);
		//Task DeactivateUserAsync(string userId);
	}
}
