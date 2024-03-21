using AutoMapper;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Dto;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ImageSharingPlatform.Pages.Profile
{
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IAzureBlobService _azureBlobService;

        public EditModel(IMapper mapper, IUserService userService
            , ISubscriptionPackageService subscriptionPackageService
            , IAzureBlobService azureBlobService)
        {
            _mapper = mapper;
            _userService = userService;
            _subscriptionPackageService = subscriptionPackageService;
            _azureBlobService = azureBlobService;
        }

        [BindProperty]
        public UserEditDto UserEditDto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Guid currentUserId = GetCurrentUserId();

            var user = await _userService.GetUserByIdAsync(currentUserId);

            if (user == null)
            {
                return Redirect("/Authentication/Login");
            }

            UserEditDto = _mapper.Map<UserEditDto>(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile avatar)
        {
            ModelState.Remove("avatar");
            ModelState.Remove("UserEditDto.Password");

            if (!IsValidEmail(UserEditDto.Email))
            {
                TempData["ErrorMessage"] = "Invalid email";
                return Page();
            }

            if (ModelState.IsValid)
            {
                var userToUpdate = await _userService.GetUserByIdAsync(UserEditDto.Id);

                if (avatar != null)
                {
                    string avatarUrl = await _azureBlobService.UploadAvatar(avatar);
                    userToUpdate.AvatarUrl = avatarUrl;
                }

                userToUpdate.Username = UserEditDto.Username;
                userToUpdate.Email = UserEditDto.Email;
                userToUpdate.FirstName = UserEditDto.FirstName;
                userToUpdate.LastName = UserEditDto.LastName;

                try
                {
                    var updatedUser = await _userService.EditUser(userToUpdate);
					var userJson = JsonConvert.SerializeObject(updatedUser);
					HttpContext.Session.SetString("LoggedInUser", userJson);
				}
				catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
					return Page();
				}
			}

            // If ModelState is not valid, return to the edit page with errors
            return Page();
        }

        public async Task<IActionResult> OnPostBecomeArtistAsync()
        {
            Guid currentUserId = GetCurrentUserId();
            await _userService.UpdateRoleToArtist(currentUserId);
            await _subscriptionPackageService.CreateSubscriptionPackage(new SubscriptionPackage
            {
                ArtistId = currentUserId,
                Price = 100000,
                Description = "Default package price for new artists"
            });
            TempData["SuccessMessage"] = "You have became to an artist <3";
            return RedirectToPage("/Authentication/Logout");
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

		private bool IsValidEmail(string email)
		{
			// Define a regular expression pattern for validating email addresses
			string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			// Create a Regex object
			Regex regex = new Regex(pattern);

			// Use the Regex.IsMatch method to check if the email matches the pattern
			return regex.IsMatch(email);
		}
	}
}
