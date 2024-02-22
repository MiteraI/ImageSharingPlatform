using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.ImageCategoryMng
{
    public class CreateModel : PageModel
    {
        private readonly IImageCategoryRepository _imageCategoryRepository;

        public CreateModel(IImageCategoryRepository imageCategoryRepository)
        {
            _imageCategoryRepository = imageCategoryRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ImageCategory ImageCategory { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _imageCategoryRepository.Add(ImageCategory);
            await _imageCategoryRepository.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
