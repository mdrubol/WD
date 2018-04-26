// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 04-27-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-21-2017
// ***********************************************************************
// <copyright file="AppConfig.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WD.DataAccess.Attributes;



// namespace: WD.XLC.Domain.Entities
//
// summary:	.


namespace WD.XLC.Domain.Entities
{
    
    /// <summary>   (Serializable) an application data. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    

    [Serializable]
    [XmlRootAttribute("AppData", Namespace = "", IsNullable = false)]
    public class AppData {

        [XmlArray("AppConfigs"), XmlArrayItem("AppConfig", typeof(AppConfig))]
        public List<AppConfig> AppConfigs { get; set; }
    }

    
    /// <summary>   An application configuration. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    
    [Custom(Name="XLC_AppConfig")]
    public class AppConfig
    {
        
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        

        [CustomAttribute(Name="ID")]
        [XmlElementAttribute("ID")]
        public string Id { get; set; }

        
        /// <summary>   Gets or sets the identifier of the record. </summary>
        ///
        /// <value> The identifier of the record. </value>
        

        [XmlElementAttribute("TemplateName")]
        public string RecId { get; set; }

        
        /// <summary>   Gets or sets the configuration. </summary>
        ///
        /// <value> The configuration. </value>
        

        [XmlElementAttribute("Config")]
        public string Config { get; set; }

        
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
        /// <summary>
        /// 
        /// </summary>
        [XmlElementAttribute("ServerId")]
        public string ServerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElementAttribute("TargetTableName")]
        public string TargetTableName { get; set; }
         [XmlElementAttribute("ConnString")]
        public string ConnString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElementAttribute("DbProvider")]
        public int DbProvider { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        [XmlElementAttribute("ServerName")]
        public string ServerName { get; set; }

    }

    
    /// <summary>   A file process. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    

    public class FileProcess {

        
        /// <summary>   Gets or sets the created on. </summary>
        ///
        /// <value> The created on. </value>
        

        public string CreatedOn { get; set; }

        
        /// <summary>   Gets or sets the number of files. </summary>
        ///
        /// <value> The number of files. </value>
        

        public int FileCount { get; set; }
    
    }


    public class TableInfo
    {
        public string COLUMNNAME { get; set; }
        public string DATATYPE { get; set; }
        public string COLUMNSIZE { get; set; }
        public bool IsPrimary { get; set; }
    }
}
