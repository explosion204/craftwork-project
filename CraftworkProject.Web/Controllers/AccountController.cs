using System.Threading.Tasks;
using Craftwork_Project.ViewModels;
using CraftworkProject.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;

        public AccountController(IUserManager userManager)
        {
            _userManager = userManager;
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
        public IActionResult Signup()
        {
            return View();
        }
    }
}