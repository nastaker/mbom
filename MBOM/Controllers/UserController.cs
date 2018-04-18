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
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginView model, string returnUrl)
        {
            int userid = 0;
            string name = "超级管理员", login = "Admin";
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
                roleids = (from role in db.SysUserRoles.Where(ur => ur.UserId == userid)
                           select role.RoleId).Distinct().ToList();
                rightids = (from roleRight in db.SysRoleRights.Where(rr => roleids.Contains(rr.RoleId))
                            select roleRight.RightId).Distinct().ToList();
            }
            //登录成功
            //验证是否重复登录，清空上次登录信息
            Hashtable logins = HttpContext.Application["Logins"] as Hashtable;
            logins[name] = Session.SessionID;
            LoginUserInfo userInfo = new LoginUserInfo
            {
                UserId = userid,
                LoginName = login,
                Name = name,
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