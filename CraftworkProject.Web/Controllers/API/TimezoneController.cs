using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers.API
{
    public class TimezoneController : Controller
    {
        [HttpPost]
        public IActionResult Index(string timeZoneOffset)
        {
            HttpContext.Session.SetString("timeZoneOffset", timeZoneOffset);
            return Json(new {success = true});
        }
    }
}