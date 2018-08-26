namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TN_80_APP_0040_MBOM_HLINK")]
    public partial class AppMbomHlink
    {
        [Key]
        [Column(Order = 0)]
        public int CN_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CN_GUID { get; set; }

        public Guid? CN_GUID_LINK { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string CN_CODE_PARENT { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(30)]
        public string CN_CODE { get; set; }

        [Key]
        [Column(Order = 4)]
        public double CN_QUANTITY { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(80)]
        public string CN_DISPLAYNAME { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_ORDER { get; set; }

        [StringLength(10)]
        public string CN_UNIT { get; set; }

        public bool? CN_ISASSEMBLY { get; set; }

        public bool? CN_ISBORROW { get; set; }

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
        public Guid CN_GUID_EF { get; set; }

        public Guid? CN_GUID_EX { get; set; }

        [Key]
        [Column(Order = 11)]
        public DateTime CN_DT_TOERP { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(10)]
        public string CN_TYPE { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CN_IS_TOERP { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }
    }
}
