namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VIEW_MATERIALBILLBOARDS")]
    public partial class ViewMaterialBillboards
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(24)]
        public string CN_CODE { get; set; }

        [StringLength(18)]
        public string CN_ITEM_CODE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(24)]
        public string CN_NAME { get; set; }

        public short? CN_IS_TOERP { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime CN_DT_EFFECTIVE_ERP { get; set; }

        [StringLength(10)]
        public string 销售件 { get; set; }

        [StringLength(10)]
        public string 采购件 { get; set; }

        [StringLength(10)]
        public string 自制件 { get; set; }

        [StringLength(10)]
        public string 标准件 { get; set; }

        [StringLength(10)]
        public string 原材料 { get; set; }

        [StringLength(10)]
        public string 包装件 { get; set; }

        [StringLength(10)]
        public string 工艺件 { get; set; }

        [StringLength(10)]
        public string PBOM虚拟件 { get; set; }

        [StringLength(10)]
        public string PBOM合件 { get; set; }

        [StringLength(10)]
        public string DE选装件 { get; set; }

        [StringLength(10)]
        public string PBOM选装件 { get; set; }

        [StringLength(10)]
        public string MBOM虚拟件 { get; set; }

        [StringLength(10)]
        public string MBOM合件 { get; set; }

        [StringLength(10)]
        public string MBOM选装件 { get; set; }

        [StringLength(10)]
        public string 装配件 { get; set; }
    }
}
