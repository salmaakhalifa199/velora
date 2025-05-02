using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities;

namespace velora.repository.Specifications.ProductSpecs
{
    public class ProductCategoryByNameSpec : BaseSpecifications<ProductCategory>
    {
        public ProductCategoryByNameSpec(string name)
        : base(c => c.Name.ToLower() == name.ToLower())
        {
        }
    }
}
