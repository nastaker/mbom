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
using System.Collections;

namespace MBOM.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class UserAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = LoginUserInfo.GetLoginUser();
            // 未登录，返回登录界面
            if (user == null)
            {
                return false;
            }
            Hashtable logins = httpContext.Application["Logins"] as Hashtable;
            // 使用的缓存Cookie登录，返回登录界面
            if (logins[user.Name] == null)
            {
                return false;
            }
            // 登录被注销（重复登录），提示重新登录
            if (logins[user.Name] != null && logins[user.Name].ToString() != httpContext.Session.SessionID)
            {
                httpContext.Response.StatusCode = 412;
                return false;
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
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("没有操作权限，请联系管理员！"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                    case 412:
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("您的账号已在其他地方登录，请重新登录！"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                    default:
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("登录已失效，请重新登录。"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                }
            }
            else
            {
                var viewData = new ViewDataDictionary();
                switch (errorCode)
                {
                    case 403:
                        viewData["Message"] = "没有操作权限，请联系管理员！";
                        filterContext.Result = new ViewResult
                        {
                            ViewName = "~/Views/Shared/Error.cshtml",
                            ViewData = viewData
                        };
                        break;
                    case 412:
                        viewData["Message"] = "您的账号已在其他地方登录，请重新登录！";
                        filterContext.Result = new ViewResult
                        {
                            ViewName = "~/Views/Shared/Error.cshtml",
                            ViewData = viewData
                        };
                        break;
                    default:
                        base.HandleUnauthorizedRequest(filterContext);
                        break;
                }
            }
        }

    }
}