namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TN_80_APP_0005_PROJECT")]
    public partial class AppProject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CN_ID { get; set; }

        [Key]
        [StringLength(16)]
        public string CN_CODE { get; set; }

        [Required]
        [StringLength(64)]
        public string CN_NAME { get; set; }

        public int CN_OWNER_ID { get; set; }

        [Required]
        [StringLength(32)]
        public string CN_OWNER_NAME { get; set; }

        [Required]
        [StringLength(32)]
        public string CN_OWNER_LOGIN { get; set; }

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
