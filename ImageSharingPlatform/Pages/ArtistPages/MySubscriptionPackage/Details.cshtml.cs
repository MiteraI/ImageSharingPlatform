using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;

namespace ImageSharingPlatform.Pages.ArtistPages.MySubscriptionPackage
{
    public class DetailsModel : PageModel
    {
        private readonly ImageSharingPlatform.Domain.Entities.ApplicationDbContext _context;

        public DetailsModel(ImageSharingPlatform.Domain.Entities.ApplicationDbContext context)
        {
            _context = context;
        }

        public SubscriptionPackage SubscriptionPackage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionpackage = await _context.SubscriptionPackages.FirstOrDefaultAsync(m => m.Id == id);
            if (subscriptionpackage == null)
            {
                return NotFound();
            }
            else
            {
                SubscriptionPackage = subscriptionpackage;
            }
            return Page();
        }
    }
}
