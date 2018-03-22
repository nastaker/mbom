namespace DAL.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VIEW_MBOM_MAINTENANCE")]
    public partial class ViewMbomMaintenance
    {
        [Column]
        public int CN_PROJECT_ID { get; set; }

        [Column]
        [StringLength(16)]
        public string CN_CODE { get; set; }

        [Column]
        [StringLength(64)]
        public string CN_PROJECT_NAME { get; set; }

        [Key]
        [Column]
        [StringLength(24)]
        public string CN_PRODUCT_CODE { get; set; }

        [Column]
        [StringLength(32)]
        public string CN_PRODUCT_NAME { get; set; }

        [Column]
        [StringLength(6)]
        public string CN_PRODUCT_STATUS { get; set; }

        [Column]
        [StringLength(6)]
        public string CN_TECH_STATUS { get; set; }

        [Column]
        [StringLength(10)]
        public string CN_CHECK_STATUS { get; set; }

        [Column]
        [StringLength(10)]
        public string CN_SALE_SET { get; set; }

        [Column]
        public int CN_BOMID { get; set; }

        [Column]
        [StringLength(4)]
        public string CN_PBOMVER { get; set; }

        [Column]
        public DateTime CN_DT_PBOMVER { get; set; }

        [StringLength(32)]
        public string CN_PBOM_CREATE_NAME { get; set; }

        [Column]
        public int CN_OWNER_ID { get; set; }

        [Column]
        [StringLength(32)]
        public string CN_OWNER_NAME { get; set; }
    }
}
