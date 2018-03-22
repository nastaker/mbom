using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("VIEW_PBOM_CHANGE_PRODUCT")]
    public class ViewPbomChangeProduct
    {
        [Key]
        public int CN_PRODUCT_ID { get; set; }
        public string CN_PRODUCT_CODE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_VER { get; set; }
        public DateTime CN_DT_VER { get; set; }
        public DateTime CN_DT_TOERP { get; set; }
        public string CN_CREATE_NAME { get; set; }
    }

    [Table("VIEW_PBOM_CHANGE_ITEM")]
    public class ViewPbomChangeItem
    {
        [Key]
        public string CN_ITEM_CODE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_PRODUCT_BASE { get; set; }
        public string CN_DESC { get; set; }
    }

    [Table("VIEW_ITEM_CHANGE_DETAIL")]
    public class ViewItemChangeDetail
    {
        [Key]
        public int hlinkid { get; set; }
        public string displayname { get; set; }
        public double? quantity { get; set; }
        public string ver { get; set; }
        public DateTime dtver { get; set; }
        public int idnotice { get; set; }
        public string status { get; set; }
        public string isimplement { get; set; }
        public string itemcode { get; set; }
    }

}
