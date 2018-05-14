using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class ViewMbomMaintenanceView
    {
        public int PROJECT_ID { get; set; }
        public string CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_STATUS { get; set; }
        public string TECH_STATUS { get; set; }
        public string CHECK_STATUS { get; set; }
        public string SALE_SET { get; set; }
        public int BOMID { get; set; }
        public string PBOMVER { get; set; }
        public DateTime DT_PBOMVER { get; set; }
        public string PBOM_CREATE_NAME { get; set; }
        public int OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        public bool MARK { get; set; }
    }
}