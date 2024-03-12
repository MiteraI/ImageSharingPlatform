using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageSharingPlatform.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
			HttpContext.Session.Remove("LoggedInUser");
            TempData["SuccessMessage"] = "Logout successfully";
            RedirectToPage("/Index");
		}
	}
}
