using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.AdminPages.ImageCategoryMng
{
    public class EditModel : PageModel
    {
        private readonly IImageCategoryService _imageCategoryService;

        public EditModel(IImageCategoryService imageCategoryService)
        {
            _imageCategoryService = imageCategoryService;
        }

        [BindProperty]
        public ImageCategory ImageCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    ImageCategory = await _imageCategoryService.GetImageCategoryByIdAsync(id);

                    if (ImageCategory == null)
                    {
                        return NotFound();
                    }
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

            bool isModified = false;

            try
            {
                var currentImageCategory = await _imageCategoryService.GetImageCategoryByIdAsync(ImageCategory.Id);
                if (currentImageCategory == null)
                {
                    return NotFound();
                }

                if (currentImageCategory.CategoryName != ImageCategory.CategoryName)
                {
                    currentImageCategory.CategoryName = ImageCategory.CategoryName;
                    isModified = true;
                }

				if (currentImageCategory.Description != ImageCategory.Description)
				{
					currentImageCategory.Description = ImageCategory.Description;
					isModified = true;
				}

				if (isModified)
                {
                    _imageCategoryService.EditImageCategory(currentImageCategory);
                    TempData["SuccessMessage"] = "Category is edited successfully!";
                }
                else
                {
                    TempData["SuccessMessage"] = "No information has been modified.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _imageCategoryService.ImageCategoryExistsAsync(x => x.Id == ImageCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
