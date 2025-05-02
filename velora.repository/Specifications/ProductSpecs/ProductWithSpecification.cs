using Microsoft.EntityFrameworkCore;
using velora.core.Data;
using velora.core.Entities;

namespace velora.repository.Specifications.ProductSpecs
{
    public class ProductWithSpecification : BaseSpecifications<Product>
    {
        public ProductWithSpecification(ProductSpecification specs, bool isForQuiz = false)
            : base(product =>
                (!specs.BrandId.HasValue || product.ProductBrandId == specs.BrandId.Value) &&
                (!specs.CategoryId.HasValue || product.ProductCategoryId == specs.CategoryId.Value) &&
                (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search)) &&
                (string.IsNullOrEmpty(specs.Concern) || product.Concern.ToLower().Contains(specs.Concern.ToLower())) &&
                (string.IsNullOrEmpty(specs.SkinType) || product.SkinType.ToLower().Contains(specs.SkinType.ToLower())) &&
                (!specs.IsBestSeller.HasValue || (product.IsBestSeller || product.SalesCount > 100)) &&
                (!specs.IsNewArrival.HasValue || EF.Functions.DateDiffDay(product.CreatedAt, DateTime.UtcNow) <= 30) &&
                (string.IsNullOrEmpty(specs.Category) || product.ProductCategory.Name.ToLower() == specs.Category.ToLower())

            )
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductCategory);

            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort.ToLower())
                {
                    case "price":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDes(p => p.Price);
                        break;
                    case "stock":
                        AddOrderBy(p => p.StockQuantity);
                        break;
                    case "stockdesc":
                        AddOrderByDes(p => p.StockQuantity);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name); 
            }

            if (!isForQuiz)
            {
                AddOrderBy(x => x.Name);
                ApplyPagination(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);
            }
        }

        public ProductWithSpecification(int? id) : base(product => product.Id == id)
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductCategory);
        }
    }
}
