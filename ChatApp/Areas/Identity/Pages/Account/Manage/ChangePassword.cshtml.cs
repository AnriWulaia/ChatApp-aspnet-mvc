using ChatApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApp.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ISharedService _sharedService;
        private readonly UserManager<SampleUser> _userManager;

        public ChangePasswordModel(UserManager<SampleUser> userManager, ISharedService sharedService)
        {
            _userManager = userManager;
            _sharedService = sharedService;
        }
        public string FirstName { get; set; }
        public void OnGet()
        {
            FirstName = _sharedService.GetFirstName();
        }
    }
}
