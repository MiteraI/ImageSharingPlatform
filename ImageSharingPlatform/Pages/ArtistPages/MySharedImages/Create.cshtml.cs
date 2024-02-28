using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
{
    public class CreateModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
        private readonly IImageCategoryService _imageCategoryService;
        private readonly IAzureBlobService _azureBlobService;
        public CreateModel(ISharedImageService sharedImageService, IImageCategoryService imageCategoryService, IAzureBlobService azureBlobService)
        {
            _sharedImageService = sharedImageService;
			_imageCategoryService = imageCategoryService;
			_azureBlobService = azureBlobService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
        ViewData["ImageCategoryId"] = new SelectList(await _imageCategoryService.GetAllImageCategoriesAsync(), "Id", "CategoryName");
            return Page();
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile uploadImage)
        {
            if (!ModelState.IsValid)
            {
            }

            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            if (!string.IsNullOrEmpty(loggedInUser))
            {
				var user = JsonConvert.DeserializeObject<User>(loggedInUser);
				SharedImage.ArtistId = user.Id;
                SharedImage.CreatedAt = DateTime.Now; 
			} else
            {
				RedirectToPage("/Authentication/Login");
			}

            if (uploadImage != null)
            {
				SharedImage.ImageUrl = await _azureBlobService.UploadImage(uploadImage);
                await _sharedImageService.CreateSharedImage(SharedImage);
            }

            return RedirectToPage("./Index");
        }
    }
}
