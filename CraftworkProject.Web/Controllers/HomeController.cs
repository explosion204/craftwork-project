using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}