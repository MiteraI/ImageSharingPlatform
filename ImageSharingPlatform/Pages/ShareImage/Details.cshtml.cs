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
