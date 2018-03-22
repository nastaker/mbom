using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBOM.Models
{
    public class BomDiffView
    {
        public string code { get; set; }
        public string item_code { get; set; }
        public int hlink_id { get; set; }
        public string s_bom_type { get; set; }
        public int bom_id { get; set; }
        public int bom_id_pre { get; set; }
        public string displayname { get; set; }
        public string mbomname { get; set; }
        public double? quantity { get; set; }
        public int order { get; set; }
        public string sys_status { get; set; }
        public DateTime dt_create { get; set; }
        public string status_pbom { get; set; }
        public string status_mbom { get; set; }
        public DateTime dt_ef_pbom { get; set; }
        public DateTime dt_ex_pbom { get; set; }
        public DateTime dt_ef_mbom { get; set; }
        public DateTime dt_ex_mbom { get; set; }
    }
    public class AppBomView
    {
        public int id { get; set; }
        public string code { get; set; }
        public string item_code { get; set; }
        public string name { get; set; }
    }
    public class AppBomHLinkView
    {
        public int HLINK_ID { get; set; }
        public string S_BOM_TYPE { get; set; }
        public int BOM_ID { get; set; }
        public int COMPONENT_CLASS_ID { get; set; }
        public int COMPONENT_OBJECT_ID { get; set; }
        public int PDM_CLASS_ID { get; set; }
        public int PDM_OBJECT_ID { get; set; }
        public string CODE { get; set; }
        public string DISPLAYNAME { get; set; }
        public int? ORDER { get; set; }
        public int? NUMBER { get; set; }
        public double F_QUANTITY { get; set; }
        public string UNIT { get; set; }
        public bool? B_IS_ASSEMBLY { get; set; }
        public bool? ISBORROW { get; set; }
        public bool? ISDELETE { get; set; }
        public string STATUS_PBOM { get; set; }
        public string STATUS_MBOM { get; set; }
        public string S_FROM { get; set; }
        public string DESC { get; set; }
        public int CREATE_BY { get; set; }
        public string CREATE_NAME { get; set; }
        public string CREATE_LOGIN { get; set; }
        public string SYS_STATUS { get; set; }
        public int? COMPONENT_OBJECT_VER_ID { get; set; }
        public string COMPONENT_OBJECT_VERSION { get; set; }
        public string S_ATTACH_DATA { get; set; }
        public int? ITEMSTATE_TAGGER_DATA { get; set; }
        public bool? ISFOLDER { get; set; }
        public string SYS_NOTE { get; set; }
        public DateTime DT_CREATE { get; set; }
        public DateTime DT_EF_PBOM { get; set; }
        public DateTime DT_EX_PBOM { get; set; }
        public DateTime DT_EF_MBOM { get; set; }
        public DateTime DT_EX_MBOM { get; set; }
        public DateTime DT_EFFECTIVE { get; set; }
        public DateTime DT_EXPIRY { get; set; }
        public string GUID { get; set; }
    }
}