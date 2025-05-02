using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.Quiz.Dto
{
    public class TreatmentQuizDto
    {
        public string SkinType { get; set; }
        public bool IsAcneProne { get; set; }
        public string DryFrequency { get; set; }
        public bool GetsOilyQuickly { get; set; }
        public List<string> PrimaryConcerns { get; set; }
        public string SensitivityLevel { get; set; }
        public string AgeGroup { get; set; }

    }
}
