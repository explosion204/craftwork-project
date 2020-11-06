using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}