using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using AutoMapper;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistProfile
{
    public class DetailsModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IOwnedSubscriptionService _ownedSubscriptionService;
        private readonly IUserService _userService;

        public DetailsModel(IMapper mapper,ISubscriptionPackageService subscriptionPackageService
            ,IOwnedSubscriptionService ownedSubscriptionService
            ,IUserService userService)
        {
            _mapper = mapper;
            _subscriptionPackageService = subscriptionPackageService;
            _ownedSubscriptionService = ownedSubscriptionService;
            _userService = userService;
        }

        [BindProperty]
        public SubscriptionPackage SubscriptionPackage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return Redirect("/");
            }

			var userJson = HttpContext.Session.GetString("LoggedInUser");
			if (userJson != null)
			{
				User user = JsonConvert.DeserializeObject<User>(userJson);
				if (user.Id.Equals(id))
				{
					return Redirect("/ArtistPages/MySubscriptionPackage");
				}
			}


			SubscriptionPackage = await _subscriptionPackageService.GetSubscriptionPackageByArtistId(id);
            if (SubscriptionPackage == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (userJson == null)
            {
				TempData["ErrorMessage"] = "You must login to subscribe";
				return Redirect("/Authentication/Login");
			}
            var user = await _userService.GetUserByIdAsync(JsonConvert.DeserializeObject<User>(userJson).Id);

			var subscriptionPackage = await _subscriptionPackageService.GetSubscriptionPackageById(SubscriptionPackage.Id);

			// Check if the subscription package belongs to the user making the request
			if (subscriptionPackage.ArtistId == user.Id)
			{
				TempData["ErrorMessage"] = "I don't know how you access this, but you cannot subscribe to your own subscription package";
				return Page();
			}

            // Check if the user already has a subscription package of the artist
			var existingSubscriptionPackage = await _ownedSubscriptionService.GetUserOwnedSubscriptionPackage(user.Id, subscriptionPackage.Id);
			if (existingSubscriptionPackage != null)
			{
				if (DateTime.Now > existingSubscriptionPackage.PurchasedTime.AddDays(30))
				{
					if (user.Balance < subscriptionPackage.Price)
					{
						TempData["ErrorMessage"] = "Your balance is not enough to purchase this subscription package.";
						return Page();
					}
					else
					{
						await _ownedSubscriptionService.renewSubscription(existingSubscriptionPackage.Id);
						await _userService.DecreaseBalance(user.Id, subscriptionPackage.Price);
						await _userService.IncreaseBalance(subscriptionPackage.ArtistId.Value, subscriptionPackage.Price);
						ViewData["PaymentSuccess"] = "Your subscription have been renewed";
						return Page();
					}
				}
				TempData["ErrorMessage"] = "You already have a subscription package of the artist";
				return Page();
			}

			if (user.Balance < subscriptionPackage.Price)
			{
				TempData["ErrorMessage"] = "Your balance is not enough to purchase this subscription package.";
				return Page();
			}
			else
			{
				var newOwnedSubscription = new OwnedSubscription
				{
					PurchasedTime = DateTime.Now,
					UserId = user.Id,
					SubscriptionPackage = subscriptionPackage
				};
				await _ownedSubscriptionService.CreateOwnedSubscription(newOwnedSubscription);
				await _userService.DecreaseBalance(user.Id, subscriptionPackage.Price);
				await _userService.IncreaseBalance(subscriptionPackage.ArtistId.Value, subscriptionPackage.Price);
				ViewData["PaymentSuccess"] = "You have succesfully subscribed to the artist";
				return Page();
			}
		}
    }
}
