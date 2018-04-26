using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WD.XLC.Domain.Entities
{
    [Serializable]
    [XmlRootAttribute("MyApplication", Namespace = "", IsNullable = false)]
    public class MyApplication
    {


        /// <summary>   Gets or sets the instances. </summary>
        ///
        /// <value> The instances. </value>


        [XmlElement("MyInstance")]
        public List<MyInstance> Instances { get; set; }
    }
}
