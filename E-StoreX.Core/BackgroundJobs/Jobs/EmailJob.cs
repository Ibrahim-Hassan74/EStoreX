using EStoreX.Core.BackgroundJobs.Interfaces;
using EStoreX.Core.DTO.Account.Requests;
using EStoreX.Core.ServiceContracts.Account;
using EStoreX.Core.ServiceContracts.Common;
using EStoreX.Core.Services.Common;
using Hangfire.Console;
using Hangfire.Server;

namespace EStoreX.Core.BackgroundJobs.Jobs
{
    public class EmailJob : IEmailJob
    {
        private readonly IUserManagementService _userService;
        private readonly IEmailSenderService _emailSender;

        public EmailJob(IUserManagementService userService, IEmailSenderService emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        public async Task SendWeeklyEmailsAsync(PerformContext context)
        {
            var users = await _userService.GetAllUsersAsync();
            context.WriteLine($"Starting weekly email job at {DateTime.Now}, total users: {users.Count}");

            foreach (var user in users)
            {
                if (!user.IsConfirmed)
                    continue;

                var email = new EmailDTO
                {
                    Email = user.Email,
                    Subject = "✨ Your Weekly E-StoreX Highlights Are Here!",
                    HtmlMessage = EmailTemplateService.GetWeeklyNewsletterTemplate(user.DisplayName)
                };
                await _emailSender.SendEmailAsync(email);

                context.WriteLine($"Email sent to {user.Email} at {DateTime.Now}");
            }

            context.WriteLine($"Weekly email job finished at {DateTime.Now}");
        }
    }
}
