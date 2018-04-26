using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    [WD.DataAccess.Attributes.Custom(Name = "LU_MVX_MASTER_TEST_PLAN")]
    public class MasterTestPlan
    {
        public string PRODUCT_FAMILY { get; set; }
        public string MFG_ID { get; set; }
        public string PROCESS_ID { get; set; }
        public string TRIAL_ID { get; set; }
        public string TEST_CODE { get; set; }
        public string TEST_PLATFORM { get; set; }
        public string FEAT_CODE { get; set; }
        public DateTime EFFECTIVE_DATE { get; set; }
        public DateTime EXPIRY_DATE { get; set; }
        public Guid MTP_ID { get; set; }
        public string REMARKS { get; set; }
        public string USERID { get; set; }
        public DateTime TIMESTAMP2 { get; set; }
        public int AUDIT_VERSION { get; set; }
    }
}
