using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.services.Services.SkinPrediction.Dto;

namespace velora.services.Services.SkinPrediction
{
    public static class SkinDiseaseInfoProvider
    {
        private static readonly Dictionary<string, SkinLesionResultDto> _diseaseInfo = new()
        {
            ["mel"] = new SkinLesionResultDto
            {
                Class = "mel",
                Label = "Melanoma",
                Description = "A serious form of skin cancer that requires urgent care.",
                NextSteps = "Consult a dermatologist immediately."
            },
            ["nv"] = new SkinLesionResultDto
            {
                Class = "nv",
                Label = "Melanocytic Nevus",
                Description = "A common mole. Usually benign.",
                NextSteps = "Monitor for changes in size, shape, or color."
            },
            ["bcc"] = new SkinLesionResultDto
            {
                Class = "bcc",
                Label = "Basal Cell Carcinoma",
                Description = "A common type of skin cancer that grows slowly.",
                NextSteps = "Requires dermatological treatment."
            },
            ["df"] = new SkinLesionResultDto
            {
                Class = "df",
                Label = "Dermatofibroma",
                Description = "A benign skin nodule.",
                NextSteps = "No treatment needed unless symptomatic."
            },
            ["bkl"] = new SkinLesionResultDto
            {
                Class = "bkl",
                Label = "Benign Keratosis",
                Description = "Non-cancerous skin growths like seborrheic keratosis.",
                NextSteps = "Can be removed cosmetically if needed."
            },
            ["akiec"] = new SkinLesionResultDto
            {
                Class = "akiec",
                Label = "Actinic Keratoses",
                Description = "Precancerous lesions caused by sun exposure.",
                NextSteps = "Should be evaluated and treated to prevent cancer."
            },
            ["vas"] = new SkinLesionResultDto
            {
                Class = "vas",
                Label = "Vascular Lesions",
                Description = "Includes hemangiomas and angiomas, typically benign.",
                NextSteps = "Treatment only if symptomatic or cosmetic."
            }
        };

        public static SkinLesionResultDto? GetDiseaseInfo(string? classKey)
        {
            if (string.IsNullOrWhiteSpace(classKey))
                return null;

            return _diseaseInfo.TryGetValue(classKey.ToLower(), out var info) ? info : null;
        }
    }
}
