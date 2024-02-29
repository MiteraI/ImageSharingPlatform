using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace ImageSharingPlatform.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;
        private readonly IImageCategoryService _imageCategoryService;

        public IndexModel(ISharedImageService sharedImageService, IImageCategoryService imageCategoryService)
        {
			_sharedImageService = sharedImageService;
            _imageCategoryService = imageCategoryService;
		}

        IList<SharedImage> SharedImages { get; set; }

        public async Task OnGetAsync()
        {
            var searchQuery = HttpContext.Request.Query["query"];
            SharedImages = (List<SharedImage>) await _sharedImageService.FindSharedImageWithSearchNameAndCate(searchQuery, null);
        }
    }
}
