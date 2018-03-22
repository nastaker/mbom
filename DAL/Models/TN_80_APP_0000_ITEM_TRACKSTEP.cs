namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TN_80_APP_0000_ITEM_TRACKSTEP
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CN_N_OBJECT_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CN_N_TRACK_TYPE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(64)]
        public string CN_STR_STEP_NAME { get; set; }

        public DateTime? CN_DT_CREATE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string CN_STR_OPERATOR { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(32)]
        public string CN_STR_ACTION_NAME { get; set; }

        [StringLength(256)]
        public string CN_STR_NOTE { get; set; }

        public int? CN_N_WF_ID { get; set; }

        public int? CN_N_WFSTEP_ID { get; set; }

        public int? CN_N_REVIEW_BLOBID { get; set; }

        public int? CN_N_PROCESS_ID { get; set; }

        public short? CN_N_WF_STATUS { get; set; }

        public short? CN_N_STATE_INDEX { get; set; }
    }
}
