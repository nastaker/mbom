using Model;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace BLL
{
    public class SysRoleRightBLL : BaseBLL<SysRoleRight>
    {
        [Dependency]
        public SysMenuBLL rightbll { get; set; }

        public void EditRoleRight(int roleid, int[] menuids)
        {
            Delete(r => r.RoleId == roleid);
            if (menuids == null)
            {
                SaveChanges();
                return;
            }
            List<SysRoleRight> list = new List<SysRoleRight>();
            for (int i = 0; i < menuids.Length; i++)
            {
                list.Add(new SysRoleRight
                {
                    RoleId = roleid,
                    RightId = menuids[i]
                });
            }
            AddRange(list);
            SaveChanges();
        }
    }
}
