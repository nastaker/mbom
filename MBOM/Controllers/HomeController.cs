using MBOM.Filters;
using System.ComponentModel;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class HomeController : Controller
    {
        [Description("访问首页权限")]
        public ActionResult Index()
        {
            return View();
        }
    }
}