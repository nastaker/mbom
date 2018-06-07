namespace MBOM.Models
{
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
}