﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.AdminPages.UserMng
{
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;

        public DetailsModel(IUserService userService)
        {
            _userService = userService;
        }

        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return Redirect("/Authentication/Login");
            }
            else
            {
                var userAccount = JsonConvert.DeserializeObject<User>(userJson);
                var isAdmin = userAccount.Roles.Any(r => r.UserRole == UserRole.ROLE_ADMIN);

                if (isAdmin)
                {
                    User = await _userService.GetUserByIdAsync(id);

                    if (User == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "You are not authorized to view this page";
                    return Redirect("/Index");
                }
            }

            return Page();
        }
    }
}
