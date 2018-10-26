namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_50_DIC_0010_STATUS")]
    public partial class DictStatus
    {
        [Key]
        public int CN_ID { get; set; }
        public Guid CN_GUID { get; set; }
        public string CN_TYPE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_DESC { get; set; }
        public int CN_ORDER { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public DateTime CN_DT_EFFECTIVE { get; set; }
        public DateTime CN_DT_EXPIRY { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
    }
}
