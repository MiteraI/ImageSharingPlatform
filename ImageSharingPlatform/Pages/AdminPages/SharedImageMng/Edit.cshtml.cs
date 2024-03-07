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

namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
{
    public class EditModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
        private readonly IUserService _userService;
        private readonly IImageCategoryService _imageCategoryService;

        public EditModel(ISharedImageService sharedImageService, IUserService userService, IImageCategoryService imageCategoryService)
        {
            _sharedImageService = sharedImageService;
            _userService = userService;
            _imageCategoryService = imageCategoryService;
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var users = await _userService.GetAllUsersAsync();
            var categories = await _imageCategoryService.GetAllImageCategoriesAsync();

            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "CategoryName");

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var sharedimage = _sharedImageService.EditSharedImage(SharedImage);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _sharedImageService.SharedImageExistsAsync(x => x.Id == SharedImage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["SuccessMessage"] = "Image is edited successfully!";
            return RedirectToPage("./Index");
        }
    }
}
