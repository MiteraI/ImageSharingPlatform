﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;
using Newtonsoft.Json;
using ImageSharingPlatform.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace ImageSharingPlatform.Pages.ArtistPages.ReceivedImageRequest
{
    public class ReceivedImageRequestModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;
        private readonly IUserService _userService;

        public ReceivedImageRequestModel(IImageRequestService imageRequestService, IUserService userService)
        {
            _imageRequestService = imageRequestService;
            _userService = userService;
        }

        public IList<ImageRequest> ImageRequests { get; set; } = default!;
        [BindProperty]
        public Role role { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return RedirectToPage("/Authentication/Login");
            }
            var useraccount = JsonConvert.DeserializeObject<User>(userJson);
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

            var isArtist = useraccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ARTIST);
            if (isArtist)
            {
                ImageRequests = (IList<ImageRequest>)await _imageRequestService.GetAllImageRequestsByArtistAsync(userId);
            }
            else
            {
                TempData["ErrorMessage"] = "You are not authorized to view this page";
                return Redirect("/Index");
            }

            return Page();
        }
    }
}
