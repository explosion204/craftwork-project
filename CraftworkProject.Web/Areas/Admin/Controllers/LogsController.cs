using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogsController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public LogsController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        
        public IActionResult GetAllLogs(int date)
        {
            var filePath = $"{_environment.WebRootPath}/logs/all-{date}.log";

            if (System.IO.File.Exists(filePath))
            {
                return new PhysicalFileResult(filePath, "text/plain");
            }

            return new NotFoundResult();
        }

        public IActionResult GetErrorLogs(int date)
        {
            var filePath = $"{_environment.WebRootPath}/logs/error-{date}.log";

            if (System.IO.File.Exists(filePath))
            {
                return new PhysicalFileResult(filePath, "text/plain");
            }

            return new NotFoundResult();
        }
    }
}