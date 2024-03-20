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
using ImageSharingPlatform.Dto;
using AutoMapper;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.Search
{
    public class DetailsModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly ISharedImageService _sharedImageService;
        private readonly IUserService _userService;
        private readonly IImageCategoryService _imageCategoryService;

        public DetailsModel(IUserService userService, IImageCategoryService imageCategoryService, ISharedImageService sharedImage, IMapper mapper)
        {
            _userService = userService;
            _imageCategoryService = imageCategoryService;
            _sharedImageService = sharedImage;
            _mapper = mapper;
        }

        [BindProperty]  
        public SharedImage SharedImage { get; set; } = default!;

        [BindProperty]
        public ReviewDto Review { get; set; } = new ReviewDto();

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

        public async Task<IActionResult> OnPostAsync()
        {
			var userJson = HttpContext.Session.GetString("LoggedInUser");
			if (string.IsNullOrEmpty(userJson))
			{
				return Redirect("/Authentication/Login");
			}
			var userAccount = JsonConvert.DeserializeObject<User>(userJson);
            ModelState.Remove("ImageName");
			if (ModelState.IsValid)
			{
                var review = _mapper.Map<Review>(Review);
                review.UserId = userAccount.Id;
                review.CreatedAt = DateTime.Now;
                await _sharedImageService.CreateReviewForImage(SharedImage.Id, review);
				return Redirect($"./Details?id={SharedImage.Id}");
			}
			return Redirect($"./Details?id={SharedImage.Id}");
		}
    }
}
