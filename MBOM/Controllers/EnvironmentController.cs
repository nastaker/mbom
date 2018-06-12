using MBOM.Filters;
using MBOM.Unity;
using System.ComponentModel;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class EnvironmentController : Controller
    {
        [Description("更新控制器动作数据")]
        public ActionResult InitActions()
        {
            ApplicationInit.InitActions();
            return Content("已成功更新控制器动作数据", "text/html");
        }

        [Description("更新权限操作数据")]
        public ActionResult RefreshRightActions()
        {
            ApplicationInit.InitRightActions();
            return Content("已成功更新权限操作数据", "text/html");
        }
    }
}