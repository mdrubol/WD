using System;
using WD.DataAccess.Attributes;


    [WD.DataAccess.Attributes.Custom(Name = "MVX_APP_EMAIL_SUBSCRIPTION")]
    public class EmailSubscription
    {
        private string _mode ="";
        public int ID { get; set; }

        public string AppCode { get; set; }

        public string EmployeeID { get; set; }

        public int ModuleID { get; set; }

        //[CustomAttribute(Name="[TableName]")]
        public string TableName { get; set; }

        public string AttributeKey { get; set; }
        public string AttributeOperator { get; set; }

        public string AttributeValue { get; set; }

        public string Mode { get; set; }

        //[CustomAttribute(Name = "[TIMESTAMP2]")]
        public DateTime TIMESTAMP2 { get; set; }

        //[CustomAttribute(NotMapped = true)]
        //public string ModuleName { get; set; }
        
        //[CustomAttribute(NotMapped = true)]
        //public  bool ModeInsert { get; set; }
        ////
        //[CustomAttribute(NotMapped = true)]
        //public bool ModeUpdate { get; set; }

        //[CustomAttribute(NotMapped = true)]
        //public bool ModeDelete { get; set; }


    }

