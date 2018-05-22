namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SysClass
    {
        [Key]
        [Column(Order = 0)]
        public int CN_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_CLASS_BASE_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string CN_SYS_TYPE_CODE { get; set; }

        public int? CN_PARENT_ID { get; set; }

        [StringLength(16)]
        public string CN_PARENT_CODE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(16)]
        public string CN_CLASS_CODE { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(32)]
        public string CN_CLASS_NAME { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(36)]
        public string CN_TABLET_NAME { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string CN_LINK_SIGN { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(16)]
        public string CN_LINK_CODE { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(32)]
        public string CN_LINK_NAME { get; set; }

        [Key]
        [Column(Order = 9)]
        public bool CN_REVISIONCONTROL { get; set; }

        [Key]
        [Column(Order = 10)]
        public bool CN_FILECONTROL { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_TEMPLATE_ID { get; set; }

        public bool? CN_ISPARTTREE { get; set; }

        public bool? CN_ISDOCUMENT { get; set; }

        [Key]
        [Column(Order = 12)]
        public bool CN_ISMASTER_CLASS { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_LEAFICON_ID { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_MAINICON_ID { get; set; }

        [StringLength(16)]
        public string CN_GROUPNAME { get; set; }

        public int? CN_MAINGIF_BLOBID { get; set; }

        public int? CN_LEAFGIF_BLOBID { get; set; }

        public int? CN_REVBMP_BLOBID { get; set; }

        public int? CN_REVGIF_BLOBID { get; set; }

        public bool? CN_B_SECRET { get; set; }

        public bool? CN_B_COOPERATE { get; set; }

        public bool? CN_B_PRJROOT { get; set; }

        public bool? CN_B_HAVE_DISPLAY_FILE { get; set; }

        public bool? CN_B_COMPLEX_CLASS { get; set; }

        public bool? CN_B_HAVE_TALK { get; set; }

        public bool? CN_B_HAVE_DATAIMPLEMENT_DEF { get; set; }

        public bool? CN_B_PRODUCT { get; set; }

        public short? CN_N_DATA_ACCESS_LVL { get; set; }

        public short? CN_N_BROWSE_RESTRICT { get; set; }

        public short? CN_N_QUERY_RESTRICT { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(2)]
        public string CN_SYS_STATUS { get; set; }

        [StringLength(64)]
        public string CN_SYS_NOTE { get; set; }

        [Key]
        [Column(Order = 17)]
        public DateTime CN_DT_CREATE { get; set; }

        [Key]
        [Column(Order = 18, TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Key]
        [Column(Order = 19, TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(36)]
        public string CN_GUID { get; set; }
    }
}
