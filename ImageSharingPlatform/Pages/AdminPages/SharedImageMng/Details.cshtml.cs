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

<<<<<<<< HEAD:ImageSharingPlatform/Pages/AdminPages/ShareImage/Details.cshtml.cs
namespace ImageSharingPlatform.Pages.AdminPages.ShareImage
========
namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
>>>>>>>> 389bcac198f0998964858186489bdd87c822de48:ImageSharingPlatform/Pages/AdminPages/SharedImageMng/Details.cshtml.cs
{
    public class DetailsModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;

        public DetailsModel(ISharedImageService sharedImageService)
        {
            _sharedImageService = sharedImageService;
        }

        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SharedImage = await _sharedImageService.GetSharedImageByIdAsync(id);

            if (SharedImage == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
