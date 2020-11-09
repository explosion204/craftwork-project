using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CraftworkProject.Web.Service.ActionFilters
{
    public class ImportModelStateFromTempDataAttribute : ModelStateTempDataTransferAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = (Controller) filterContext.Controller;

            if (controller.TempData[Key] is ModelStateDictionary modelState)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    //Otherwise remove it.
                    controller.TempData.Remove(Key);
                }
            }
 
            base.OnActionExecuted(filterContext);
        }
    }
}