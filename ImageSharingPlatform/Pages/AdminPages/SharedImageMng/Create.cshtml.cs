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
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

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

        [BindProperty]
        public SharedImage SharedImage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return Redirect("/Authentication/Login");
            }
            else
            {
                var userAccount = JsonConvert.DeserializeObject<User>(userJson);
                var isAdmin = userAccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ADMIN);

                if (isAdmin)
                {
                    var users = await _userService.GetAllUsersAsync();
                    var categories = await _imageCategoryService.GetAllImageCategoriesAsync();

                    ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
                    ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "CategoryName");
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to view this page";
                    return Redirect("/Index");
                }
            }
            return Page();
        }

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
            TempData["SuccessMessage"] = "Image is created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
