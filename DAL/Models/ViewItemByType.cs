using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("VIEW_ITEM_BY_TYPE")]
    public class ViewItemByType
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }
        public string itemcode { get; set; }
        public string name { get; set; }
        public int typeid { get; set; }
        public string typename { get; set; }
        public string xh { get; set; }
        public string gg { get; set; }
        public double weight { get; set; }
        public string unit { get; set; }
        public string productbase { get; set; }
        public int createby { get; set; }
        public string createname { get; set; }
        public string createlogin { get; set; }
        public DateTime createdate { get; set; }
    }
    [Table("VIEW_ITEM_WITH_TYPE")]
    public class ViewItemWithType
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }
        public string itemcode { get; set; }
        public string name { get; set; }
        public string typeids { get; set; }
        public string typenames { get; set; }
        public string xh { get; set; }
        public string gg { get; set; }
        public double weight { get; set; }
        public string unit { get; set; }
        public string productbase { get; set; }
        public int createby { get; set; }
        public string createname { get; set; }
        public string createlogin { get; set; }
        public DateTime createdate { get; set; }
    }
}
