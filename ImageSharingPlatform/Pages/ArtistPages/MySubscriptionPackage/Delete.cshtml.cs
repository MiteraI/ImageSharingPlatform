using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImageSharingPlatform.Pages.ArtistPages.MySubscriptionPackage
{
    public class DeleteModel : PageModel
    {
        private readonly ISubscriptionPackageService _subscriptionPackageService;
        private readonly IUserService _userService;

        public DeleteModel(ISubscriptionPackageService subscriptionPackageService, IUserService userService)
        {
            _subscriptionPackageService = subscriptionPackageService;
            _userService = userService;
        }

        [BindProperty]
        public SubscriptionPackage SubscriptionPackage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubscriptionPackage = await _subscriptionPackageService.GetSubscriptionPackageById(id);

            if (SubscriptionPackage == null)
            {
                return NotFound();
            }
            var users = await _userService.GetAllUsersAsync();
            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SubscriptionPackage = await _subscriptionPackageService.DeleteSubscriptionPackage(SubscriptionPackage.Id);
            TempData["SuccessMessage"] = "The subscription package is deleted successfully !";
            return RedirectToPage("./Index");
        }
    }
}
