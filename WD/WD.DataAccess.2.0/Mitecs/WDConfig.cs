
// file:	Mitecs\WDConfig.cs
//
// summary:	Implements the wd configuration class


using System;
using System.Collections.Generic;

using System.Text;

using System.Xml.Serialization;



// namespace: WD.DataAccess.Mitecs
//
// summary:	.


namespace WD.DataAccess.Mitecs
{
    
    /// <summary>   (Serializable) a wd configuration. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    [Serializable]
    [XmlRootAttribute("DBCONFIG", Namespace = "", IsNullable = false)]
    public class WDConfig
    {
        
        /// <summary>   Gets or sets the default site. </summary>
        ///
        /// <value> The default site. </value>
        

        [XmlAttribute("DEFAULT_DB_SITE")]
        public string DefaultSite{get;set;}

        
        /// <summary>   Gets or sets the inventory flag. </summary>
        ///
        /// <value> The inventory flag. </value>
        

        [XmlAttribute("INVENTORY_FLAG")]
        public string InventoryFlag{get;set;}

        
        /// <summary>   Gets or sets the testerd identifier check. </summary>
        ///
        /// <value> The testerd identifier check. </value>
        

        [XmlAttribute("TESTERID_CHECK")]
        public string TesterdIdCheck{get;set;}

        
        /// <summary>   Gets or sets the import drive. </summary>
        ///
        /// <value> The import drive. </value>
        

        [XmlAttribute("IMPORT_DRIVE")]
        public string ImportDrive{get;set;}

        
        /// <summary>   Gets or sets the user server transmit configuration. </summary>
        ///
        /// <value> The user server transmit configuration. </value>
        

        [XmlAttribute("USE_SERVER_TXCONFIG")]
        public string UserServerTxConfig{get;set;}

        
        /// <summary>   Gets or sets the type of the user server connection. </summary>
        ///
        /// <value> The type of the user server connection. </value>
        

        [XmlAttribute("USE_SERVER_CONNECTION_TYPE")]
        public string UserServerConnectionType{get;set;}

        
        /// <summary>   Gets or sets a list of SQL throw errors. </summary>
        ///
        /// <value> A List of SQL throw errors. </value>
        

        [XmlAttribute("SQLTHROWERRORLIST")]
        public string SqlThrowErrorList{get;set;}

        
        /// <summary>   Gets or sets the instances. </summary>
        ///
        /// <value> The instances. </value>
        

        [XmlElement("INSTANCES")]
        public List<Instance> INSTANCES { get; set; }

    }

    
    /// <summary>   (Serializable) an instance. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    [Serializable]
    [XmlRootAttribute("INSTANCES", Namespace = "", IsNullable = true)]
    public class Instance
    {
        
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        

        [XmlAttribute("NAME")]
        public string Name { get; set; }

        
        /// <summary>   Gets or sets the site location. </summary>
        ///
        /// <value> The site location. </value>
        

        [XmlAttribute("SITE_Location")]
        public string SiteLocation { get; set; }

        
        /// <summary>   Gets or sets the time log. </summary>
        ///
        /// <value> The time log. </value>
        

        [XmlAttribute("TIMELOG")]
        public string TimeLog { get; set; }

        
        /// <summary>   Gets or sets the name of the reporting database. </summary>
        ///
        /// <value> The name of the reporting database. </value>
        

        [XmlAttribute("REPORTING_DBNAME")]
        public string ReportingDbName { get; set; }

        
        /// <summary>   Gets or sets the dbs. </summary>
        ///
        /// <value> The dbs. </value>
        

        [XmlElement("DB")]
        public List<DB> Dbs { get; set; }
    }

    
    /// <summary>   (Serializable) a database. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    [Serializable]
    [XmlRootAttribute("DB", Namespace = "", IsNullable = true)]
    public class DB
    {
        
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        

        [XmlAttribute("ID")]
        public string ID { get; set; }

        
        /// <summary>   Gets or sets the type of the database. </summary>
        ///
        /// <value> The type of the database. </value>
        

        [XmlAttribute("DBType")]
        public string DBType { get; set; }

        
        /// <summary>   Gets or sets the name of the database. </summary>
        ///
        /// <value> The name of the database. </value>
        

        [XmlAttribute("DBName")]
        public string DBName { get; set; }

        
        /// <summary>   Gets or sets the server. </summary>
        ///
        /// <value> The server. </value>
        

        [XmlAttribute("Server")]
        public string Server { get; set; }

        
        /// <summary>   Gets or sets the type of the connect. </summary>
        ///
        /// <value> The type of the connect. </value>
        

        [XmlAttribute("CONNECTTYPE")]
        public string ConnectType { get; set; }
    }
}
