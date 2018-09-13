namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0040_MBOM_VER")]
    public partial class AppMbomVer
    {
        [Key]
        public int CN_ID { get; set; }
        public Guid CN_GUID { get; set; }
        public Guid? CN_GUID_PBOM { get; set; }
        public string CN_STATUS { get; set; }
        public string CN_VER { get; set; }
        public string CN_NAME { get; set; }
        public string CN_CODE { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_DESC { get; set; }
        public short CN_IS_TOERP { get; set; }
        public DateTime CN_DT_TOERP { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public DateTime CN_DT_EFFECTIVE { get; set; }
        public DateTime CN_DT_EXPIRY { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
    }
}
