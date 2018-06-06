namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("View_Project_Product_Pbom")]
    public class ViewProjectProductPbom : GroupEntity
    {
        public int PROJECT_ID { get; set; }
        public string CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        public int OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        [Key]
        [ColumnAttribute(Order = 1)]
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_STATUS { get; set; }
        public string CHECK_STATUS { get; set; }
        public string SALE_SET { get; set; }
        [Key]
        [ColumnAttribute(Order = 2)]
        public string VER { get; set; }
        public DateTime DT_VER { get; set; }
        public string CREATE_NAME { get; set; }
    }
}
