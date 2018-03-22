namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TN_80_APP_0000_ITEM_SIGN
    {
        [Key]
        [Column(Order = 0)]
        public int CN_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(24)]
        public string CN_CODE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(18)]
        public string CN_ITEM_CODE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string CN_TYPE { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "numeric")]
        public decimal CN_WEIGHT { get; set; }

        [StringLength(24)]
        public string CN_DESC { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_CREATE_BY { get; set; }

        [StringLength(10)]
        public string CN_CREATE_NAME { get; set; }

        [StringLength(16)]
        public string CN_CREATE_LOGIN { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(2)]
        public string CN_SYS_STATUS { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime CN_DT_CREATE { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "date")]
        public DateTime CN_DT_EFFECTIVE { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "date")]
        public DateTime CN_DT_EXPIRY { get; set; }
    }
}
