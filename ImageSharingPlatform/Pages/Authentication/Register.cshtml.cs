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

            try
            {
                var isAvailable = await _userService.CheckRegisterUser(InputUser.Username, InputUser.Email);

                if (isAvailable == null)
                {
                    var user = await _userService.RegisterUser(_mapper.Map<User>(InputUser));

                    if (user != null)
                    {
                        TempData["SuccessMessage"] = "Register successfully <3";
                        return RedirectToPage("/Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }
    }
}
