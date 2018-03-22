namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TN_80_APP_0000_ITEM_HLINK")]
    public partial class AppItemHLink
    {
        [Column(Order = 0)]
        public int CN_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CN_HLINK_ID { get; set; }

        public bool? CN_ISDELETE { get; set; }

        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_COMPONENT_CLASS_ID { get; set; }

        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_COMPONENT_OBJECT_ID { get; set; }

        public int? CN_COMPONENT_OBJECT_VER_ID { get; set; }

        [StringLength(8)]
        public string CN_COMPONENT_OBJECT_VERSION { get; set; }

        [StringLength(32)]
        public string CN_S_ATTACH_DATA { get; set; }

        public int? CN_ITEMSTATE_TAGGER_DATA { get; set; }

        public bool? CN_ISFOLDER { get; set; }

        public bool? CN_ISBORROW { get; set; }

        public int? CN_ORDER { get; set; }

        public int? CN_NUMBER { get; set; }

        [Column(Order = 4)]
        public double CN_F_QUANTITY { get; set; }

        [Column(Order = 5)]
        [StringLength(10)]
        public string CN_UNIT { get; set; }

        public bool? CN_B_IS_ASSEMBLY { get; set; }

        [Column(Order = 6)]
        [StringLength(80)]
        public string CN_DISPLAYNAME { get; set; }

        [StringLength(8)]
        public string CN_S_FROM { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }

        [Column(Order = 8)]
        [StringLength(2)]
        public string CN_SYS_STATUS { get; set; }

        [StringLength(64)]
        public string CN_SYS_NOTE { get; set; }

        [Column(Order = 9)]
        public DateTime CN_DT_CREATE { get; set; }

        [Column(Order = 10, TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Column(Order = 11, TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }

        [Column(Order = 12)]
        [StringLength(36)]
        public string CN_GUID { get; set; }
    }
}
