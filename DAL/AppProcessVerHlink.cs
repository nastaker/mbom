namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0020_PROCESS_VER_HLINK")]
    public partial class AppProcessVerHlink
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int CN_HLINK_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string CN_GX_CODE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string CN_GX_NAME { get; set; }

        [StringLength(128)]
        public string CN_GXNR { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string CN_TYPE { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(2)]
        public string CN_SYS_STATUS { get; set; }

        [StringLength(64)]
        public string CN_SYS_NOTE { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime CN_DT_CREATE { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(36)]
        public string CN_GUID { get; set; }
    }
}
