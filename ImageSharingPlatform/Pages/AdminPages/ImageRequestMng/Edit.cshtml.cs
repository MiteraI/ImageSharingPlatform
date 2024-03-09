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
using Newtonsoft.Json;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.AdminPages.ImageRequestMng
{
    public class EditModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;
        private readonly IUserService _userService;
        private readonly IRequestDetailService _requestDetailService;
        public EditModel(IImageRequestService imageRequestService, IUserService userService, IRequestDetailService requestDetailService)
        {
            _imageRequestService = imageRequestService; 
            _userService = userService;
            _requestDetailService = requestDetailService;
        }

        [BindProperty]
        public ImageRequest ImageRequests { get; set; } = default!;
		[BindProperty]
		public RequestDetail RequestDetail { get; set; } = new RequestDetail();


		[BindProperty]
        public ImageRequest ImageRequestCopy { get; set; } = new ImageRequest();

        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

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

            ImageRequestCopy.RequestStatus = ImageRequests.RequestStatus;
            ImageRequestCopy.ImageBlob = ImageRequests.ImageBlob;

            var users = await _userService.GetAllUsersAsync();
            ViewData["RequestStatus"] = new SelectList(Enum.GetValues(typeof(RequestStatus)), ImageRequests.RequestStatus);
            ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
            ViewData["RequesterUserId"] = new SelectList(users, "Id", "Email");
            return Page();
        }

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

			if (ImageRequestCopy.RequestStatus == RequestStatus.CANCELLED
                || ImageRequestCopy.RequestStatus == RequestStatus.SUCCESS
                || ImageRequestCopy.RequestStatus == RequestStatus.REJECTED) 
            {
                TempData["error"] = "Can't update in this status";
                return Page();
            }

            try
            {
				{
					await _imageRequestService.EditImageRequest(ImageRequests);

					RequestDetail.RequestId = ImageRequests.Id;
					RequestDetail.CreatedAt = DateTime.Now;
                    RequestDetail.UserId = userId;

                    await _requestDetailService.AddRequestDetailAsync(RequestDetail);
				}
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

			TempData["success"] = "The request is updated successfully !";
            return RedirectToPage("./Index");
        }
    }
}
