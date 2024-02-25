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
        private readonly IAzureBlobService _azureBlobService;

        public EditModel(IMapper mapper, IUserService userService, IAzureBlobService azureBlobService)
        {
            _mapper = mapper;
            _userService = userService;
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
                var updatedUser = _mapper.Map<User>(UserEditDto);

                if (avatar != null)
                {
                    string avatarUrl = await _azureBlobService.UploadAvatar(avatar);
                    updatedUser.AvatarUrl = avatarUrl;
                }

                await _userService.EditUser(updatedUser);
                // Redirect to profile page or show success message
                return RedirectToPage("/Profile/Index");
            }

            // If ModelState is not valid, return to the edit page with errors
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
