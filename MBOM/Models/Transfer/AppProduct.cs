using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class IntegrityCheckView
    {
        public string CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public string NAME { get; set; }
        public string CHECK_STATUS { get; set; }
        public string SALE_SET { get; set; }
        public string STATUS { get; set; }
    }
}