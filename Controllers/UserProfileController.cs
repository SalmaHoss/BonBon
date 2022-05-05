﻿using AngularProject.Models;
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

        private ApplicationDbContext context;
        private UserManager<User> userManager;

        public UserProfileController(ApplicationDbContext _context, UserManager<User> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<Object> GetAllUsers()
        {
            var users = userManager.Users.ToList();
            List<User> allUsers = new List<User>();
            foreach(var user in users)
            {
                await userManager.FindByIdAsync(user.Id);

                allUsers.Add(new User()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfileImage = user.ProfileImage,
                    Gender = user.Gender,
                    Role = user.Role,
                });
            }
            
            return allUsers;
        }

        //[Authorize(Roles = "Admin, Customer")]
        [HttpGet("GetUser/{id}")]
        public async Task<Object> GetUserProfile(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return new
            {
                user.UserName,
                user.Email,
                user.ProfileImage,
                user.Gender,
                user.Role
            };
        }

        //[Authorize(Roles ="Admin, Customer")]
        [HttpPut("EditUser/{id}")]
        public async Task<Object> EditUserProfile(string id, RegisterViewModel _user)
        {
            //string userId = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            await userManager.RemoveFromRoleAsync(user, user.Role);

            user.UserName = _user.Username;
            user.Email = _user.Email;
            user.ProfileImage = _user.ProfileImage;
            user.Gender = _user.Gender;
            user.Role = _user.Role;


            await userManager.UpdateAsync(user);

            await userManager.AddToRoleAsync(user, _user.Role);

            return user;
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUserProfile(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
