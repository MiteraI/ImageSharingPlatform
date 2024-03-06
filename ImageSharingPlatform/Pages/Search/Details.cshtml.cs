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

namespace ImageSharingPlatform.Pages.Search
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
