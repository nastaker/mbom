using System;
namespace MBOM.Models
{
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