using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public LoginModel(IUserService userService) {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    var result = await _userService.LoginUser(Username, Password);

                    if (result != null)
                    {
                        var userJson = JsonConvert.SerializeObject(result);
                        HttpContext.Session.SetString("LoggedInUser", userJson);
                        TempData["SuccessMessage"] = "Login successfully <3";
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid login attempt.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Username and Password cannot be empty.";
            }
            return Page();
        }
    }
}
