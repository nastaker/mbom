using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("TN_80_APP_0095_SITEOFSALE")]
    public class DictShippingAddr
    {
        [Key]
        public int CN_ID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_SYS_STATUS { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
    }
}
