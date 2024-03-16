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
using JHipsterNet.Core.Pagination;

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
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

        public IPage<SharedImage> SharedImages { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 4;
        public string SearchQuery { get; set; }
        public bool? IsPremium { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get user session
            //var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            //if (!string.IsNullOrEmpty(loggedInUser))
            //{
            //    var user = JsonConvert.DeserializeObject<User>(loggedInUser);
            //    SharedImage = (IList<SharedImage>) await _sharedImageService.FindSharedImagesByUserIdWithFullDetails(user.Id);
            //} else
            //{
            //    RedirectToPage("/Authentication/Login");
            //}

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

            var page = HttpContext.Request.Query["page"];

            if (!string.IsNullOrWhiteSpace(page) && int.TryParse(page, out var pageNumber))
            {
                PageNumber = pageNumber;
            }

            var isPremiumQuery = HttpContext.Request.Query["isPremium"];
            if (!string.IsNullOrWhiteSpace(isPremiumQuery))
            {
                IsPremium = bool.TryParse(isPremiumQuery, out bool isPremiumValue) ? isPremiumValue : (bool?)null;
                SharedImages = await _sharedImageService.FindSharedImageForArtistPageable(userId, IsPremium, Pageable.Of(PageNumber, PageSize, null));
            } 
            else
            {
                SharedImages = await _sharedImageService.FindSharedImageForArtistPageable(userId, null, Pageable.Of(PageNumber, PageSize, null));
            }

            return Page();
        }
    }
}
