namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VIEW_MBOM_MAINTENANCE")]
    public partial class ViewMbomMaintenance
    {
        public int PROJECT_ID { get; set; }
        [Key]
        [Column(Order = 0)]
        public string CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        [Key]
        [Column(Order = 1)]
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_STATUS { get; set; }
        public string TECH_STATUS { get; set; }
        public string CHECK_STATUS { get; set; }
        public string SALE_SET { get; set; }
        public string PBOMVER { get; set; }
        public DateTime DT_PBOMVER { get; set; }
        public string PBOM_CREATE_NAME { get; set; }
        public int OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
    }
}
