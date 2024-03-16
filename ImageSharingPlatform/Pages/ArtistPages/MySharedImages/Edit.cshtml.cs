using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
{
	public class EditModel : PageModel
	{
		private readonly ImageSharingPlatform.Domain.Entities.ApplicationDbContext _context;

		public EditModel(ImageSharingPlatform.Domain.Entities.ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
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
			if (sharedimage == null)
			{
				return NotFound();
			}
			SharedImage = sharedimage;
			ViewData["ArtistId"] = new SelectList(_context.Users, "Id", "Email");
			ViewData["ImageCategoryId"] = new SelectList(_context.ImageCategories, "Id", "CategoryName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Attach(SharedImage).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SharedImageExists(SharedImage.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			TempData["SuccessMessage"] = "Image is edited successfully!";
			return RedirectToPage("./Index");
		}

		private bool SharedImageExists(Guid id)
		{
			return _context.SharedImages.Any(e => e.Id == id);
		}
	}
}
