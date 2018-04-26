using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD.DataAccess.Attributes;


    [WD.DataAccess.Attributes.Custom(Name = "MVX_APP_SITE_SERVER")]
    public class SiteInfo
    {
        public int AuditVersion { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string Remarks { get; set; }
        public string AppCode { get; set; }
        public string LocationID { get; set; }
        public string LocationName { get; set; }
        public string HostIP { get; set; }
        public string HostDNS { get; set; }
        public string HostType { get; set; }
        public string URL { get; set; }
        public string HostOwner { get; set; }
        public bool StatusCode { get; set; }

        public DateTime TimeStamp2 { get; set; }

    }

