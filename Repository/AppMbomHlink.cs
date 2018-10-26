namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0040_MBOM_HLINK")]
    public partial class AppMbomHlink
    {
        [Key]
        public int CN_ID { get; set; }

        public Guid CN_GUID { get; set; }

        public Guid? CN_GUID_PBOM { get; set; }

        public Guid? CN_GUID_LINK { get; set; }

        public Guid? CN_GUID_EF { get; set; }

        public Guid? CN_GUID_EX { get; set; }

        [Required]
        [StringLength(24)]
        public string CN_ITEMCODE_PARENT { get; set; }

        [Required]
        [StringLength(24)]
        public string CN_ITEMCODE { get; set; }

        public double CN_QUANTITY { get; set; }

        [Required]
        [StringLength(80)]
        public string CN_DISPLAYNAME { get; set; }

        public int CN_ORDER { get; set; }

        [StringLength(10)]
        public string CN_UNIT { get; set; }

        public bool? CN_ISBORROW { get; set; }

        public bool CN_ISMBOM { get; set; }

        public DateTime CN_DT_CREATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE_PBOM { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EXPIRY_PBOM { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }

        public DateTime CN_DT_TOERP { get; set; }

        public short CN_IS_TOERP { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }
    }
}
