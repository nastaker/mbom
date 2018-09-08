﻿using System.ComponentModel.DataAnnotations;
using Localization;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;
using Model;
using Repository;

namespace MBOM.Models
{
    public class LoginView
    {
        [Display(Name = "LoginName", ResourceType = typeof(Lang))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Lang))]
        [MinLength(3, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Lang))]
        public string loginname { get; set; }
        [Display(Name = "Password", ResourceType = typeof(Lang))]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Group", ResourceType = typeof(Lang))]
        public int groupid { get; set; }
        [Display(Name = "RememberMe", ResourceType = typeof(Lang))]
        public bool rememberme { get; set; }
    }

    public class LoginUserInfo
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public int groupid { get; set; }
        public string groupname { get; set; }
        public List<int> RightIds { get; set; }

        public static LoginUserInfo GetLoginUser()
        {
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                return new LoginUserInfo
                {
                    UserId = 0,
                    Login = "Admin",
                    Name = "超级管理员",
                    groupname = "测试",
                };
            }
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return null;
            }
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            return JsonConvert.DeserializeObject<LoginUserInfo>(authTicket.UserData);
        }

        public static UserInfo GetUserInfo()
        {
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                return new UserInfo
                {
                    UserId = 0,
                    Login = "Admin",
                    Name = "超级管理员"
                };
            }
            LoginUserInfo loginUser = GetLoginUser();
            if(loginUser == null)
            {
                return null;
            }
            return new UserInfo
            {
                UserId = loginUser.UserId,
                Login = loginUser.Login,
                Name = loginUser.Name
            };
        }
    }
}