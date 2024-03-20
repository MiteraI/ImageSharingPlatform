using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;
using Newtonsoft.Json;
using ImageSharingPlatform.Domain.Enums;

namespace ImageSharingPlatform.Pages.ImageRequestMng
{
    public class DetailsModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;
        private readonly IRequestDetailService _requestDetailService;
        private readonly IUserService _userService;

        public DetailsModel(IImageRequestService imageRequestService, IRequestDetailService requestDetailService, IUserService userService)
        {
            _imageRequestService = imageRequestService;
            _requestDetailService = requestDetailService;
            _userService = userService;
        }

        [BindProperty]
        public ImageRequest ImageRequest { get; set; } = default!;

        [BindProperty]
        public IList<RequestDetail> RequestDetail { get; set; }
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return RedirectToPage("/Authentication/Login");
            }
            if (id == Guid.Empty)
            {
                return NotFound();
            }
			var useraccount = JsonConvert.DeserializeObject<User>(userJson);
			var userId = useraccount.Id;

			ImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(id);
            if (ImageRequest == null)
            {
                return NotFound();
            }
			if (userId != ImageRequest.RequesterUserId && userId != ImageRequest.ArtistId)
			{
				TempData["ErrorMessage"] = "You do not have permission to view this page.";
				return RedirectToPage("./Index"); 
			}

			RequestDetail = (IList<RequestDetail>)await _requestDetailService.GetRequestDetailByRequetIdAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptedAsync(Guid id)
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return RedirectToPage("/Authentication/Login");
            }

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
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            ImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(id);
            if (ImageRequest == null)
            {
                return NotFound();
            }
            var editImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(ImageRequest.Id);
            editImageRequest.RequesterUserId = ImageRequest.RequesterUserId;
            if (editImageRequest.RequestStatus == RequestStatus.USER_ACCEPTED || editImageRequest.RequestStatus == RequestStatus.ARTIST_ACCEPTED)
            {
                editImageRequest.RequestStatus = RequestStatus.ACCEPTED;
                await _imageRequestService.EditImageRequest(editImageRequest);
                TempData["SuccessMessage"] = "Details of Image Request is accepted successfully <3";
                return Redirect($"./Details?id={ImageRequest.Id.ToString()}");
            }

            if (userId != editImageRequest.RequesterUserId)
            {
                editImageRequest.RequestStatus = RequestStatus.ARTIST_ACCEPTED;
                await _imageRequestService.EditImageRequest(editImageRequest);
            }
            else
            {
                editImageRequest.RequestStatus = RequestStatus.USER_ACCEPTED;
                await _imageRequestService.EditImageRequest(editImageRequest);
            }

            TempData["SuccessMessage"] = "Details of Image Request is accepted successfully <3";
            return Redirect($"./Details?id={ImageRequest.Id.ToString()}");
        }

        public async Task<IActionResult> OnPostCanReAsync(Guid id)
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return RedirectToPage("/Authentication/Login");
            }
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
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            ImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(id);
            if (ImageRequest == null)
            {
                return NotFound();
            }
            var editImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(ImageRequest.Id);
            editImageRequest.RequesterUserId = ImageRequest.RequesterUserId;

            if (userId != editImageRequest.RequesterUserId)
            {
                editImageRequest.RequestStatus = RequestStatus.REJECTED;
                await _imageRequestService.EditImageRequest(editImageRequest);
            }
            else
            {
                editImageRequest.RequestStatus = RequestStatus.CANCELLED;
                await _imageRequestService.EditImageRequest(editImageRequest);
            }
            TempData["SuccessMessage"] = "Details of Image Request is accepted successfully <3";
            return Redirect($"./Details?id={ImageRequest.Id.ToString()}");
        }
        public async Task<IActionResult> OnPostUploadImage(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var editImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(id);
            if (editImageRequest == null)
            {
                return NotFound();
            }

            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    if (memoryStream.Length < 2097152)
                    {
                        // Gán giá trị cho editImageRequest thay vì ImageRequest trực tiếp
                        editImageRequest.ImageBlob = memoryStream.ToArray();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "The file is too large.";
                    }
                }
            }

            // Cập nhật trạng thái và lưu chỉnh sửa
            editImageRequest.RequestStatus = RequestStatus.UPLOADED;
            await _imageRequestService.EditImageRequest(editImageRequest);

            TempData["SuccessMessage"] = "Upload Image Request successfully <3";
            return Redirect("./Index");
        }

        public async Task<IActionResult> OnPostPayToDownLoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var editImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(id);
            if (editImageRequest == null)
            {
                return NotFound();
            }
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return RedirectToPage("/Authentication/Login");
            }
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
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            if (user.Balance < editImageRequest.Price)
            {
                TempData["ErrorMessage"] = "Your balance is not enough to download this image.";
                return Page();
            }
            else
            {
                await _userService.DecreaseBalance(user.Id, editImageRequest.Price);
                await _userService.IncreaseBalance(editImageRequest.ArtistId, editImageRequest.Price);
                editImageRequest.RequestStatus = RequestStatus.SUCCESS;
                await _imageRequestService.EditImageRequest(editImageRequest);
                ViewData["SuccessMessage"] = "You have succesfully pay the image to the artist";
                return Redirect($"./Details?id={ImageRequest.Id.ToString()}");
            }
        }
    }
}