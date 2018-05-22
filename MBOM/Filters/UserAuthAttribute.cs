using MBOM.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.Collections.Generic;
using MBOM.Unity;
using Model;
using System.Collections;
using Repository;
using Newtonsoft.Json;

namespace MBOM.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class UserAuthAttribute : AuthorizeAttribute
    {
      
        private void LogUserInfo(string actionUrl, string actionName, string requestType, bool issuccess, string desc, LoginUserInfo user)
        {
            using (BaseDbContext db = new BaseDbContext())
            {
                db.TN_SYS_LOG.Add(new TN_SYS_LOG
                {
                    CN_ACTIONURL = actionUrl,
                    CN_USERIP = actionName,
                    CN_DESC = desc,
                    CN_REQUESTTYPE = requestType,
                    CN_ISSUCCESS = issuccess,
                    CN_USERID = user.UserId,
                    CN_USERNAME = user.Name,
                    CN_USERLOGIN = user.LoginName,
                    CN_DT_DATE = DateTime.Now
                });
                db.SaveChanges();
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = LoginUserInfo.GetLoginUser();
            var controller = httpContext.Request.RequestContext.RouteData.Route.GetRouteData(httpContext).Values["controller"];
            var action = httpContext.Request.RequestContext.RouteData.Route.GetRouteData(httpContext).Values["action"];
            var userip = httpContext.Request.UserHostAddress;
            var requestType = httpContext.Request.RequestType;
            var actionUrl = controller + "/" + action;
            // 未登录，返回登录界面
            if (user == null)
            {
                LogUserInfo(actionUrl, userip, requestType, false, "用户未登录", user);
                return false;
            }
            Hashtable logins = httpContext.Application["Logins"] as Hashtable;
            // 使用的缓存Cookie登录，返回登录界面
            if (logins[user.Name] == null)
            {
                LogUserInfo(actionUrl, userip, requestType, false, "用户信息失效", user);
                return false;
            }
            // 登录被注销（重复登录），提示重新登录
            if (logins[user.Name] != null && logins[user.Name].ToString() != httpContext.Session.SessionID)
            {
                LogUserInfo(actionUrl, userip, requestType, false, "用户在其他地方登录", user);
                httpContext.Response.StatusCode = 412;
                return false;
            }
            if (user.UserId == 0) //超级管理员不设限制
            {
                LogUserInfo(actionUrl, userip, requestType, true, "", user);
                return true;
            }
            var rightactions = (CacheHelper.GetCache("RightActions") as List<SysRightAction>);

            var urls =  (from r in rightactions
                        where user.RightIds.Contains(r.MenuId)
                        select r.ActionInfo.Url).ToList();

            if(urls.Contains(actionUrl))
            {
                LogUserInfo(actionUrl, userip, requestType, true, "", user);
                return true;
            }
            else
            {
                LogUserInfo(actionUrl, userip, requestType, false, "用户没有权限", user);
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