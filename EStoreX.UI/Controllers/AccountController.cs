using EStoreX.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.Services;
using System.Security.Claims;
using EStoreX.Core.DTO;


namespace EStoreX.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Errors = error;
                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser()
            {
                PersonName = registerDTO.PersonName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                if (registerDTO.UserType == UserTypeOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() });
                    }
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.User.ToString() });
                    }
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                user.LastEmailConfirmationToken = token;

                await _userManager.UpdateAsync(user);

                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token = token },
                    Request.Scheme
                );

                string html = EmailTemplateService.GetConfirmationEmailTemplate(confirmationLink);

                await _emailSender.SendEmailAsync(user.Email, "Confirm Your Email", html);

                ViewBag.IsRegister = true;
                ViewBag.Message = "Registration successful! Please check your email to confirm your account.";

                return View("ResendConfirmationEmailFromRegister");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }
            return View(registerDTO);
        }

        [Authorize("NotAuthorized")]
        public IActionResult ResendConfirmationEmailFromLogin()
        {
            return View();
        }

        //[AllowAnonymous]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Invalid confirmation request.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return View("EmailConfirmed");
            }

            if (user.LastEmailConfirmationToken != token)
            {
                return View("EmailConfirmationFailed");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                user.LastEmailConfirmationToken = null;
                await _userManager.UpdateAsync(user);

                return View("EmailConfirmed");
            }
            else
            {
                return View("EmailConfirmationFailed");
            }
        }
        [Authorize("NotAuthorized")]
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationEmailFromRegister(string email)
        {
            return await ResendConfirmationEmail(email, nameof(AccountController.ResendConfirmationEmailFromRegister));
        }

        [Authorize("NotAuthorized")]
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationEmailFromLogin(string email)
        {
            return await ResendConfirmationEmail(email, nameof(AccountController.ResendConfirmationEmailFromLogin));
        }

        private async Task<IActionResult> ResendConfirmationEmail(string email, string viewName)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewData["Error"] = "Please provide a valid email.";
                return View(viewName);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewData["Error"] = "No user found with this email.";
                return View(viewName);
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                ViewData["Success"] = "Your email is already confirmed. You can log in now.";
                return View(viewName);
            }

            user.LastResendTime ??= DateTime.MinValue;

            if (DateTime.Now.Subtract(user.LastResendTime.Value).Minutes < 3)
            {
                ViewData["Error"] = "You can only resend the confirmation email every 3 minutes.";
                return View(viewName);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            user.LastEmailConfirmationToken = token;

            user.LastResendTime = DateTime.Now;
            await _userManager.UpdateAsync(user);

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = token }, Request.Scheme);

            string html = EmailTemplateService.GetConfirmationEmailTemplate(confirmationLink);


            await _emailSender.SendEmailAsync(user.Email, "Confirm Your Email", html);

            ViewData["Success"] = "A new confirmation email has been sent. Please check your inbox.";

            return View(viewName);
        }

        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Login()
        {
            var loginDTO = new LoginDTO()
            {
                Schemes = await _signInManager.GetExternalAuthenticationSchemesAsync()
            };

            return View(loginDTO);
        }

        [Authorize("NotAuthorized")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            loginDTO.Schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(loginDTO);
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                ModelState.AddModelError("Login", "Invalid email or password.");
                return View(loginDTO);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("Login", "Please confirm your email before logging in.");
                ViewBag.NeedConfirm = true;
                return View(loginDTO);
            }

            var logins = await _userManager.GetLoginsAsync(user);
            if (logins.Count > 0)
            {
                ModelState.AddModelError("Login", "You registered with Google or GitHub. Please login using that provider.");
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: loginDTO.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {

                //if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                //{
                //    return RedirectToAction("Index", "Home", new { area = "Admin" });
                //}

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Persons");
            }

            if (result.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                if (lockoutEnd.HasValue)
                {
                    TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                    DateTime lockoutEndLocal = TimeZoneInfo.ConvertTimeFromUtc(lockoutEnd.Value.UtcDateTime, egyptTimeZone);
                    var remainingTime = lockoutEndLocal - DateTime.Now;

                    ModelState.AddModelError("Login",
                        $"Account is locked out until {lockoutEndLocal:HH:mm}. " +
                        $"Try again in {remainingTime.Minutes} minutes.");
                }
                else
                {
                    ModelState.AddModelError("Login", "Account is locked out. Try again later.");
                }
                return View(loginDTO);
            }

            ModelState.AddModelError("", $"In correct email or password");

            return View(loginDTO);
        }
        [Authorize("NotAuthorized")]
        //[AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = "")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            properties.Items["prompt"] = "select_account";

            if (provider == "GitHub")
            {
                properties.Items["login"] = "";
            }

            return Challenge(properties, provider);

        }

        //[AllowAnonymous]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "", string remoteError = "")
        {
            var loginDTO = new LoginDTO()
            {
                Schemes = await _signInManager.GetExternalAuthenticationSchemesAsync()
            };

            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from external login provider: {remoteError}");
                return View("Login", loginDTO);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", $"Error from external login provider: {remoteError}");
                return View("Login", loginDTO);
            }

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Index", "Persons");
            }


            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (!string.IsNullOrEmpty(email))
            {
                user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = email,
                        Email = email,
                        PersonName = info.Principal.FindFirstValue(ClaimTypes.Name),
                        EmailConfirmed = true
                    };

                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }

                var result = await _userManager.AddLoginAsync(user, info);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Persons");
                }
            }

            var userName = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (!string.IsNullOrEmpty(userName))
            {
                user = await _userManager.FindByEmailAsync(userName);
                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = userName,
                        Email = userName,
                        PersonName = info.Principal.FindFirstValue(ClaimTypes.Name),
                        EmailConfirmed = true
                    };

                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }

                var result = await _userManager.AddLoginAsync(user, info);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Persons");
                }
            }

            ModelState.AddModelError("", $"Something went wrong");
            return View("Login", loginDTO);
        }

        [Authorize("NotAuthorized")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(forgotPasswordDTO);
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);

            if (user == null)
            {
                ModelState.AddModelError("ForgotPassword", "In correct email");
                return View(forgotPasswordDTO);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("ForgotPassword", "Please confirm your email before Reset Password.");
                ViewBag.NeedConfirm = true;
                return View(forgotPasswordDTO);
            }

            var logins = await _userManager.GetLoginsAsync(user);
            if (logins.Count > 0)
            {
                ModelState.AddModelError("ForgotPassword", "You registered with Google or GitHub. We can't reset your password go to login and use one of the providers to login");
                return View(forgotPasswordDTO);
            }

            var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "Token");

            if (!string.IsNullOrEmpty(existingToken))
            {
                var tokenTime = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");
                if (!string.IsNullOrEmpty(tokenTime) && DateTime.Now < DateTime.Parse(tokenTime).AddMinutes(10))
                {
                    ModelState.AddModelError("ForgotPassword", "A password reset email has already been sent. Please wait before requesting another.");
                    return View(forgotPasswordDTO);
                }
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "Token", token);
            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime", DateTime.Now.ToString());

            var resetLink = Url.Action("ResetPassword", "Account",
                new { email = user.Email, token = token }, Request.Scheme);

            string html = EmailTemplateService.GetPasswordResetEmailTemplate(resetLink);


            await _emailSender.SendEmailAsync(user.Email, "Reset Your Password", html);

            ViewBag.Message = "A password reset link has been sent to your email.";

            return View("ForgotPassword");

        }

        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid reset password request.");
            }
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "Token");

            if (existingToken != token)
            {
                return BadRequest("Invalid reset password request.");
            }

            var tokenTime = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "TokenTime");

            if (string.IsNullOrEmpty(tokenTime) || DateTime.Now > DateTime.Parse(tokenTime).AddMinutes(10))
            {
                return BadRequest("Reset password link has expired.");
            }

            var resetPasswordDTO = new ResetPasswordDTO()
            {
                Email = email,
                Token = token
            };

            return View(resetPasswordDTO);
        }


        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(resetPasswordDTO);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);

            if (user is null)
            {
                ModelState.AddModelError("", "in correct Email");
                return View(resetPasswordDTO);
            }

            var isValidToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", resetPasswordDTO.Token);

            if (!isValidToken)
            {
                ModelState.AddModelError("", "Invalid or expired token.");
                return View(resetPasswordDTO);
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);

            if (!resetResult.Succeeded)
            {
                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordDTO);
            }

            return View("ResetPasswordSuccessful");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return Json(result == null);
        }
    }
}
