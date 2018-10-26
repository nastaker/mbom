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
        public string PRODUCT_ITEM_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_STATUS { get; set; }
        public string CHECK_STATUS { get; set; }
        public string SALE_SET { get; set; }
        public string PBOMVER { get; set; }
        public DateTime? PBOMVER_DT { get; set; }
        public string PBOMVER_CREATE_NAME { get; set; }
        public string PRODVER_NAME { get; set; }
        public short? PRODVER_STATUS { get; set; }

        public DateTime? DT_PDM { get; set; }
        public string USER_SELL { get; set; }
        public DateTime? DT_SELL { get; set; }
        public string USER_PRE { get; set; }
        public DateTime? DT_PRE { get; set; }
        public string USER_MAINTAIN { get; set; }
        public DateTime? DT_MAINTAIN { get; set; }
        public string USER_MBOM { get; set; }
        public DateTime? DT_MBOM { get; set; }
        public DateTime? DT_DATA_INTE { get; set; }
        public DateTime? DT_DATA_MDM { get; set; }
        public DateTime? DT_DATA_ERP { get; set; }

    }
}
