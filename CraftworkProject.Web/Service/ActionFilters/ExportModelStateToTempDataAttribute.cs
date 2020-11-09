using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CraftworkProject.Web.Service.ActionFilters
{
    public class ExportModelStateToTempDataAttribute : ModelStateTempDataTransferAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = (Controller) filterContext.Controller;
            //Only export when ModelState is not valid
            if (!controller.ViewData.ModelState.IsValid)
            {
                //Export if we are redirecting
                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                {
                    controller.TempData[Key] = controller.ViewData.ModelState;
                }
            }
 
            base.OnActionExecuted(filterContext);
        }
    }
}