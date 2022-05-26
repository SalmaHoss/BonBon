using AngularProject.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AngularProject.Services
{
    public class SendGridMailService : IMailService
    {
        private IConfiguration configuration;
        public SendGridMailService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("nosamaosman@gmail.com", "BonBon");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
