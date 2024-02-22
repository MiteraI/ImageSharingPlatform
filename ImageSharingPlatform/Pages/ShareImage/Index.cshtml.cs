using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.ShareImage
{
    public class IndexModel : PageModel
    {
        private readonly ISharedImageRepository _sharedImageRepository;

        public IndexModel(ISharedImageRepository sharedImageRepository)
        {
            _sharedImageRepository = sharedImageRepository;
        }

        public IList<SharedImage> SharedImage { get; set; } = default!;

        public async Task OnGetAsync()
        {
            SharedImage = _sharedImageRepository.GetAllAsync().Result.ToList();
        }
    }
}
