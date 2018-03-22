namespace DAL.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CommonOptionalItem
    {
        public string code { get; set; }
        public string itemcode { get; set; }
        public string name { get; set; }
        public string xh { get; set; }
        public string gg { get; set; }
        public double weight { get; set; }
        public string unit { get; set; }
        public string desc { get; set; }
    }

    [Table("VIEW_OPTIONAL_ITEM")]
    public class ViewOptionalItem : CommonOptionalItem
    {
        [Key]
        public int hlinkid { get; set; }
        public int id { get; set; }
    }

    [Table("VIEW_NO_OPTIONAL_ITEM")]
    public class ViewNoOptionalItem : CommonOptionalItem
    {
        [Key]
        public int id { get; set; }
    }
}
