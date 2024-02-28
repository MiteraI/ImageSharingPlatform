using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;

namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
{
    public class DeleteModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;

        public DeleteModel(ISharedImageService sharedImageService)
        {
            _sharedImageService = sharedImageService;
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            SharedImage = await _sharedImageService.GetSharedImageByIdAsync(id);

            if (SharedImage == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SharedImage = await _sharedImageService.DeleteSharedImage(SharedImage.Id);

            return RedirectToPage("./Index");
        }
    }
}
