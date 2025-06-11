using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.services.Services.SkinPrediction.Dto;

namespace velora.services.Services.SkinPrediction
{
    public interface ISkinLesionDetectionService
    {
        Task<SkinLesionResultDto> PredictSkinLesionAsync(IFormFile file);
    }
}
