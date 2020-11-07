using System;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;
        
        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.userManager = _userManager;
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
                if (String.IsNullOrEmpty(model.NewPassword))
                {
                    ModelState.AddModelError(nameof(UserViewModel.NewPassword), "This field must be not empty");
                }

                if (String.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "This field must be not empty");
                }

                if (ModelState.ErrorCount == 0)
                {
                    if (model.NewPassword != null && model.NewPassword.Equals(model.ConfirmNewPassword))
                    {
                        User newUser = new User()
                        {
                            Username = model.Username,
                            Email = model.Email,
                            EmailConfirmed = model.Verified
                        };

                        var result = await _userManager.CreateUser(newUser, model.NewPassword, model.RoleId);

                        if (result)
                        {
                            return Redirect("/admin/users");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(UserViewModel.NewPassword), "Passwords do not match");
                        ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "Passwords do not match");
                    }

                    return View(model);
                }

                return View(model);
            }

            return View(model);
        }
        
        [HttpPost]
        public async Task<bool> Delete(Guid id)
        {
            Guid currentUserId = _userManager.GetUserId(User);
            
            if (!currentUserId.ToString().Equals(id.ToString()))
            {
                await _userManager.DeleteUser(id);
                return true;
            }
            
            return false;
        }
        
        public async Task<IActionResult> Update(Guid id)
        {
            var user = await _userManager.FindUser(id);
            var roleId = await _userManager.GetUserRoleId(id);
            
            UserViewModel model = new UserViewModel()
            {
                Username = user.Username,
                Email = user.Email,
                Verified = user.EmailConfirmed,
                RoleId = roleId
            };

            ViewBag.AllRoles = _userManager.GetAllRoles();
            ViewBag.RoleEditAllowed = !_userManager.GetUserId(User).ToString()?.Equals(id.ToString());
            return View(model);
        }
    
        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            var user = await _userManager.FindUser(model.Username);
            bool? roleEditAllowed = !_userManager.GetUserId(User).ToString()?.Equals(user.Id.ToString());
            ViewBag.RoleEditAllowed = roleEditAllowed;
            ViewBag.AllRoles = _userManager.GetAllRoles();

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.EmailConfirmed = model.Verified;

                if (!String.IsNullOrEmpty(model.NewPassword) && !String.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    if (!model.NewPassword.Equals(model.ConfirmNewPassword))
                    {
                        ModelState.AddModelError(nameof(UserViewModel.NewPassword), "Passwords do not match");
                        ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "Passwords do not match");
                        return View(model);
                    }
                    
                    await _userManager.SetUserPassword(user, model.NewPassword);
                }
                
                if (roleEditAllowed ?? false)
                {
                    await _userManager.SetUserRole(user, model.RoleId);
                }

                await _userManager.UpdateUser(user);
                
                return Redirect("/admin/users");
            }

            return View(model);
        }
    }
}
