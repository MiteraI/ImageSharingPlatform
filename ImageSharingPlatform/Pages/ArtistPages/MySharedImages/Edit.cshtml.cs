using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
{
    public class EditModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
        private readonly IUserService _userService;
        private readonly IImageCategoryService _imageCategoryService;

        public EditModel(ISharedImageService sharedImageService, IUserService userService, IImageCategoryService imageCategoryService)
        {
            _sharedImageService = sharedImageService;
            _userService = userService;
            _imageCategoryService = imageCategoryService;
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; } = default!;

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
                var isArtist = userAccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ARTIST);

                if (isArtist)
                {
                    var users = await _userService.GetAllUsersAsync();
                    var categories = await _imageCategoryService.GetAllImageCategoriesAsync();

                    ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
                    ViewData["ImageCategoryId"] = new SelectList(categories, "Id", "CategoryName");

                    SharedImage = await _sharedImageService.GetSharedImageByIdAsync(id);

                    if (SharedImage == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to view this page!";
                    return Redirect("/Index");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var entity = await _sharedImageService.GetSharedImageByIdAsync(SharedImage.Id);

            if (entity == null)
            {
                return NotFound();
            }

            bool hasChanges = false;

            if (!string.IsNullOrEmpty(SharedImage.ImageUrl) && entity.ImageUrl != SharedImage.ImageUrl)
            {
                entity.ImageUrl = SharedImage.ImageUrl;
                hasChanges = true;
            }

            if (entity.ImageName != SharedImage.ImageName)
            {
                entity.ImageName = SharedImage.ImageName;
                hasChanges = true;
            }

            if (entity.Description != SharedImage.Description)
            {
                entity.Description = SharedImage.Description;
                hasChanges = true;
            }

            if (entity.ImageCategoryId != SharedImage.ImageCategoryId)
            {
                entity.ImageCategoryId = SharedImage.ImageCategoryId;
                hasChanges = true;
            }

            if (entity.IsPremium != SharedImage.IsPremium)
            {
                entity.IsPremium = SharedImage.IsPremium;
                hasChanges = true;
            }

            if (hasChanges)
            {
                try
                {
                    await _sharedImageService.EditSharedImage(entity);
                    TempData["SuccessMessage"] = "Image is edited successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _sharedImageService.SharedImageExistsAsync(x => x.Id == SharedImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No information has been modified.";
            }

            return RedirectToPage("./Index");
        }

    }
}
