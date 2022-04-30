using AngularProject.Models;
using AngularProject.Services;
using AngularProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Controllers
{
    public class AuthController : Controller
    {
        private IUserService userService;
        private IMailService mailService;
        private IConfiguration configuration;

        public AuthController(IUserService _userService, IMailService _mailService, IConfiguration _configuration)
        {
            userService = _userService;
            mailService = _mailService;
            configuration = _configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.RegisterUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code: 200 

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    //Confirmation Mail
                    //await mailService.SendEmailAsync(model.Email, "New login", "<h1>A new login to your account noticed, </h1><p>New login to your account at " + DateTime.Now + "</p>");
                    
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }

        //[HttpGet("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        //        return NotFound();

        //    var result = await userService.ConfirmEmailAsync(userId, token);

        //    if (result.IsSuccess)
        //    {
        //        //This html page will change according to our angular UI
        //        return Redirect($"{configuration["AppUrl"]}/ConfirmEmail.html"); //in wwwroot
        //    }

        //    return BadRequest(result);
        //}

        //[HttpPost("ForgetPassword")]
        //public async Task<IActionResult> ForgetPassword(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //        return NotFound();

        //    var result = await userService.ForgetPasswordAsync(email);

        //    if (result.IsSuccess)
        //        return Ok(result); // 200

        //    return BadRequest(result); // 400
        //}

        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await userService.ResetPasswordAsync(model);

        //        if (result.IsSuccess)
        //            return Ok(result);

        //        return BadRequest(result);
        //    }

        //    return BadRequest("Some properties are not valid");
        //}
    }
}
