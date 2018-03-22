using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Model
{
    [Table("TN_SYSUSERROLE")]
    public class SysUserRole : BaseModel
    {
        [Column("CN_USERID")]
        [Description("用户ID")]
        public int UserId { get; set; }
        [Column("CN_NAME")]
        [Description("用户名")]
        public string Name { get; set; }
        [Column("CN_ROLEID")]
        [Description("角色ID")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual SysRole RoleInfo { get; set; }
    }
}
