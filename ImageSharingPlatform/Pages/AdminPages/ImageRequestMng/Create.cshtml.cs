using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.AdminPages.ImageRequestMng
{
    public class CreateModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;
        private readonly IUserService _userService;

        public CreateModel(IImageRequestService imageRequestService, IUserService userService)
        {
            _imageRequestService = imageRequestService;
            _userService = userService; 
        }

        public async Task<IActionResult> OnGet()
        {
            var users =  await _userService.GetAllUsersAsync(); 
            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            ViewData["RequesterUserId"] = new SelectList(users, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public ImageRequest ImageRequests { get; set; } = default!;
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 2097152)
                    {
                        ImageRequests.ImageBlob = memoryStream.ToArray();
                    }
                    else
                    {
                        TempData["error"] = "The file is too large.";
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userJson = HttpContext.Session.GetString("LoggedInUser");
            var useraccount = JsonConvert.DeserializeObject<User>(userJson);

            var userId = useraccount.Id;

            if (userId == Guid.Empty)
            {
                return NotFound("User not found.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            ImageRequests.RequesterUserId = userId;
            ImageRequests.CreateTime = DateTime.Now;

            ImageRequests.RequestStatus = RequestStatus.PROCESSING;

            

            var newImageRequest = _imageRequestService.CreateImageRequest(ImageRequests);

            if (newImageRequest == null)
            {
                return Page();
            }
            TempData["SuccessMessage"] = "The request is created successfully !";
            return RedirectToPage("./Index");
        }
    }
}
