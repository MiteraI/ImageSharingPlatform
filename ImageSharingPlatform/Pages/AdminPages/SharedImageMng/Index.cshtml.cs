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

<<<<<<<< HEAD:ImageSharingPlatform/Pages/AdminPages/ShareImage/Index.cshtml.cs
namespace ImageSharingPlatform.Pages.AdminPages.ShareImage
========
namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
>>>>>>>> 389bcac198f0998964858186489bdd87c822de48:ImageSharingPlatform/Pages/AdminPages/SharedImageMng/Index.cshtml.cs
{
    public class IndexModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;

        public IndexModel(ISharedImageService sharedImageService)
        {
            _sharedImageService = sharedImageService;
        }

        public IList<SharedImage> SharedImage { get; set; }/* = default!;*/

        public async Task OnGetAsync()
        {
            SharedImage = _sharedImageService.GetAllSharedImagesAsync().Result.ToList();
        }
    }
}
