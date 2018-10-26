namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppPbomHlink
    {
        [Key]
        public int CN_HLINK_ID { get; set; }

        public int CN_BOM_ID { get; set; }

        public Guid CN_GUID { get; set; }

        [StringLength(24)]
        public string CN_ITEMCODE_PARENT { get; set; }

        [Required]
        [StringLength(24)]
        public string CN_ITEMCODE { get; set; }

        public int CN_ORDER { get; set; }

        public double CN_F_QUANTITY { get; set; }

        [Required]
        [StringLength(80)]
        public string CN_DISPLAYNAME { get; set; }

        [StringLength(10)]
        public string CN_UNIT { get; set; }

        public bool? CN_ISBORROW { get; set; }

        [Required]
        [StringLength(1)]
        public string CN_CHA_PBOM_IMPACT_DO { get; set; }

        [Required]
        [StringLength(1)]
        public string CN_STATUS { get; set; }

        [StringLength(128)]
        public string CN_DESC { get; set; }

        [StringLength(64)]
        public string CN_SYS_NOTE { get; set; }

        public DateTime CN_DT_CREATE { get; set; }

        public short CN_IS_TOERP { get; set; }

        public DateTime CN_DT_TOERP { get; set; }

        [Required]
        [StringLength(10)]
        public string CN_S_BOM_TYPE { get; set; }

        [Required]
        [StringLength(1)]
        public string CN_STATUS_PBOM { get; set; }

        [Required]
        [StringLength(1)]
        public string CN_STATUS_MBOM { get; set; }

        public int CN_ID_NOTICE { get; set; }

        [Required]
        [StringLength(32)]
        public string CN_CODE_NOTICE { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EF_PBOM { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EX_PBOM { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EF_MBOM { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EX_MBOM { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Column(TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }

        public int CN_PDM_CLASS_ID { get; set; }

        public int CN_PDM_OBJECT_ID { get; set; }

        public int CN_PBOM_HLINKID { get; set; }

        public DateTime CN_DT_CREATE_PBOM { get; set; }

        public DateTime CN_DT_TOERP_PBOM { get; set; }

        public DateTime CN_DT_MODIFY { get; set; }

        public int CN_BOM_ID_PRE { get; set; }

        public int CN_HLINK_ID_PRE { get; set; }

        public int CN_COMPONENT_CLASS_ID { get; set; }

        public int CN_COMPONENT_OBJECT_ID { get; set; }

        [Required]
        [StringLength(1)]
        public string CN_CHA_PBOM_SIGN { get; set; }

        [StringLength(10)]
        public string CN_S_FROM { get; set; }

        public int CN_CREATE_BY { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }

        [Required]
        [StringLength(2)]
        public string CN_SYS_STATUS { get; set; }

        public int CN_PBOM_VERID { get; set; }

        public int CN_PBOM_VERID_EX { get; set; }
    }
}
