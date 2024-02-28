using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
{
    public class CreateModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
        // Assuming you have other services or repositories to fetch Users and ImageCategories
        private readonly IUserService _userService;
        private readonly IImageCategoryService _imageCategoryService;

        public CreateModel(ISharedImageService sharedImageService, IUserService userService, IImageCategoryService imageCategoryService)
        {
            _sharedImageService = sharedImageService;
            _userService = userService;
            _imageCategoryService = imageCategoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var users = await _userService.GetAllUsersAsync();
            var categories = await _imageCategoryService.GetAllImageCategoriesAsync();

            ViewData["ArtistId"] = new SelectList(users, "Id", "Username"); // Assuming "Name" is a property you want to display
            ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "CategoryName");

            return Page();
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var sharedimage = await _sharedImageService.CreateSharedImage(SharedImage);

            if (sharedimage == null)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
