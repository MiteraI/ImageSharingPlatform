using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Pages.ArtistProfile
{
    public class DetailsModel : PageModel
    {
        private readonly ISubscriptionPackageService _subscriptionPackageService;

        public DetailsModel(ISubscriptionPackageService subscriptionPackageService)
        {
            _subscriptionPackageService = subscriptionPackageService;
        }

        public SubscriptionPackage SubscriptionPackage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubscriptionPackage = await _subscriptionPackageService.GetSubscriptionPackageByArtistId(id);
            if (SubscriptionPackage == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
