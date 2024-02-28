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

namespace ImageSharingPlatform.Pages.AdminPages.ImageCategoryMng
{
    public class DetailsModel : PageModel
    {
        private readonly IImageCategoryService _imageCategoryService;

        public DetailsModel(IImageCategoryService imageCategoryService)
        {
            _imageCategoryService = imageCategoryService;
        }

        public ImageCategory ImageCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImageCategory = await _imageCategoryService.GetImageCategoryByIdAsync(id);

            if (ImageCategory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
