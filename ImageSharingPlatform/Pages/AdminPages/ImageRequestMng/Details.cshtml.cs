using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Pages.AdminPages.ImageRequestMng
{
    public class DetailsModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;

        public DetailsModel(IImageRequestService imageRequestService)
        {
            _imageRequestService = imageRequestService;
        }

        public ImageRequest ImageRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(id);
            if (ImageRequest == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
