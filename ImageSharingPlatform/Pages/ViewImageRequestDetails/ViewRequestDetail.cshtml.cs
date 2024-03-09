using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Enums;
using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ViewImageRequestDetails
{
    public class ViewRequestDetailModel : PageModel
    {
        private readonly IRequestDetailService _requestDetailService;
        private readonly IImageRequestService _imageRequestService;
        private readonly IUserService _userService;

        public ViewRequestDetailModel(IRequestDetailService requestDetailService, IImageRequestService imageRequestService ,IUserService userService)
        {
            _requestDetailService = requestDetailService;
            _imageRequestService = imageRequestService;
            _userService = userService;
        }
        [BindProperty]
        public IList<ImageRequest> imageRequests { get; set; }

        public IList<RequestDetail> RequestDetail { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var imagerequests = await _imageRequestService.GetAllImageRequestsAsync();

            ViewData["RequestId"] = new SelectList(imagerequests, "Id", "Title");
            RequestDetail = (IList<RequestDetail>)await _requestDetailService.GetRequestDetailByRequetIdAsync(id);
            return Page();
        }
    }
}
