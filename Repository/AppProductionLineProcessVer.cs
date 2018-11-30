namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TN_80_APP_1050_PRODUCTION_LINE_PROCESS_VER")]
    public partial class AppProductionLineProcessVer
    {
        [Key]
        public int CN_ID { get; set; }
        public Guid CN_GUID { get; set; }
        public Guid CN_GUID_LINE_PRODUCT { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_PRODUCT_ITEMCODE { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_NAME { get; set; }
        [Required]
        [StringLength(256)]
        public string CN_DESC { get; set; }
        public short CN_IS_TOERP { get; set; }
        public DateTime CN_DT_TOERP { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        [Column(TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }
        [Column(TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }
        public int CN_CREATE_BY { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }
    }
}
