using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.MVX.HDD.Domain.Models
{
    [WD.DataAccess.Attributes.Custom(Name = "MVX_APP_DATA_ENTRY_VALIDATION")]
    public class DataEntryRules
    {
        public int AuditVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remarks { get; set; }
        public string AppCode { get; set; }
        public int ModuleID { get; set; }
        public string ModuleTitle { get; set; }
        public string ColumnName { get; set; }
        public string ColumnDisplayName { get; set; }
        public string ValidationType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsEditable { get; set; }
        public bool AllowSpace { get; set; }
        public bool S_AllowNumeric { get; set; }
        public bool UpperCase { get; set; }
        public bool LowerCase { get; set; }
        public int MaxLen { get; set; }
        public int? MinLen { get; set; }
        public bool S_AllowSplChar { get; set; }
        public string S_SplChar { get; set; }
        public string S_DefaultValue { get; set; }
        public Nullable<decimal> N_MinValue { get; set; }
        public decimal? N_MaxValue { get; set; }
        public bool N_AllowNegative { get; set; }
        public bool N_AllowDecimal { get; set; }
        public int? N_DecimalPrecision { get; set; }
        public string D_MinDatetime { get; set; }
        public string D_MaxDatetime { get; set; }
        public string D_DefaultDatetime { get; set; }
        public string DependentColumnName { get; set; }
        public string DependentComparision { get; set; }
        public string ColumnSample { get; set; }
        public DateTime TIMESTAMP2 { get; set; }

    }
}
