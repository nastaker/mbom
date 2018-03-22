using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Model
{
    [Table("TN_SYSACTION")]
    public class SysAction : BaseModel
    {
        [Column("CN_URL")]
        [Description("Action名称")]
        public string Url { get; set; }
        [Column("CN_DESCRIPTION")]
        [Description("操作描述")]
        public string Description { get; set; }
    }
}
