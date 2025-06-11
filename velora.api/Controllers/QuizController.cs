using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using velora.core.Data;
using velora.core.Entities;
using velora.repository.Specifications.ProductSpecs;
using velora.services.Services.Quiz.Dto;

namespace velora.api.Controllers
{
    public class QuizController : APIBaseController
    {

        private readonly IUnitWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuizController(IUnitWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize (AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> GetRecommendation([FromBody] TreatmentQuizDto quiz)
        {
            var allProducts = await _unitOfWork.Repository<Product, int>()
                .GetAllWithSpecAsync(new ProductWithSpecification(new ProductSpecification(), isForQuiz: true));

            var matched = allProducts
                  .Where(p =>
                    (!string.IsNullOrEmpty(quiz.SkinType) && p.SkinType.ToLower().Contains(quiz.SkinType.ToLower())) ||
                    (quiz.IsAcneProne && p.Concern.ToLower().Contains("acne")) ||
                    quiz.PrimaryConcerns.Any(c => p.Concern.ToLower().Contains(c.ToLower()))
                  )
                 .Take(10)
                 .ToList();

            var results = matched.Select(p =>
            {
                var reasons = new List<string>();

                if (!string.IsNullOrEmpty(p.Concern))
                {
                    reasons.AddRange(
                        quiz.PrimaryConcerns
                            .Where(c => p.Concern.ToLower().Contains(c.ToLower()))
                    );
                }

                if (quiz.IsAcneProne && p.Concern?.ToLower().Contains("acne") == true)
                {
                    reasons.Add("Acne-prone");
                }

                
                if (!string.IsNullOrEmpty(quiz.SkinType) &&
                    !string.IsNullOrEmpty(p.SkinType) &&
                    p.SkinType.ToLower().Contains(quiz.SkinType.ToLower()))
                {
                    reasons.Add($"For {quiz.SkinType} skin");
                }

                return new ProductRecommendationDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.PictureUrl,
                    MatchingReasons = reasons.Distinct().ToList()
                };
            });

            return Ok(results);
        }
    }
}
