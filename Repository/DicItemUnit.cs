namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_50_DIC_0010_ITEMUNIT")]
    public partial class DicItemUnit
    {
        [Key]
        public int CN_ID { get; set; }
        public string CN_NAME { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        public string CN_DESC { get; set; }
    }
}
