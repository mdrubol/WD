using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD.DataAccess.Attributes;


    [WD.DataAccess.Attributes.Custom(Name = "MVX_APP_MASTER_USER")]
    public class MasterUser
    {
        [CustomAttribute(IsPrimary = true)]
        [Display(Name = "EmployeeID")]
        [Required]
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string SupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorEmail { get; set; }

    }

