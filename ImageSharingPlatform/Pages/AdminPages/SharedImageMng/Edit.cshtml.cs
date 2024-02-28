﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;

<<<<<<<< HEAD:ImageSharingPlatform/Pages/AdminPages/ShareImage/Edit.cshtml.cs
namespace ImageSharingPlatform.Pages.AdminPages.ShareImage
========
namespace ImageSharingPlatform.Pages.AdminPages.SharedImageMng
>>>>>>>> 389bcac198f0998964858186489bdd87c822de48:ImageSharingPlatform/Pages/AdminPages/SharedImageMng/Edit.cshtml.cs
{
    public class EditModel : PageModel
    {
        private readonly ISharedImageService _sharedImageService;

        public EditModel(ISharedImageService sharedImageService)
        {
            _sharedImageService = sharedImageService;
        }

        [BindProperty]
        public SharedImage SharedImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SharedImage = await _sharedImageService.GetSharedImageByIdAsync(id);

            if (SharedImage == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var sharedimage = _sharedImageService.EditSharedImage(SharedImage);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _sharedImageService.SharedImageExistsAsync(x => x.Id == SharedImage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
