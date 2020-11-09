using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using CraftworkProject.Web.Service.ActionFilters;
using CraftworkProject.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.String;
using UserViewModel = CraftworkProject.Web.ViewModels.UserViewModel;

namespace CraftworkProject.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IEmailService _emailService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _environment;

        public AccountController(
            IUserManager userManager,
            IEmailService emailService, 
            IImageService imageService,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _emailService = emailService;
            _imageService = imageService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindUserByName(User.Identity.Name);
            var viewModel = new UserViewModel()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CurrentProfilePicture = user.ProfilePicture,
                PhoneNumber = user.PhoneNumber
            };

            if (TempData["errors"] != null)
            {
                var errors = (string[]) TempData["errors"];
                foreach (var error in errors)
                {
                    if (error.Contains("first", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(viewModel.FirstName), error);
                    }

                    if (error.Contains("last", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(viewModel.LastName), error);
                    }

                    if (error.Contains("file", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(viewModel.ProfilePicture), error);
                    }

                    if (error.Contains("current password", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(viewModel.CurrentPassword), error);
                    }
                    else if (error.Contains("password", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(viewModel.NewPassword), "Passwords do not match");
                        ModelState.AddModelError(nameof(viewModel.ConfirmNewPassword), "Passwords do not match");   
                    }
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePersonalInfo(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindUserById(model.Id);
                var updatedUser = new User()
                {
                    Id = model.Id,
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PasswordHash = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    ProfilePicture = user.ProfilePicture
                };

                if (model.ProfilePicture != null)
                {
                    DeleteFile(user.ProfilePicture);
                    updatedUser.ProfilePicture = await UploadFile(model.ProfilePicture);
                }
                
                await _userManager.UpdateUser(updatedUser);
                return Redirect("/account");
            }

            var errors = ModelState.Values.Select(
                v => v.Errors.Count != 0 ? v.Errors[0].ErrorMessage : Empty).ToList();
            TempData["errors"] = errors;

            return Redirect("/account");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindUserById(model.Id);

                if (model.NewPassword.Equals(model.ConfirmNewPassword))
                {
                    bool status = await _userManager.ChangeUserPassword(model.Id, model.CurrentPassword, model.NewPassword);

                    if (status)
                    {
                        return Redirect("/account");
                    }

                    ModelState.AddModelError(nameof(UserViewModel.CurrentPassword), "Incorrect current password.");
                }
                else
                {
                    ModelState.AddModelError(nameof(UserViewModel.NewPassword), "Passwords do not match");
                    ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "Passwords do not match");   
                }
            }
            
            var errors = ModelState.Values.Select(
                v => v.Errors.Count != 0 ? v.Errors[0].ErrorMessage : Empty).ToList();
            TempData["errors"] = errors;

            return Redirect("/account");
        }  
        
        private async Task<string> UploadFile(IFormFile file)
        {
            string uploadDir = Path.Combine(_environment.WebRootPath, "img/profile");
            string fileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";
            string filePath = Path.Combine(uploadDir, fileName);
            
            await _imageService.SaveImage(file, filePath);
            return fileName;
        }

        private void DeleteFile(string fileName)
        {
            string uploadDir = Path.Combine(_environment.WebRootPath, "img/profile");
            string filePath = Path.Combine(uploadDir, fileName);
            System.IO.File.Delete(filePath);
        }
        
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            ViewBag.returnUrl = returnUrl;

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
                bool status = await _userManager.SignIn(model.Username, model.Password);
                
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

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Signup(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            ViewBag.returnUrl = returnUrl;
            return View();
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
                if (user != null && user.EmailConfirmed)
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.Username), "This username is already taken.");
                }
                
                if (IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.Password), "This field must be not empty");
                }

                if (IsNullOrEmpty(model.ConfirmPassword))
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.ConfirmPassword), "This field must be not empty");
                }

                if (IsNullOrEmpty(model.Email))
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.Email), "This field must be not empty");
                }

                if (IsNullOrEmpty(model.ConfirmEmail))
                {
                    ModelState.AddModelError(nameof(SignUpViewModel.ConfirmEmail), "This field must be not empty");
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
                        Email = model.Email,
                        EmailConfirmed = false,
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false
                    };

                    // prevent duplicate users
                    await _userManager.DeleteUser(model.Username);

                    var roleId = await _userManager.GetRoleId("customer");
                    var result = await _userManager.CreateUser(newUser, model.Password, roleId);

                    if (result)
                    {
                        var createdUser = await _userManager.FindUserByName(model.Username);
                        var token = await _userManager.GenerateEmailConfirmationToken(createdUser.Id);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                            new {userId = createdUser.Id, token}, Request.Scheme);
                        
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

                if (user == null || !user.EmailConfirmed)
                {
                    return View("ForgotPasswordEmailSent");
                }

                var token = await _userManager.GeneratePasswordResetToken(user.Id);
                var resetLink = Url.Action("ResetPassword", "Account",
                    new {userId = user.Id, token}, Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, "Password reset",
                    $"Password reset link: {resetLink}");

                return View("ForgotPasswordEmailSent");
            }

            return View();
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