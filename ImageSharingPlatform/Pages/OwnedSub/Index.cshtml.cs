using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.OwnedSub
{
    public class IndexModel : PageModel
    {
        private readonly IOwnedSubscriptionService _ownedSubscriptionService;
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IUserService _userService;

        public IndexModel(IOwnedSubscriptionService ownedSubscriptionService, ISubscriptionPackageService subscriptionPackageService, IUserService userService)
        {
            _ownedSubscriptionService = ownedSubscriptionService;
            _subscriptionPackageService = subscriptionPackageService;
            _userService = userService;
        }

        public IList<OwnedSubscription> OwnedSubscription { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _userService.GetAllUsersAsync();

            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");

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


            OwnedSubscription = (IList<OwnedSubscription>)await _ownedSubscriptionService.GetAllOwnedSubscriptionsAsync();


            return Page();
        }
    }
}
