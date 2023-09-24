using ChatApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Areas.Identity.Pages.Account
{
    public interface ISharedService
    {
        string GetFirstName();
        byte[] GetImage();
        string GetFullName();
        Task<bool> UpdateProfileImage(string username, byte[] newImageBytes);
    }

    public class SharedService : ISharedService
    {
        private readonly UserManager<SampleUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedService(UserManager<SampleUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetFirstName()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (username != null)
            {
                var user = _userManager.FindByNameAsync(username).Result;
                return user.FirstName;
            }
            return "Guest"; // Handle the case when there's no authenticated user
        }
        public string GetFullName()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (username != null)
            {
                var user = _userManager.FindByNameAsync(username).Result;
                return user.FirstName + " " + user.LastName;
            }
            return "Guest"; // Handle the case when there's no authenticated user
        }
        public byte[] GetImage()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = _userManager.FindByNameAsync(username).Result;
            return user.ImageFile;
        }

        public async Task<bool> UpdateProfileImage(string username, byte[] newImageBytes)
        {
            if (username != null)
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    user.ImageFile = newImageBytes;

                    var result = await _userManager.UpdateAsync(user);

                    return result.Succeeded;
                }
            }

            return false;
        }
    }
}
