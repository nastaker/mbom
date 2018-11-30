namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_1050_PRODUCTION_LINE_PROCESS_VER_HLINK")]
    public partial class AppProductionLineProcessVerHlink
    {
        [Key]
        public int CN_ID { get; set; }
        public Guid CN_GUID_VER { get; set; }
        public Guid CN_GUID_MBOM { get; set; }
        public Guid? CN_GUID_PROCESS { get; set; }
        public bool CN_IS_ASSEMBLED { get; set; }
        public bool CN_IS_FEEDED { get; set; }
        public bool CN_IS_EMPTY { get; set; }
        public DateTime CN_DT_CREATE { get; set; }
        [Column(TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }
        [Column(TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }
        public int CN_CREATE_BY { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }
        [Required]
        [StringLength(32)]
        public string CN_CREATE_LOGIN { get; set; }
    }
}
