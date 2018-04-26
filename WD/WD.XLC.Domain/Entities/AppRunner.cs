using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.XLC.Domain.Entities
{
    public class AppRunner: System.Configuration.ConfigurationSection
    {
        [System.Configuration.ConfigurationProperty("run", DefaultValue = "false", IsRequired = true)]
        public Boolean Run
        {
            get
            { 
                return (Boolean)this["run"]; 
            }
            set
            {
                this["run"] = value; 
            }
        }
        [System.Configuration.ConfigurationProperty("loader")]
        public int Loader
        {
            get
            {
                return (int)this["loader"];
            }
            set
            { this["loader"] = value; }
        }
    }
}
