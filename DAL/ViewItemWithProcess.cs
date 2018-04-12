namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VIEW_MBOM_ITEMWITHPROCESS")]
    public partial class ViewItemWithProcess
    {
        [Key]
        [Column]
        [StringLength(24)]
        public string CN_CODE { get; set; }

        [StringLength(18)]
        public string CN_ITEM_CODE { get; set; }

        [Column]
        [StringLength(24)]
        public string CN_NAME { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PDM_RELEASE_DATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TOERP_DATE { get; set; }
    }
}
