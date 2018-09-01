using Model;
using Repository;
using System;
using System.Collections;
using System.Data.Entity;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MBOM.Unity
{
    public class ApplicationInit
    {
        public static void InitData()
        {
            InitActions();
            InitRightActions();
            InitLogins();
        }


        internal static void InitActions()
        {
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
            using (BaseDbContext db = new BaseDbContext())
            {
                var existsUrls = db.SysActions.Select(s => s.Url);
                var news = urls.Where(url => !existsUrls.Contains(url.Url));
                db.SysActions.AddRange(news);
                db.SaveChanges();
            }
        }

        public static void InitRightActions()
        {
            using (BaseDbContext db = new BaseDbContext())
            {
                var actions = db.SysRightActions
                     .Include(s => s.MenuInfo)
                     .Include(s => s.ActionInfo).ToList();
                CacheHelper.SetCache("RightActions", actions);
            }
        }

        internal static void InitLogins()
        {
            HttpContext.Current.Application["Logins"] = new Hashtable();
        }
    }
}