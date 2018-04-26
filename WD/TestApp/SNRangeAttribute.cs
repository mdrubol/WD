using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    [WD.DataAccess.Attributes.Custom(Name = "LU_MVX_SN_RANGE_ATTRIBUTE")]
   public class SNRangeAttribute
    {
        public string PRODUCT_FAMILY { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string DEID { get; set; }
        public string PREAMP_SUPPLIER { get; set; }
        public string HEAD_COUNT { get; set; }
        public string WAFER_SUPPLIER { get; set; }
        public string MEDIA_COUNT { get; set; }
        public string HEAD_SUPPLIER { get; set; }
        public string MEDIA_SUPPLIER { get; set; }
        public string REMARKS { get; set; }
        public string HEAD_MAP { get; set; }
        public string USERID { get; set; }
        public DateTime TIMESTAMP2 { get; set; }
        public decimal AUDIT_VERSION { get; set; }
    }

