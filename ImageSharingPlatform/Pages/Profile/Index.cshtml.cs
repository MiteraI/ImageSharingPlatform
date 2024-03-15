using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Guid currentUserId = GetCurrentUserId();

            string responseCode = HttpContext.Request.Query["vnp_ResponseCode"];
            string amount = HttpContext.Request.Query["vnp_Amount"];

            if (!string.IsNullOrEmpty(responseCode) && !string.IsNullOrEmpty(amount) && responseCode == "00")
            {
                ViewData["PaymentSuccess"] = true;
                await _userService.IncreaseBalance(currentUserId, double.Parse(amount) / 100);
            }
            else if (!string.IsNullOrEmpty(responseCode) && !string.IsNullOrEmpty(amount) && responseCode != "00")
            {
                ViewData["PaymentSuccess"] = false;
            }

			User = await _userService.GetUserByIdAsync(currentUserId);

			if (User == null)
			{
				return Redirect("/Authentication/Login");
			}

			return Page();
        }

        private Guid GetCurrentUserId()
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");

            if (string.IsNullOrEmpty(userJson))
            { 
                return Guid.Empty; 
            }

            var loggedInUser = JsonConvert.DeserializeObject<User>(userJson);

            return loggedInUser?.Id ?? Guid.Empty;
        }
    }
}
