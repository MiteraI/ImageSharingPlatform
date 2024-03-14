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
        [BindProperty]
        public SubscriptionPackage SubscriptionPackage { get; set; } = default;

        public async Task<IActionResult> OnPostAsync()
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (userJson == null)
            {
                return Redirect("/Authentication/Login");
            }
            var user = JsonConvert.DeserializeObject<User>(userJson);

            var existingSubscriptionPackage = await _ownedSubscriptionService.GetOwnedSubscriptionPackage(SubscriptionPackage.Id);
            if (existingSubscriptionPackage != null)
            {
                if (DateTime.Now > existingSubscriptionPackage.PurchasedTime.AddDays(30))
                {
                    if (user.Balance < SubscriptionPackage.Price)
                    {
                        TempData["ErrorMessage"] = "Your balance is not enough to purchase this subscription package.";
                        return Page();
                    }
                    else
                    {
                        await _ownedSubscriptionService.renewSubscription(OwnedSubscription.Id);
                        await _userService.DecreaseBalance(user.Id, SubscriptionPackage.Price);
                        ViewData["PaymentSuccess"] = "Your subscription have been renewed";
                        return Page();
                    }
                }
                TempData["ErrorMessage"] = "You already have a subscription package of the artist";
                return Page();
            }

            OwnedSubscription.PurchasedTime = DateTime.Now;
            OwnedSubscription.UserId = user.Id;
            var sub = await _subscriptionPackageService.GetSubscriptionPackageById(SubscriptionPackage.Id);
            //OwnedSubscription.SubscriptionPackageId = sub.Id;
            OwnedSubscription.SubscriptionPackage = sub;

            if (user.Balance < SubscriptionPackage.Price)
            {
                TempData["ErrorMessage"] = "Your balance is not enough to purchase this subscription package.";
                return Page();
            }
            else
            {
                await _ownedSubscriptionService.CreateOwnedSubscription(OwnedSubscription);
                await _userService.DecreaseBalance(user.Id, SubscriptionPackage.Price);
                ViewData["PaymentSuccess"] = "You have succesfully subscribed to the artist";
                return Page();
            }
        }
    }
}
