using System.ComponentModel.DataAnnotations;
using Localization;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;
using Model;

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
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Lang))]
        //[MinLength(3, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Lang))]
        public string password { get; set; }
        [Display(Name = "RememberMe", ResourceType = typeof(Lang))]
        public bool rememberme { get; set; }
    }

    public class LoginUserInfo
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public List<int> RightIds { get; set; }

        public static LoginUserInfo GetLoginUser()
        {
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
            LoginUserInfo loginUser = GetLoginUser();
            if(loginUser == null)
            {
                return null;
            }
            return new UserInfo
            {
                UserId = loginUser.UserId,
                Login = loginUser.LoginName,
                Name = loginUser.Name
            };
        }
    }
}