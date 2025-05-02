using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.repository.Specifications.ProductSpecs;
using velora.services.HandlerResponses;
using velora.services.Services.ProductService.Dto;

namespace velora.services.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync(ProductSpecification specParams);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<int> GetTotalCountAsync(ProductSpecification specParams);
        Task<bool> UpdateProductStockAsync(int productId, int stockQuantity);
        Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto);
        Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
