using System;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain.Identity;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole<Guid>> roleManager;
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            ViewBag.userManager = userManager;
            return View(userManager.Users);
        }

        public IActionResult Create()
        {
            ViewBag.AllRoles = roleManager.Roles.ToList();
            return View(new UserViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            ViewBag.AllRoles = roleManager.Roles.ToList();

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
                    if (model.NewPassword.Equals(model.ConfirmNewPassword))
                    {
                        ApplicationUser newUser = new ApplicationUser()
                        {
                            UserName = model.Username,
                            Email = model.Email,
                            EmailConfirmed = model.Verified
                        };

                        var userCreation = await userManager.CreateAsync(newUser, model.NewPassword);
                        if (userCreation.Succeeded)
                        {
                            var userRole = await roleManager.FindByIdAsync(model.RoleId.ToString());
                            var roleAssigning = await userManager.AddToRoleAsync(newUser, userRole.Name);
                            if (roleAssigning.Succeeded)
                            {
                                return Redirect("/admin/users");
                            }
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
            if (!userManager.GetUserId(User).Equals(id.ToString()))
            {
                ApplicationUser user = await userManager.FindByIdAsync(id.ToString());
                await userManager.DeleteAsync(user);
                return true;
            }

            return false;
        }
        
        public async Task<IActionResult> Update(Guid id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id.ToString());
            UserViewModel model = new UserViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Verified = user.EmailConfirmed
            };
            
            ViewBag.AllRoles = roleManager.Roles.ToList();
            ViewBag.RoleEditAllowed = !userManager.GetUserId(User).Equals(id.ToString());
            return View(model);
        }
    
        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            ApplicationUser user = await userManager.FindByNameAsync(model.Username);
            ViewBag.RoleEditAllowed = !userManager.GetUserId(User).Equals(user.Id.ToString());
            ViewBag.AllRoles = roleManager.Roles.ToList();

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.EmailConfirmed = model.Verified;
                
                if (!userManager.GetUserId(User).Equals(user.Id.ToString()))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var newRole = await roleManager.FindByIdAsync(model.RoleId.ToString());
                    await userManager.RemoveFromRoleAsync(user, userRoles[0]);
                    await userManager.AddToRoleAsync(user, newRole.Name);
                }

                if (!String.IsNullOrEmpty(model.NewPassword) && !String.IsNullOrEmpty(model.ConfirmNewPassword))
                {
                    if (!model.NewPassword.Equals(model.ConfirmNewPassword))
                    {
                        ModelState.AddModelError(nameof(UserViewModel.NewPassword), "Passwords do not match");
                        ModelState.AddModelError(nameof(UserViewModel.ConfirmNewPassword), "Passwords do not match");
                        return View(model);
                    }
                    
                    string newPasswordHash = userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                    user.PasswordHash = newPasswordHash;
                }

                await userManager.UpdateAsync(user);

                return Redirect("/admin/users");
            }

            return View(model);
        }
    }
}
