﻿using MBOM.Models;
using System.Text;
using System.Web.Mvc;

namespace MBOM.Filters
{
    public class CustomErrorFilter : HandleErrorAttribute
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CustomErrorFilter));

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            //处理
            //获取出错的【控制器】和【动作】
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            string errMsg = filterContext.Exception.Message;
            string innerErrMsg = filterContext.Exception.InnerException == null ? "没有详细错误信息" : filterContext.Exception.InnerException.Message;
            //
            log.ErrorFormat("{0}.{1}；{2}{3}。", controllerName, actionName, errMsg, innerErrMsg);
            //
            filterContext.ExceptionHandled = true;
#if DEBUG
                // 调试用代码
                filterContext.ExceptionHandled = false;
#endif
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                switch (filterContext.HttpContext.Response.StatusCode)
                {
                    case 403:
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("用户没有操作权限"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                    case 404:
                        filterContext.Result = new JsonResult { Data = ResultInfo.Fail("404——资源未找到"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        break;
                    default:
                        filterContext.Result = new ContentResult
                        {
                            Content = "<p>" + errMsg + "</p><p>" + innerErrMsg + "</p>",
                            ContentEncoding = Encoding.UTF8,
                            ContentType = "html/text"
                        };
                        break;
                }
            }
            else
            {
                var viewData = new ViewDataDictionary();
                viewData["Message"] = errMsg;
                viewData["InnerMessage"] = innerErrMsg;
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = viewData
                };
            }
        }
    }
}