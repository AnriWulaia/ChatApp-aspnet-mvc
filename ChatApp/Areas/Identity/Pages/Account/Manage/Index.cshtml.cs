using ChatApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChatApp.Areas.Identity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly ISharedService _sharedService;
        private readonly UserManager<SampleUser> _userManager;
        public ProfileModel(ISharedService sharedService, UserManager<SampleUser> userManager)
        {
            _sharedService = sharedService;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Code { get; set; }
        }
        public async Task<IActionResult> OnPostUpdateImageAsync(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var username = User.Identity.Name;
                byte[] newImageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    newImageBytes = memoryStream.ToArray();
                }

                var updateResult = await _sharedService.UpdateProfileImage(username, newImageBytes);

                if (updateResult)
                {
                    // Handle success, e.g., redirect to a success page
                    return Page();
                }
                else
                {
                    // Handle failure, e.g., show an error message
                    ModelState.AddModelError(string.Empty, "Failed to update profile image.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to update profile image.");
            }

            // If we got this far, something failed, redisplay the form
            return Page();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

    }
}
