using AutoMapper;
using Repository;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Linq;

namespace MBOM.Controllers
{
    [UserAuth]
    public class RoleRightController : Controller
    {
        private BaseDbContext db;

        public RoleRightController(BaseDbContext db)
        {
            this.db = db;
        }
        // GET: RoleRight
        [Description("角色权限页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("角色权限列表")]
        public JsonResult List()
        {
            BaseDbContext db = new BaseDbContext();
            var roles = db.SysRoles.ToList();
            var roleviews = Mapper.Map<List<SysRoleView>>(roles);
            return Json(ResultInfo.Success(roleviews));
        }

        [Description("角色权限编辑")]
        public JsonResult Edit(int roleId, int[] menuIds)
        {
            //判断role是否存在，不存在刷新
            BaseDbContext db = new BaseDbContext();
            SysRole roleInfo = db.SysRoles.Find(roleId);
            if (roleInfo == null)
            {
                return Json(ResultInfo.Fail(Lang.RoleNotExist));
            }
            db.SysRoleRights.RemoveRange(db.SysRoleRights.Where(w => w.RoleId == roleId));
            if (menuIds == null)
            {
                db.SaveChanges();
                return Json(ResultInfo.Success(Lang.EditRoleRightSuccess));
            }
            List<SysRoleRight> list = new List<SysRoleRight>();
            for (int i = 0; i < menuIds.Length; i++)
            {
                list.Add(new SysRoleRight
                {
                    RoleId = roleId,
                    RightId = menuIds[i]
                });
            }
            db.SysRoleRights.AddRange(list);
            db.SaveChanges();
            return Json(ResultInfo.Success(Lang.EditRoleRightSuccess));
        }
    }
}