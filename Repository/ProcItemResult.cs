using System;

namespace Repository
{
    public class ProcBomVer
    {
        public string Ver { get; set; }
        public string DateTimeCreate { get; set; }
        public string Desc { get; set; }
    }

    public class ProcItemTree
    {
        public int Level { get; set; }
        public string ParentId { get; set; }
        public string ItemCodeParent { get; set; }
        public string Id { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ItemCode { get; set; }
        public Guid? Guid { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public bool? IsBorrow { get; set; }
        public string DtPbomEf { get; set; }
        public string DtPbomEx { get; set; }
        public string DtMbomEf { get; set; }
        public string DtMbomEx { get; set; }
        public int? PbomVer { get; set; }
        public string PbomVerCode { get; set; }
        public int IsMbom { get; set; }
        public int IsToErp { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Desc { get; set; }
        public int Order { get; set; }
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
        public int? ORDER { get; set; }
        public bool? ISBORROW { get; set; }
        public string ISBORROWSTR { get; set; }
        public string PDATE { get; set; }
        public string CUS_CODE { get; set; }
        public string CUS_NAME { get; set; }
        public string CUS_ITEMCODE { get; set; }
        public string CUS_ITEMNAME { get; set; }
        public string CUS_SHIPPINGADDR { get; set; }
    }

    public class ProcBomDiff
    {
        public string Code { get; set; }
        public string ItemCode { get; set; }
        public float? Quantity { get; set; }
        public int Order { get; set; }
        public string Status { get; set; }
        public DateTime DtCreate { get; set; }
        public DateTime DtEfPbom { get; set; }
        public DateTime DtExPbom { get; set; }
        public DateTime DtEfMbom { get; set; }
        public DateTime DtExMbom { get; set; }
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
        public string ITEMCODE { get; set; }
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
