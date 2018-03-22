using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Model
{
    [Table("TN_SYSATTACHMENT")]
    public class SysAttachment : BaseModel
    {
        [Column("CN_ITEMID")]
        [Description("ITEMID")]
        public int ItemId { get; set; }
        [Column("CN_CODE")]
        [Description("CODE")]
        public string Code { get; set; }
        [Column("CN_ITEM_CODE")]
        [Description("ITEMCODE")]
        public string ItemCode { get; set; }
        [Column("CN_FILEPATH")]
        [Description("文件路径")]
        public string FilePath { get; set; }
        [Column("CN_FILENAME")]
        [Description("文件名")]
        public string FileName { get; set; }
        [Column("CN_FILESIZE")]
        [Description("文件大小")]
        public int FileSize { get; set; }
        [Column("CN_FILETYPE")]
        [Description("文件类型")]
        public int FileType { get; set; }
    }
}
