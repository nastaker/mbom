using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class ProcItemTreeView
    {
        public int LEVEL { get; set; }
        public string PARENT_LINK { get; set; }
        public string LINK { get; set; }
        public string PARENTID { get; set; }
        public string ID { get; set; }
        public int ITEMID { get; set; }
        public string ITEM_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public int BOM_ID { get; set; }
        public int BOM_ID_PRE { get; set; }
        public int HLINK_ID { get; set; }
        public double QUANTITY { get; set; }
        public double QUANTITY_ALL { get; set; }
        public string UNIT { get; set; }
        public bool? ISBORROW { get; set; }
        public bool? IS_ASSEMBLY { get; set; }
        public int? ORDER { get; set; }
        public bool? ISROOT { get; set; }
        public bool? ISLINKED { get; set; }
        public string MBOMTYPE { get; set; }
        public string KL { get; set; }
    }
    public class ProcItemView
    {
        public int? ITEMID { get; set; }
        public string ITEM_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public bool? B_IS_ASSEMBLY { get; set; }
        public int? ORDER { get; set; }
        public bool? ISBORROW { get; set; }
        public string 部件 { get; set; }
        public string 借用 { get; set; }
        public string 销售件 { get; set; }
        public string 采购件 { get; set; }
        public string 自制件 { get; set; }
        public string 标准件 { get; set; }
        public string 原材料 { get; set; }
        public string 包装件 { get; set; }
        public string 工艺件 { get; set; }
    }
    public partial class ProcCateItemView
    {
        public int? ITEMID { get; set; }
        public string ITEM_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public bool? B_IS_ASSEMBLY { get; set; }
        public int? ORDER { get; set; }
        public bool? ISBORROW { get; set; }
        public string ISBORROWSTR { get; set; }
        public string PDATE { get; set; }
    }

    public class ProcItemSetInfoView
    {
        public int? LEVEL { get; set; }
        public int? PARENT_ID { get; set; }
        public int? ITEM_HLINK_ID { get; set; }
        public int? ITEMID { get; set; }
        public string PARENT_CODE { get; set; }
        public string PARENT_NAME { get; set; }
        public string ITEM_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public double? F_QUANTITY { get; set; }
        public string UNIT { get; set; }
        public int? SALESET { get; set; }
        public bool? ISBORROW { get; set; }
        public bool? B_IS_ASSEMBLY { get; set; }
        public int? ORDER { get; set; }
    }

    public class ProcProcessItemView
    {
        public int ITEMID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
    }

    public partial class ProcItemProcessView
    {
        public int HLINK_ID { get; set; }
        public string GX_CODE { get; set; }
        public string GX_NAME { get; set; }
        public string GXNR { get; set; }
    }
}