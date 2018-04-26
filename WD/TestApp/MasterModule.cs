using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    [WD.DataAccess.Attributes.Custom(Name = "MVX_APP_MASTER_MODULE")]
    public class MasterModule
    {
        public int AuditVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Remarks { get; set; }
        public string AppCode { get; set; }

        [WD.DataAccess.Attributes.Custom(IsPrimary = true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string VersionNo { get; set; }
        public string ObjectName { get; set; }
        public int SortOrder { get; set; }
        public bool StatusCode { get; set; }
        public string ToolTip { get; set; }
        public int Hits { get; set; }

    }

