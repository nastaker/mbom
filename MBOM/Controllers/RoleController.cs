using MBOM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;
using AutoMapper;
using Localization;
using System.ComponentModel;
using MBOM.Filters;
using Repository;

namespace MBOM.Controllers
{
    [UserAuth]
    public class RoleController : Controller
    {
        private BaseDbContext db;

        public RoleController(BaseDbContext db)
        {
            this.db = db;
        }

        [Description("角色页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("添加角色")]
        public JsonResult Add(SysRoleView roleView)
        {
            db.SysRoles.Add(Mapper.Map<SysRole>(roleView));
            db.SaveChanges();
            return Json(ResultInfo.Success(Lang.AddRoleInfoSuccess));
        }

        [Description("编辑角色")]
        public JsonResult Edit(SysRoleView roleView)
        {
            var model = Mapper.Map<SysRole>(roleView);
            db.SysRoles.Attach(model);
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(ResultInfo.Success(Lang.EditRoleInfoSuccess));
        }

        [Description("删除角色")]
        public JsonResult Delete(SysRoleView roleView)
        {
            db.SysRoles.Remove(db.SysRoles.Find(roleView.ID));
            db.SaveChanges();
            return Json(ResultInfo.Success(Lang.DeleteRoleInfoSuccess));
        }

        [Description("角色数据（分页）")]
        public JsonResult PageList(SysRoleView roleView, int page = 1, int rows = 10)
        {
            var query = db.SysRoles.AsQueryable();
            if (!string.IsNullOrEmpty(roleView.RoleName))
            {
                query = from role in query
                        where role.RoleName.Contains(roleView.RoleName)
                        select role;
            }
            var roles = query.OrderBy(a => a.ID).Skip((page - 1) * rows).Take(rows).ToList();
            var dtoroles = Mapper.Map<List<SysRoleView>>(roles);
            var total = db.SysRoles.Count();
            return Json(ResultInfo.Success(new { rows = dtoroles, total = total }));
        }
    }
}