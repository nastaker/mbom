using BLL;
using Repository;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Microsoft.Practices.Unity;
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
        [Dependency]
        public AppWorkgroupBLL awgbll { get; set; }
        [Dependency]
        public AppWorkgroupUserBLL awgubll { get; set; }

        //首页
        [Description("域管理页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("查看域列表")]
        public JsonResult List()
        {
            var list = awgbll.GetAll();
            return Json(ResultInfo.Success(list));
        }

        [Description("查看域用户")]
        public JsonResult Users(int groupid)
        {
            if (groupid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var list = awgubll.GetList(where=> where.CN_GROUPID == groupid);
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
            awgubll.AddRange(users2add);
            awgubll.SaveChanges();
            return Json(ResultInfo.Success("添加成功。"));
        }

        [Description("删除域用户")]
        public JsonResult UserDel(int[] groupuserids)
        {
            awgubll.Delete(where => groupuserids.Contains(where.CN_ID));
            awgubll.SaveChanges();
            return Json(ResultInfo.Success("删除成功。"));
        }
    }
}