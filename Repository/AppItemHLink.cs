namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0000_ITEM_HLINK")]
    public partial class AppItemHLink
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_COMPONENT_CLASS_ID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_COMPONENT_OBJECT_ID { get; set; }

        public bool? CN_ISBORROW { get; set; }

        public int? CN_ORDER { get; set; }

        [Key]
        [Column(Order = 4)]
        public double CN_F_QUANTITY { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string CN_UNIT { get; set; }

        public bool? CN_B_IS_ASSEMBLY { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(80)]
        public string CN_DISPLAYNAME { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(2)]
        public string CN_SYS_STATUS { get; set; }

        [StringLength(64)]
        public string CN_SYS_NOTE { get; set; }

        [Key]
        [Column(Order = 9)]
        public DateTime CN_DT_CREATE { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Key]
        [Column(Order = 11, TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }

        [StringLength(36)]
        public string CN_GUID { get; set; }

        public short? CN_IS_TOERP { get; set; }

        [Key]
        [Column(Order = 12)]
        public DateTime CN_DT_TOERP { get; set; }

        public int? CN_PBOM_VERID { get; set; }

        public int? CN_PBOM_LINKID { get; set; }
    }
}
