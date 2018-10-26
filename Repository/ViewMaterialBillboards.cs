namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VIEW_MATERIALBILLBOARDS")]
    public partial class ViewMaterialBillboards
    {
        public string Code { get; set; }
        [Key]
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public short? ErpStatus { get; set; }
        public DateTime ErpDate { get; set; }
        public bool Sell { get; set; }
        public bool Purchase { get; set; }
        public bool SelfMade { get; set; }
        public bool Standard { get; set; }
        public bool RawMaterial { get; set; }
        public bool Package { get; set; }
        public bool Process { get; set; }
        public bool Assembly { get; set; }
        public bool DEOptional { get; set; }
        public bool PBOMOptional { get; set; }
        public bool MBOMOptional { get; set; }
    }
}
