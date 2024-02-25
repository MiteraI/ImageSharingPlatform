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

        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Guid currentUserId = GetCurrentUserId();

            User = await _userService.GetUserByIdAsync(currentUserId);

            if (User == null)
            {
                return NotFound();
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
