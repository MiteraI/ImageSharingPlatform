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
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public int PageSize { get; set; } = 8;
        public string SearchQuery { get; set; }
        public bool? IsPremium { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return Redirect("/Authentication/Login");
            }
            else
            {
                var userAccount = JsonConvert.DeserializeObject<User>(userJson);
                var isArtist = userAccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ARTIST);

                if (isArtist)
                {
                    var userId = userAccount.Id;

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
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to view this page!";
                    return Redirect("/Index");
                }
            }
            return Page();
        }
    }
}
