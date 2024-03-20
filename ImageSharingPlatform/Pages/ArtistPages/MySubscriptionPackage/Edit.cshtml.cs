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
using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistPages.MySubscriptionPackage
{
    public class EditModel : PageModel
    {
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IUserService _userService;

        public EditModel(ISubscriptionPackageService subscriptionPackageService, IUserService userService)
        {
            _subscriptionPackageService = subscriptionPackageService;
            _userService = userService;
        }

        [BindProperty]
        public SubscriptionPackage SubscriptionPackage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
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
                TempData["ErrorMessage"] = "You are not authorized to see this page";
				return Redirect("/Index");
			}

			SubscriptionPackage = await _subscriptionPackageService.GetSubscriptionPackageById(id);
            if (SubscriptionPackage == null)
            {
                return NotFound();
            }
            var users = await _userService.GetAllUsersAsync();
            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var userJson = HttpContext.Session.GetString("LoggedInUser");
			if (string.IsNullOrEmpty(userJson))
			{
				return Redirect("/Authentication/Login");
			}
			var userAccount = JsonConvert.DeserializeObject<User>(userJson);

			try
            {
                SubscriptionPackage.ArtistId = userAccount.Id;
                await _subscriptionPackageService.EditSubscriptionPackage(SubscriptionPackage);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _subscriptionPackageService.SubscriptionPackageExistsAsync(x => x.Id == SubscriptionPackage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["SuccessMessage"] = "The subscription package is updated successfully !";
            return RedirectToPage("./Index");
        }
    }
}
