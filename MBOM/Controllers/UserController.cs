using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MBOM.Models;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using Repository;

namespace MBOM.Controllers
{
    public class UserController : Controller
    {
        private BaseDbContext db;

        public UserController(BaseDbContext db)
        {
            this.db = db;
        }

        public ActionResult Login()
        {
            ViewData["Groups"] = db.AppWorkgroups.ToList();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginView model, string returnUrl)
        {
            int userid = 0;
            string name = "超级管理员", login = "Admin";
            var groups = db.AppWorkgroups.ToList();
            ViewData["Groups"] = groups;
            List<int> roleids, rightids;
            //登录验证
            //内置管理员账号
            if(model.loginname == "admin")
            {
                string passport = ConfigurationManager.AppSettings["admin"];
                if(model.password != null && model.password == passport)
                {
                    rightids = (from roleRight in db.SysRoleRights.ToList()
                                select roleRight.RightId).Distinct().ToList();
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码错误");
                    return View();
                }
            }
            else
            {
                string[] rt = Utils.Utility.AMPassValidate(Constants.Server, Constants.Database, model.loginname, model.password);
                if (rt[0] == "N")
                {
                    ModelState.AddModelError("", rt[1]);
                    return View();
                }
                userid = int.Parse(rt[1]);
                login = model.loginname;
                name = rt[2];
                roleids = (from role in db.SysUserRoles.Where(ur => ur.UserId == userid).ToList()
                           select role.RoleId).Distinct().ToList();
                rightids = (from roleRight in db.SysRoleRights.Where(rr => roleids.Contains(rr.RoleId)).ToList()
                            select roleRight.RightId).Distinct().ToList();
                //验证用户ID是否有这个域
                var logingroup = db.AppWorkgroupUsers.Where(w => w.CN_USERID == userid && w.CN_GROUPID == model.groupid).ToList();
                if (logingroup == null || logingroup.Count() == 0)
                {
                    ModelState.AddModelError("", "用户不属于此域，无法登录");
                    return View();
                }
            }
            //登录成功
            //验证是否重复登录，清空上次登录信息
            Hashtable logins = HttpContext.Application["Logins"] as Hashtable;
            var groupname = groups.Find(g => g.CN_ID == model.groupid).CN_NAME;
            logins[name] = Session.SessionID;
            LoginUserInfo userInfo = new LoginUserInfo
            {
                UserId = userid,
                Login = login,
                Name = name,
                groupid = model.groupid,
                groupname = groupname,
                RightIds = rightids
            };
            //写入cookie
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                0,
                name,
                DateTime.Now,
                DateTime.Now.AddYears(1),
                true,
                JsonConvert.SerializeObject(userInfo) //写入用户角色  
            );
            //对authTickoet进行加密  
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            //存入cookie  
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            FormsAuthentication.SetAuthCookie(name, true, FormsAuthentication.FormsCookiePath);
            authCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(authCookie);
            //跳转至来时页面
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignOut()
        {
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}