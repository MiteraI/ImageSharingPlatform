using AutoMapper;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Dto;
using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageSharingPlatform.Pages.Authentication
{
    public class RegisterModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        [BindProperty]
        public UserEditDto InputUser { get; set; }

        public RegisterModel(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _userService.CheckDuplicateUsername(InputUser.Username))
            {
				ModelState.AddModelError("InputUser.Username", "Username already exists");
				return Page();
			}

            if (await _userService.CheckDuplicateEmail(InputUser.Email))
            {
                ModelState.AddModelError("InputUser.Email", "Email already exists");
                return Page();
            }

            var user = _mapper.Map<User>(InputUser);
            user = await _userService.RegisterUser(user);

            return Redirect("/Authentication/Login");
        }
    }
}
