namespace DAL.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0010_PRODUCT")]
    public partial class AppProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CN_ID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_CHECK_STATUS { get; set; }
        public string CN_SALE_SET { get; set; }
        public string CN_STATUS { get; set; }
        public string CN_TYPE { get; set; }
        public string CN_DESC { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
        public string CN_SYS_STATUS { get; set; }
        public string CN_SYS_NOTE { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public DateTime CN_DT_EFFECTIVE { get; set; }
        public DateTime CN_DT_EXPIRY { get; set; }
        public string CN_GUID { get; set; }
        public DateTime CN_DT_TOERP { get; set; }
    }
}
