namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0025_BOM_HLINK")]
    public class AppBomHlink
    {
        [Key]
        public int CN_HLINK_ID { get; set; }
        public string CN_S_BOM_TYPE { get; set; }
        public int CN_BOM_ID { get; set; }
        public int CN_COMPONENT_CLASS_ID { get; set; }
        public int CN_COMPONENT_OBJECT_ID { get; set; }
        public int CN_PDM_CLASS_ID { get; set; }
        public int CN_PDM_OBJECT_ID { get; set; }
        public string CN_S_CODE { get; set; }
        public string CN_DISPLAYNAME { get; set; }
        public int? CN_ORDER { get; set; }
        public int? CN_NUMBER { get; set; }
        public double CN_F_QUANTITY { get; set; }
        public string CN_UNIT { get; set; }
        public bool? CN_B_IS_ASSEMBLY { get; set; }
        public bool? CN_ISBORROW { get; set; }
        public bool? CN_ISDELETE { get; set; }
        public string CN_CHA_PBOM_SIGN { get; set; }
        public string CN_CHA_PBOM_IMPACT_DO { get; set; }
        public string CN_STATUS_PBOM { get; set; }
        public string CN_STATUS_MBOM { get; set; }
        public string CN_S_FROM { get; set; }
        public string CN_DESC { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
        public string CN_SYS_STATUS { get; set; }
        public string CN_SYS_NOTE { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public DateTime CN_DT_EF_PBOM { get; set; }
        public DateTime CN_DT_EX_PBOM { get; set; }
        public DateTime CN_DT_EF_MBOM { get; set; }
        public DateTime CN_DT_EX_MBOM { get; set; }
        public DateTime CN_DT_EFFECTIVE { get; set; }
        public DateTime CN_DT_EXPIRY { get; set; }
        public string CN_MBOM_NOTICE { get; set; }
        public string CN_GUID { get; set; }
    }
}
