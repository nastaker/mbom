using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Model
{
    [Table("TN_USERPRODUCTLIBRARY")]
    public class UserProductLibrary : BaseModel
    {
        [Column("CN_PARENTID")]
        [Description("父级ID")]
        public int? ParentId { get; set; }
        [Column("CN_USERID")]
        [Description("用户ID")]
        public int UserId { get; set; }
        [Column("CN_NAME")]
        [Description("名称")]
        public string Name { get; set; }
        [Column("CN_DESC")]
        [Description("描述")]
        public string Desc { get; set; }
        [Column("CN_ORDER")]
        [Description("菜单排序")]
        public int Order { get; set; }

        public virtual UserProductLibrary Parent { get; set; }
        public virtual List<UserProductLibrary> Children { get; set; }
    }

    [Table("TN_USERPRODUCTLIBRARY_LINK")]
    public class UserProductLibraryLink : BaseModel
    {
        [Column("CN_LIB_ID")]
        [Description("父级ID")]
        public int LibraryId { get; set; }
        [Column("CN_PRODUCT_ID")]
        [Description("产品ID")]
        public int ProductId { get; set; }
        [Column("CN_PRODUCT_CODE")]
        [Description("产品编号")]
        public string ProductCode { get; set; }
        [Column("CN_PRODUCT_NAME")]
        [Description("产品名称")]
        public string ProductName { get; set; }
        [Column("CN_ITEM_ID")]
        [Description("物料ID")]
        public int ItemId { get; set; }
        [Column("CN_ITEM_CODE")]
        [Description("物料编码")]
        public string ItemCode { get; set; }
    }
}
