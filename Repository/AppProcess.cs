namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0020_PROCESS")]
    public partial class AppProcess
    {
        [Key]
        [Column(Order = 0)]
        public int CN_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(24)]
        public string CN_CODE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(18)]
        public string CN_ITEM_CODE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(24)]
        public string CN_NAME { get; set; }

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
