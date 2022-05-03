using AngularProject.ViewModels;
namespace AngularProject.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> ConfirmEmailASync(string userId, string token);
        Task<UserManagerResponse> ForgetPasswordASync(string email);
        Task<UserManagerResponse> ResetPasswordASync(ResetPasswordViewModel model);
        Task<UserManagerResponse> LogoutUserAsync();
    }
}
