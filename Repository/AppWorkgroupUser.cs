using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("TN_WORKGROUP_USER")]
    public class AppWorkgroupUser
    {
        [Key]
        public int CN_ID { get; set; }
        public int CN_GROUPID { get; set; }
        public int CN_USERID { get; set; }
        public string CN_USERNAME { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
    }
}
