﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ArtistPages.MySubscriptionPackage
{
    public class CreateModel : PageModel
    {
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IUserService _userService;

        public CreateModel(ISubscriptionPackageService subscriptionPackageService, IUserService userService)
        {
            _subscriptionPackageService = subscriptionPackageService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            var users = await _userService.GetAllUsersAsync();
            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public SubscriptionPackage SubscriptionPackage { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            var user = JsonConvert.DeserializeObject<User>(loggedInUser);
            if (!string.IsNullOrEmpty(loggedInUser))
            {
                
                SubscriptionPackage.ArtistId = user.Id;
            }
            else
            {
                RedirectToPage("/Authentication/Login");
            }

            var existingSubscriptionPackage = await _subscriptionPackageService.GetSubscriptionPackageByArtistId(user.Id);
            if (existingSubscriptionPackage != null)
            {
                ModelState.AddModelError(string.Empty, "You already have a subscription package. You cannot create another one.");
                return Page();
            }

            var isArtist = user.Roles.Any(r => r.UserRole == UserRole.ROLE_ARTIST);
            if (isArtist)
            {
                var newSubcription = _subscriptionPackageService.CreateSubscriptionPackage(SubscriptionPackage);

                if (newSubcription == null)
                {
                    return Page();
                }
            }
            TempData["SuccessMessage"] = "The subscription package is created successfully !";
            return RedirectToPage("./Index");
        }
    }
}
