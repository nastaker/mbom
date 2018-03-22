using AutoMapper;
using BLL;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Microsoft.Practices.Unity;
using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class RoleRightController : Controller
    {
        [Dependency]
        public SysRoleBLL rolebll { get; set; }
        [Dependency]
        public SysRoleRightBLL rolerightbll { get; set; }
        // GET: RoleRight
        [Description("角色权限页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("角色权限列表")]
        public JsonResult List()
        {
            var roles = rolebll.GetAll();
            var roleviews = Mapper.Map<List<SysRoleView>>(roles);
            return Json(ResultInfo.Success(roleviews));
        }

        [Description("角色权限编辑")]
        public JsonResult Edit(int roleId, int[] menuIds)
        {
            //判断role是否存在，不存在刷新
            SysRole roleInfo = rolebll.Get(roleId);
            if (roleInfo == null)
            {
                return Json(ResultInfo.Fail(Lang.RoleNotExist));
            }
            rolerightbll.EditRoleRight(roleId, menuIds);
            return Json(ResultInfo.Success(Lang.EditRoleRightSuccess));
        }
    }
}