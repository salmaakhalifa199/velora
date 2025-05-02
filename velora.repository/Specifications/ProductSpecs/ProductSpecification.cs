using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.repository.Specifications.ProductSpecs
{
    public class ProductSpecification
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string? Category { get; set; }
        public bool? IsBestSeller { get; set; }
        public bool? IsNewArrival { get; set; }
        public string? Sort { get; set; }

        public int PageIndex { get; set; } = 1;

        public const int MaxPageSize = 50;


        public int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? int.MaxValue : value;
        }

        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }

        private string? _concern;
        public string? Concern
        {
            get => _concern;
            set => _concern = value?.Trim().ToLower();
        }

        private string? _skinType;
        public string? SkinType
        {
            get => _skinType;
            set => _skinType = value?.Trim().ToLower();
        }

    }
}
