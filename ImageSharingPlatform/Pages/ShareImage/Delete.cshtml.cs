using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.ShareImage
{
    public class DeleteModel : PageModel
    {
        private readonly ISharedImageRepository _sharedImageRepository;

        public DeleteModel(ISharedImageRepository sharedImageRepository)
        {
            _sharedImageRepository = sharedImageRepository;
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            SharedImage = await _sharedImageRepository.GetOneAsync(id);

            if (SharedImage == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            SharedImage = await _sharedImageRepository.GetOneAsync(id);

            if (SharedImage != null)
            {
                await _sharedImageRepository.DeleteAsync(SharedImage);
                await _sharedImageRepository.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
