using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.ImageCategoryMng
{
    public class IndexModel : PageModel
    {
        private readonly IImageCategoryRepository _imageCategoryRepository;

        public IndexModel(IImageCategoryRepository imageCategoryRepository)
        {
            _imageCategoryRepository = imageCategoryRepository;
        }

        public IList<ImageCategory> ImageCategory { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ImageCategory = _imageCategoryRepository.GetAllAsync().Result.ToList();
        }
    }
}
