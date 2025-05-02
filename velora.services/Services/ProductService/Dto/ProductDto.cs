using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.ProductService.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Concern { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductBrand { get; set; }
        public string ProductCategory { get; set; }
        public string SkinType { get; set; }
        public int SalesCount { get; set; }
        public bool IsBestSeller { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StockQuantity { get; set; }
    }
}
