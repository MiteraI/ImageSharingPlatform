using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Pages.ShareImage
{
    public class IndexModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;

        public IndexModel(ISharedImageService sharedImageService)
        {
            _sharedImageService = sharedImageService;
        }

        public IList<SharedImage> SharedImage { get; set; } = default!;

        public async Task OnGetAsync()
        {
            SharedImage = _sharedImageService.GetAllSharedImagesAsync().Result.ToList();
        }
    }
}
