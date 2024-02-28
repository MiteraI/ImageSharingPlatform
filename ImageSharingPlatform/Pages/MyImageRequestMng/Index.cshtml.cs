using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Pages.MyImageRequestMng
{
    public class IndexModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;

        public IndexModel(IImageRequestService imageRequestService)
        {
            _imageRequestService = imageRequestService;
        }

        public IList<ImageRequest> ImageRequests { get;set; } = default!;

        public async Task OnGetAsync(Guid userid)
        {
            ImageRequests = (IList<ImageRequest>)await _imageRequestService.GetAllImageRequestsByUserAsync(userid);
        }
    }
}
