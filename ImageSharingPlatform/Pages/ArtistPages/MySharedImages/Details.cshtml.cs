using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
{
    public class DetailsModel : PageModel
    {
        private readonly ImageSharingPlatform.Domain.Entities.ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IImageCategoryService _imageCategoryService;

        public DetailsModel(ImageSharingPlatform.Domain.Entities.ApplicationDbContext context, IUserService userService, IImageCategoryService imageCategoryService)
        {
            _context = context;
            _userService = userService;
            _imageCategoryService = imageCategoryService;
        }

        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
			var userJson = HttpContext.Session.GetString("LoggedInUser");
			if (string.IsNullOrEmpty(userJson))
			{
				return Redirect("/Authentication/Login");
			}
			var userAccount = JsonConvert.DeserializeObject<User>(userJson);
			var isArtist = userAccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ARTIST);
            if (!isArtist)
            {
                TempData["ErrorMessage"] = "You are not authorized to view this page!";
				return Redirect("/Index");
			}

			if (id == null)
            {
                return NotFound();
            }

            var sharedimage = await _context.SharedImages.FirstOrDefaultAsync(m => m.Id == id);
            var users = await _userService.GetAllUsersAsync();
            var categories = await _imageCategoryService.GetAllImageCategoriesAsync();

            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "CategoryName");
            if (sharedimage == null)
            {
                return NotFound();
            }
            else
            {
                SharedImage = sharedimage;
            }
            return Page();
        }
    }
}
