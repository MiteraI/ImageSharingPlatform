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
    public class DetailsModel : PageModel
    {
        private readonly ISharedImageRepository _sharedImageRepository;

        public DetailsModel(ISharedImageRepository sharedImageRepository)
        {
            _sharedImageRepository = sharedImageRepository;
        }

        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SharedImage = await _sharedImageRepository.GetOneAsync(id);

            if (SharedImage == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
