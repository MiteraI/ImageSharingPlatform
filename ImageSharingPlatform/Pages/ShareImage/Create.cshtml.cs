using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.ShareImage
{
    public class CreateModel : PageModel
    {
        private readonly ISharedImageRepository _sharedImageRepository;
        // Assuming you have other services or repositories to fetch Users and ImageCategories
        private readonly IUserRepository _userRepository;
        private readonly IImageCategoryRepository _imageCategoryRepository;

        public CreateModel(ISharedImageRepository sharedImageRepository, IUserRepository userRepository, IImageCategoryRepository imageCategoryRepository)
        {
            _sharedImageRepository = sharedImageRepository;
            _userRepository = userRepository;
            _imageCategoryRepository = imageCategoryRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var users = await _userRepository.GetAllAsync();
            var categories = await _imageCategoryRepository.GetAllAsync();

            ViewData["ArtistId"] = new SelectList(users, "Id", "Name"); // Assuming "Name" is a property you want to display
            ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "Name");

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

            _sharedImageRepository.Add(SharedImage);
            await _sharedImageRepository.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
