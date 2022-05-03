using AngularProject.ViewModels;

namespace AngularProject.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail,string subject,string content);
    }
}
