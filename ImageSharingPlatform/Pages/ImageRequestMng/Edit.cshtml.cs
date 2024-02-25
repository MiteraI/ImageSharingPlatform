using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Pages.ImageRequestMng
{
    public class EditModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;
        private readonly IUserService _userService;
        public EditModel(IImageRequestService imageRequestService, IUserService userService)
        {
            _imageRequestService = imageRequestService; 
            _userService = userService;
        }

        [BindProperty]
        public ImageRequest ImageRequests { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImageRequests = await _imageRequestService.GetImageRequestById(id);
            if (ImageRequests == null)
            {
                return NotFound();
            }
            var users = await _userService.GetAllUsersAsync();
            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            ViewData["RequesterUserId"] = new SelectList(users, "Id", "Email");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var imageRequest = _imageRequestService.EditImageRequest(ImageRequests);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _imageRequestService.ImageRequestExistsAsync(x => x.Id == ImageRequests.Id))
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
    }
}
