using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Model
{
    [Table("TN_SYSMENU")]
    public class SysMenu : BaseModel
    {
        [Column("CN_PARENTID")]
        [Description("父级ID")]
        public int? ParentId { get; set; }
        [Column("CN_MENUNAME")]
        [Description("菜单名称")]
        public string MenuName { get; set; }
        [Column("CN_URL")]
        [Description("菜单链接")]
        public string Url { get; set; }
        [Column("CN_ICONCLS")]
        [Description("菜单图标")]
        public string IconCls { get; set; }
        [Column("CN_DESCRIPTION")]
        [Description("菜单描述")]
        public string Description { get; set; }
        [Column("CN_ORDER")]
        [Description("菜单排序")]
        public int Order { get; set; }

        public virtual SysMenu Parent { get; set; }
        public virtual List<SysMenu> Children { get; set; }
    }
}
