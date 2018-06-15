using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("TN_APP_0090_CUSTOMER")]
    public class DictCustomer
    {
        [Key]
        public int CN_ID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_NAME_SHORT { get; set; }
        public string CN_DESC { get; set; }
    }
}
