using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MBOM.Filters
{
    public class MaintenanceActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var code = filterContext.RequestContext.HttpContext.Request.Params["code"];
            if (string.IsNullOrWhiteSpace(code))
            {
                filterContext.Result = new HttpNotFoundResult();
            }
            base.OnActionExecuting(filterContext);
        }
    }
}