using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class ProfileController : Controller
    {
        // GET
        public IActionResult User()
        {
            return View();
        }
    }
}