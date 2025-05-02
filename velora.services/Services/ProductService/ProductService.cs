using AutoMapper;
using Azure;
using Store.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities;
using velora.repository.Specifications.ProductSpecs;
using velora.services.HandlerResponses;
using velora.services.Services.ProductService.Dto;
using StoreResponse = velora.services.HandlerResponses.ApiResponse<velora.services.Services.ProductService.Dto.ProductDto>;

namespace velora.services.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync(ProductSpecification specParams)
        {
            var repo = _unitOfWork.Repository<Product, int>();
            var spec = new ProductWithSpecification(specParams);
            var products = await repo.GetAllWithSpecAsync(spec);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Product, int>();
            var spec = new ProductWithSpecification(id);
            var product = await repo.GetByIdWithSpecAsync(spec);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<int> GetTotalCountAsync(ProductSpecification specParams)
        {
            var repo = _unitOfWork.Repository<Product, int>();
            var countSpec = new ProductWithCountSpecification(specParams);
            return await repo.CountAsync(countSpec);
        }


        public async Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto)
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand, int>();
            var brand = await brandRepo.GetAsync(b => b.Name.ToLower() == productDto.ProductBrand.ToLower());

            if (brand == null)
            {
                brand = new ProductBrand { Name = productDto.ProductBrand };
                await brandRepo.AddAsync(brand);
                await _unitOfWork.CompleteAsync();
            }

            var CategoryRepo = _unitOfWork.Repository<ProductCategory, int>();
            var Category = await CategoryRepo.GetAsync(t => t.Name.ToLower() == productDto.ProductCategory.ToLower());

            if (Category == null)
            {
                Category = new ProductCategory { Name = productDto.ProductCategory };
                await CategoryRepo.AddAsync(Category);
                await _unitOfWork.CompleteAsync();
            }

            var product = _mapper.Map<Product>(productDto);
            product.ProductBrandId = brand.Id;
            product.ProductCategoryId = Category.Id;
            product.CreatedAt = DateTime.UtcNow;

            var productRepo = _unitOfWork.Repository<Product, int>();
            await productRepo.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return new StoreResponse(_mapper.Map<ProductDto>(product), success: true, statusCode: 201, message: "Product created successfully.");
        }



        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            var productRepo = _unitOfWork.Repository<Product, int>();
            var product = await productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return null; 
            }
            _mapper.Map(productDto, product);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateProductStockAsync(int productId, int stockQuantity)
        {
            var productRepo = _unitOfWork.Repository<Product, int>();

            var product = await productRepo.GetByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            product.StockQuantity = stockQuantity;
            await _unitOfWork.CompleteAsync();

            return true;  
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var productRepo = _unitOfWork.Repository<Product, int>();
            var product = await productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return false;  
            }
            productRepo.Delete(product);
 
            await _unitOfWork.CompleteAsync();

            return true;  
        }

    }
}
