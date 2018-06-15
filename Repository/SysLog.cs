namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_SYS_LOG")]
    public class SysLog
    {
        [Key]
        public int CN_ID { get; set; }
        public string CN_ACTIONURL { get; set; }
        public string CN_USERIP { get; set; }
        public int CN_USERID { get; set; }
        public string CN_USERNAME { get; set; }
        public string CN_USERLOGIN { get; set; }
        public bool CN_ISSUCCESS { get; set; }
        public string CN_DESC { get; set; }
        public string CN_REQUESTTYPE { get; set; }
        public DateTime CN_DT_DATE { get; set; }
    }
}
