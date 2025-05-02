using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.Quiz.Dto
{
    public class ProductRecommendationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<string> MatchingReasons { get; set; }
    }

}
