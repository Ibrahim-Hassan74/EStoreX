using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using EStoreX.Core.ServiceContracts;
using EStoreX.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ServiceContracts;

namespace EStoreX.Infrastructure.Repository
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string?> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "UserName is already registered";
            }
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "Email is already registered";
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                return result.Errors.FirstOrDefault()?.Description;
            }

            await SendEmail(user);

            return "Registration successful";
        }

        public async Task<string?> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO is null)
                return null;
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
            {
                return "User not found";
            }

            if (!user.EmailConfirmed)
            {
                await SendEmail(user);
                return "User not confirmed. A confirmation email has been sent to your email address.";
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.RememberMe, true);
            if (result.Succeeded)
            {
                return "Login successful";
            }
            else if (result.IsLockedOut)
            {
                return "User is locked out";
            }
            else if (result.IsNotAllowed)
            {
                return "User is not allowed to login";
            }
            else
            {
                return "Invalid login attempt";
            }
        }


        private bool IsMobileDevice(HttpRequest? request)
        {
            if (request is null) return false;

            var userAgent = request.Headers["User-Agent"].ToString().ToLower();

            return userAgent.Contains("android") || userAgent.Contains("iphone") || userAgent.Contains("ipad");
        }

        private async Task SendEmail(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            user.LastEmailConfirmationToken = token;

            await _userManager.UpdateAsync(user);

            var request = _httpContextAccessor.HttpContext?.Request;
            var scheme = request?.Scheme ?? "https";
            var host = request?.Host.Value ?? "localhost:5000";

            string redirectUrl;
            if (IsMobileDevice(request))
            {
                redirectUrl = "estorex://account-verified";
            }
            else
            {
                redirectUrl = "https://loaclhost4200/active";
            }

            string confirmationLink = $"{scheme}://{host}/api/account/confirmemail?userId={user.Id}&token={Uri.EscapeDataString(token)}&redirectTo={Uri.EscapeDataString(redirectUrl)}";

            string html = EmailTemplateService.GetConfirmationEmailTemplate(confirmationLink);

            var emailDTO = new EmailDTO(user.Email, "Confirm Your Email", html);
            await _emailSender.SendEmailAsync(emailDTO);
        }

    }
}
