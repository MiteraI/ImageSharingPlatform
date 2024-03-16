using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;

namespace ImageSharingPlatform.Pages.AdminPages.ImageCategoryMng
{
	public class CreateModel : PageModel
	{
		private readonly IImageCategoryService _imageCategoryService;

		public CreateModel(IImageCategoryService imageCategoryService)
		{
			_imageCategoryService = imageCategoryService;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		[BindProperty]
		public ImageCategory ImageCategory { get; set; } = default!;

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				var existingCategory = await _imageCategoryService.CheckCategoryName(ImageCategory.CategoryName);

				if (existingCategory == null)
				{
					var imagecategory = await _imageCategoryService.CreateImageCategory(ImageCategory);
					if (imagecategory != null)
					{
						TempData["SuccessMessage"] = "Category is created successfully!";
						return RedirectToPage("./Index");
					}
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			return Page();
		}
	}
}
