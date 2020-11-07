﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Craftwork_Project.ViewModels;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.String;

namespace CraftworkProject.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IEmailService _emailService;

        public AccountController(IUserManager userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;

            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
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
            _userManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Signup(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindUser(model.Username);
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

                var userWithEmail = _userManager.GetAllUsers().FirstOrDefault(x => x.Email == model.Email);
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
                        var createdUser = await _userManager.FindUser(model.Username);
                        var token = await _userManager.GenerateEmailConfirmationToken(createdUser);
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
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId != null && token != null)
            {
                bool status = await _userManager.ConfirmEmail(Guid.Parse(userId), token);
                
                return status ? View("EmailConfirmed") : View("InvalidEmailConfirmationLink");
            }

            return Redirect("/");
        }
    }
}