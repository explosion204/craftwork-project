using Microsoft.AspNetCore.Mvc.Filters;

namespace CraftworkProject.Web.Service.ActionFilters
{
    public class ModelStateTempDataTransferAttribute : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTempDataTransferAttribute).FullName;
    }
}