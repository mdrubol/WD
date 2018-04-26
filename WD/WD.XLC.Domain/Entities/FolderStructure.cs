using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WD.DataAccess.Attributes;

namespace WD.XLC.Domain.Entities
{
    // <summary>   An application configuration. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>

    [WD.DataAccess.Attributes.Custom(Name = "XLC_FolderStructure")]
   public class FolderStructure
    {
        [CustomAttribute(Name = "ID")]
        [XmlElementAttribute("ID")]
        public string Id { get; set; }


        /// <summary>   Gets or sets the identifier of the record. </summary>
        ///
        /// <value> The identifier of the record. </value>


        [XmlElementAttribute("FolderName")]
        public string FolderName { get; set; }


        /// <summary>   Gets or sets the configuration. </summary>
        ///
        /// <value> The configuration. </value>


        [XmlElementAttribute("FolderExtension")]
        public string FolderExtension { get; set; }


        /// <summary>   Gets or sets the Date/Time of the created on. </summary>
        ///
        /// <value> The created on. </value>

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [XmlElementAttribute("CreatedOn")]
        public DateTime CreatedOn { get; set; }


        /// <summary>   Gets or sets the Date/Time of the updated on. </summary>
        ///
        /// <value> The updated on. </value>

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [XmlElementAttribute("UpdatedOn")]
        public DateTime UpdatedOn { get; set; }


        /// <summary>   Gets or sets who created this object. </summary>
        ///
        /// <value> Describes who created this object. </value>


        [XmlElementAttribute("CreatedBy")]
        public string CreatedBy { get; set; }


        /// <summary>   Gets or sets who updated this object. </summary>
        ///
        /// <value> Describes who updated this object. </value>


        [XmlElementAttribute("UpdatedBy")]
        public string UpdatedBy { get; set; }


        /// <summary>   Gets or sets the is active. </summary>
        ///
        /// <value> The is active. </value>


        [XmlElementAttribute("IsActive")]
        public int IsActive { get; set; }

    }
}
