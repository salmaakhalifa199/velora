using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities;

namespace velora.repository.Specifications.ProductSpecs
{
    public class ProductWithCountSpecification : BaseSpecifications<Product>
    {
        public ProductWithCountSpecification(ProductSpecification specs)
         : base(product => 
                (!specs.BrandId.HasValue || product.ProductBrandId == specs.BrandId.Value) &&
                (!specs.CategoryId.HasValue || product.ProductCategoryId == specs.CategoryId.Value) &&
                (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search)) &&
                (string.IsNullOrEmpty(specs.Concern) || product.Concern.ToLower().Contains(specs.Concern.ToLower())) &&
                (string.IsNullOrEmpty(specs.SkinType) || product.SkinType.ToLower().Contains(specs.SkinType.ToLower())) &&
                (!specs.IsBestSeller.HasValue || (product.IsBestSeller || product.SalesCount > 100)) &&
                (!specs.IsNewArrival.HasValue || EF.Functions.DateDiffDay(product.CreatedAt, DateTime.UtcNow) <= 30)
         )

        { }
    }
}
