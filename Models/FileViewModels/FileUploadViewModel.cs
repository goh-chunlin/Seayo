using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Seayo.Models.FileViewModels
{
    public class FileUploadViewModel
    {
        [Required]
        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile File { get; set; }
    }
}