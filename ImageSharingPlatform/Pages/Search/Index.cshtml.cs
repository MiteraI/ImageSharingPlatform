using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public IList<SharedImage> SharedImages { get; set; }
        public IList<ImageCategory> ImageCategories { get; set; }

        public async Task OnGetAsync()
        {
            var searchQuery = HttpContext.Request.Query["query"];
            ImageCategories = (List<ImageCategory>)await _imageCategoryService.GetAllImageCategoriesAsync();
            
            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["category"].ToString()) || HttpContext.Request.Query["category"].Count > 0)
            {
                var cateGuid = new Guid(HttpContext.Request.Query["category"].ToString());
                SharedImages = (List<SharedImage>)await _sharedImageService.FindSharedImageWithSearchNameAndCate(searchQuery, cateGuid);
            } else
            {
                SharedImages = (List<SharedImage>)await _sharedImageService.FindSharedImageWithSearchNameAndCate(searchQuery, null);
            }
        }
    }
}
