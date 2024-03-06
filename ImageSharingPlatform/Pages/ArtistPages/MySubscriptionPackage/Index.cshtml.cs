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
            var useraccount = JsonConvert.DeserializeObject<User>(userJson);

            var userId = useraccount.Id;

            if (userId == Guid.Empty)
            {
                return NotFound("User not found.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var isAdmin = useraccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ADMIN);
            if (isAdmin)
            {
                SubscriptionPackage = (IList<SubscriptionPackage>) await _subscriptionPackageService.GetAllSubscriptionsAsync();
            }
            else
            {
                SubscriptionPackage = (IList<SubscriptionPackage>)await _subscriptionPackageService.GetSubscriptionPackagesByArtistIdWithFullDetails(userId);
            }           
            return Page();
        }
    }
}
