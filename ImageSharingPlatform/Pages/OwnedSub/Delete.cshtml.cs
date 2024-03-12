using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;

namespace ImageSharingPlatform.Pages.OwnedSub
{
    public class DeleteModel : PageModel
    {
        private readonly ImageSharingPlatform.Domain.Entities.ApplicationDbContext _context;

        public DeleteModel(ImageSharingPlatform.Domain.Entities.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OwnedSubscription OwnedSubscription { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownedsubscription = await _context.OwnedSubscriptions.FirstOrDefaultAsync(m => m.Id == id);

            if (ownedsubscription == null)
            {
                return NotFound();
            }
            else
            {
                OwnedSubscription = ownedsubscription;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownedsubscription = await _context.OwnedSubscriptions.FindAsync(id);
            if (ownedsubscription != null)
            {
                OwnedSubscription = ownedsubscription;
                _context.OwnedSubscriptions.Remove(OwnedSubscription);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
