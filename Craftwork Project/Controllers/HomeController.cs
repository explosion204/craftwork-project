using Microsoft.AspNetCore.Mvc;

namespace Craftwork_Project.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}