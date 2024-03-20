using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistProfile
{
    public class ImagesModel : PageModel
    {
        private readonly IOwnedSubscriptionService _ownedSubscriptionService;
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly ISharedImageService _sharedImageService;
        private readonly IUserService _userService;

        public ImagesModel(IOwnedSubscriptionService ownedSubscriptionService
            , ISubscriptionPackageService subscriptionPackageService
            , IUserService userService
            , ISharedImageService sharedImageService)
        {
			_ownedSubscriptionService = ownedSubscriptionService;
			_subscriptionPackageService = subscriptionPackageService;
            _sharedImageService = sharedImageService;
			_userService = userService;
		}

        [BindProperty]
        public SubscriptionPackage SubscriptionPackageModel { get; set; } = default!;
        [BindProperty]
        public IList<SharedImage> SharedImages { get; set; } = default!;
        [BindProperty]
        public User Artist { get; set; } = default!;

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
					return Redirect("/ArtistPages/MySharedImages/Index");
				}
			}

			Artist = await _userService.GetUserByIdAsync(id);
            var artistSubscription = await _subscriptionPackageService.GetSubscriptionPackageByArtistId(id);
            if (artistSubscription == null)
            {
				return NotFound();
			}
            SubscriptionPackageModel = artistSubscription;

            // Check if the user has a subscription to the artist
            if (string.IsNullOrEmpty(userJson))
            {
				// If the user is not logged in, show the images that are not premium
				ViewData["Subscription"] = "You can subscribe to the artist to view premium content after logging in";
				SharedImages = (List<SharedImage>)await _sharedImageService.FindSharedImageByArtistId(id, false);
            } else
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);
                var existingSubscription = await _ownedSubscriptionService.GetUserOwnedSubscriptionPackage(user.Id, artistSubscription.Id);
                if (existingSubscription == null)
                {
                    // If the user is logged in but does not have a subscription, show the images that are not premium
                    ViewData["Subscription"] = "You haven't subscribed to this artist. Consider subscribe to view premium images";
                    SharedImages = (List<SharedImage>)await _sharedImageService.FindSharedImageByArtistId(id, false);
                } else
                {
                    // If the user is logged in and has a subscription, show all the images
                    if (existingSubscription.PurchasedTime.AddMonths(1) < DateTime.Now)
                    {
                        // If the subscription has expired, show the images that are not premium
                        ViewData["Subscription"] = "Your subscription has expired. Please renew your subscription to view premium images";
                        SharedImages = (List<SharedImage>)await _sharedImageService.FindSharedImageByArtistId(id, false);
                        return Page();
                    } 

                    SharedImages = (List<SharedImage>)await _sharedImageService.FindSharedImageByArtistId(id, true);
                }
            }
            return Page();
        }
    }
}
