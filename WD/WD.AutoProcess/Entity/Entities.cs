using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD.DataAccess.Attributes;

namespace WD.AutoProcess
{
     
    public class ProcessItem
    {
        public int Index { get; set; }
        [Custom(Name="PROCESS_CODE")]
        public string ProcessCode{get;set;}
        [Custom(Name = "SID")]
        public int Sid{get;set;}
        [Custom(Name = "SERIAL#")]
        public int SerialNo{get;set;}
        [Custom(Name = "SPID")]
        public int SpId { get; set; }
        [Custom(Name = "PROCESS_START_TIME")]
        public DateTime StartTime{get;set;}
        [Custom(Name = "PROCESS_END_TIME")]
        public DateTime EndTime{get;set;}
        [Custom(Name = "DELAY")]
        public decimal Delay{get;set;}
        [Custom(Name = "THRESHOLD")]
        public decimal Threshold{get;set;}
        [Custom(Name = "STATUS")]
        public char Status{get;set;}
        [Custom(Name = "UNIXSCRIPT")]
        public string UnixScript{get;set;}
        [Custom(Name = "SQL_TEXT")]
        public string SqlText{get;set;}
        [Custom(Name = "CREATE_DATE")]
        public DateTime CreatedOn{get;set;}
        [Custom(Name = "DIFFERENCE")]
        public decimal Difference { get; set; }
        [Custom(Name = "Dependency")]
        public int Dependency { get; set; }
    }

    
}
