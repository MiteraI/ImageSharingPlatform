using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DiaSymReader;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.AdminPages.ImageRequestMng
{
    public class CreateRequestDetailModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;
        private readonly IRequestDetailService _requestDetailService;
        private readonly IUserService _userService;

        public CreateRequestDetailModel(IImageRequestService imageRequestService, IRequestDetailService requestDetailService, IUserService userService)
        {
            _imageRequestService = imageRequestService;
            _requestDetailService = requestDetailService;
            _userService = userService;
        }

        [BindProperty]
        public ImageRequest ImageRequest { get; set; }
        [BindProperty]
        public RequestDetail RequestDetail { get; set; } = new RequestDetail();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(id);
            if (ImageRequest == null)
            {
                return NotFound();
            }
            RequestDetail.RequestId = id;
            RequestDetail.NewPrice = ImageRequest.Price;
            RequestDetail.ExpectedTime = ImageRequest.ExpectedTime;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

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

            RequestDetail.CreatedAt = DateTime.Now;
            RequestDetail.UserId = userId;

            var newRequestDetail = await _requestDetailService.AddRequestDetailAsync(RequestDetail);

            var updateImageRequest = await _imageRequestService.GetImageRequestByIdWithFullDetailsAsync(RequestDetail.RequestId);

            if (updateImageRequest == null)
            {
                return NotFound("Image Request not found.");
            }

            updateImageRequest.Price = (double)newRequestDetail.NewPrice;
            updateImageRequest.ExpectedTime = newRequestDetail.ExpectedTime;
            updateImageRequest.RequestStatus = Domain.Enums.RequestStatus.PROCESSING;
            await _imageRequestService.EditImageRequest(updateImageRequest);
            return RedirectToPage("./Details", new {id = RequestDetail.RequestId});
        }
    }
}
