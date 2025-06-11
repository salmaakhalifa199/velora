using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using velora.services.Services.SkinPrediction.Dto;
using velora.services.Services.SkinPrediction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace velora.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User")]
    public class SkinLesionController : APIBaseController
    {
        private readonly ISkinLesionDetectionService _skinLesionDetectionService;

        public SkinLesionController(ISkinLesionDetectionService skinLesionDetectionService)
        {
            _skinLesionDetectionService = skinLesionDetectionService;
        }

        /// <summary>
        /// Predicts skin disease based on the uploaded image.
        /// </summary>
        /// <param name="file">Image file of the skin lesion</param>
        /// <returns>Disease info including class, label, description, and next steps</returns>
        [HttpPost("predict")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<SkinLesionResultDto>> Predict([FromForm] FileUploadDto input)
        {
            if (input.File == null || input.File.Length == 0)
                return BadRequest("Image file is required.");

            try
            {
                var result = await _skinLesionDetectionService.PredictSkinLesionAsync(input.File);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    statusCode = 500,
                    success = false
                });
            }
        }
    }
}
