namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("View_Project_Product_Pbom")]
    public partial class ViewProjectProductPbom
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_PROJECT_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(16)]
        public string CN_CODE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(64)]
        public string CN_PROJECT_NAME { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_OWNER_ID { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(32)]
        public string CN_OWNER_NAME { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(24)]
        public string CN_PRODUCT_CODE { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(32)]
        public string CN_PRODUCT_NAME { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(6)]
        public string CN_PRODUCT_STATUS { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(10)]
        public string CN_CHECK_STATUS { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(10)]
        public string CN_SALE_SET { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_BOMID { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(4)]
        public string CN_VER { get; set; }

        [Key]
        [Column(Order = 12)]
        public DateTime CN_DT_VER { get; set; }

        [StringLength(32)]
        public string CN_CREATE_NAME { get; set; }
    }
}
