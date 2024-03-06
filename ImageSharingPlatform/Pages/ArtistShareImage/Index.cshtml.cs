using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Newtonsoft.Json;
using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Domain.Enums;

namespace ImageSharingPlatform.Pages.ArtistShareImage
{
    public class IndexModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
        private readonly IUserService _userService;

        public IndexModel(ISharedImageService sharedImageService, IUserService userService)
        {
			_sharedImageService = sharedImageService;
            _userService = userService;
		}

        public IList<SharedImage> SharedImage { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");

            var useraccount = JsonConvert.DeserializeObject<User>(loggedInUser);

            var userId = useraccount.Id;

            if (userId == Guid.Empty)
            {
                return NotFound("User not found.");
            }
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var isUser = useraccount.Roles.Any(r => r.UserRole == UserRole.ROLE_USER);
            if (isUser)
            {
                SharedImage = (IList<SharedImage>)await _sharedImageService.GetAllSharedImagesAsync();
            } 
            return Page();
        }
    }
}
