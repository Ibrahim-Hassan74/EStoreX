namespace EStoreX.Core.Services.Common
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

        public static string GetWeeklyNewsletterTemplate(string userName)
        {
            return $@"
<!DOCTYPE html>
<html lang='en'>
  <head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>E-StoreX Weekly Newsletter</title>
    <style>
      body {{
        background-color: #f4f6f9;
        font-family: 'Segoe UI', Arial, sans-serif;
        margin: 0;
        padding: 0;
        color: #1e293b;
      }}
      .container {{
        max-width: 650px;
        width: 95%;
        margin: 20px auto;
        background: #ffffff;
        border-radius: 14px;
        box-shadow: 0 6px 18px rgba(0, 0, 0, 0.06);
        overflow: hidden;
      }}
      .header {{
        background: linear-gradient(135deg, #0f172a, #2563eb);
        padding: 32px 20px;
        text-align: center;
        color: #ffffff;
        font-size: 28px;
        font-weight: 700;
        letter-spacing: 0.5px;
      }}
      .body {{
        padding: 36px 28px;
        font-size: 18px;
      }}
      .body h2 {{
        margin-bottom: 18px;
        font-size: 26px;
        font-weight: 600;
        color: #0f172a;
      }}
      .body p {{
        color: #475569;
        font-size: 18px;
        line-height: 1.8;
        margin-bottom: 22px;
      }}
      .highlight {{
        background-color: #fff7ed;
        border-left: 5px solid #f97316;
        padding: 18px 20px;
        border-radius: 8px;
        margin-bottom: 26px;
        font-size: 16px;
        color: #7c2d12;
      }}
      .cta {{
        text-align: center;
        margin: 30px 0;
      }}
      .cta a {{
        display: inline-block;
        padding: 14px 32px;
        background-color: #2563eb;
        color: #ffffff;
        text-decoration: none;
        border-radius: 8px;
        font-weight: 600;
        font-size: 16px;
        transition: all 0.3s ease;
      }}
      .cta a:hover {{
        background-color: #1d4ed8;
      }}
      .footer {{
        padding: 20px;
        text-align: center;
        font-size: 16px;
        color: #475569;
        border-top: 1px solid #e2e8f0;
        background-color: #f8fafc;
      }}

      /* Responsive styles */
      @media only screen and (max-width: 480px) {{
        .header {{
          font-size: 24px;
          padding: 24px 15px;
        }}
        .body {{
          padding: 24px 15px;
          font-size: 16px;
        }}
        .body h2 {{
          font-size: 22px;
        }}
        .highlight {{
          padding: 14px 15px;
          font-size: 14px;
        }}
        .cta a {{
          padding: 12px 24px;
          font-size: 15px;
        }}
        .footer {{
          font-size: 14px;
        }}
      }}
    </style>
  </head>
  <body>
    <div class='container'>
      <div class='header'>✨ E-StoreX Weekly Newsletter</div>
      <div class='body'>
        <h2>Hi {userName}, 👋</h2>
        <p>
          We’re thrilled to share the latest updates and highlights from
          <strong>E-StoreX</strong> this week.
        </p>

        <div class='highlight'>
          🌟 Exciting new features are coming soon to improve your
          experience.<br />
          ⚡ Faster performance & smoother navigation across the platform.<br />
          🤝 Thanks for being part of our growing community!
        </div>

        <p>
          Stay tuned — more updates, tips, and exclusive insights are on the way
          🚀
        </p>
        <div class='cta'>
          <a href='https://estorex.runasp.net/swagger/index.html'>Open Dashboard</a>
        </div>
      </div>
      <div class='footer'>
        &copy; {DateTime.Now.Year} E-StoreX. Developed by Ibrahim Hassan.
      </div>
    </div>
  </body>
</html>";
        }

    }

}
