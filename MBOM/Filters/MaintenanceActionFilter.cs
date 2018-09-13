using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MBOM.Filters
{
    public class MaintenanceActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var prod_itemcode = filterContext.RequestContext.HttpContext.Request.Params["prod_itemcode"];
            var itemcode = filterContext.RequestContext.HttpContext.Request.Params["itemcode"];
            if (string.IsNullOrWhiteSpace(prod_itemcode) && string.IsNullOrWhiteSpace(itemcode))
            {
                filterContext.Result = new HttpNotFoundResult();
            }
            base.OnActionExecuting(filterContext);
        }
    }
}