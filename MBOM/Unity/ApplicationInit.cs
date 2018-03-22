using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MBOM.Unity
{
    public class ApplicationInit
    {
        public static void InitData()
        {
            InitActions();
            InitRightActions();
        }


        internal static void InitActions()
        {
            SysActionBLL sysactionbll = new SysActionBLL();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies(); // currently loaded assemblies
            var controllerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t != null
                    && t.IsPublic // public controllers only
                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) // enfore naming convention
                    && !t.IsAbstract // no abstract controllers
                    && typeof(Controller).IsAssignableFrom(t)); // should implement IController (happens automatically when you extend Controller)
            var urls = from controllerType
                       in controllerTypes
                       from method
                        in controllerType.GetMethods()
                       where
                        method.Module.Name == controllerType.Module.Name
                        && typeof(ActionResult).IsAssignableFrom(method.ReturnType)
                       select new SysAction
                       {
                           Url = controllerType.Name.Replace("Controller", "") + "/" + method.Name,
                           Description = method.GetCustomAttribute<DescriptionAttribute>() == null ? "" : method.GetCustomAttribute<DescriptionAttribute>().Description
                       };
            var existsUrls = sysactionbll.GetAll().Select(s => s.Url);
            var news = urls.Where(url => !existsUrls.Contains(url.Url));
            sysactionbll.AddRange(news);
            sysactionbll.SaveChanges();
        }

        public static void InitRightActions()
        {
            SysRightActionBLL rightactionbll = new SysRightActionBLL();
            var actions = rightactionbll.GetAll();
            CacheHelper.SetCache("RightActions", actions);
        }
    }
}