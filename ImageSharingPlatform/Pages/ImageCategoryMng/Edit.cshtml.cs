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

namespace ImageSharingPlatform.Pages.ImageCategoryMng
{
    public class EditModel : PageModel
    {
        private readonly IImageCategoryRepository _imageCategoryRepository;

        public EditModel(IImageCategoryRepository imageCategoryRepository)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _imageCategoryRepository.Update(ImageCategory);
                await _imageCategoryRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _imageCategoryRepository.Exists(x => x.Id == ImageCategory.Id))
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
