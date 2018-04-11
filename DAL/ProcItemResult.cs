using System;

namespace DAL
{
    public class ProcItemTree
    {
        public int LEVEL { get; set; }
        public string PARENT_LINK { get; set; }
        public string LINK { get; set; }
        public string PARENTID { get; set; }
        public string ID { get; set; }
        public int CN_ITEMID { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_CODE { get; set; }
        public string CN_NAME { get; set; }
        public int CN_BOM_ID { get; set; }
        public int CN_BOM_ID_PRE { get; set; }
        public int CN_HLINK_ID { get; set; }
        public double QUANTITY { get; set; }
        public double QUANTITY_ALL { get; set; }
        public string CN_UNIT { get; set; }
        public bool? CN_ISBORROW { get; set; }
        public bool? IS_ASSEMBLY { get; set; }
        public int? CN_ORDER { get; set; }
        public string MBOMTYPE { get; set; }
        public string KL { get; set; }
        public bool? ISROOT { get; set; }
        public bool? ISLINKED { get; set; }
    }

    public class ProcItem
    {
        public int? CN_ITEMID { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_CODE { get; set; }
        public string CN_NAME { get; set; }
        public bool? CN_B_IS_ASSEMBLY { get; set; }
        public int? CN_ORDER { get; set; }
        public bool? CN_ISBORROW { get; set; }
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

    public class ProcCateItem
    {
        public int? CN_ITEMID { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_CODE { get; set; }
        public string CN_NAME { get; set; }
        public bool? CN_B_IS_ASSEMBLY { get; set; }
        public int? CN_ORDER { get; set; }
        public bool? CN_ISBORROW { get; set; }
        public string ISBORROWSTR { get; set; }
        public string PDATE { get; set; }
    }

    public class ProcBomDiff
    {
        public int CN_HLINK_ID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_ITEM_CODE { get; set; }
		public string CN_S_BOM_TYPE { get; set; }
		public int CN_BOM_ID { get; set; }
		public int CN_BOM_ID_PRE { get; set; }
		public string CN_DISPLAYNAME { get; set; }
		public string MBOMNAME { get; set; }
        public double? QUANTITY { get; set; }
		public int CN_ORDER { get; set; }
		public string CN_SYS_STATUS { get; set; }
		public DateTime CN_DT_CREATE { get; set; }
		public string CN_STATUS_PBOM { get; set; }
		public string CN_STATUS_MBOM { get; set; }
	    public DateTime CN_DT_EF_PBOM { get; set; }
		public DateTime CN_DT_EX_PBOM { get; set; }
		public DateTime CN_DT_EF_MBOM { get; set; }
		public DateTime CN_DT_EX_MBOM { get; set; }
    }

    public class ProcItemSetInfo
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
        public string SHIPPINGADDR { get; set; }
        public double? F_QUANTITY { get; set; }
        public string UNIT { get; set; }
        public int? SALESET { get; set; }
        public bool? ISBORROW { get; set; }
        public bool? B_IS_ASSEMBLY { get; set; }
        public int? ORDER { get; set; }
    }

    public class ProcProcessItem
    {
        public int CN_ITEMID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_NAME { get; set; }
    }

    public class ProcItemProcess
    {
        public int CN_HLINK_ID { get; set; }
        public string CN_GX_CODE { get; set; }
        public string CN_GX_NAME { get; set; }
        public string CN_GXNR { get; set; }
    }

    public class ProcProductChangeDetail
    {
        public string itemcode { get; set; }
        public string name { get; set; }
        public string effect { get; set; }
        public string displayname { get; set; }
        public int childid { get; set; }
        public double? quantity { get; set; }
        public string ver { get; set; }
        public DateTime dtver { get; set; }
        public string status { get; set; }
        public string isimplement { get; set; }
        public string notice { get; set; } 
    }

    public class ProcReturnMsg
    {
        public string msg { get; set; }
        public bool success { get; set; }
    }
}
