namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppPbom
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
        [StringLength(24)]
        public string CN_NAME { get; set; }

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

        [Required]
        [StringLength(36)]
        public string CN_GUID { get; set; }
    }
}
