using Repository;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Linq;

namespace MBOM.Controllers
{
    [UserAuth]
    public class GroupController : Controller
    {
        private BaseDbContext db;
        
        public GroupController(BaseDbContext db)
        {
            this.db = db;
        }
        //首页
        [Description("域管理页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("查看域列表")]
        public JsonResult List()
        {
            var list = db.AppWorkgroups.ToList();
            return Json(ResultInfo.Success(list));
        }

        [Description("查看域用户")]
        public JsonResult Users(int groupid)
        {
            if (groupid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var list = db.AppWorkgroupUsers.Where(where=> where.CN_GROUPID == groupid);
            return Json(ResultInfo.Success(list));
        }

        [Description("添加域用户")]
        public JsonResult UserAdd(int groupid, UserProductLibraryView[] users)
        {
            if(groupid == 0 || users.Length == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            //添加用户
            List<AppWorkgroupUser> users2add = new List<AppWorkgroupUser>();
            DateTime now = DateTime.Now;
            foreach (var user in users)
            {
                users2add.Add(new AppWorkgroupUser
                {
                    CN_GROUPID = groupid,
                    CN_USERID = user.id,
                    CN_USERNAME = user.name,
                    CN_DT_CREATE = now,
                    CN_CREATE_BY = LoginUserInfo.GetUserInfo().UserId,
                    CN_CREATE_LOGIN = LoginUserInfo.GetUserInfo().Login,
                    CN_CREATE_NAME = LoginUserInfo.GetUserInfo().Name
                });
            }
            db.AppWorkgroupUsers.AddRange(users2add);
            db.SaveChanges();
            return Json(ResultInfo.Success("添加成功。"));
        }

        [Description("删除域用户")]
        public JsonResult UserDel(int[] groupuserids)
        {
            db.AppWorkgroupUsers.RemoveRange(db.AppWorkgroupUsers.Where(where => groupuserids.Contains(where.CN_ID)));
            db.SaveChanges();
            return Json(ResultInfo.Success("删除成功。"));
        }
    }
}