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

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
{
    public class IndexModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;

        public IndexModel(ISharedImageService sharedImageService)
        {
			_sharedImageService = sharedImageService;
		}

        public IList<SharedImage> SharedImage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // Get user session
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            if (!string.IsNullOrEmpty(loggedInUser))
            {
                var user = JsonConvert.DeserializeObject<User>(loggedInUser);
                SharedImage = (IList<SharedImage>) await _sharedImageService.FindSharedImagesByUserIdWithFullDetails(user.Id);
            } else
            {
                RedirectToPage("/Authentication/Login");
            }
        }
    }
}
