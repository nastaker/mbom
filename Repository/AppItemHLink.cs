namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0000_ITEM_HLINK")]
    public partial class AppItemHLink
    {
        [Key]
        public int CN_HLINK_ID { get; set; }
        public int CN_ID { get; set; }
        public int CN_COMPONENT_CLASS_ID { get; set; }
        public int CN_COMPONENT_OBJECT_ID { get; set; }
        public bool? CN_ISBORROW { get; set; }
        public int? CN_ORDER { get; set; }
        public double CN_F_QUANTITY { get; set; }
        public string CN_UNIT { get; set; }
        public string CN_DISPLAYNAME { get; set; }
        public string CN_DESC { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
        public string CN_SYS_STATUS { get; set; }
        public string CN_SYS_NOTE { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public DateTime CN_DT_EFFECTIVE { get; set; }
        public DateTime CN_DT_EXPIRY { get; set; }
        public string CN_GUID { get; set; }
        public int? CN_IS_TOERP { get; set; }
        public DateTime CN_DT_TOERP { get; set; }
    }
}
