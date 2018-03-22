using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Model
{
    [Table("TN_SYSROLERIGHT")]
    public class SysRoleRight : BaseModel
    {
        [Column("CN_ROLEID")]
        [Description("角色ID")]
        public int RoleId { get; set; }
        [Column("CN_RIGHTID")]
        [Description("权限ID")]
        public int RightId { get; set; }

        [ForeignKey("RoleId")]
        public virtual SysRole RoleInfo { get; set; }
        [ForeignKey("RightId")]
        public virtual SysMenu RightInfo { get; set; }
    }
}
