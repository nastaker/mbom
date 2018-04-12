namespace Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TN_80_APP_0000_ITEM")]
    public partial class AppItem
    {
        [Key]
        public int CN_ID { get; set; }
        public string CN_CODE { get; set; }
        public string CN_ITEM_CODE { get; set; }
        public string CN_NAME { get; set; }
        public string CN_XH { get; set; }
        public string CN_GG { get; set; }
        public bool CN_MBOM_VIRTUAL { get; set; }
        public bool CN_MBOM_GROUP { get; set; }
        public double CN_WEIGHT { get; set; }
        public string CN_UNIT { get; set; }
        public string CN_PRODUCT_BASE { get; set; }
        public string CN_DESIGN_PHASE { get; set; }
        public int CN_PDM_CLS_ID { get; set; }
        public int CN_PDM_OBJ_ID { get; set; }
        public string CN_DESC { get; set; }
        public int? CN_BATCH_ID_INPUT { get; set; }
        public int? CN_BATCH_ID_OUTPUT { get; set; }
        public short? CN_IS_TOERP { get; set; }
        public int CN_CREATE_BY { get; set; }
        public string CN_CREATE_NAME { get; set; }
        public string CN_CREATE_LOGIN { get; set; }
        public string CN_SYS_STATUS { get; set; }
        public DateTime CN_DT_CREATE
        {
            get
            {
                if(_createTime == null)
                {
                    return DateTime.Now;
                }
                if (_createTime.HasValue && _createTime.Value > DateTime.MinValue)
                {
                    return _createTime.Value;
                }
                return DateTime.Now;
            }
            set { _createTime = value; }
        }
        public DateTime CN_DT_EFFECTIVE_DESIGN
        {
            get
            {
                if (_createTime == null)
                {
                    return DateTime.Now;
                }
                if (_createTime.HasValue && _createTime.Value > DateTime.MinValue)
                {
                    return _createTime.Value;
                }
                return DateTime.Now;
            }
            set { _createTime = value; }
        }
        public DateTime CN_DT_EXPIRY_DESIGN
        {
            get
            {
                if (_createTime == null)
                {
                    return DateTime.Now;
                }
                if (_createTime.HasValue && _createTime.Value > DateTime.MinValue)
                {
                    return _createTime.Value;
                }
                return DateTime.Now;
            }
            set { _createTime = value; }
        }
        public DateTime CN_DT_EFFECTIVE_ERP
        {
            get
            {
                if (_createTime == null)
                {
                    return DateTime.Now;
                }
                if (_createTime.HasValue && _createTime.Value > DateTime.MinValue)
                {
                    return _createTime.Value;
                }
                return DateTime.Now;
            }
            set { _createTime = value; }
        }
        public DateTime CN_DT_EXPIRY_ERP
        {
            get
            {
                if (_createTime == null)
                {
                    return DateTime.Now;
                }
                if (_createTime.HasValue && _createTime.Value > DateTime.MinValue)
                {
                    return _createTime.Value;
                }
                return DateTime.Now;
            }
            set { _createTime = value; }
        }
        private DateTime? _createTime;
    }
}
