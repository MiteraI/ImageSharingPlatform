using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;

namespace ImageSharingPlatform.Pages.AdminPages.ImageCategoryMng
{
    public class EditModel : PageModel
    {
        private readonly IImageCategoryService _imageCategoryService;

        public EditModel(IImageCategoryService imageCategoryService)
        {
            _imageCategoryService = imageCategoryService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var sharedimage = _imageCategoryService.EditImageCategory(ImageCategory);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _imageCategoryService.ImageCategoryExistsAsync(x => x.Id == ImageCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
