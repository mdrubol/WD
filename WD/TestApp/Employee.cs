using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using WD.DataAccess.Attributes;

namespace TestApp
{
    [CustomAttribute(Name="dbo.EMPLOYEE")]
    public class Employee
    {

        //[CustomAttribute(IsPrimary = true)]
        //[Display(Name = "Employee Id")]
        //[Required]
        //public int? EmployeeId { get; set; }

        //[Display(Name = "First Name")]
        //[Required]
        //[CustomAttribute(Name = "FIRSTNAME")]
        //public string FirstName { get; set; }

        //[Display(Name = "Middle Name")]
        //[Required]
        //[CustomAttribute(Name = "MIDDLENAME")]
        //public string MiddleName { get; set; }

        //[Display(Name = "Last Name")]
        //[Required]
        //[CustomAttribute(Name = "LASTNAME")]
        //public string LastName { get; set; }

        //[Display(Name = "Date Of Joining")]
        //[CustomAttribute(Name = "DATEOFJOINING")]
        //public DateTime DateOfJoining { get; set; }

        //[Required]
        //public string Provider { get; set; }

        [CustomAttribute(IsPrimary = true)]
        [Display(Name = "Id")]
        [Required]
        public int? ID { get; set; }

        [Display(Name = "Name")]
        [Required]
        [CustomAttribute(Name = "NAME")]
        public string Name { get; set; }

        [Display(Name = "Country")]
        [Required]
        [CustomAttribute(Name = "COUNTRY")]
        public string Country { get; set; }

        [Display(Name = "Location")]
        [Required]
        [CustomAttribute(Name = "LOCATION")]
        public string Location { get; set; }

        [Display(Name = "Address")]
        [Required]
        [CustomAttribute(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Request No.")]
        [Required]
        //[CustomAttribute(NotMapped=true)]
        public string ReqNo { get; set; }

        [CustomAttribute(Name = "AGE")]
        [Display(Name = "Age")]
        [Required]
        public int? Age { get; set; }

    }
}
