using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ImageSharingPlatform.Domain.Enums;

namespace ImageSharingPlatform.Pages.AdminPages.ImageCategoryMng
{
    public class IndexModel : PageModel
    {
        private readonly IImageCategoryService _imageCategoryService;

        public IndexModel(IImageCategoryService imageCategoryService)
        {
            _imageCategoryService = imageCategoryService;
        }

        public IList<ImageCategory> ImageCategory { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ImageCategory = _imageCategoryService.GetAllImageCategoriesAsync().Result.ToList();
        }
    }
}
