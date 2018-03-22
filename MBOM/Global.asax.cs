using MBOM.Models;
using MBOM.Unity;
using Microsoft.Practices.Unity;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace MBOM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //注入 Ioc
            var container = new UnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            //automapper
            AutoMapperConfig.Initialize();
            //BundleTable.EnableOptimizations = true;

            Constants.Server = ConfigurationManager.AppSettings["Server"];
            Constants.Database = ConfigurationManager.AppSettings["Database"];

            ApplicationInit.InitData();
        }
    }
}
