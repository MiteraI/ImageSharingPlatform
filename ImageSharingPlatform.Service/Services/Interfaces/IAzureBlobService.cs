﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface IAzureBlobService
    {
        Task<string> UploadAvatar(IFormFile image);
        Task<string> UploadImage(IFormFile image);
        Task<IList<string>> UploadImages(IList<IFormFile> image);
    }
}
