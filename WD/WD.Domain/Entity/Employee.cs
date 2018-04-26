using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WD.DataAccess.Attributes;

namespace WD.Domain.Entity
{
    [CustomAttribute(Name="TempEmployee")]
    public class Employee
    {

        [CustomAttribute(IsPrimary=true)]
        [Display(Name="Employee Id")]
        [Required]
        public int? EmployeeId { get; set; }
        [Display(Name="First Name")]
        [Required]
        [CustomAttribute(Name = "FIRSTNAME")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        [Required]
        [CustomAttribute(Name = "MIDDLENAME")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        [CustomAttribute(Name = "LASTNAME")]
        public string LastName { get; set; }
        [Display(Name = "Date Of Joining")]
       // [Required]
        [DisplayFormat(DataFormatString="MM/dd/yyyy",ConvertEmptyStringToNull=true)]
        [CustomAttribute(Name = "DATEOFJOINING")]
        public DateTime DateOfJoining { get; set; }
        [Required]
        public string Provider { get; set; }
        public virtual Customer12 Customer { get; set; }

    }
    [CustomAttribute(Name = "CUSTOMER")]
    public class Customer12 {

        [CustomAttribute(Name = "Name")]
        public string CustomerName { get; set; }
    }

    
}