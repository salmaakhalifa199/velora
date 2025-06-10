using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.services.Services.SkinPrediction.Dto
{
    public class FileUploadDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
