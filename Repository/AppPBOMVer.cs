namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0030_PBOM_VER")]
    public partial class AppPbomVer
    {
        [Key]
        public int CN_ID { get; set; }
        public Guid CN_GUID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_NAME { get; set; }
        public int CN_PDM_VERID { get; set; }
        public string CN_VER { get; set; }
        public DateTime CN_DT_VER { get; set; }
        public string CN_SYS_STATUS { get; set; }
        public string CN_DESC { get; set; }
        public string CN_SYS_NOTE { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public DateTime CN_DT_EFFECTIVE { get; set; }
        public DateTime CN_DT_EXPIRY { get; set; }
        public DateTime CN_DT_MODIFY { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
    }
}
