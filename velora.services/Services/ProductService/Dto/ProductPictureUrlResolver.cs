using AutoMapper;
using Microsoft.Extensions.Configuration;
using velora.core.Data;
using velora.core.Entities;
using velora.services.Services.ProductService.Dto;

namespace Store.services.Services.ProductService.Dto
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _configuration;
        public ProductPictureUrlResolver(IConfiguration configuration )
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["BaseUrl"]}{source.PictureUrl}";
            }
            return null;
        }
    }
}
