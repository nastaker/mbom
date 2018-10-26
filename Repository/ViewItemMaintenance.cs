using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
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
        public string CN_PRODUCTLINECODE { get; set; }
        public short CN_IS_TOERP { get; set; }
        public bool CN_IS_PDM { get; set; }
        public DateTime CN_DT_TOERP { get; set; }
        public string CN_TYPEIDS { get; set; }
        public string CN_TYPENAMES { get; set; }
    }
}
