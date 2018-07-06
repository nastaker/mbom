using System;

namespace Repository
{
    public class ProcItemTree
    {
        public int HLINK_ID { get; set; }
        public int BOM_ID { get; set; }
        public int LEVEL { get; set; }
        public string PARENT_LINK { get; set; }
        public string LINK { get; set; }
        public string PARENTID { get; set; }
        public string ID { get; set; }
        public int ITEMID { get; set; }
        public string ITEM_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public double QUANTITY { get; set; }
        public string UNIT { get; set; }
        public int ORDER { get; set; }
        public string MBOMTYPE { get; set; }
        public string CHA_SIGN { get; set; }
        public string CHA_DO { get; set; }
        public bool? ISBORROW { get; set; }
        public bool? IS_ASSEMBLY { get; set; }
        public bool? ISROOT { get; set; }
        public bool? ISLINKED { get; set; }
    }

    public class ProcItem
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

    public class ProcCateItem
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

    public class ProcBomDiff
    {
        public string code { get; set; }
        public string item_code { get; set; }
        public int hlink_id { get; set; }
        public string s_bom_type { get; set; }
        public int bom_id { get; set; }
        public int bom_id_pre { get; set; }
        public string displayname { get; set; }
        public string mbomname { get; set; }
        public double? quantity { get; set; }
        public int order { get; set; }
        public string sys_status { get; set; }
        public DateTime dt_create { get; set; }
        public string status_pbom { get; set; }
        public string status_mbom { get; set; }
        public DateTime dt_ef_pbom { get; set; }
        public DateTime dt_ex_pbom { get; set; }
        public DateTime dt_ef_mbom { get; set; }
        public DateTime dt_ex_mbom { get; set; }
    }

    public class ProcItemSetInfo
    {
        public int ITEMID { get; set; }
        public string ITEM_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string UNIT { get; set; }
        public string SHIPPINGADDR { get; set; }
        public int? CUSTOMER_ID { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string CUSTOMERITEMCODE { get; set; }
        public string CUSTOMERITEMNAME { get; set; }
        public int ORDER { get; set; }
        public string TYPE { get; set; }
        public bool? B_IS_ASSEMBLY { get; set; }
        public int? SALESET { get; set; }
        public double? F_QUANTITY { get; set; }
    }

    public class ProcProcessItem
    {
        public int ITEMID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
    }

    public class ProcItemProcess
    {
        public int HLINK_ID { get; set; }
        public string GX_CODE { get; set; }
        public string GX_NAME { get; set; }
        public string GXNR { get; set; }
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
