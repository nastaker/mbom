namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0010_PRODUCT_VER")]
    public partial class AppProductVer
    {
        [Key]
        public int CN_ID { get; set; }
        public Guid CN_GUID { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_PRODUCT_CODE { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_PRODUCT_ITEMCODE { get; set; }
        [Required]
        [StringLength(128)]
        public string CN_PRODUCT_NAME { get; set; }
        [StringLength(32)]
        public string CN_NAME { get; set; }
        [StringLength(255)]
        public string CN_DESC { get; set; }
        [Required]
        [StringLength(1)]
        public string CN_STATUS { get; set; }
        public short CN_IS_TOERP { get; set; }
        public DateTime? CN_DT_TOERP { get; set; }
        public DateTime? CN_DT_CREATE { get; set; }
        public int CN_CREATE_BY { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }
    }
}
