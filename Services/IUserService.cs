using AngularProject.ViewModels;
namespace AngularProject.Services
{
    public interface IUserService
    {
        /* ---------------------- Authentication -------------------- */
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> ConfirmEmailASync(string userId, string token);
        Task<UserManagerResponse> ForgetPasswordASync(string email);
        Task<UserManagerResponse> ResetPasswordASync(ResetPasswordViewModel model);
        Task<UserManagerResponse> LogoutUserAsync();


        /* ---------------------- Authorization -------------------- */
        Task<UserManagerResponse> AddRoleAsync(RegisterViewModel model);
        Task<UserManagerResponse> GetRolesAsync();
    }
}
