﻿using EStoreX.Core.BackgroundJobs.Interfaces;
using EStoreX.Core.DTO.Account.Requests;
using EStoreX.Core.DTO.Orders.Responses;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Account;
using EStoreX.Core.ServiceContracts.Common;
using EStoreX.Core.ServiceContracts.Orders;
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
        private readonly IExportService _exportService;
        private readonly IOrderService _orderService;
        public EmailJob(IUserManagementService userService, IEmailSenderService emailSender, IUnitOfWork unitOfWork, IExportService exportService, IOrderService orderService)
        {
            _userService = userService;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _exportService = exportService;
            _orderService = orderService;
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
        public async Task SendActiveDiscountsEmailAsync(PerformContext context)
        {
            var activeDiscounts = (await _unitOfWork.DiscountRepository
                .GetActiveDiscountsAsync())
                .MaxBy(x => x.CurrentUsageCount);

            if (activeDiscounts == null)
            {
                context.WriteLine("⚠️ No active discounts found, skipping email job.");
                return;
            }

            context.WriteLine($"🎯 Selected discount code: {activeDiscounts.Code} ({activeDiscounts.Percentage}% off)");
            context.WriteLine($"⏳ Discount valid until: {activeDiscounts.EndDate?.ToString("yyyy-MM-dd") ?? "N/A"}");

            var users = await _userService.GetAllUsersAsync();
            context.WriteLine($"👥 Total users fetched: {users.Count}");

            foreach (var user in users)
            {
                if (!user.IsConfirmed)
                {
                    context.WriteLine($"⏭️ Skipping {user.Email} (not confirmed).");
                    continue;
                }

                var email = new EmailDTO
                {
                    Email = user.Email,
                    Subject = "🔥 Exclusive Discount Just for You!",
                    HtmlMessage = EmailTemplateService.GetDiscountEmailTemplate(
                        user.DisplayName,
                        activeDiscounts.Code,
                        activeDiscounts.Percentage,
                        activeDiscounts.EndDate ?? DateTime.UtcNow.AddDays(10)
                    )
                };

                await _emailSender.SendEmailAsync(email);
                context.WriteLine($"✅ Discount email sent to {user.Email}");
            }

            context.WriteLine("🎉 Discount email job completed successfully!");
        }


        public async Task SendDailySalesReportAsync(PerformContext context)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-1); 
            var endDate = DateTime.UtcNow.Date;

            context.WriteLine($"📊 Generating sales report from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            var report = await _orderService.GetSalesReportAsync(startDate, endDate);

            if (report == null || report.TotalOrders == 0)
            {
                context.WriteLine("⚠️ No sales data found for this period.");
                return;
            }

            var fileBytes = _exportService.ExportToExcel(new List<SalesReportResponse> { report });
            var fileName = $"sales-report-{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.xlsx";

            context.WriteLine($"✅ Sales report generated: {fileName}");

            var admins = await _userService.GetAdminsAsync(); 

            foreach (var admin in admins)
            {
                if (!admin.IsConfirmed)
                    continue;

                var email = new EmailDTO
                {
                    Email = admin.Email,
                    Subject = $"📈 Daily Sales Report ({startDate:yyyy-MM-dd})",
                    HtmlMessage = EmailTemplateService.GetDailySalesReportTemplate(startDate, endDate),
                    Attachments = new List<EmailAttachmentDTO>
                    {
                        new EmailAttachmentDTO
                        {
                            FileName = fileName,
                            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            FileBytes = fileBytes
                        }
                    }
                };

                await _emailSender.SendEmailAsync(email);
                context.WriteLine($"📧 Sales report sent to {admin.Email} at {DateTime.Now}");
            }
        }

    }
}
