using AngularProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using AngularProject.Models;

namespace AngularProject.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IConfiguration configuration;
        private IMailService mailService;

        public UserService(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, IConfiguration _configuration, IMailService _mailService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
            mailService = _mailService;
        }

        public async  Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if(model == null)
            {
                throw new NullReferenceException("Register Model is null");

            }
            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false
                };
            }
            var Identityuser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await userManager.CreateAsync(Identityuser,model.Password);

            if (result.Succeeded)
            {
                //var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(Identityuser);

                //var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                //var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                //string url = $"{configuration["AppUrl"]}/api/auth/confirmemail?userid={Identityuser.Id}&token={validEmailToken}";

                //await mailService.SendEmailAsync(Identityuser.Email, "Confirm your email", $"<h1>Welcome to BonBon Auth</h1>" +
                //    $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "User wasn't created",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with this Email address",
                    IsSuccess = false,
                };
            }

            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };

            var claims = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        //public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        //{
        //    var user = await userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        return new UserManagerResponse
        //        {
        //            IsSuccess = false,
        //            Message = "User not found"
        //        };

        //    var decodedToken = WebEncoders.Base64UrlDecode(token);
        //    string normalToken = Encoding.UTF8.GetString(decodedToken);

        //    var result = await userManager.ConfirmEmailAsync(user, normalToken);

        //    if (result.Succeeded)
        //        return new UserManagerResponse
        //        {
        //            Message = "Email confirmed successfully!",
        //            IsSuccess = true,
        //        };

        //    return new UserManagerResponse
        //    {
        //        IsSuccess = false,
        //        Message = "Email did not confirm",
        //        Errors = result.Errors.Select(e => e.Description)
        //    };
        //}

        //public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        //{
        //    var user = await userManager.FindByEmailAsync(email);
        //    if (user == null)
        //        return new UserManagerResponse
        //        {
        //            IsSuccess = false,
        //            Message = "No user associated with this email",
        //        };

        //    var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //    var encodedToken = Encoding.UTF8.GetBytes(token);
        //    var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        //    string url = $"{configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

        //    await mailService.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
        //        $"<p>To reset your password <a href='{url}'>Click here</a></p>");

        //    return new UserManagerResponse
        //    {
        //        IsSuccess = true,
        //        Message = "Reset password URL has been sent to the email successfully!"
        //    };
        //}

        //public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        //{
        //    var user = await userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //        return new UserManagerResponse
        //        {
        //            IsSuccess = false,
        //            Message = "No user associated with email",
        //        };

        //    if (model.NewPassword != model.ConfirmPassword)
        //        return new UserManagerResponse
        //        {
        //            IsSuccess = false,
        //            Message = "Password doesn't match its confirmation",
        //        };

        //    var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
        //    string normalToken = Encoding.UTF8.GetString(decodedToken);

        //    var result = await userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

        //    if (result.Succeeded)
        //        return new UserManagerResponse
        //        {
        //            Message = "Password has been reset successfully!",
        //            IsSuccess = true,
        //        };

        //    return new UserManagerResponse
        //    {
        //        Message = "Something went wrong",
        //        IsSuccess = false,
        //        Errors = result.Errors.Select(e => e.Description),
        //    };
        //}

        public async Task<UserManagerResponse> LogoutUserAsync()
        {
            await signInManager.SignOutAsync();
            return new UserManagerResponse
            {
                Message = "User logged out successfully!",
                IsSuccess = true
            };
        }
    }
}
