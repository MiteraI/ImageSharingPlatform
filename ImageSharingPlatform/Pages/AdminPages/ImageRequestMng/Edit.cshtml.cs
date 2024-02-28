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
using ImageSharingPlatform.Domain.Enums;

namespace ImageSharingPlatform.Pages.AdminPages.ImageRequestMng
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

        /*[BindProperty]
        public RequestStatus SelectedStatus { get; set; }*/

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
            ViewData["RequestStatus"] = new SelectList(Enum.GetValues(typeof(RequestStatus)), ImageRequests.RequestStatus);
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

            if (ImageRequests.RequestStatus != RequestStatus.PROCESSING)
            {
                ModelState.AddModelError(string.Empty, "You can only edit request PROCESSING status !");
                return Page();
            }
            try
            {
                await _imageRequestService.EditImageRequest(ImageRequests);
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
            TempData["SuccessMessage"] = "The request is updated successfully !";
            return RedirectToPage("./Index");
        }
    }
}
