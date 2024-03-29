﻿using Azure.Storage;
using Azure.Storage.Blobs;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _avatarContainerClient;
        private readonly BlobContainerClient _sharedImageContainerClient;
        private readonly string _azureBlobStorageAccountName;
        private readonly string _azureBlobStorageKey;
        public AzureBlobService(IConfiguration configuration)
        {
            _configuration = configuration;

            //Azure Credentials
            _azureBlobStorageAccountName = configuration.GetValue<string>("AzureBlobStorageAccountName");
            _azureBlobStorageKey = configuration.GetValue<string>("AzureBlobStorageKey");
            var azureCredentials = new StorageSharedKeyCredential(_azureBlobStorageAccountName, _azureBlobStorageKey);

            var blobUri = new Uri($"https://{_azureBlobStorageAccountName}.blob.core.windows.net");
            var blobServiceClient = new BlobServiceClient(blobUri, azureCredentials);

            //Azure Container Clients
            _avatarContainerClient = blobServiceClient.GetBlobContainerClient("avatar");
            _sharedImageContainerClient = blobServiceClient.GetBlobContainerClient("shared-image");
        }

        public async Task<string> UploadAvatar(IFormFile image)
        {
            // Create blob client from file name from IFormFile image with guid
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var blobClient = _avatarContainerClient.GetBlobClient(fileName);

            using (var stream = image.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    await blobClient.UploadAsync(new MemoryStream(imageBytes));
                }
            }
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            // Create blob client from file name from IFormFile image with guid
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var blobClient = _sharedImageContainerClient.GetBlobClient(fileName);

            using (var stream = image.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    await blobClient.UploadAsync(new MemoryStream(imageBytes));
                }
            }
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<IList<string>> UploadImages(IList<IFormFile> image)
        {
            var imageUrls = new List<string>();
            if (image != null && image.Count > 0)
            {
                foreach (var img in image)
                {
                    imageUrls.Add(await UploadImage(img));
                }
            }
            return imageUrls;
        }
    }
}
