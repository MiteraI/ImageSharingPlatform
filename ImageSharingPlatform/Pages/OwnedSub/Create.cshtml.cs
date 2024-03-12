using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.OwnedSub
{
    public class CreateModel : PageModel
    {
        private readonly IOwnedSubscriptionService _ownedSubscriptionService;
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IUserService _userService;
        
        public CreateModel(IOwnedSubscriptionService ownedSubscriptionService, ISubscriptionPackageService subscriptionPackageService, IUserService userService)
        {
            _ownedSubscriptionService = ownedSubscriptionService;
            _subscriptionPackageService = subscriptionPackageService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var packages = await _subscriptionPackageService.GetAllSubscriptionsAsync();
            var users = await _userService.GetAllUsersAsync();

            ViewData["SubscriptionPackageId"] = new SelectList(packages, "Id", "Id");
            ViewData["UserId"] = new SelectList(users, "Id", "Email");

            SubscriptionPackage = await _subscriptionPackageService.GetSubscriptionPackageByArtistId(id);
            return Page();
        }

        [BindProperty]
        public OwnedSubscription OwnedSubscription { get; set; } = default!;
        public SubscriptionPackage SubscriptionPackage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
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



            var existingSubscriptionPackageId = await _ownedSubscriptionService.GetOwnedSubscriptionPackage(OwnedSubscription.SubscriptionPackageId);
            if (existingSubscriptionPackageId != null)
            {
                TempData["ErrorMessage"] = "You already have a subscription package of the artist. You cannot subscribe another one.";
                return RedirectToPage("./Index");
            }

            OwnedSubscription.PurchasedTime = DateTime.Now;
            OwnedSubscription.UserId = userId;

            await _ownedSubscriptionService.CreateOwnedSubscription(OwnedSubscription);

            return RedirectToPage("./Index");
        }
    }
}
