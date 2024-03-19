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
using Microsoft.AspNetCore.Authorization;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.AdminPages.ImageCategoryMng
{
    public class IndexModel : PageModel
    {
        private readonly IImageCategoryService _imageCategoryService;

        public IndexModel(IImageCategoryService imageCategoryService)
        {
            _imageCategoryService = imageCategoryService;
        }

        public IList<ImageCategory> ImageCategory { get; set; } = default!;

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
                    ImageCategory = _imageCategoryService.GetAllImageCategoriesAsync().Result.ToList();
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
