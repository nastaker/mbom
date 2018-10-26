namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_50_DIC_0005_PRODUCT_LINE")]
    public partial class DictProductLine
    {
        [Key]
        public int CN_ID { get; set; }
        public string CN_CODE { get; set; }
        public int CN_NUMBER { get; set; }
        public string CN_NAME { get; set; }
    }
}
