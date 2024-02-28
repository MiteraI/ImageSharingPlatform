using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;

namespace ImageSharingPlatform.Pages.ArtistPages.MySharedImages
{
    public class DeleteModel : PageModel
    {
        private readonly ImageSharingPlatform.Domain.Entities.ApplicationDbContext _context;

        public DeleteModel(ImageSharingPlatform.Domain.Entities.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sharedimage = await _context.SharedImages.FirstOrDefaultAsync(m => m.Id == id);

            if (sharedimage == null)
            {
                return NotFound();
            }
            else
            {
                SharedImage = sharedimage;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sharedimage = await _context.SharedImages.FindAsync(id);
            if (sharedimage != null)
            {
                SharedImage = sharedimage;
                _context.SharedImages.Remove(SharedImage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
