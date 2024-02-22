using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;

namespace ImageSharingPlatform.Pages.UserMng
{
    public class DetailsModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public DetailsModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _userRepository.GetOneAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
