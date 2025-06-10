using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using velora.core.Data;
using velora.repository.Specifications.ProductSpecs;
using velora.services.Services.AdminService;
using velora.services.Services.AuthService.Dto;
using velora.services.Services.ProductService;
using velora.services.Services.ProductService.Dto;

namespace velora.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminController : APIBaseController
    {
        private readonly IAdminService _adminService;
        private readonly IProductService _productService;

        public AdminController(IAdminService adminService, IProductService productService)
        {
            _adminService = adminService;
            _productService = productService;
        }


        #region Products
        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecification filters)
        {
            var products = await _productService.GetAllProductsAsync(filters);
            var count = await _productService.GetTotalCountAsync(filters);

            var response = new
            {
                pageIndex = filters.PageIndex,
                pageSize = filters.PageSize,
                count,
                data = products
            };

            return Ok(response);
        }

        [HttpGet("get-product/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }

            var product = await _productService.CreateProductAsync(productDto);

            if (product == null)
            {
                return BadRequest("Error creating product.");
            }

            return Ok(new { Message = "Product created successfully.", Product = product });
        }

        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }

            var product = await _productService.UpdateProductAsync(id, productDto);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(new { Message = "Product updated successfully.", Product = product });
        }

        [HttpPut("update-product-stock/{id}")]
        public async Task<IActionResult> UpdateProductStock(int id, [FromBody] int stockQuantity)
        {
            var success = await _productService.UpdateProductStockAsync(id, stockQuantity);

            if (!success)
            {
                return NotFound("Product not found.");
            }

            return Ok(new { Message = "Product stock updated successfully." });
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);

            if (!success)
            {
                return NotFound("Product not found.");
            }

            return Ok(new { Message = "Product deleted successfully." });
        }
        #endregion

        #region Sales
        [HttpGet("sales/total")]
        public async Task<IActionResult> GetTotalSales()
        {
            var result = await _adminService.GetTotalSalesAsync();
            return Ok(result);
        }

        [HttpGet("sales/orders/count")]
        public async Task<IActionResult> GetTotalOrders()
        {
            var result = await _adminService.GetTotalOrdersAsync();
            return Ok(result);
        }

        [HttpGet("sales/recent-orders")]
        public async Task<IActionResult> GetRecentOrders()
        {
            var result = await _adminService.GetRecentOrdersAsync();
            return Ok(result);
        }

        [HttpGet("sales/top-products")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            var result = await _adminService.GetTopSellingProductsAsync();
            return Ok(result);
        }
        #endregion



        [HttpGet("dashboard")]
        public IActionResult AdminOnly()
        {
            return Ok("Hello Admin 👋 — this is your dashboard");
        }

		[HttpGet("users/count")]
		public async Task<IActionResult> GetUsersCount()
		{
			var count = await _adminService.GetUsersCountAsync();
			return Ok(new { UsersCount = count });
		}
	}
}
