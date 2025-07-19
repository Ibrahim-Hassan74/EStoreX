using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreX.Core.Services
{
    public static class EmailTemplateService
    {
        public static string GetConfirmationEmailTemplate(string? confirmationLink)
        {
            return $@"
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <style>
            body {{
                background-color: #f9fafb;
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                margin: 0;
                padding: 0;
            }}
            .container {{
                max-width: 600px;
                margin: 40px auto;
                background: #fff;
                border-radius: 12px;
                box-shadow: 0 8px 20px rgba(0,0,0,0.05);
                overflow: hidden;
            }}
            .header {{
                background-color: #0e7490;
                padding: 24px;
                text-align: center;
                color: #ffffff;
                font-size: 24px;
                font-weight: bold;
                letter-spacing: 1px;
            }}
            .body {{
                padding: 32px 24px;
            }}
            .body h2 {{
                margin-bottom: 16px;
                font-size: 22px;
                color: #0f172a;
            }}
            .body p {{
                color: #475569;
                font-size: 15px;
                line-height: 1.6;
                margin-bottom: 24px;
            }}
            .cta {{
                text-align: center;
            }}
            .cta a {{
                display: inline-block;
                padding: 12px 28px;
                background-color: #0e7490;
                color: #ffffff;
                text-decoration: none;
                border-radius: 6px;
                font-weight: 600;
                transition: background-color 0.3s ease;
            }}
            .cta a:hover {{
                background-color: #0c637a;
            }}
            .footer {{
                padding: 16px;
                text-align: center;
                font-size: 12px;
                color: #94a3b8;
                border-top: 1px solid #e2e8f0;
                background-color: #f1f5f9;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>E-StoreX</div>
            <div class='body'>
                <h2>Confirm Your Email</h2>
                <p>Hi there,</p>
                <p>Thanks for signing up with <strong>E-StoreX</strong>. Click the button below to confirm your email address and get started:</p>
                <div class='cta'>
                    <a href='{confirmationLink}'>Confirm Email</a>
                </div>
                <p>If you didn’t create this account, you can safely ignore this email.</p>
            </div>
            <div class='footer'>&copy; {DateTime.Now.Year} E-StoreX. Developed by Ibrahim Hassan.</div>
        </div>
    </body>
    </html>";
        }

        public static string GetPasswordResetEmailTemplate(string? resetLink)
        {
            return $@"
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <style>
            body {{
                background-color: #f9fafb;
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                margin: 0;
                padding: 0;
            }}
            .container {{
                max-width: 600px;
                margin: 40px auto;
                background: #fff;
                border-radius: 12px;
                box-shadow: 0 8px 20px rgba(0,0,0,0.05);
                overflow: hidden;
            }}
            .header {{
                background-color: #e11d48;
                padding: 24px;
                text-align: center;
                color: #ffffff;
                font-size: 24px;
                font-weight: bold;
                letter-spacing: 1px;
            }}
            .body {{
                padding: 32px 24px;
            }}
            .body h2 {{
                margin-bottom: 16px;
                font-size: 22px;
                color: #0f172a;
            }}
            .body p {{
                color: #475569;
                font-size: 15px;
                line-height: 1.6;
                margin-bottom: 24px;
            }}
            .cta {{
                text-align: center;
            }}
            .cta a {{
                display: inline-block;
                padding: 12px 28px;
                background-color: #e11d48;
                color: #ffffff;
                text-decoration: none;
                border-radius: 6px;
                font-weight: 600;
                transition: background-color 0.3s ease;
            }}
            .cta a:hover {{
                background-color: #be123c;
            }}
            .footer {{
                padding: 16px;
                text-align: center;
                font-size: 12px;
                color: #94a3b8;
                border-top: 1px solid #e2e8f0;
                background-color: #f1f5f9;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>E-StoreX</div>
            <div class='body'>
                <h2>Reset Your Password</h2>
                <p>We received a request to reset the password for your E-StoreX account. Click the button below to continue:</p>
                <div class='cta'>
                    <a href='{resetLink}'>Reset Password</a>
                </div>
                <p>If you didn’t request a password reset, you can safely ignore this email. This link is valid for 10 minutes.</p>
            </div>
            <div class='footer'>&copy; {DateTime.Now.Year} E-StoreX. Developed by Ibrahim Hassan.</div>
        </div>
    </body>
    </html>";
        }

    }
}
