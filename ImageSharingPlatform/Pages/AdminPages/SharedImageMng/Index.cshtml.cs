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
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
{
    public class IndexModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
        private readonly IUserService _userService;
        private readonly IImageCategoryService _imageCategoryService;

        public IndexModel(ISharedImageService sharedImageService, IUserService userService, IImageCategoryService imageCategoryService)
        {
            _sharedImageService = sharedImageService;
            _userService = userService;
            _imageCategoryService = imageCategoryService;
        }

        public IList<SharedImage> SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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

                    SharedImage = _sharedImageService.GetAllNonPremiumSharedImagesAsync().Result.ToList();
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to view this page";
                    return Redirect("/Index");
                }
            }
            return Page();
        }
    }
}
