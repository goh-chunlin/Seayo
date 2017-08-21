using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Seayo.Models;

namespace Seayo.Services
{
    public class FileProcessor : IFileProcessor
    {
		public async Task<FileUpload> InsertFileRecordToDatabaseWithImageUploadingAsync(
			IFormFile file, string storageConnectionString, string containerName, ApplicationUser user)
		{
			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);

				string azureStorageUrl = await UploadFileToAzureStorageAsync(
					storageConnectionString, 
					containerName,
					memoryStream.ToArray(),
					file.FileName);

				string fileName = file.FileName.ToLower();
				var fileType = FileType.UNKNOWN;

				if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg")) fileType = FileType.JPG;
				if (fileName.EndsWith(".png")) fileType = FileType.PNG;

				var newFileUpload = new FileUpload()
				{
					Url = azureStorageUrl,
					Type = fileType,
					CreatedBy = user.UserName,
					CreatedAt = DateTime.Now
				};
				
				return newFileUpload;
			}
		}

		private async Task<string> UploadFileToAzureStorageAsync(string storageConnectionString, string containerName, byte[] fileData, string fileName)
		{
			var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

			var blobClient = storageAccount.CreateCloudBlobClient();
                blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

			var blobContainer = blobClient.GetContainerReference(containerName);

			var fileBlob = blobContainer.GetBlockBlobReference(fileName);

            await fileBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);

            return fileBlob.Uri.ToString();
		}
	}
}