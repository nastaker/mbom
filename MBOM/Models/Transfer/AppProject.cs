using System;

namespace MBOM.Models
{
    public class AppProjectView
    {
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public int OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        public string OWNER_LOGIN { get; set; }
        public string DESC { get; set; }
        public int CREATE_BY { get; set; }
        public string CREATE_NAME { get; set; }
        public string CREATE_LOGIN { get; set; }
        public string SYS_STATUS { get; set; }
        public string SYS_NOTE { get; set; }
        public DateTime DT_CREATE { get; set; }
        public DateTime DT_EFFECTIVE { get; set; }
        public DateTime DT_EXPIRY { get; set; }
        public string GUID { get; set; }
    }

    public class ViewProjectProductPbomView
    {
        public int PROJECT_ID { get; set; }
        public string CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        public int OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_STATUS { get; set; }
        public string CHECK_STATUS { get; set; }
        public string SALE_SET { get; set; }
        public int BOMID { get; set; }
        public string VER { get; set; }
        public DateTime DT_VER { get; set; }
        public string CREATE_NAME { get; set; }

    }
}