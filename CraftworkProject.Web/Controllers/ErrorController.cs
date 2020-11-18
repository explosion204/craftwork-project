using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            return statusCode switch
            {
                403 => View("403"),
                404 => View("404"),
                _ => Redirect("/")
            };
        }
    }
}