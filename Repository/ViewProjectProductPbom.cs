namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("View_Project_Product_Pbom")]
    public partial class ViewProjectProductPbom : GroupEntity
    {
        public int PROJECT_ID { get; set; }
        [Key]
        [Column(Order = 0)]
        public string CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        public int OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        [Key]
        [Column(Order = 1)]
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_STATUS { get; set; }
        public string CHECK_STATUS { get; set; }
        public string SALE_SET { get; set; }
        public string VER { get; set; }
        public DateTime DT_VER { get; set; }
        public string CREATE_NAME { get; set; }
        public DateTime? PDM_PUB_DATE { get; set; }
        public string PDM_PUB_NAME { get; set; }
        public DateTime? MBOM_PUBED_DATE { get; set; }
        public string MBOM_PUBED_NAME { get; set; }
        public DateTime? MBOM_PUBING_DATE { get; set; }
        public string MBOM_PUBING_NAME { get; set; }
        public DateTime? MBOM_TRANED_DATE { get; set; }
        public string MBOM_TRANED_NAME { get; set; }
        public DateTime? TRANSFERING_DATE { get; set; }
        public string TRANSFERING_NAME { get; set; }
        public DateTime? SALESET_DATE { get; set; }
        public string SALESET_NAME { get; set; }

    }
}
