using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IDataManager _dataManager;
        private readonly IUserManager _userManager;
        private readonly IUserManagerHelper _helper;
        private readonly ISmsService _smsService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(
            IDataManager dataManager,
            IUserManager userManager,
            IUserManagerHelper helper,
            ISmsService smsService,
            IImageService imageService,
            IWebHostEnvironment environment
        )
        {
            _dataManager = dataManager;
            _userManager = userManager;
            _helper = helper;
            _smsService = smsService;
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
            
            ViewData["PhoneNumberConfirmed"] = user.PhoneNumberConfirmed;
            ViewData["Email"] = user.Email;
            ViewData["EmailConfirmed"] = user.EmailConfirmed;

            var userOrders = _dataManager.OrderRepository.GetAllEntities()
                .Where(x => x.User.Id == user.Id);

            ViewData["pendingOrdersCount"] = userOrders.Count(x => x.Processed && !x.Canceled && !x.Finished);
            ViewData["canceledOrdersCount"] = userOrders.Count(x => x.Processed && x.Canceled && !x.Finished);
            ViewData["finishedOrdersCount"] = userOrders.Count(x => x.Processed && !x.Canceled && x.Finished);

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
                var updatedUser = new User
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
                return Redirect("/profile");
            }

            var errors = ModelState.Values.Select(
                v => v.Errors.Count != 0 ? v.Errors[0].ErrorMessage : string.Empty).ToList();
            TempData["errors"] = errors;

            return Redirect("/profile");
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword.Equals(model.ConfirmNewPassword))
                {
                    var status = await _userManager.ChangeUserPassword(model.Id, model.CurrentPassword, model.NewPassword);

                    if (status)
                    {
                        return Redirect("/profile");
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
                v => v.Errors.Count != 0 ? v.Errors[0].ErrorMessage : string.Empty).ToList();
            TempData["errors"] = errors;

            return Redirect("/profile");
        }

        public IActionResult ChangePhoneNumber()
        {
            ViewData["SmsSent"] = false;
            return View(new ChangePhoneNumberViewModel() { UserId = _helper.GetUserId(User) });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePhoneNumber(ChangePhoneNumberViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userWithPhoneNumber = await _userManager.FindUserByPhoneNumber(model.PhoneNumber);
                if (userWithPhoneNumber != null && userWithPhoneNumber.EmailConfirmed)
                {
                    ModelState.AddModelError(nameof(ChangePhoneNumberViewModel.PhoneNumber), "This phone number is already taken");
                }

                if (ModelState.ErrorCount == 0)
                {
                    if (!string.IsNullOrWhiteSpace(model.Code))
                    {
                        ViewData["SmsSent"] = true;
                        var result = await _userManager.ConfirmPhoneNumber(model.UserId, model.Code);

                        if (result)
                        {
                            return Redirect("/profile");
                        }

                        ModelState.AddModelError(nameof(ChangePhoneNumberViewModel.Code), "Invalid code.");
                        return View(model);
                    }

                    var user = await _userManager.FindUserById(model.UserId);
                    var token = await _userManager.GenerateChangePhoneNumberToken(model.UserId, model.PhoneNumber);
                    var body = $"Confirmation code: {token}";
                    var status = await _smsService.SendAsync(model.PhoneNumber, body);

                    if (status)
                    {
                        user.PhoneNumber = model.PhoneNumber;
                        await _userManager.UpdateUser(user);
                    }
                }
            }

            ViewData["SmsSent"] = false;
            return View(model);
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            var uploadDir = Path.Combine(_environment.WebRootPath, "img/profile");

            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            
            var fileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";
            var filePath = Path.Combine(uploadDir, fileName);
            
            await _imageService.SaveImage(file, filePath);
            return fileName;
        }

        private void DeleteFile(string fileName)
        {
            var uploadDir = Path.Combine(_environment.WebRootPath, "img/profile");
            var filePath = Path.Combine(uploadDir, fileName ?? string.Empty);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}