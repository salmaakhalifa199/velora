using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using velora.services.Services.SkinPrediction.Dto;

namespace velora.services.Services.SkinPrediction
{
    public class SkinLesionDetectionService : ISkinLesionDetectionService
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<SkinLesionDetectionService> _logger;
        private readonly FlaskApiSettings _flaskSettings;

        public SkinLesionDetectionService(
            HttpClient httpClient,
            ILogger<SkinLesionDetectionService> logger,
            IOptions<FlaskApiSettings> flaskApiOptions)
        {
            _httpClient = httpClient;
            _logger = logger;
            _flaskSettings = flaskApiOptions.Value;
        }

        public async Task<SkinLesionResultDto> PredictSkinLesionAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent();
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var fileContent = new ByteArrayContent(ms.ToArray());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync($"{_flaskSettings.BaseUrl}/predict", content);

            var rawContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Flask API raw response: {Raw}", rawContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Flask API returned error: {StatusCode} - {Content}", response.StatusCode, rawContent);
                throw new ApplicationException("Flask model failed to process image.");
            }

            // ✅ DESERIALIZE JSON from Flask
            var prediction = await JsonSerializer.DeserializeAsync<SkinLesionPredictionDto>(
                await response.Content.ReadAsStreamAsync());

            if (string.IsNullOrWhiteSpace(prediction?.Class))
                throw new ApplicationException("Unknown disease class returned by model.");

            // ✅ Get disease info for the predicted class
            var info = SkinDiseaseInfoProvider.GetDiseaseInfo(prediction.Class);

            if (info == null)
                throw new ApplicationException($"No disease info found for class '{prediction.Class}'.");

            return info;
        }
    }
}
