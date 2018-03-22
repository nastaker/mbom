using AutoMapper;
using BLL;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Microsoft.Practices.Unity;
using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class UserRoleController : Controller
    {
        [Dependency]
        public SysUserRoleBLL bll { get; set; }
        [Dependency]
        public SysMenuBLL menubll { get; set; }
        // GET: UserRole
        [Description("用户角色页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("组织结构树数据")]
        public JsonResult OrganizationTree()
        {
            ResultInfo rt = null;
            try
            {
                var list = Utils.Utility.GetEmGroupAndUsers(Constants.Server, Constants.Database);
                list.Sort((x, y) => { return x.order.CompareTo(y.order); });
                rt = ResultInfo.Success(list);
            }
            catch (System.Exception ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        [Description("获取用户拥有角色")]
        public JsonResult GetUserRoles(int userid)
        {
            var list = bll.GetList(where => where.UserId == userid);
            var dtoModels = Mapper.Map<List<SysUserRoleView>>(list);
            return Json(ResultInfo.Success(dtoModels));
        }

        [Description("获取角色用户列表")]
        public JsonResult GetRoleUsers(int roleid)
        {
            var list = bll.GetList(where => where.RoleId == roleid);
            var dtoModels = Mapper.Map<List<SysUserRoleView>>(list);
            return Json(ResultInfo.Success(dtoModels));
        }

        [Description("编辑用户角色")]
        public JsonResult Edit(SysUserRoleView[] users, int[] roleIds)
        {
            var dtoModels = Mapper.Map<SysUserRole[]>(users);
            bll.EditUserRole(dtoModels, roleIds);
            return Json(ResultInfo.Success(Lang.EditUserRoleSuccess));
        }
    }
}