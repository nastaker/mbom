namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VIEW_MBOM_MAINTENANCE")]
    public partial class ViewMbomMaintenance
    {
        public int CN_PROJECT_ID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_PROJECT_NAME { get; set; }
        [Key]
        [ColumnAttribute(Order = 1)]
        public string CN_PRODUCT_CODE { get; set; }
        public string CN_PRODUCT_NAME { get; set; }
        public string CN_PRODUCT_STATUS { get; set; }
        public string CN_TECH_STATUS { get; set; }
        public string CN_CHECK_STATUS { get; set; }
        public string CN_SALE_SET { get; set; }
        [Key]
        [ColumnAttribute(Order = 2)]
        public string CN_PBOMVER { get; set; }
        public DateTime CN_DT_PBOMVER { get; set; }
        public string CN_PBOM_CREATE_NAME { get; set; }
        public int CN_OWNER_ID { get; set; }
        public string CN_OWNER_NAME { get; set; }
    }
}
