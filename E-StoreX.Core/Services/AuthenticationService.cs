using AutoMapper;
using Domain.Entities;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using EStoreX.Core.Enums;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using ServiceContracts;
using System.Security.Claims;
using System.Text;
using EStoreX.Core.Helper;

namespace EStoreX.Core.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;
        private readonly IUserManagementService _userManagementService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor, IJwtService jwtService, IUnitOfWork unitOfWork, IMapper mapper, IUserManagementService userManagementService, RoleManager<ApplicationRole> roleManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
            _userManagementService = userManagementService;
            _roleManager = roleManager;
        }
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                return AuthenticationResponseFactory.Failure("Invalid registration data.", 400, "Registration data cannot be null.");


            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
                return AuthenticationResponseFactory.Failure("Email is already registered.", 409, "This email is already in use.");



            ApplicationUser user = new ApplicationUser()
            {
                DisplayName = registerDTO.UserName,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return AuthenticationResponseFactory.Failure("Registration failed.", 400, result.Errors.Select(e => e.Description).ToArray());


            await EnsureRoleExistsAndAssignAsync(user, UserTypeOptions.User.ToString());

            await SendEmail(user);

            return AuthenticationResponseFactory.Success("Registration successful. Please check your email to confirm your account.");
        }
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return AuthenticationResponseFactory.Failure("Invalid login data.", 400, "Login data cannot be null.");

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "No account found with this email.");


            if (!user.EmailConfirmed)
            {
                await SendEmail(user);
                return AuthenticationResponseFactory.Failure("Email not confirmed.", 403, "You must confirm your email before logging in.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.RememberMe, true);

            if (result.Succeeded)
            {
                var tokenResponse = await _jwtService.CreateJwtToken(user) as AuthenticationSuccessResponse;
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
                string message = "Your account is temporarily locked due to multiple failed login attempts. Please try again later.";
                return AuthenticationResponseFactory.Failure(message, 423, message);
            }
            else if (result.IsNotAllowed)
            {
                return AuthenticationResponseFactory.Failure("User is not allowed to login.", 403, "User is not allowed to login.");
            }
            else
            {
                return AuthenticationResponseFactory.Failure("Invalid login attempt.", 401, "Incorrect email or password.");
            }
        }


        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "User with the provided ID does not exist.");

            if (await _userManager.IsEmailConfirmedAsync(user))
                return AuthenticationResponseFactory.Failure("Email is already confirmed.", 200, "Email already confirmed.");

            if (user.LastEmailConfirmationToken != dto.Token)
                return AuthenticationResponseFactory.Failure("Invalid or expired confirmation token.", 400, "Token mismatch or already used.");

            var result = await _userManager.ConfirmEmailAsync(user, dto.Token);

            if (result.Succeeded)
            {
                user.LastEmailConfirmationToken = null;
                await _userManager.UpdateAsync(user);

                return AuthenticationResponseFactory.Success("Email confirmed successfully.");
            }
            return AuthenticationResponseFactory.Failure("Failed to confirm email.", 400, result.Errors.Select(e => e.Description).ToArray());

        }
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ForgotPasswordAsync(ForgotPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return AuthenticationResponseFactory.Failure("Incorrect email.", 400, "Email not found.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return AuthenticationResponseFactory.Failure("Please confirm your email before resetting password.", 400, "Email is not confirmed.");

            var logins = await _userManager.GetLoginsAsync(user);
            if (logins.Any())
                return AuthenticationResponseFactory.Failure("You registered using an external provider (Google/GitHub). Use it to log in.", 400, "External login detected.");

            #region Timer
            var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "Token");

            if (!string.IsNullOrEmpty(existingToken))
            {
                var tokenTimeStr = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");
                if (!string.IsNullOrEmpty(tokenTimeStr) && DateTime.TryParse(tokenTimeStr, out var tokenTime))
                {
                    if (DateTime.UtcNow < tokenTime.AddMinutes(5))
                        return AuthenticationResponseFactory.Failure("A password reset email was already sent recently. Please wait before trying again.", 429, "Reset already requested.");
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

            return AuthenticationResponseFactory.Success("A password reset link has been sent to your email.");
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> VerifyResetPasswordTokenAsync(VerifyResetPasswordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Token))
                return AuthenticationResponseFactory.Failure("Invalid verification request.", 400, "UserId and token are required.");

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "No account found for the provided user ID.");

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            }
            catch
            {
                return AuthenticationResponseFactory.Failure("Invalid token format.",400, "The token format is invalid or corrupted.");
            }

            var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "Token");

            if (storedToken == null || storedToken != decodedToken)
                return AuthenticationResponseFactory.Failure("Invalid or expired token.", 400, "The token is invalid or has already been used.");

            var tokenTimeStr = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");

            if (string.IsNullOrEmpty(tokenTimeStr) || !DateTime.TryParse(tokenTimeStr, out var tokenTime))
                return AuthenticationResponseFactory.Failure("Token validation failed.", 400, "The token timestamp is invalid.");

            if (DateTime.UtcNow > tokenTime.AddMinutes(5))
                return AuthenticationResponseFactory.Failure("Reset password link has expired.", 400, "The reset password link has expired. Please request a new one.");

            return AuthenticationResponseFactory.Success("The reset password token is valid.");
        }
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (dto == null)
                return AuthenticationResponseFactory.Failure("Invalid reset password data.", 400, "Request body cannot be null.");

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
                return AuthenticationResponseFactory.Failure("Failed to reset password.", 400, resetResult.Errors.Select(e => e.Description).ToArray());

            await _userManager.RemoveAuthenticationTokenAsync(user, "ResetPassword", "Token");
            await _userManager.RemoveAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");

            return AuthenticationResponseFactory.Success("Password has been reset successfully.");
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> RefreshTokenAsync(TokenModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Token) || string.IsNullOrWhiteSpace(model.RefreshToken))
                return AuthenticationResponseFactory.Failure("Invalid token model.", 400, "Token and refresh token are required.");

            ClaimsPrincipal? principal;

            try
            {
                principal = _jwtService.GetPrincipalFromJwtToken(model.Token);
            }
            catch (SecurityTokenException)
            {
                return AuthenticationResponseFactory.Failure("Invalid token.", 400, "Access token is invalid.");
            }

            if (principal is null)
                return AuthenticationResponseFactory.Failure("Invalid token.", 400, "Access token is invalid.");

            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                return AuthenticationResponseFactory.Failure("Invalid token.", 400, "Email claim is missing in token.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "User does not exist.");

            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
                return AuthenticationResponseFactory.Failure("Invalid refresh token.", 400, "Refresh token is invalid or expired.");

            var authResponse = await _jwtService.CreateJwtToken(user) as AuthenticationSuccessResponse;

            // Rotate refresh token
            user.RefreshToken = authResponse?.RefreshToken;
            user.RefreshTokenExpirationDateTime = authResponse.RefreshTokenExpirationDateTime;

            await _userManager.UpdateAsync(user);

            authResponse.Success = true;
            authResponse.StatusCode = 200;
            authResponse.Message = "Token refreshed successfully.";

            return authResponse;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAddress(string? email, Address? address)
        {
            if (email is null || address is null)
                return false;

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return false;

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

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> LogoutAsync(string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpirationDateTime = DateTime.MinValue;
                    await _userManager.UpdateAsync(user);
                }
            }

            await _signInManager.SignOutAsync();

            return AuthenticationResponseFactory.Success("Logged out successfully.");
        }
        /// <inheritdoc/>
        public async Task<ApplicationUserResponse?> GetUserByIdAsync(string userId)
            => await _userManagementService.GetUserByIdAsync(userId);
        /// <inheritdoc/>
        public async Task<AuthenticationResponse> UpdateUserProfileAsync(UpdateUserDTO dto)
        {
            if (dto == null)
                return AuthenticationResponseFactory.Failure("Invalid update data.", 400, "Update data cannot be null.");

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return AuthenticationResponseFactory.Failure("User not found.", 404, "No user found with the provided ID.");

            if (!string.IsNullOrWhiteSpace(dto.DisplayName))
                user.DisplayName = dto.DisplayName;

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return AuthenticationResponseFactory.Failure("Failed to update profile.", 400, updateResult.Errors.Select(e => e.Description).ToArray());

            if (!string.IsNullOrWhiteSpace(dto.CurrentPassword) && !string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!passwordResult.Succeeded)
                    return AuthenticationResponseFactory.Failure("Failed to change password.", 400, passwordResult.Errors.Select(e => e.Description).ToArray());
            }

            return AuthenticationResponseFactory.Success("Profile updated successfully.");
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
        private async Task EnsureRoleExistsAndAssignAsync(ApplicationUser user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

            await _userManager.AddToRoleAsync(user, roleName);
        }

    }
}
