using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Model
{
    public abstract class BaseModel
    {
        [Key]
        [Column("CN_ID")]
        [Description("主键ID")]
        public int ID { get; set; }
        [Description("创建人ID")]
        [Column("CN_Create_By")]
        public int? CreateBy { get; set; }
        [Column("CN_Create_Name")]
        [StringLength(200)]
        [Description("创建人名字")]
        public string CreateName { get; set; }
        [Column("CN_Create_Login")]
        [StringLength(200)]
        [Description("创建人登录名")]
        public string CreateLogin { get; set; }
        [Column("CN_DT_Create")]
        [Description("创建时间")]
        public DateTime CreateTime
        {
            get { return _createTime ?? DateTime.Now; }
            set { this._createTime = value; }
        }

        private DateTime? _createTime;
    }
}
