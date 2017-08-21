using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Seayo.Models;

namespace Seayo.Services
{
    public interface IFileProcessor
    {
		Task<FileUpload> InsertFileRecordToDatabaseWithImageUploadingAsync(
			IFormFile file, string storageConnectionString, string containerName, ApplicationUser user);
    }
}
