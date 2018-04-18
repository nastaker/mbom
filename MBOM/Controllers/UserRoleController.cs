using AutoMapper;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Model;
using Repository;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class UserRoleController : Controller
    {
        private BaseDbContext db;

        public UserRoleController(BaseDbContext db)
        {
            this.db = db;
        }

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
            using (db)
            {
                var list = db.SysUserRoles.Where(where => where.UserId == userid);
                var dtoModels = Mapper.Map<List<SysUserRoleView>>(list);
                return Json(ResultInfo.Success(dtoModels));
            }
        }

        [Description("获取角色用户列表")]
        public JsonResult GetRoleUsers(int roleid)
        {
            var list = db.SysUserRoles.Where(where => where.RoleId == roleid);
            var dtoModels = Mapper.Map<List<SysUserRoleView>>(list);
            return Json(ResultInfo.Success(dtoModels));
        }

        [Description("编辑用户角色")]
        public JsonResult Edit(SysUserRoleView[] users, int[] roleIds)
        {
            var dtoModels = Mapper.Map<SysUserRole[]>(users);
            var userids = users.Select(us => us.UserId);
            db.SysUserRoles.RemoveRange(db.SysUserRoles.Where(r => userids.Contains(r.UserId)));
            List<SysUserRole> list = new List<SysUserRole>();
            for (int i = 0; i < users.Length; i++)
            {
                for (int j = 0; j < roleIds.Length; j++)
                {
                    list.Add(new SysUserRole
                    {
                        UserId = users[i].UserId,
                        Name = users[i].Name,
                        RoleId = roleIds[j]
                    });
                }
            }
            db.SysUserRoles.AddRange(list);
            db.SaveChanges();
            return Json(ResultInfo.Success(Lang.EditUserRoleSuccess));
        }
    }
}