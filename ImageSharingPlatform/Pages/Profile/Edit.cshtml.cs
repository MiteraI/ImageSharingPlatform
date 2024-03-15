using AutoMapper;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Dto;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

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
                return NotFound();
            }

            UserEditDto = _mapper.Map<UserEditDto>(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile avatar)
        {
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

                await _userService.EditUser(userToUpdate);
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
    }
}
