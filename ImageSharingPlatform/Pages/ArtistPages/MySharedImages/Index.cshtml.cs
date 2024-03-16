using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Newtonsoft.Json;
using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Domain.Enums;

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
{
	public class IndexModel : PageModel
	{
		private readonly ISharedImageService _sharedImageService;
		private readonly IUserService _userService;

		public IndexModel(ISharedImageService sharedImageService, IUserService userService)
		{
			_sharedImageService = sharedImageService;
			_userService = userService;
		}

		public IList<SharedImage> SharedImage { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			var userJson = HttpContext.Session.GetString("LoggedInUser");
			if (string.IsNullOrEmpty(userJson))
			{
				return Redirect("/Authentication/Login");
			}
			var userAccount = JsonConvert.DeserializeObject<User>(userJson);
			var isArtist = userAccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ARTIST);

			if (isArtist)
			{
				SharedImage = (IList<SharedImage>)await _sharedImageService.FindSharedImagesByUserIdWithFullDetails(userAccount.Id);
			}
			else
			{
				TempData["ErrorMessage"] = "You are not authorized to see this page";
				return Redirect("/Index");
			}

			return Page();
		}
	}
}
