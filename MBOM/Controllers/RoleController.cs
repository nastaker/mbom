using BLL;
using MBOM.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using AutoMapper;
using Localization;
using System.ComponentModel;
using MBOM.Filters;

namespace MBOM.Controllers
{
    [UserAuth]
    public class RoleController : Controller
    {
        [Dependency]
        public SysRoleBLL bll { get; set; }
        // GET: Authority
        [Description("角色页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("添加角色")]
        public JsonResult Add(SysRoleView roleView)
        {
            bll.Add(Mapper.Map<SysRole>(roleView));
            bll.SaveChanges();
            return Json(ResultInfo.Success(Lang.AddRoleInfoSuccess));
        }

        [Description("编辑角色")]
        public JsonResult Edit(SysRoleView roleView)
        {
            bll.Edit(Mapper.Map<SysRole>(roleView));
            bll.SaveChanges();
            return Json(ResultInfo.Success(Lang.EditRoleInfoSuccess));
        }

        [Description("删除角色")]
        public JsonResult Delete(SysRoleView roleView)
        {
            bll.Delete(roleView.ID);
            bll.SaveChanges();
            return Json(ResultInfo.Success(Lang.DeleteRoleInfoSuccess));
        }

        [Description("角色数据（分页）")]
        public JsonResult PageList(SysRoleView roleView, int page = 1, int rows = 10)
        {
            var query = bll.GetQueryable();
            if (!string.IsNullOrEmpty(roleView.RoleName))
            {
                query = from role in query
                        where role.RoleName.Contains(roleView.RoleName)
                        select role;
            }
            var roles = query.OrderBy(a => a.ID).Skip((page - 1) * rows).Take(rows).ToList();
            var dtoroles = Mapper.Map<List<SysRoleView>>(roles);
            var total = bll.Count();
            return Json(ResultInfo.Success(new { rows = dtoroles, total = total }));
        }
    }
}