using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Model
{
    [Table("TN_SYSRIGHTACTION")]
    public class SysRightAction : BaseModel
    {
        [Column("CN_MENUID")]
        [Description("角色ID")]
        public int MenuId { get; set; }
        [Column("CN_ACTIONID")]
        [Description("模块ID")]
        public int ActionId { get; set; }
        [Column("CN_DESCRIPTION")]
        [Description("描述")]
        public string Description { get; set; }

        [ForeignKey("MenuId")]
        public virtual SysMenu MenuInfo { get; set; }
        [ForeignKey("ActionId")]
        public virtual SysAction ActionInfo { get; set; }
    }
}
