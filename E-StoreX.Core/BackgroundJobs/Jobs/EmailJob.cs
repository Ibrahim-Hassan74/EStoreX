using EStoreX.Core.BackgroundJobs.Interfaces;
using EStoreX.Core.DTO.Account.Requests;
using EStoreX.Core.RepositoryContracts.Common;
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
        private readonly IUnitOfWork _unitOfWork;
        public EmailJob(IUserManagementService userService, IEmailSenderService emailSender, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
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

        public async Task SendOrderConfirmationEmailAsync(Guid orderId, PerformContext context)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, d => d.DeliveryMethod, u => u.Buyer);

            if (order == null)
            {
                context?.WriteLine($"Order {orderId} not found.");
                return;
            }

            var email = new EmailDTO
            {
                Email = order.BuyerEmail,
                Subject = "🛒 Order Confirmation - E-StoreX",
                HtmlMessage = EmailTemplateService.GetOrderConfirmationEmailTemplate(order)
            };

            await _emailSender.SendEmailAsync(email);
            context?.WriteLine($"Order confirmation email sent to {order.BuyerEmail}");
        }
        public async Task SendPaymentFailedEmailAsync(Guid orderId, PerformContext context)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, d => d.DeliveryMethod, u => u.Buyer);

            if (order == null)
            {
                context?.WriteLine($"Order {orderId} not found.");
                return;
            }

            var email = new EmailDTO
            {
                Email = order.BuyerEmail,
                Subject = "⚠️ Payment Failed - E-StoreX",
                HtmlMessage = EmailTemplateService.GetPaymentFailedEmailTemplate(order)
            };

            await _emailSender.SendEmailAsync(email);
            context?.WriteLine($"Payment failed email sent to {order.BuyerEmail}");
        }
    }
}
