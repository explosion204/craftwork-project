using System;
using System.IO;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IImageService _imageService;
        private readonly IUserManagerHelper _helper;
        
        public UsersController(IUserManager userManager, IUserManagerHelper helper, IWebHostEnvironment environment, IImageService imageService)
        {
            _userManager = userManager;
            _helper = helper;
            _environment = environment;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            return View(_userManager.GetAllUsers());
        }

        public IActionResult Create()
        {
            ViewBag.AllRoles = _userManager.GetAllRoles();
            return View(new UserViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            ViewBag.AllRoles = _userManager.GetAllRoles();

            if (ModelState.IsValid)
            {
                var userWithEmail = await _userManager.FindUserByEmail(model.Email);
                if (userWithEmail != null && userWithEmail.EmailConfirmed)
                {
                    ModelState.AddModelError(nameof(UserViewModel.Email), "This email is already taken");
                }
                
                if (string.IsNullOrEmpty(model.NewPassword))
                {
                    ModelState.AddModelError(nameof(UserViewModel.NewPassword), "This field must be not empty");
                }

                if (string.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "This field must be not empty");
                }

                if (ModelState.ErrorCount == 0)
                {
                    if (model.NewPassword != null && model.NewPassword.Equals(model.ConfirmNewPassword))
                    {
                        string fileName = null;
                        if (model.ProfilePicture != null)
                        {
                            fileName = await UploadFile(model.ProfilePicture);
                        } 
                        
                        User newUser = new User()
                        {
                            Username = model.Username,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            EmailConfirmed = model.Verified,
                            PhoneNumber = model.PhoneNumber,
                            PhoneNumberConfirmed = model.PhoneNumberConfirmed,
                            ProfilePicture = fileName
                        };
                        var result = await _userManager.CreateUser(newUser, model.NewPassword, model.RoleId);

                        if (result != default)
                        {
                            return Redirect("/admin/users");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(UserViewModel.NewPassword), "Passwords do not match");
                        ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "Passwords do not match");
                    }
                }
            }

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUserId = _helper.GetUserId(User);
            
            if (!currentUserId.ToString().Equals(id))
            {
                await _userManager.DeleteUser(Guid.Parse(id));
                return Json(new {success = true});
            }
            
            return Json(new {success = false});
        }
        
        public async Task<IActionResult> Update(Guid id)
        {
            var user = await _userManager.FindUserById(id);
            var roleId = await _userManager.GetUserRoleId(id);
            
            UserViewModel model = new UserViewModel()
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Verified = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                RoleId = roleId
            };

            ViewBag.AllRoles = _userManager.GetAllRoles();
            ViewBag.RoleEditAllowed = !_helper.GetUserId(User).ToString()?.Equals(id.ToString());
            return View(model);
        }
    
        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            var user = await _userManager.FindUserByName(model.Username);
            var roleEditAllowed = !_helper.GetUserId(User).ToString()?.Equals(user.Id.ToString());
            ViewBag.RoleEditAllowed = roleEditAllowed;
            ViewBag.AllRoles = _userManager.GetAllRoles();

            if (ModelState.IsValid)
            {
                if (model.ProfilePicture != null)
                {
                    DeleteFile(user.ProfilePicture);
                    user.ProfilePicture = await UploadFile(model.ProfilePicture);
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.EmailConfirmed = model.Verified;
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;

                if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    if (!model.NewPassword.Equals(model.ConfirmNewPassword))
                    {
                        ModelState.AddModelError(nameof(UserViewModel.NewPassword), "Passwords do not match");
                        ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "Passwords do not match");
                        return View(model);
                    }
                    
                    await _userManager.SetUserPassword(user.Id, model.NewPassword);
                }
                
                if (roleEditAllowed ?? false)
                {
                    await _userManager.SetUserRole(user.Id, model.RoleId);
                }

                await _userManager.UpdateUser(user);
                
                return Redirect("/admin/users");
            }

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
            var filePath = Path.Combine(uploadDir, fileName ?? "");
            
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
