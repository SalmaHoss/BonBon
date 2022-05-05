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
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration configuration;
        private IMailService mailService;

        public UserService(UserManager<User> _userManager, SignInManager<User> _signInManager, RoleManager<IdentityRole> _roleManager, IConfiguration _configuration, IMailService _mailService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
            mailService = _mailService;
            roleManager = _roleManager;
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

            //Roles
            model.Role = "Customer";

            var identityUser = new User
            {
                Email = model.Email,
                UserName = model.Email,
                ProfileImage = model.ProfileImage,
                Gender = model.Gender,
                Role = model.Role,
            };

            var result = await userManager.CreateAsync(identityUser, model.Password);
            
            //roles
            await userManager.AddToRoleAsync(identityUser, model.Role);
            
            if (result.Succeeded)
            {
                var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);

                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                string url = $"{configuration["AppUrl"]}/api/auth/confirmemail?userId={identityUser.Id}&token={validEmailToken}";

                await mailService.SendEmailAsync(identityUser.Email, "Confirm your email",$"<h1>Welcome to BonBon Website</h1>"+
                    $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

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

            //get role assigned to the user 
            var role = await userManager.GetRolesAsync(user);
            IdentityOptions options = new IdentityOptions();

            var claims = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Token = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> ConfirmEmailASync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await userManager.ConfirmEmailAsync(user,normalToken); 
            
            if(result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true
                };
            }
            return new UserManagerResponse
            {
                Message = "Email isn't confirmed",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> ForgetPasswordASync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "No user associated with this email"
                };
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);

            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

            await mailService.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click here</a></p>");

            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = "reset password URL has been sent to the email Successfully!"
            };
        }

        public async Task<UserManagerResponse> ResetPasswordASync(ResetPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No user associated with this email"
                };
            }

            if (model.NewPassword != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new UserManagerResponse
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new UserManagerResponse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        public async Task<UserManagerResponse> LogoutUserAsync()
        {
            await signInManager.SignOutAsync();
            return new UserManagerResponse
            {
                Message = "User logged out successfully!",
                IsSuccess = true
            };
        }

        public Task<IdentityUser> UserExistAsync(string id)
        {
            var user = userManager.FindByIdAsync(id);
            return user;
        }


        /* ------------------------------------------------ Authorization -------------------------------------- */

        public async Task<UserManagerResponse> AddRoleAsync(RegisterViewModel model)
        {
            if (model == null || model.Role == "")
            {
                throw new NullReferenceException("Role is missing");
            }

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                return new UserManagerResponse
                {
                    Message = "Role already exists",
                    IsSuccess = true,
                };
            }

            var role = new IdentityRole() { Name = model.Role };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Role added successfully",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        public async Task<UserManagerResponse> GetRolesAsync()
        {
            var roles = roleManager.Roles.Select(x => x.Name).ToList();

            return new UserManagerResponse
            {
                Roles = roles,
                IsSuccess = true
            };
        }
    }
}
