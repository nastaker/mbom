using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("VIEW_CREATE_PUBLISH_DETAIL")]
    public class ViewCreatePublishDetail
    {
        [Key]
        public int hlinkid { get; set; }
        public string type { get; set; }
        public int itemid { get; set; }
        public string pitemcode { get; set; }
        public string bywhat { get; set; }
        public string itemcode { get; set; }
        public string name { get; set; }
        public double? quantity { get; set; }
        public string source { get; set; }
        public short istoerp { get; set; }
        public string changesign { get; set; }
        public string impactdo { get; set; }
    }
}
