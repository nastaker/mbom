using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("[TN_OPTIONAL_ITEM_HLINK]")]
    public class AppOptionalItemHlink
    {
        [Key]
        public int CN_HLINK_ID { get; set; }
        public int CN_ID { get; set; }
        public int CN_OPTIONAL_ITEM_ID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_OPTIONAL_CODE { get; set; }
        public string CN_OPTIONAL_ITEM_CODE { get; set; }
        public string CN_OPTIONAL_NAME { get; set; }
        public short CN_IS_TOERP { get; set; }
        public string CN_SYS_STATUS { get; set; }
        public DateTime CN_DT_EXPIRY_ERP { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_DESC { get; set; }
    }
}
