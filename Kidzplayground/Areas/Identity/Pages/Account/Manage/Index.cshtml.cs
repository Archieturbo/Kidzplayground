// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Kidzplayground.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;


namespace Kidzplayground.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<KidzplaygroundUser> _userManager;
        private readonly SignInManager<KidzplaygroundUser> _signInManager;
        

        public IndexModel(
            UserManager<KidzplaygroundUser> userManager,
            SignInManager<KidzplaygroundUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           
        }
        [BindProperty]
        public IFormFile ProfilePicture {  get; set; }
        public string ProfileImagePath { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            
            public string PhoneNumber { get; set; }

            
            public IFormFile ProfilePicture { get; set; }
        }

        private async Task LoadAsync(KidzplaygroundUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var profileImage = user.ProfileImage ?? "default-profile.png";

            Username = userName;
            ProfileImagePath = profileImage;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.ProfilePicture != null)
            {
                var uploadsDirectoryPath = Path.Combine("wwwroot", "ProfileImage");

                // Skapa mappen om den inte finns
                if (!Directory.Exists(uploadsDirectoryPath))
                {
                    Directory.CreateDirectory(uploadsDirectoryPath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.ProfilePicture.FileName);
                var filePath = Path.Combine(uploadsDirectoryPath, uniqueFileName);

                // Spara den nya bilden
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(fileStream);
                }

                // Ta bort den gamla bilden om den finns
                if (!string.IsNullOrEmpty(user.ProfileImage) && user.ProfileImage != "default-profile.png")
                {
                    var oldProfilePicturePath = Path.Combine(uploadsDirectoryPath, user.ProfileImage);
                    if (System.IO.File.Exists(oldProfilePicturePath))
                    {
                        System.IO.File.Delete(oldProfilePicturePath);
                    }
                }

                // Uppdatera användarens profilbild
                user.ProfileImage = uniqueFileName;
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "nånting gick fel.";
                    return RedirectToPage();
                }
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Din profilbild har blivit uppdaterad!";
            return RedirectToPage();
        }

    }
}
