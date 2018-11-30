namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TN_80_APP_1050_PRODUCTION_LINE_PRODUCT_PROCESS")]
    public partial class AppProductionLineProductProcess
    {
        [Key]
        public int CN_ID { get; set; }
        public Guid CN_GUID { get; set; }
        public Guid CN_GUID_LINE_PRODUCT { get; set; }

        [Required]
        [StringLength(10)]
        public string CN_PROCESS_ORDER { get; set; }

        [Required]
        [StringLength(32)]
        public string CN_PROCESS_NAME { get; set; }

        [StringLength(256)]
        public string CN_PROCESS_INFO { get; set; }

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
