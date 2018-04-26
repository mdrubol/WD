using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WD.XLC.Domain.Entities
{
    /// <summary>   (Serializable) my instance. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>


    [Serializable]
    [XmlRootAttribute("MyApplication", Namespace = "", IsNullable = false)]
    public class MyInstance
    {

        /// <summary>   Gets or sets the zero-based index of this object. </summary>
        ///
        /// <value> The index. </value>


        public int Index { get; set; }


        /// <summary>   Gets or sets a value indicating whether this object is active. </summary>
        ///
        /// <value> True if this object is active, false if not. </value>


        public bool IsActive { get; set; }


        /// <summary>   Gets or sets the inbox. </summary>
        ///
        /// <value> The inbox. </value>


        public string Inbox { get; set; }


        /// <summary>   Gets or sets the folders. </summary>
        ///
        /// <value> The folders. </value>


        public List<string> Folders { get; set; }


        /// <summary>   Gets or sets the templates. </summary>
        ///
        /// <value> The templates. </value>


        public List<string> Templates { get; set; }



    }
}
