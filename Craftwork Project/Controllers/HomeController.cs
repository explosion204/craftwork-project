using Microsoft.AspNetCore.Mvc;

namespace Craftwork_Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}