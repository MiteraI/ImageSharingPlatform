﻿using System;
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
        private readonly ISharedImageService _sharedImageService;
        private readonly IUserService _userService;
        private readonly IImageCategoryService _imageCategoryService;

        public DetailsModel(IUserService userService, IImageCategoryService imageCategoryService, ISharedImageService sharedImage)
        {
            _userService = userService;
            _imageCategoryService = imageCategoryService;
            _sharedImageService = sharedImage;
        }

        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SharedImage = await _sharedImageService.GetSharedImageByIdAsync(id);
            var users = await _userService.GetAllUsersAsync();
            var categories = await _imageCategoryService.GetAllImageCategoriesAsync();

            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "CategoryName");
            if (SharedImage == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}