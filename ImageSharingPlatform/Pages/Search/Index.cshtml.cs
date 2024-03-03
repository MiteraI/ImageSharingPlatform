using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using JHipsterNet.Core.Pagination;
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

		public int PageNumber { get; set; } = 0;
		public int PageSize { get; set; } = 4;
        public string SearchQuery { get; set; }
        public string CategoryId { get; set; }
		public IPage<SharedImage> SharedImages { get; set; }
        public IList<ImageCategory> ImageCategories { get; set; }

        public async Task OnGetAsync()
        {
			var page = HttpContext.Request.Query["page"];

			if (!string.IsNullOrWhiteSpace(page) && int.TryParse(page, out var pageNumber))
			{
				PageNumber = pageNumber;
			}

			var searchQuery = HttpContext.Request.Query["query"];
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
				SearchQuery = searchQuery;
			}

            ImageCategories = (List<ImageCategory>)await _imageCategoryService.GetAllImageCategoriesAsync();

            var cateGuid = HttpContext.Request.Query["category"];
            
            if (!string.IsNullOrWhiteSpace(cateGuid))
            {
                SharedImages = await _sharedImageService.FindSharedImageWithSearchNameAndCatePageable(searchQuery, new Guid(cateGuid), Pageable.Of(PageNumber, PageSize, null));
                CategoryId = cateGuid;
            } else
            {
                SharedImages = await _sharedImageService.FindSharedImageWithSearchNameAndCatePageable(searchQuery, null, Pageable.Of(PageNumber, PageSize, null));
            }
        }
    }
}
