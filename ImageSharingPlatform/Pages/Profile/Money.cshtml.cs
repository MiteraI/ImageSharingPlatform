using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageSharingPlatform.Pages.Profile
{
    public class MoneyModel : PageModel
    {
        private readonly IVnpayService _vnpayService;
        public MoneyModel(IVnpayService vnpayService)
        {
			_vnpayService = vnpayService;
		}

        private string returnUrl = "/Profile";
        [BindProperty]		
        public double Money { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedInUser")))
            {
				Response.Redirect("/Authentication/Login");
			}
        }

        public IActionResult OnPost() 
        {
            if (Money < 100000 || Money % 1000 != 0)
            {
                // Show error message in view asp-validation
                ModelState.AddModelError("Money", "The amount must be greater than 100,000 and divisible by 1,000,000");
                return Page();
			}
            // Vnpay needs the amount to divide by 100
			var url = _vnpayService.CreateVnpayPortalUrl(Money, returnUrl, HttpContext);
            return Redirect(url);
        }
    }
}
