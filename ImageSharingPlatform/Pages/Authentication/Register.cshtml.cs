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


            var user = await _userService.RegisterUser(_mapper.Map<User>(InputUser));

            if (user == null)
            {
                // Handle registration failure
                return Page();
            }

            // Redirect on successful registration, adjust as needed
            return RedirectToPage("/Index");
        }
    }
}
