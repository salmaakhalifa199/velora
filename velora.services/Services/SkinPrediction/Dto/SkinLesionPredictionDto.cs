using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace velora.services.Services.SkinPrediction.Dto
{
    public class SkinLesionPredictionDto
    {
        [JsonPropertyName("prediction")]
        public string Class { get; set; } = string.Empty;
    }
}
