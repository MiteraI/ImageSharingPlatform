﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Newtonsoft.Json;
using ImageSharingPlatform.Domain.Enums;

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
			}
			else
			{
				RedirectToPage("/Authentication/Login");
			}

			if (uploadImage != null)
			{
				SharedImage.ImageUrl = await _azureBlobService.UploadImage(uploadImage);
				await _sharedImageService.CreateSharedImage(SharedImage);
			}
			TempData["SuccessMessage"] = "Image is created successfully!";
			return RedirectToPage("./Index");
		}
	}
}
