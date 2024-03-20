using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;
using Newtonsoft.Json;
using ImageSharingPlatform.Domain.Enums;

namespace ImageSharingPlatform.Pages.ArtistPages.MySubscriptionPackage
{
    public class IndexModel : PageModel
    {
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IUserService _userService;
        public IndexModel(ISubscriptionPackageService subscriptionPackageService, IUserService userService)
        {
            _subscriptionPackageService = subscriptionPackageService;
            _userService = userService;
        }

        public IList<SubscriptionPackage> SubscriptionPackage { get;set; } = default!;

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
				SubscriptionPackage = (IList<SubscriptionPackage>)await _subscriptionPackageService.GetSubscriptionPackagesByArtistIdWithFullDetails(userAccount.Id);
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
