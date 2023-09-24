using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApp.Areas.Identity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly ISharedService _sharedService;

        public ProfileModel(ISharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile imageFile)
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
    }
}
