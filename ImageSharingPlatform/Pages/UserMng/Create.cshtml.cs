using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Utils;

namespace ImageSharingPlatform.Pages.UserMng
{
    public class CreateModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public CreateModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User.Password = PasswordHasher.HashPassword(User.Password);

            _userRepository.Add(User);
            await _userRepository.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
