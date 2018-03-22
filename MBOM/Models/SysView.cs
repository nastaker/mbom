using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class SysMenuView
    {
        public int ID { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string IconCls { get; set; }
        public string Description { get; set; }
        public virtual List<SysMenuView> Children { get; set; }
        public virtual List<SysRightActionView> RightActions { get; set; }
    }
    public class SysActionView
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
    public class SysRightActionView
    {
        public int ID { get; set; }
        public int MenuId { get; set; }
        public int ActionId { get; set; }
        public string Description { get; set; }

        public virtual SysMenuView MenuInfo { get; set; }
        public virtual SysActionView ActionInfo { get; set; }
    }
    public class SysRoleView
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public virtual List<SysRoleRightView> RoleRights { get; set; }
    }
    public class SysRoleRightView
    {
        public int RoleId { get; set; }
        public int RightId { get; set; }
        public virtual SysMenuView RightInfo { get; set; }
    }
    public class SysUserRoleView
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public virtual SysRoleView RoleInfo { get; set; }
    }
    public class UserProductLibraryView
    {
        public int id { get; set; }
        public int? parentid { get; set; }
        public string name { get; set; }
        public virtual List<UserProductLibraryView> children { get; set; }
    }
}