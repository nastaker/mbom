using MBOM.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Microsoft.Practices.Unity;
using BLL;
using System.Collections.Generic;
using MBOM.Unity;
using Model;

namespace MBOM.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class UserAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = LoginUserInfo.GetLoginUser();
            if (user == null)
            {
                return false;
            }
            if (httpContext.IsDebuggingEnabled)
            {
                return true;
            }
            if(user.UserId == 0) //超级管理员不设限制
            {
                return true;
            }
            var rightactions = (CacheHelper.GetCache("RightActions") as List<SysRightAction>);

            var urls =  (from r in rightactions
                        where user.RightIds.Contains(r.MenuId)
                        select r.ActionInfo.Url).ToList();

            var controller = httpContext.Request.RequestContext.RouteData.Route.GetRouteData(httpContext).Values["controller"];
            var action = httpContext.Request.RequestContext.RouteData.Route.GetRouteData(httpContext).Values["action"];
            if(urls.Contains(controller + "/" + action))
            {
                return true;
            }
            else
            {
                httpContext.Response.StatusCode = 403;
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var errorCode = filterContext.HttpContext.Response.StatusCode;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                switch (errorCode)
                {
                    case 403:
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("用户没有操作权限"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                    case 404:
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("404——资源未找到"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                    default:
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("其他错误，错误代码："+ errorCode), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                }
            }
            else
            {
                switch (errorCode)
                {
                    case 403:
                        filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                        filterContext.Result = new ContentResult() { Content = "用户没有所需权限，请联系管理员！" };
                        break;
                    default:
                        filterContext.Result = new RedirectResult("/User/Login");
                        break;
                }
            }
        }

    }
}