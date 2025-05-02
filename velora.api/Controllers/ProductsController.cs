using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using velora.api.Helper;
using velora.repository.Specifications.ProductSpecs;
using velora.services.Services.ProductService;
using velora.services.Services.ProductService.Dto;

namespace velora.api.Controllers
{
    
    public class ProductsController : APIBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Cache(10)]
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecification filters)
        {
            var products = await _productService.GetAllProductsAsync(filters);
            var count = await _productService.GetTotalCountAsync(filters);

            var response = new
            {
                pageIndex = filters.PageIndex,
                pageSize = filters.PageSize,
                count ,
                data = products
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }
        [HttpPut("update-stock/{id}")]
        public async Task<IActionResult> UpdateProductStock(int id, [FromBody] int stockQuantity)
        {
            var success = await _productService.UpdateProductStockAsync(id, stockQuantity);

            if (!success)
            {
                return NotFound("Product not found.");
            }

            return Ok("Product stock updated successfully.");
        }
    }
}
