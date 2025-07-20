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
        private readonly IJwtService _jwtService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender, 
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
        }
        public async Task<AuthenticationResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid registration data.",
                    StatusCode = 400
                };
            }

            if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Username is already registered.",
                    StatusCode = 409
                };
            }

            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email is already registered.",
                    StatusCode = 409
                };
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
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.",
                    StatusCode = 400
                };
            }


            await SendEmail(user);

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Registration successful. Please check your email to confirm your account.",
                StatusCode = 200
            };
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid login data.",
                    StatusCode = 400
                };
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404
                };
            }

            if (!user.EmailConfirmed)
            {
                await SendEmail(user);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Your email is not confirmed yet. A confirmation email has been sent to your inbox. Please check your Spam, Junk, or Promotions folders if you don't see it.",
                    StatusCode = 403
                };
            }


            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.RememberMe, true);

            if (result.Succeeded)
            {
                var tokenResponse = _jwtService.CreateJwtToken(user);
                tokenResponse.Success = true;
                tokenResponse.Message = "Login successful.";
                tokenResponse.StatusCode = 200;
                return tokenResponse;
            }
            else if (result.IsLockedOut)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Your account is temporarily locked due to multiple failed login attempts. Please try again later.",
                    StatusCode = 423
                };
            }
            else if (result.IsNotAllowed)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "User is not allowed to login.",
                    StatusCode = 403
                };
            }
            else
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid login attempt.",
                    StatusCode = 401
                };
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
                redirectUrl = "https://loaclhost:4200/active";
            }

            string confirmationLink = $"{scheme}://{host}/api/account/confirmemail?userId={user.Id}&token={Uri.EscapeDataString(token)}&redirectTo={Uri.EscapeDataString(redirectUrl)}";

            string html = EmailTemplateService.GetConfirmationEmailTemplate(confirmationLink);

            var emailDTO = new EmailDTO(user.Email, "Confirm Your Email", html);
            await _emailSender.SendEmailAsync(emailDTO);
        }

    }
}
