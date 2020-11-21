using System;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IUserManagerHelper _helper;
        private readonly IEmailService _emailService;

        public AccountController(
            IUserManager userManager,
            IUserManagerHelper helper,
            IEmailService emailService
        )
        {
            _userManager = userManager;
            _helper = helper;
            _emailService = emailService;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            ViewData["returnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            if (ModelState.IsValid)
            {
                var status = await _userManager.SignIn(model.Username, model.Password);
                
                if (status)
                {
                    return Redirect(returnUrl ?? "/");
                }
                
                ModelState.AddModelError(nameof(LoginViewModel.Password), "Invalid login or password.");
            }

            return View(model);
        }
        
        public IActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            _userManager.SignOut();

            return Redirect("/");
        }

        [AllowAnonymous]
        public IActionResult Signup(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            ViewData["returnUrl"] = returnUrl;
            return View(new SignUpViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindUserByName(model.Username);
                if (user != null && user!.EmailConfirmed)
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.Username), "This username is already taken.");
                }
                
                var userWithEmail = await _userManager.FindUserByEmail(model.Email);
                if (userWithEmail != null && userWithEmail.EmailConfirmed)
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.Email), "This email is already taken");
                    ModelState.AddModelError(nameof(SignUpViewModel.ConfirmEmail), "This email is already taken");
                }

                if (model.Password != null && !model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.Password), "Passwords do not match");
                    ModelState.AddModelError(nameof(SignUpViewModel.ConfirmPassword), "Passwords do not match");
                }

                if (model.Email != null && !model.Email.Equals(model.ConfirmEmail))
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.Email), "Passwords do not match");
                    ModelState.AddModelError(nameof(SignUpViewModel.ConfirmEmail), "Passwords do not match");
                }

                if (ModelState.ErrorCount == 0)
                {
                    User newUser = new User()
                    {
                        Username = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        EmailConfirmed = false,
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false
                    };

                    // prevent duplicate users
                    await _userManager.DeleteUser(model.Username);

                    var roleId = await _userManager.GetRoleId("customer");
                    var userId = await _userManager.CreateUser(newUser, model.Password, roleId);

                    if (userId != default)
                    {
                        var externalLoginInfo = await _helper.GetExternalLoginInfo();
                        var createdUser = await _userManager.FindUserById(userId);

                        if (externalLoginInfo != null && createdUser != null)
                        {
                            await _helper.AddExternalLogin(createdUser.Id, externalLoginInfo);
                            createdUser.EmailConfirmed = true;
                            await _userManager.UpdateUser(createdUser);
                            await _userManager.SignIn(createdUser.Username, model.Password);

                            return Redirect("/profile");
                        }

                        var token = await _userManager.GenerateEmailConfirmationToken(createdUser?.Id ?? default);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                            new {userId = createdUser?.Id ?? default, token}, Request.Scheme);
                        
                        await _emailService.SendEmailAsync(model.Email, "Email activation",
                            $"Email activation link: {confirmationLink}");
                        
                        ViewData["Email"] = model.Email;
                        return View("EmailSent");
                    }
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ExternalSignUp(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            // get google provider
            var googleProvider = (await _helper.GetExternalAuthenticationSchemes())[0].Name;

            var callbackUrl = $"/account/externalsignupcallback?returnUrl={returnUrl}";
            var properties = _helper.ConfigureExternalAuthenticationProperties(googleProvider, callbackUrl);

            return new ChallengeResult(googleProvider, properties);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            // get google provider
            var googleProvider = (await _helper.GetExternalAuthenticationSchemes())[0].Name;

            var callbackUrl = $"/account/externallogincallback?returnUrl={returnUrl}";
            var properties = _helper.ConfigureExternalAuthenticationProperties(googleProvider, callbackUrl);

            return new ChallengeResult(googleProvider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalSignUpCallback(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            returnUrl ??= "/";

            var info = await _helper.GetExternalLoginInfo();

            if (info != null)
            {
                ViewData["email"] = info.Principal.Claims.FirstOrDefault(x => x.Type.Contains("email"))?.Value ??
                                    string.Empty;
                
                return View("Signup", new SignUpViewModel());
            }

            return Redirect($"/signup?returnUrl={returnUrl}");
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            returnUrl ??= "/";

            var info = await _helper.GetExternalLoginInfo();

            if (info != null)
            {
                var result = await _helper.ExternalLoginSignIn(info);

                return Redirect(result ? returnUrl : "/signup");
            }
            
            return Redirect($"/login?returnUrl={returnUrl}");
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            return View(new ForgotPasswordViewModel());
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindUserByEmail(model.Email);

                if (user != null && user.EmailConfirmed)
                {
                    var token = await _userManager.GeneratePasswordResetToken(user.Id);
                    var resetLink = Url.Action("ResetPassword", "Account",
                        new {userId = user.Id, token}, Request.Scheme);
                    await _emailService.SendEmailAsync(model.Email, "Password reset",
                        $"Password reset link: {resetLink}");

                    return View("ForgotPasswordEmailSent");
                }
                
                ModelState.AddModelError(nameof(ForgotPasswordViewModel.Email),
                    "Cannot find user with this email or email is not confirmed.");
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            if (userId != null && token != null)
            {
                bool status = await _userManager.ConfirmEmail(Guid.Parse(userId), token);
                
                return status ? View("EmailConfirmed") : View("InvalidEmailConfirmationLink");
            }

            return Redirect("/");
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (userId != null && token != null)
            {
                var viewModel = new ResetPasswordViewModel()
                {
                    UserId = userId,
                    Token = token
                };
                return View(viewModel);
            }

            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (ModelState.IsValid)
            {
                if (model.NewPassword != null && !model.NewPassword.Equals(model.ConfirmNewPassword))
                {
                    ModelState.AddModelError(nameof(ResetPasswordViewModel.NewPassword), "Passwords do not match");
                    ModelState.AddModelError(nameof(ResetPasswordViewModel.ConfirmNewPassword), "Passwords do not match");
                }

                if (ModelState.ErrorCount == 0)
                {
                    bool status =
                        await _userManager.ResetPassword(Guid.Parse(model.UserId), model.Token, model.NewPassword);

                    return status ? View("PasswordChanged") : View("InvalidPasswordResetLink");
                }
            }

            return View(model);
        }
    }
}