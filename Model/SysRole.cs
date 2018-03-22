using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Model
{
    [Table("TN_SYSROLE")]
    public class SysRole : BaseModel
    {
        [Column("CN_ROLENAME")]
        [Description("角色名称")]
        public string RoleName { get; set; }
        [Column("CN_DESCRIPTION")]
        [Description("角色描述")]
        public string Description { get; set; }

        public virtual List<SysRoleRight> RoleRights { get; set; }
    }
}
