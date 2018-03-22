using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("VIEW_ITEM_MAINTENANCE")]
    public class ViewItemMaintenance
    {
        [Key]
        public int CN_ID { get; set; }
        public string CN_CODE { get; set; }
	    public string CN_ITEM_CODE { get; set; }
	    public string CN_NAME { get; set; }
        public string CN_UNIT { get; set; }
        public double CN_WEIGHT { get; set; }
        public string CN_GG { get; set; }
        public string CN_SYS_NOTE { get; set; }
	    public short? CN_IS_TOERP { get; set; }
        public DateTime? CN_DT_CREATE { get; set; }
        public string 自制件 { get; set; }
        public string 采购件 { get; set; }
        public string MBOM合件 { get; set; }
        public int? 自制件ID { get; set; }
        public int? 采购件ID { get; set; }
        public int? MBOM合件ID { get; set; }
    }
}
