using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SufraX.Core.Services
{
    public static class EmailTemplateService
    {
        public static string GetConfirmationEmailTemplate(string? confirmationLink)
        {
            return $@"
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                    body {{
                        margin: 0;
                        padding: 0;
                        background-color: #f2f4f6;
                        font-family: 'Segoe UI', sans-serif;
                        color: #333;
                    }}
                    .email-wrapper {{
                        width: 100%;
                        padding: 20px;
                    }}
                    .email-content {{
                        max-width: 600px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        border-radius: 10px;
                        overflow: hidden;
                        box-shadow: 0 4px 12px rgba(0,0,0,0.05);
                    }}
                    .email-header {{
                        background-color: #4CAF50;
                        padding: 20px;
                        text-align: center;
                        color: white;
                    }}
                    .email-body {{
                        padding: 30px 20px;
                    }}
                    .email-body h1 {{
                        font-size: 24px;
                        margin-bottom: 20px;
                    }}
                    .email-body p {{
                        font-size: 16px;
                        line-height: 1.6;
                        margin-bottom: 20px;
                    }}
                    .cta-button {{
                        display: inline-block;
                        background-color: #4CAF50;
                        color: white;
                        padding: 12px 24px;
                        text-decoration: none;
                        border-radius: 5px;
                        font-weight: bold;
                        transition: background-color 0.3s ease;
                    }}
                    .cta-button:hover {{
                        background-color: #45a049;
                    }}
                    .email-footer {{
                        text-align: center;
                        font-size: 13px;
                        color: #888;
                        padding: 20px;
                        border-top: 1px solid #eee;
                    }}
                </style>
            </head>
            <body>
                <div class='email-wrapper'>
                    <div class='email-content'>
                        <div class='email-header'>
                            <h2>Contact Manager</h2>
                        </div>
                        <div class='email-body'>
                            <h1>Confirm Your Email Address</h1>
                            <p>Thank you for registering with Contact Manager. To complete your registration, please confirm your email address by clicking the button below:</p>
                            <p style='text-align:center;'>
                                <a href='{confirmationLink}' class='cta-button'>Confirm Email</a>
                            </p>
                            <p>If you didn’t create this account, you can safely ignore this email.</p>
                        </div>
                        <div class='email-footer'>
                            &copy; {DateTime.Now.Year} Contact Manager. All rights reserved.
                        </div>
                    </div>
                </div>
            </body>
            </html>";
        }


        public static string GetPasswordResetEmailTemplate(string? resetLink)
        {
            return $@"
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            margin: 0;
                            padding: 0;
                            background-color: #f2f4f6;
                            font-family: 'Segoe UI', sans-serif;
                            color: #333;
                        }}
                        .email-wrapper {{
                            width: 100%;
                            padding: 20px;
                        }}
                        .email-content {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            border-radius: 10px;
                            overflow: hidden;
                            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
                        }}
                        .email-header {{
                            background-color: #4CAF50;
                            padding: 20px;
                            text-align: center;
                            color: white;
                        }}
                        .email-body {{
                            padding: 30px 20px;
                        }}
                        .email-body h1 {{
                            font-size: 24px;
                            margin-bottom: 20px;
                        }}
                        .email-body p {{
                            font-size: 16px;
                            line-height: 1.6;
                            margin-bottom: 20px;
                        }}
                        .cta-button {{
                            display: inline-block;
                            background-color: #4CAF50;
                            color: white;
                            padding: 12px 24px;
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold;
                            transition: background-color 0.3s ease;
                        }}
                        .cta-button:hover {{
                            background-color: #45a049;
                        }}
                        .email-footer {{
                            text-align: center;
                            font-size: 13px;
                            color: #888;
                            padding: 20px;
                            border-top: 1px solid #eee;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-wrapper'>
                        <div class='email-content'>
                            <div class='email-header'>
                                <h2>Contact Manager</h2>
                            </div>
                            <div class='email-body'>
                                <h1>Password Reset Request</h1>
                                <p>We received a request to reset your password. If you made this request, click the button below to reset your password.</p>
                                <p style='text-align:center;'>
                                    <a href='{resetLink}' class='cta-button'>Reset Password</a>
                                </p>
                                <p>If you didn't request a password reset, please ignore this email. This link will expire in 10 minutes for security reasons.</p>
                            </div>
                            <div class='email-footer'>
                                &copy; {DateTime.Now.Year} Contact Manager. All rights reserved.
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }

    }
}
