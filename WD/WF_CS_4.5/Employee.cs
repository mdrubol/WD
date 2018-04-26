using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WD.DataAccess.Attributes;

namespace WF_CS_4._5
{
    class Employee
    {
        [CustomAttribute(Name = "EmployeeId", IsPrimary = true)]
        [Display(Name = "Employee Id")]
        [Required]
        public dynamic EmployeeId { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        [Required]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Date Of Joining")]
        [Required]
        [DisplayFormat(DataFormatString = "MM/dd/yyyy", ConvertEmptyStringToNull = true)]
        public DateTime DateOfJoining { get; set; }
        [Required]
        public string Provider { get; set; }

    }
}
