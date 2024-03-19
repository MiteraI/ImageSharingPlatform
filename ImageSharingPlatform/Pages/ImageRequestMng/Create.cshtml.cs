using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Domain.Enums;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.ImageRequestMng
{
    public class CreateModel : PageModel
    {
        private readonly IImageRequestService _imageRequestService;
        private readonly IUserService _userService;

        public CreateModel(IImageRequestService imageRequestService, IUserService userService)
        {
            _imageRequestService = imageRequestService;
            _userService = userService;
        }
        [BindProperty]
        public ImageRequest ImageRequests { get; set; } = default!;
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "You must login to access";
                return RedirectToPage("/Authentication/Login");
            }

            if (id == Guid.Empty)
            {
                var users1 = await _userService.GetUserByRoles();
                List<User> listArtist = new List<User>();
                foreach (var user in users1)
                {
                    List<Role> userRole = user.Roles.ToList();
                    if (userRole.Count > 0)
                    {
                        foreach (var role in userRole)
                        {
                            if (role.UserRole.Equals(UserRole.ROLE_ARTIST))
                            {
                                listArtist.Add(user);
                            }
                        }
                    }
                }
                ViewData["ArtistId"] = new SelectList(listArtist, "Id", "Email");
                ViewData["RequesterUserId"] = new SelectList(users1, "Id", "Email");
                return Page();
            } else
            {
                var users = await _userService.GetAllUsersAsync();
                users = users.Where(u => u.Id == id).ToList();
                ViewData["ArtistId"] = new SelectList(users, "Id", "Email");
                ViewData["RequesterUserId"] = new SelectList(users, "Id", "Email");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userJson = HttpContext.Session.GetString("LoggedInUser");
            var useraccount = JsonConvert.DeserializeObject<User>(userJson);
            var userId = useraccount.Id;

            if (userId == Guid.Empty)
            {
                return NotFound("User not found.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            ImageRequests.RequesterUserId = userId;
            ImageRequests.CreateTime = DateTime.Now;
            ImageRequests.RequestStatus = RequestStatus.PROCESSING;
            if (ImageRequests.RequesterUserId == ImageRequests.ArtistId)
            {
                TempData["ErrorMessage"] = "Cannot create duplicated !";
                return Redirect("./Create");
            }
            if (ImageRequests.ExpectedTime < ImageRequests.CreateTime)
            {
                TempData["ErrorMessage"] = "The expected time cannot be less than the created time !";
                return Redirect("./Create");
            }

            var newImageRequest = _imageRequestService.CreateImageRequest(ImageRequests);

            if (newImageRequest == null)
            {
                TempData["ErrorMessage"] = "Cannot create a Image Request!";
                return Redirect("./Index");
            }
            TempData["SuccessMessage"] = "The request is created successfully !";
            return Redirect("./Index");
        }
    }
}
