using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.SkinPrediction.Dto
{
    public class SkinLesionResultDto
    {
        public string Class { get; set; }       
        public string Label { get; set; }       
        public string Description { get; set; }  
        public string NextSteps { get; set; }
    }
}
