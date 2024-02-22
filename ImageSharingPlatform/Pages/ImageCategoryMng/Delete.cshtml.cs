using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.ImageCategoryMng
{
    public class DeleteModel : PageModel
    {
        private readonly IImageCategoryRepository _imageCategoryRepository;

        public DeleteModel(IImageCategoryRepository imageCategoryRepository)
        {
            _imageCategoryRepository = imageCategoryRepository;
        }

        [BindProperty]
        public ImageCategory ImageCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImageCategory = await _imageCategoryRepository.GetOneAsync(id);

            if (ImageCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImageCategory = await _imageCategoryRepository.GetOneAsync(id);

            if (ImageCategory != null)
            {
                await _imageCategoryRepository.DeleteAsync(ImageCategory);
                await _imageCategoryRepository.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
