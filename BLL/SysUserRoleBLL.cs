using Model;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class SysUserRoleBLL : BaseBLL<SysUserRole>
    {
        public void EditUserRole(SysUserRole users, int[] roleIds)
        {
            Delete(r => r.UserId == users.UserId);
            if (roleIds == null)
            {
                SaveChanges();
                return;
            }
            List<SysUserRole> list = new List<SysUserRole>();
            for (int i = 0; i < roleIds.Length; i++)
            {
                list.Add(new SysUserRole
                {
                    UserId = users.UserId,
                    Name = users.Name,
                    RoleId = roleIds[i]
                });
            }
            AddRange(list);
            SaveChanges();
        }

        public void EditUserRole(SysUserRole[] users, int[] roleIds)
        {
            var userids = users.Select(us => us.UserId);
            Delete(r => userids.Contains(r.UserId));
            if (roleIds == null)
            {
                SaveChanges();
                return;
            }
            List<SysUserRole> list = new List<SysUserRole>();
            for(int i = 0; i < users.Length; i++)
            {
                for (int j = 0; j < roleIds.Length; j++)
                {
                    list.Add(new SysUserRole
                    {
                        UserId = users[i].UserId,
                        Name = users[i].Name,
                        RoleId = roleIds[j]
                    });
                }
            }
            AddRange(list);
            SaveChanges();
        }
    }
}
