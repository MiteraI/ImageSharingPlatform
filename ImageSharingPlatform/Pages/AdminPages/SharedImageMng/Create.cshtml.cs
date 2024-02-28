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

<<<<<<<< HEAD:ImageSharingPlatform/Pages/AdminPages/ShareImage/Create.cshtml.cs
namespace ImageSharingPlatform.Pages.AdminPages.ShareImage
========
namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
>>>>>>>> 389bcac198f0998964858186489bdd87c822de48:ImageSharingPlatform/Pages/AdminPages/SharedImageMng/Create.cshtml.cs
{
    public class CreateModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
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

<<<<<<<< HEAD:ImageSharingPlatform/Pages/AdminPages/ShareImage/Create.cshtml.cs
            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "Name");
========
            ViewData["ArtistId"] = new SelectList(users, "Id", "Username"); // Assuming "Name" is a property you want to display
            ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "CategoryName");
>>>>>>>> 389bcac198f0998964858186489bdd87c822de48:ImageSharingPlatform/Pages/AdminPages/SharedImageMng/Create.cshtml.cs

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
