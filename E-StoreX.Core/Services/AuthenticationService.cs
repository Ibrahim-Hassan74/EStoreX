using AutoMapper;
using Domain.Entities;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using ServiceContracts;
using System.Security.Claims;
using System.Text;

namespace EStoreX.Core.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor, IJwtService jwtService, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
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
                DisplayName = registerDTO.UserName,
                UserName = registerDTO.Email,
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
        /// <inheritdoc/>
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
                var tokenResponse = _jwtService.CreateJwtToken(user) as AuthenticationSuccessResponse;
                user.RefreshToken = tokenResponse?.RefreshToken;
                user.RefreshTokenExpirationDateTime = tokenResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);
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

            #region Timer
            var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "Token");

            if (!string.IsNullOrEmpty(existingToken))
            {
                var tokenTimeStr = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");
                if (!string.IsNullOrEmpty(tokenTimeStr) && DateTime.TryParse(tokenTimeStr, out var tokenTime))
                {
                    if (DateTime.UtcNow < tokenTime.AddMinutes(5))
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
            #endregion

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "Token", token);
            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime", DateTime.UtcNow.ToString());

            var request = _httpContextAccessor.HttpContext?.Request;

            //var phone = IsMobileDevice(request);
            //string frontendBaseUrl = phone
            //    ? "https://estorex/reset-password"
            //    : "https://estorex/reset-password";

            //var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //string resetLink = $"{frontendBaseUrl}?userId={Uri.EscapeDataString(user.Id.ToString())}&token={encodedToken}";

            //string baseDeepLink = "https://estorex/reset-password";

            //string dynamicLinkPrefix = "https://estorex.page.link";

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //string fullLink = $"{baseDeepLink}?userId={Uri.EscapeDataString(user.Id.ToString())}&token={encodedToken}";

            //string resetLink = $"{dynamicLinkPrefix}/?link={Uri.EscapeDataString(fullLink)}&apn=com.yourapp.package&ibi=com.yourapp.ios";

            string resetLink = $"{request.Scheme}://{request.Host}/reset-password?userId={Uri.EscapeDataString(user.Id.ToString())}&token={encodedToken}";


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

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> VerifyResetPasswordTokenAsync(VerifyResetPasswordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Token))
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Invalid verification request.",
                    StatusCode = 400,
                    Errors = new List<string> { "UserId and token are required." }
                };
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404,
                    Errors = new List<string> { "No account found for the provided user ID." }
                };
            }

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            }
            catch
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Invalid token format.",
                    StatusCode = 400,
                    Errors = new List<string> { "The token format is invalid or corrupted." }
                };
            }

            var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "Token");

            if (storedToken == null || storedToken != decodedToken)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Invalid or expired token.",
                    StatusCode = 400,
                    Errors = new List<string> { "The token is invalid or has already been used." }
                };
            }

            var tokenTimeStr = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");

            if (string.IsNullOrEmpty(tokenTimeStr) || !DateTime.TryParse(tokenTimeStr, out var tokenTime))
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Token validation failed.",
                    StatusCode = 400,
                    Errors = new List<string> { "The token timestamp is invalid." }
                };
            }

            if (DateTime.UtcNow > tokenTime.AddMinutes(5))
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    Message = "Reset password link has expired.",
                    StatusCode = 400,
                    Errors = new List<string> { "The reset password link has expired. Please request a new one." }
                };
            }

            return new AuthenticationResponse
            {
                Success = true,
                Message = "The reset password token is valid.",
                StatusCode = 200
            };
        }
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (dto == null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid reset password data.",
                    Errors = new List<string> { "Request body cannot be null." }
                };
            }

            var verifyResponse = await VerifyResetPasswordTokenAsync(
                new VerifyResetPasswordDTO
                {
                    UserId = dto.UserId!,
                    Token = dto.Token!
                }
            );

            if (!verifyResponse.Success)
                return verifyResponse;

            var user = await _userManager.FindByIdAsync(dto.UserId!);
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);


            if (!resetResult.Succeeded)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Failed to reset password.",
                    Errors = resetResult.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.RemoveAuthenticationTokenAsync(user, "ResetPassword", "Token");
            await _userManager.RemoveAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");

            return new AuthenticationResponse
            {
                Success = true,
                StatusCode = 200,
                Message = "Password has been reset successfully."
            };
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> RefreshTokenAsync(TokenModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Token) || string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid token model.",
                    Errors = new List<string> { "Token and refresh token are required." }
                };
            }

            ClaimsPrincipal? principal;

            try
            {
                principal = _jwtService.GetPrincipalFromJwtToken(model.Token);
            }
            catch (SecurityTokenException)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid token.",
                    Errors = new List<string> { "Access token is invalid." }
                };
            }

            if (principal is null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid token.",
                    Errors = new List<string> { "Access token is invalid." }
                };
            }

            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid token.",
                    Errors = new List<string> { "Email claim is missing in token." }
                };
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "User not found.",
                    Errors = new List<string> { "User does not exist." }
                };
            }

            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
            {
                return new AuthenticationFailureResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid refresh token.",
                    Errors = new List<string> { "Refresh token is invalid or expired." }
                };
            }

            var authResponse = _jwtService.CreateJwtToken(user) as AuthenticationSuccessResponse;

            // Rotate refresh token
            user.RefreshToken = authResponse?.RefreshToken;
            user.RefreshTokenExpirationDateTime = authResponse.RefreshTokenExpirationDateTime;

            await _userManager.UpdateAsync(user);

            authResponse.Success = true;
            authResponse.StatusCode = 200;
            authResponse.Message = "Token refreshed successfully.";

            return authResponse;
        }

        private bool IsMobileDevice(HttpRequest? request)
        {
            if (request is null) return false;

            var userAgent = request.Headers["User-Agent"].ToString().ToLower();
            var src = request.Headers["X-Source"].ToString().ToLower();

            return userAgent.Contains("android") ||
                userAgent.Contains("iphone") ||
                userAgent.Contains("ipad") ||
                userAgent.Contains("mobile") ||
                userAgent.Contains("opera mini") ||
                userAgent.Contains("flutter") ||
                src.Contains("flutter");
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
        /// <inheritdoc/>
        public async Task<bool> UpdateAddress(string? email, Address? address)
        {
            if (email is null || address is null)
            {
                return false;
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return false;
            }
            return await _unitOfWork.AuthenticationRepository.UpdateAddress(user.Id, address);
        }
        /// <inheritdoc/>
        public async Task<ShippingAddressDTO?> GetAddress(string? email)
        {
            if (string.IsNullOrEmpty(email)) 
                return null;
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return null;
            var address = await _unitOfWork.AuthenticationRepository.GetAddress(user.Id);
            if (address is null) return null;
            return _mapper.Map<ShippingAddressDTO>(address);
        }
    }
}
