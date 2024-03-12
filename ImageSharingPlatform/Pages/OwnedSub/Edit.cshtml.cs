using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;

namespace ImageSharingPlatform.Pages.OwnedSub
{
    public class EditModel : PageModel
    {
        private readonly ImageSharingPlatform.Domain.Entities.ApplicationDbContext _context;

        public EditModel(ImageSharingPlatform.Domain.Entities.ApplicationDbContext context)
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

            var ownedsubscription =  await _context.OwnedSubscriptions.FirstOrDefaultAsync(m => m.Id == id);
            if (ownedsubscription == null)
            {
                return NotFound();
            }
            OwnedSubscription = ownedsubscription;
           ViewData["SubscriptionPackageId"] = new SelectList(_context.SubscriptionPackages, "Id", "Id");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(OwnedSubscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnedSubscriptionExists(OwnedSubscription.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OwnedSubscriptionExists(Guid id)
        {
            return _context.OwnedSubscriptions.Any(e => e.Id == id);
        }
    }
}
