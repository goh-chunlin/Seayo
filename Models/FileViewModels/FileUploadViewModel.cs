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
		[Display(Name="Image")]
        public IFormFile File { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png")]
		public string FileName
		{
			get
			{
				if (File != null)
					return File.FileName;
				
				return "";
			}
		}
    }
}