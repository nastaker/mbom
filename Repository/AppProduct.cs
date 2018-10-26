namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0010_PRODUCT")]
    public partial class AppProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CN_ID { get; set; }

        [Key]
        [StringLength(24)]
        public string CN_CODE { get; set; }

        [Required]
        [StringLength(18)]
        public string CN_ITEM_CODE { get; set; }

        [Required]
        [StringLength(32)]
        public string CN_NAME { get; set; }

        [Required]
        [StringLength(10)]
        public string CN_CHECK_STATUS { get; set; }

        [Required]
        [StringLength(10)]
        public string CN_SALE_SET { get; set; }

        [Required]
        [StringLength(2)]
        public string CN_CHA_IMPACT { get; set; }

        [Required]
        [StringLength(2)]
        public string CN_CHA_IMPACT_DO { get; set; }

        [Required]
        [StringLength(16)]
        public string CN_STATUS { get; set; }

        [Required]
        [StringLength(10)]
        public string CN_TYPE { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }

        [Required]
        [StringLength(2)]
        public string CN_SYS_STATUS { get; set; }

        [StringLength(64)]
        public string CN_SYS_NOTE { get; set; }

        public DateTime CN_DT_CREATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }

        [StringLength(36)]
        public string CN_GUID { get; set; }

        [Required]
        [StringLength(10)]
        public string CN_DOMAIN_CODE { get; set; }

        public short CN_IS_TOERP { get; set; }

        public DateTime CN_DT_TOERP { get; set; }

        public int CN_PDMID { get; set; }

        public int CN_PRJID { get; set; }

        [Required]
        [StringLength(1)]
        public string CN_Release_Pre { get; set; }

        public DateTime? CN_DT_PDM { get; set; }

        [StringLength(10)]
        public string CN_USER_SELL { get; set; }

        public DateTime? CN_DT_SELL { get; set; }

        [StringLength(10)]
        public string CN_USER_PRE { get; set; }

        public DateTime? CN_DT_PRE { get; set; }

        [StringLength(10)]
        public string CN_USER_MAINTAIN { get; set; }

        public DateTime? CN_DT_MAINTAIN { get; set; }

        [StringLength(10)]
        public string CN_USER_MBOM { get; set; }

        public DateTime? CN_DT_MBOM { get; set; }

        public DateTime? CN_DT_DATA_INTE { get; set; }

        public DateTime? CN_DT_DATA_MDM { get; set; }

        public DateTime? CN_DT_DATA_ERP { get; set; }

        public bool CN_MARK { get; set; }
    }
}
