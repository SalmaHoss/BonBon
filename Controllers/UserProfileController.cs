using AngularProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        /// <summary>
        /// not tested
        /// </summary>
        private UserManager<RegisterViewModel> userManager;
        public UserProfileController(UserManager<RegisterViewModel> _userManager)
        {
            userManager = _userManager;
        }
        [HttpGet]
        [Authorize]

        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await userManager.FindByIdAsync(userId);
            return new
            {
                user.Username,
                user.Email,
                user.ProfileImage,
                user.Gender
            };
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<Object> PutUserProfile(User _user)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await userManager.FindByIdAsync(userId);
            user.Username = _user.UserName;
            user.Email = _user.Email;
            user.ProfileImage = _user.ProfileImage;
            user.Gender = _user.Gender;

            return user;

        }

    }
}
