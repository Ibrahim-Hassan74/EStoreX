using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using ServiceContracts;

namespace EStoreX.Core.Services
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
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Invalid registration data.",
                    StatusCode = 400,
                    Errors = new List<string> { "Registration data cannot be null." }
                };
            }

            if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Username is already registered.",
                    Errors = new List<string> { "The username is already taken." },
                    StatusCode = 409
                };
            }


            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Email is already registered.",
                    Errors = new List<string> { "This email is already in use." },
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
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Registration failed.",
                    Errors = result.Errors.Select(e => e.Description).ToList(),
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
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Invalid login data.",
                    StatusCode = 400,
                    Errors = new List<string> { "Login data cannot be null." }
                };
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "User not found.",
                    Errors = new List<string> { "No account found with this email." },
                    StatusCode = 404
                };
            }


            if (!user.EmailConfirmed)
            {
                await SendEmail(user);
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Email not confirmed.",
                    Errors = new List<string> { "You must confirm your email before logging in." },
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
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Invalid login attempt.",
                    Errors = new List<string> { "Incorrect email or password." },
                    StatusCode = 401
                };
            }
        }


        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404,
                    Errors = new List<string> { "User with the provided ID does not exist." }
                };
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return new AuthenticationFailureResponse
                {
                    Success = true,
                    Message = "Email is already confirmed.",
                    StatusCode = 200,
                    Errors = new List<string> { "Email already confirmed." }
                };
            }

            if (user.LastEmailConfirmationToken != dto.Token)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Invalid or expired confirmation token.",
                    Errors = new List<string> { "Token mismatch or already used." },
                    StatusCode = 400
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, dto.Token);

            if (result.Succeeded)
            {
                user.LastEmailConfirmationToken = null;
                await _userManager.UpdateAsync(user);

                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Email confirmed successfully.",
                    StatusCode = 200
                };
            }
            else
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Failed to confirm email.",
                    Errors = result.Errors.Select(e => e.Description).ToList(),
                    StatusCode = 400
                };
            }

        }
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ForgotPasswordAsync(ForgotPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Incorrect email.",
                    StatusCode = 400,
                    Errors = new List<string> { "Email not found." }
                };
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Please confirm your email before resetting password.",
                    StatusCode = 400,
                    Errors = new List<string> { "Email is not confirmed." }
                };
            }

            var logins = await _userManager.GetLoginsAsync(user);
            if (logins.Any())
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "You registered using an external provider (Google/GitHub). Use it to log in.",
                    StatusCode = 400,
                    Errors = new List<string> { "External login detected." }
                };
            }

            var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "Token");

            if (!string.IsNullOrEmpty(existingToken))
            {
                var tokenTimeStr = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");
                if (!string.IsNullOrEmpty(tokenTimeStr) && DateTime.TryParse(tokenTimeStr, out var tokenTime))
                {
                    if (DateTime.UtcNow < tokenTime.AddMinutes(10))
                    {
                        return new AuthenticationFailureResponse
                        {
                            Success = false,
                            Message = "A password reset email was already sent recently. Please wait before trying again.",
                            StatusCode = 429,
                            Errors = new List<string> { "Reset already requested." }
                        };
                    }
                }
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "Token", token);
            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime", DateTime.UtcNow.ToString());

            var request = _httpContextAccessor.HttpContext?.Request;
            var scheme = request?.Scheme ?? "https";
            var host = request?.Host.Value ?? "localhost:5000";

            string redirectUrl;

            if (IsMobileDevice(request))
            {
                redirectUrl = "estorex://reset-password";
            }
            else
            {
                redirectUrl = "https://loaclhost:4200/reset-password";
            }

            string resetLink = $"{scheme}://{host}/api/account/reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}&redirectTo={Uri.EscapeDataString(redirectUrl)}";

            string html = EmailTemplateService.GetPasswordResetEmailTemplate(resetLink);

            var emailDTO = new EmailDTO(user.Email, "Reset Your Password", html);

            await _emailSender.SendEmailAsync(emailDTO);


            return new AuthenticationResponse
            {
                Success = true,
                Message = "A password reset link has been sent to your email.",
                StatusCode = 200
            };
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
                redirectUrl = "estorex://active";
            }
            else
            {
                redirectUrl = "https://loaclhost:4200/active";
            }
            string confirmationLink = $"{scheme}://{host}/api/account/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}&redirectTo={Uri.EscapeDataString(redirectUrl)}";

            string html = EmailTemplateService.GetConfirmationEmailTemplate(confirmationLink);

            var emailDTO = new EmailDTO(user.Email, "Confirm Your Email", html);
            await _emailSender.SendEmailAsync(emailDTO);
        }

    }
}
