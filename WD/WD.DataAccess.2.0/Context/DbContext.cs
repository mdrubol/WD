// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="DbContext.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using WD.DataAccess.Abstract;
using WD.DataAccess.Concrete;
using WD.DataAccess.Configurations;
using WD.DataAccess.Enums;
using System;
using WD.DataAccess.Logger;
using WD.DataAccess.Helpers;
using WD.DataAccess.Factory;
using WD.DataAccess.Mitecs;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;




// namespace: WD.DataAccess.Context
//
// summary:	.


namespace WD.DataAccess.Context
{

    /// <summary>
    /// DbContext is main class which helps in initializing ICommands interface.
    /// </summary>
    /// <seealso cref="System.IDisposable" /> //
    /// <remarks>Shahid Kochak, 7/20/2017.</remarks>

    public class DbContext : IDisposable
    {

        /// <summary>
        /// The bx configuration.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal List<WDDBConfig> BXConfig { get; set; }
        /// <summary>
        /// The transmit configuration.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal List<WDDBConfig> TXConfig { get; set; }
        /// <summary>
        /// The databases.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly int databases;
        /// <summary>
        /// Identifier for the dummy.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string dummyId { get { return WebSecurityUtility.Decrypt(@"/6h52Ts3CtE91axnZu2+LA==", true); } }
        /// <summary>
        /// The dummy password.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string dummyPasswd { get { return WebSecurityUtility.Decrypt(@"duqedSneNm8VoijMkgNJ/A==", true); } }
        /// <summary>
        /// ICommands Public Field to Access all in build functions.
        /// </summary>
        public ICommands ICommands { get; set; }
        /// <summary>
        /// True if this object is changed.
        /// </summary>
        private bool isChanged { get; set; }
        /// <summary>
        /// The XML instance.
        /// </summary>
        public Instance xmlInstance { get; set; }
        /// <summary>
        /// Name of the site.
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// The site location.
        /// </summary>
        public string SiteLocation { get; set; }
        /// <summary>
        /// The inventory flag.
        /// </summary>
        public string InventoryFlag { get; set; }
        /// <summary>
        /// The tester identifier check.
        /// </summary>
        public string TesterIdCheck { get; set; }
        /// <summary>
        /// The import drive.
        /// </summary>
        public string ImportDrive { get; set; }
        /// <summary>
        /// The user server transmit configuration.
        /// </summary>
        public string UserServerTxConfig { get; set; }
        /// <summary>
        /// Type of the user server connection.
        /// </summary>
        public string UserServerConnectionType { get; set; }
        /// <summary>
        /// Name of the reporting database.
        /// </summary>
        public string ReportingDbName { get; set; }
        /// <summary>
        /// The time log.
        /// </summary>
        public string TimeLog { get; set; }
        /// <summary>
        /// The configuration.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal WDConfig Config { get; set; }
        #region Private Memebers
        /// <summary>
        /// Full pathname of the XML file.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly string xmlFilePath = null;
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly string xmlFileName = null;
        #endregion
        #region Constructors


        /// <summary>
        /// This method initializes ICommands as per the connection string and DbProvider.
        /// </summary>
        /// <param name="connectionString">.</param>
        /// <param name="provider">.</param>
        /// <example>
        /// This sample shows how to call the <see cref="Constructor" /> // method.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// string connectionString ="DataSource =XXXXX;UserId=XXX;Password=XXXX";
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// dbContext.Constructor(connectionString, WD.DataAccess.Enums.DBProvider.Sql);
        /// }
        /// }
        /// </code>
        /// </example>


        public void Constructor(string connectionString, int provider)
        {
            try
            {
                this.dbProvider = provider;
                this.connectionString = connectionString;
                switch (this.dbProvider)
                {
                    case DBProvider.Sql:
                        ICommands = AppConfiguration.LoadFactory<SqlFactory>(this.connectionString, this.dbProvider);
                        break;
                    case DBProvider.Oracle:
                    case DBProvider.Oracle2:
                        ICommands = AppConfiguration.LoadFactory<OracleFactory>(this.connectionString, this.dbProvider);
                        break;
                    case DBProvider.Db2:
                        ICommands = AppConfiguration.LoadFactory<Db2Factory>(this.connectionString, this.dbProvider);
                        break;
                    case DBProvider.PostgreSQL:
                        ICommands = AppConfiguration.LoadFactory<PostgreSQLFactory>(this.connectionString, this.dbProvider);
                        break;
                    case DBProvider.MySql:
                        ICommands = AppConfiguration.LoadFactory<MySqlFactory>(this.connectionString, this.dbProvider);
                        break;
                    default:
                        ICommands = AppConfiguration.LoadFactory<TeraFactory>(this.connectionString, this.dbProvider);
                        break;
                }
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw exc;
            }
        }


        /// <summary>   Default constructor. </summary>
        /// <example>
        /// This sample shows how to initialize <see cref="DbContext" /> //.
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>Before we initialize default constructor we should have connection string with the name DefaultConnection
        /// <code>
        ///  <connectionStrings>//
        /// <add name="DefaultConnection" connectionString="Data Source=XXXXX;User Id=XXX;Password=XXX" providerName="System.Data.SqlClient" /> //
        ///  </connectionStrings>//
        /// </code>
        /// </remarks>


        public DbContext()
            : this("DefaultConnection")
        {

        }


        /// <summary>
        /// Creating Instance of DBContext using connectionName
        /// </summary>
        /// <param name="connectionName">   Name of the connection. </param>
        /// <example>
        /// This sample shows how to initialize <see cref="DbContext(string connectionName)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext("testConnection");
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>We should have connection string with the name we passing for DbContext initialization.
        /// <code>
        ///  <connectionStrings>//
        /// <add name="testConnection" connectionString="Data Source=XXXXX;User Id=XXX;Password=XXX" providerName="System.Data.SqlClient" /> //
        /// <add name="DefaultConnection" connectionString="Data Source=XXXXX;User Id=XXX;Password=XXX" providerName="System.Data.SqlClient" /> //
        /// </connectionStrings>//
        /// </code>
        /// </remarks>


        public DbContext(string connectionName)
            : this(new Connect() { DbProvider = AppConfiguration.GetDbProvider(connectionName), ConnectionString = AppConfiguration.GetDefaultConnectionString(connectionName) })
        {


        }


        /// <summary>
        /// Creating Instance of DBContext using DBProvider (WD.DataAccess.Enums.Sql ,WD.DataAccess.Enums.Oracle,WD.DataAccess.Enums.Oracle2,WD.DataAccess.Enums.Db2 or WD.DataAccess.Enums.TeraData)
        /// </summary>
        /// <param name="dbProvider">WD.DataAccess.Enums.Sql ,WD.DataAccess.Enums.Oracle,WD.DataAccess.Enums.Oracle2,WD.DataAccess.Enums.Db2 or WD.DataAccess.Enums.TeraData</param>
        /// <example>
        /// This sample shows how to initialize <see cref="DbContext(int dbProvider)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(WD.DataAccess.Enums.Sql);
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>Default providers are maintains Connection Strings in DataAccess Layer.
        /// </remarks>


        public DbContext(int dbProvider)
            : this(new Connect() { DbProvider = dbProvider, ConnectionString = new AppConfiguration().GetConnectionString(HelperUtility.GetDbProviderName(dbProvider)) })
        {

        }


        /// <summary>
        /// Creating Instance of DBContext using DBProvider (WD.DataAccess.Enums.Sql ,WD.DataAccess.Enums.Oracle,WD.DataAccess.Enums.Oracle2,WD.DataAccess.Enums.Db2 or WD.DataAccess.Enums.TeraData) and AppkeyName
        /// </summary>
        /// <param name="dbProvider">WD.DataAccess.Enums.Sql ,WD.DataAccess.Enums.Oracle,WD.DataAccess.Enums.Oracle2,WD.DataAccess.Enums.Db2 or WD.DataAccess.Enums.TeraData</param>
        /// <param name="tokenName">tokeName</param>
        /// <example>
        /// This sample shows how to initialize <see cref="DbContext(int dbProvider, string tokenName)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(WD.DataAccess.Enums.Sql,"AppSettingKey");
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>Application should have encrypted connection string in app settings with token name which we are passing as parameter.
        ///<code>
        /// <appSettings>//
        ///  <add key="AppSettingKey" value="XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"/> //
        /// </appSettings>//
        /// </code>
        /// </remarks>


        public DbContext(int dbProvider, string tokenName)
            : this(new Connect() { ConnectionString = new AppConfiguration().GetConnectionString(tokenName), DbProvider = dbProvider })
        {
        }


        /// <summary>
        /// Creating Instance of DBContext using Connect With Options dbProvider= (WD.DataAccess.Enums.Sql ,WD.DataAccess.Enums.Oracle,WD.DataAccess.Enums.Oracle2,WD.DataAccess.Enums.Db2 or WD.DataAccess.Enums.TeraData) 
        /// ,ConnectionName or ConnectionString.
        /// </summary>
        /// <param name="aConnect">.</param>
        /// <example>
        /// This sample shows how to initialize <see cref="DbContext(Connect aConnect)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Helpers.Connect aConnect=new WD.DataAccess.Helpers.Connect();
        /// aConnect.ConnectionName="testConnection";
        /// aConnect.DbProvider=WD.DataAccess.Enums.Sql;
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(aConnect);
        /// //or
        /// aConnect.ConnectionString="Data Source=XXXXX;User Id=XXX;Password=XXX";
        /// aConnect.DbProvider=WD.DataAccess.Enums.Sql;
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(aConnect);
        /// 
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>Connection string in web.config or App.config.
        ///<code>
        /// <connectionStrings>//
        /// <add name="testConnection" connectionString="Data Source=XXXXX;User Id=XXX;Password=XXX" providerName="System.Data.SqlClient" /> //
        /// </connectionStrings>//
        /// </code>
        /// </remarks>


        public DbContext(Connect aConnect)
        {

            try
            {
                this.dbProvider = aConnect.DbProvider;
                this.connectionString = new AppConfiguration().GetConnectionString(aConnect);
                Constructor(this.connectionString, this.dbProvider);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>   Creating Instance of DBContext using boolean flag. </summary>
        /// <param name="isBackUp"> True if this object needs back up of config.xml. </param>
        ///  <example>
        /// This sample shows how to initialize <see cref="DbContext(bool isBackUp)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// We should have config.xml and its path defined in App settings before DbContext initialization.
        /// <code>
        /// 
        /// <appSettings>//
        ///     <add key="ConnectionsFile" value="E:\XX\XXX.xml" /> //
        /// </appSettings>//
        /// 
        /// <xml>//
        ///         <DBCONFIG DEFAULT_DB_SITE="SITP" INVENTORY_FLAG="OFF" TESTERID_CHECK="OFF" IMPORT_DRIVE="OFF" USE_SERVER_TXCONFIG="FALSE" USE_SERVER_CONNECTION_TYPE="FALSE">//
        ///	            <INSTANCES NAME="SITP">//
        ///                   <DB ID="TX1" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE"/> //
        ///                   <DB ID="TX2" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE"/> //
        ///                   <DB ID="BR1" DBType="SQL" DBName="WDMBR1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE" /> //
        ///                    <DB ID="BR2" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE" /> //
        ///	            </INSTANCES>//
        ///         </DBCONFIG>//
        /// </xml>//  
        /// </code>
        /// </remarks>

        public DbContext(bool isBackUp)
        {

            try
            {
                this.xmlFilePath = Path.GetDirectoryName(ConfigurationManager.AppSettings["ConnectionsFile"]);
                this.xmlFilePath = this.xmlFilePath.Contains(":\\") == true ? this.xmlFilePath : Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, this.xmlFilePath);
                this.xmlFileName = Path.GetFileName(ConfigurationManager.AppSettings["ConnectionsFile"]);
                this.Config = Helpers.ObjectXMLSerializer<WDConfig>.Load(Path.Combine(this.xmlFilePath, this.xmlFileName));
                this.SiteName = Config.DefaultSite;
                this.databases = Databases.BR;
                Bind();
                if (isBackUp && isChanged)
                    this.BackUp();
                this.Config = null;
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
        }


        /// <summary>
        /// Creating Instance of DBContext using siteName as per XML and (WD.DataAccess.Enum.Databases.TX or WD.DataAccess.Enum.Databases.BR).
        /// </summary>
        /// <param name="siteName"> Name of the site. </param>
        /// <param name="database"> WD.DataAccess.Enum.Databases.TX or WD.DataAccess.Enum.Databases.BR</param>
        ///  <example>
        /// This sample shows how to initialize <see cref="DbContext(siteName,database)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext("SITE1",WD.DataAccess.Enum.Databases.BR);
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// We should have config.xml and its path defined in App settings before DbContext initialization.
        /// <code>
        /// <appSettings>//
        ///  <add key="ConnectionsFile" value="E:\XX\XXX.xml" /> //
        /// </appSettings>//
        /// <xml>//
        /// <DBCONFIG DEFAULT_DB_SITE="SITP" INVENTORY_FLAG="OFF" TESTERID_CHECK="OFF" IMPORT_DRIVE="OFF" USE_SERVER_TXCONFIG="FALSE" USE_SERVER_CONNECTION_TYPE="FALSE">//
        ///	<INSTANCES NAME="SITP">//
        ///    <DB ID="TX1" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE"/> //
        ///    <DB ID="TX2" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE"/> //
        ///    <DB ID="BR1" DBType="SQL" DBName="WDMBR1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE" /> //
        ///    <DB ID="BR2" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE" /> //
        ///	</INSTANCES>//
        ///  <INSTANCES NAME="SITE1">//
        ///    <DB ID="TX1" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE"/> //
        ///    <DB ID="TX2" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE"/> //
        ///    <DB ID="BR1" DBType="SQL" DBName="WDMBR1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE" /> //
        ///    <DB ID="BR2" DBType="SQL" DBName="WDMTX1" Server="XXX-XX-XXXXX" CONNECTTYPE="ONLINE" /> //
        ///	</INSTANCES>//
        /// </DBCONFIG>//
        /// </xml>//
        ///  </code>
        /// </remarks>


        public DbContext(string siteName, int database)
            : this(siteName, database, false)
        {


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="database"></param>
        /// <param name="isBackUp"></param>
        public DbContext(string siteName, int database, bool isBackUp)
        {

            try
            {
                this.xmlFilePath = Path.GetDirectoryName(ConfigurationManager.AppSettings["ConnectionsFile"]);
                this.xmlFilePath = this.xmlFilePath.Contains(":\\") == true ? this.xmlFilePath : Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, this.xmlFilePath);
                this.xmlFileName = Path.GetFileName(ConfigurationManager.AppSettings["ConnectionsFile"]);
                this.Config = Helpers.ObjectXMLSerializer<WDConfig>.Load(Path.Combine(this.xmlFilePath, this.xmlFileName));
                this.SiteName = siteName;
                this.databases = database;
                Bind();
                if (isBackUp && isChanged)
                    this.BackUp();
                this.Config = null;
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
        }
        private void BackUp()
        {

            try
            {
                
                File.Copy(Path.Combine(this.xmlFilePath, this.xmlFileName), Path.Combine(this.xmlFilePath, Path.GetFileNameWithoutExtension(this.xmlFileName) + "_" + DateTime.UtcNow.ToString("yyyyMMdd") + Path.GetExtension(this.xmlFileName)), true);
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(Path.Combine(this.xmlFilePath, this.xmlFileName));
                foreach (System.Xml.XmlNode instance in xmlDoc["DBCONFIG"].ChildNodes)
                {
                    if (instance.Attributes["NAME"].Value.ToUpper() == this.SiteName.ToUpper())
                    {
                        // removes version
                        System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                        settings.OmitXmlDeclaration = true;
                        System.Xml.Serialization.XmlSerializer xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(List<DB>));
                        StringWriter sw = new StringWriter();
                        using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sw, settings))
                        {
                            // removes namespace
                            var xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
                            xmlns.Add(string.Empty, string.Empty);
                            xsSubmit.Serialize(writer, this.xmlInstance.Dbs, xmlns);
                            instance.InnerXml = sw.ToString().Replace("<ArrayOfDB>", string.Empty).Replace("</ArrayOfDB>", string.Empty);
                        }
                        break;
                    }
                }
                xmlDoc.Save(Path.Combine(this.xmlFilePath, this.xmlFileName));
            }
            catch(Exception exc)
            {
                ILogger.Fatal(exc);
            }
        }
        private void Bind()
        {
            this.InventoryFlag = Config.InventoryFlag;
            this.TesterIdCheck = Config.TesterdIdCheck;
            this.ImportDrive = Config.ImportDrive;
            this.UserServerTxConfig = Config.UserServerTxConfig;
            this.UserServerConnectionType = Config.UserServerConnectionType;
            this.BXConfig = new List<WDDBConfig>();
            this.TXConfig = new List<WDDBConfig>();
            this.LoadFromXML(this.SiteName);
            this.GetActualConnection(Databases.BR);
            this.SetActualConnection(Databases.BR);
            this.QueryDatabaseStatus(Databases.BR);
            this.GetActualConnection(Databases.BR);
            this.GetActualConnection(Databases.TX);
            this.SetActualConnection(this.databases);

        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aConnect">.</param>
        /// <param name="bxConnections">.</param>
        /// <param name="txConnections">.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        private DbContext(Connect aConnect, List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections)
        {
            try
            {
                this.dbProvider = aConnect.DbProvider;
                switch (this.dbProvider)
                {
                    case DBProvider.Sql:
                        ICommands = AppConfiguration.LoadFactory<SqlFactory>(aConnect, bxConnections, txConnections);
                        break;
                    case DBProvider.Db2:
                        ICommands = AppConfiguration.LoadFactory<Db2Factory>(aConnect, bxConnections, txConnections);
                        break;
                    case DBProvider.Oracle:
                    case DBProvider.Oracle2:
                        ICommands = AppConfiguration.LoadFactory<OracleFactory>(aConnect, bxConnections, txConnections);
                        break;
                    case DBProvider.PostgreSQL:
                        ICommands = AppConfiguration.LoadFactory<PostgreSQLFactory>(this.connectionString, this.dbProvider);
                        break;
                    case DBProvider.MySql:
                        ICommands = AppConfiguration.LoadFactory<MySqlFactory>(this.connectionString, this.dbProvider);
                        break;
                    default:
                        ICommands = AppConfiguration.LoadFactory<TeraFactory>(this.connectionString, this.dbProvider);
                        break;
                }
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }

        }
        #endregion
        #region Dispose


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Finalizer.
        /// </summary>
        /// <remarks>NOTE: Leave out the finalizer altogether if this class doesn't own unmanaged resources
        /// itself, but leave the other methods
        /// exactly as they are.</remarks>


        ~DbContext()
        {
            Dispose(false);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to
        /// release only unmanaged resources.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  
                if (ICommands != null)
                {
                    ICommands.Dispose();
                }
            }
        }
        #endregion
        #region MITECS

        /// <summary>
        /// The database provider.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal int dbProvider;
        /// <summary>
        /// The connection string.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal string connectionString;


        /// <summary>
        /// Queries database status.
        /// </summary>
        /// <param name="database">.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        internal void QueryDatabaseStatus(int database)
        {
            try
            {

                string commandText = "SELECT TX1_FLAG, TX2_FLAG, BR1_FLAG, BR2_FLAG, BR1_DBType,BR2_DBType,TX1_DBType,TX2_DBType, TX1_SERVER, TX1_DBNAME, TX2_SERVER, TX2_DBNAME, BR1_SERVER, BR1_DBNAME, BR2_SERVER, BR2_DBNAME FROM LU_DATABASE_STATUS";
                DataTable dataTable = this.ICommands.ExecuteDataTable(commandText);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    if (this.BXConfig.Count > 0)
                    {

                        bool bx = ((
                                    this.BXConfig[0].ServerName.ToLower().Equals(dataRow["BR1_SERVER"].ToString().ToLower())
                                    &&
                                    this.BXConfig[0].DatabaseName.ToLower().Equals(dataRow["BR1_DBNAME"].ToString().ToLower())
                                    &&
                                   this.BXConfig[0].ActiveFlag.Equals(dataRow["BR1_FLAG"])
                                   )
                                   ||
                                   (
                                    this.BXConfig[1].ServerName.ToLower().Equals(dataRow["BR2_SERVER"].ToString().ToLower())
                                    &&
                                    this.BXConfig[1].DatabaseName.ToLower().Equals(dataRow["BR2_DBNAME"].ToString().ToLower())
                                    &&
                                   this.BXConfig[1].ActiveFlag.Equals(dataRow["BR2_FLAG"])
                                   )
                                   ) ? true : false;
                        if (!bx)
                        {

                            this.BXConfig.Clear();
                            this.BXConfig = new List<WDDBConfig>();
                            this.BXConfig.Add(new WDDBConfig(1, HelperUtility.ConvertTo<string>(dataRow["BR1_SERVER"], string.Empty),
                                                                HelperUtility.ConvertTo<string>(dataRow["BR1_DBNAME"], string.Empty),
                                                                (dataRow["BR1_FLAG"].ToString() == "1" ? true : false),
                                                              Helpers.HelperUtility.GetDbProvider(dataRow["BR1_DBType"].ToString())));
                            this.BXConfig.Add(new WDDBConfig(2, HelperUtility.ConvertTo<string>(dataRow["BR2_SERVER"], string.Empty),
                                                             HelperUtility.ConvertTo<string>(dataRow["BR2_DBNAME"], string.Empty), (
                                                            dataRow["BR2_FLAG"].ToString() == "1" ? true : false),
                                                             Helpers.HelperUtility.GetDbProvider(dataRow["BR2_DBType"].ToString())));
                            this.isChanged = true;
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br1").Server = dataRow["BR1_SERVER"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br1").DBName = dataRow["BR1_DBNAME"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br1").DBType = dataRow["BR1_DBType"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br1").ConnectType = dataRow["BR1_FLAG"].ToString() == "1" ? "ONLINE" : "NOTUSED";

                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br2").Server = dataRow["BR2_SERVER"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br2").DBName = dataRow["BR2_DBNAME"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br2").DBType = dataRow["BR2_DBType"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "br2").ConnectType = dataRow["BR2_FLAG"].ToString() == "1" ? "ONLINE" : "NOTUSED";
                        }
                    }
                    if (this.TXConfig.Count > 0)
                    {
                        bool tx = ((
                                    this.TXConfig[0].ServerName.ToLower().Equals(dataRow["TX1_SERVER"].ToString().ToLower())
                                    &&
                                    this.TXConfig[0].DatabaseName.ToLower().Equals(dataRow["TX1_DBNAME"].ToString().ToLower())
                                    &&
                                  this.TXConfig[0].ActiveFlag.Equals(dataRow["TX1_FLAG"])
                                   )
                                   ||
                                   (
                                    this.TXConfig[1].ServerName.ToLower().Equals(dataRow["TX2_SERVER"].ToString().ToLower())
                                    &&
                                    this.TXConfig[1].DatabaseName.ToLower().Equals(dataRow["TX2_DBNAME"].ToString().ToLower())
                                    &&
                                  this.TXConfig[1].ActiveFlag.Equals(dataRow["TX2_FLAG"])
                                   )
                                   ) ? true : false;
                        if (!tx)
                        {
                            this.TXConfig.Clear();
                            this.TXConfig = new List<WDDBConfig>();
                            this.TXConfig.Add(new WDDBConfig(1, HelperUtility.ConvertTo<string>(dataRow["TX1_SERVER"], string.Empty),
                                                                HelperUtility.ConvertTo<string>(dataRow["TX1_DBNAME"], string.Empty),
                                                                (dataRow["TX1_FLAG"].ToString() == "1" ? true : false),
                                                              Helpers.HelperUtility.GetDbProvider(dataRow["TX1_DBType"].ToString())));
                            this.TXConfig.Add(new WDDBConfig(2, HelperUtility.ConvertTo<string>(dataRow["TX2_SERVER"], string.Empty),
                                                             HelperUtility.ConvertTo<string>(dataRow["TX2_DBNAME"], string.Empty), (
                                                            dataRow["TX2_FLAG"].ToString() == "1" ? true : false),
                                                             Helpers.HelperUtility.GetDbProvider(dataRow["TX2_DBType"].ToString())));

                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx1").Server = dataRow["TX1_SERVER"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx1").DBName = dataRow["TX1_DBNAME"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx1").DBType = dataRow["TX1_DBType"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx1").ConnectType = dataRow["TX1_FLAG"].ToString() == "1" ? "ONLINE" : "NOTUSED";

                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx2").Server = dataRow["TX2_SERVER"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx2").DBName = dataRow["TX2_DBNAME"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx2").DBType = dataRow["TX2_DBType"].ToString();
                            this.xmlInstance.Dbs.Find(x => x.ID.ToLower() == "tx2").ConnectType = dataRow["TX2_FLAG"].ToString() == "1" ? "ONLINE" : "NOTUSED";
                            this.isChanged = true;
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }

        }


        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="dbSite">The database site.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        private void LoadFromXML(string dbSite)
        {
            string instanceId = string.Empty;
            this.SiteName = this.GetDbSite(dbSite, this.SiteName);
            this.InitializeDatabase();
            try
            {
                this.xmlInstance = Config.INSTANCES.Find(x => x.Name.ToLower() == this.SiteName.ToLower());
                if (this.xmlInstance != null)
                {
                    this.SiteLocation = this.xmlInstance.SiteLocation;
                    this.TimeLog = this.xmlInstance.TimeLog;
                    this.ReportingDbName = this.xmlInstance.TimeLog;
                    int index = 1;
                    foreach (DB db in this.xmlInstance.Dbs)
                    {
                        index = index > 2 ? 1 : index;
                        GetDatabaseConfig(db.ID).Add(new WDDBConfig(index, db.Server, db.DBName, db.ConnectType.ToUpper() == "ONLINE" ? true : false, HelperUtility.GetDbProvider(db.DBType)));
                        index++;
                    }
                }
            }
            catch
            {

                throw;
            }
        }


        /// <summary>
        /// Gets database site.
        /// </summary>
        /// <param name="dbSite">The database site.</param>
        /// <param name="defaultSite">The default site.</param>
        /// <returns>The database site.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        internal string GetDbSite(string dbSite, string defaultSite)
        {
            bool flag = dbSite.Length > 0;
            string result;
            if (flag)
            {
                result = dbSite;
            }
            else
            {
                result = defaultSite;
            }
            return result;
        }


        /// <summary>
        /// Opens file data.
        /// </summary>
        /// <returns>A DataSet.</returns>
        /// <exception cref="System.ApplicationException">OpenFile : Could not locate this file</exception>
        /// <exception cref="ApplicationException">OpenFile : Could not locate this file</exception>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        internal DataSet OpenFileData()
        {
            DataSet dataSet = new DataSet();
            bool flag = File.Exists(this.xmlFilePath);
            if (flag)
            {
                dataSet.ReadXml(this.xmlFilePath);
                SiteName = dataSet.Tables["DBCONFIG"].Rows[0]["DEFAULT_DB_SITE"].ToString();
                InventoryFlag = dataSet.Tables["DBCONFIG"].Rows[0]["INVENTORY_FLAG"].ToString();
                ImportDrive = dataSet.Tables["DBCONFIG"].Rows[0]["IMPORT_DRIVE"].ToString();
                TesterIdCheck = dataSet.Tables["DBCONFIG"].Rows[0]["TESTERID_CHECK"].ToString();
                return dataSet;
            }
            throw new ApplicationException("OpenFile : Could not locate this file ");
        }


        /// <summary>
        /// Gets database configuration.
        /// </summary>
        /// <param name="database">.</param>
        /// <returns>The database configuration.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        internal List<WDDBConfig> GetDatabaseConfig(string database)
        {
            return String.Compare(database.ToString().Substring(0, 2).ToUpper(), "BR", false) == 0 ? this.BXConfig : this.TXConfig;
        }


        /// <summary>
        /// Gets database configuration.
        /// </summary>
        /// <param name="database">.</param>
        /// <returns>The database configuration.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        internal List<WDDBConfig> GetDatabaseConfig(int database)
        {
            return database == Databases.BR ? this.BXConfig : this.TXConfig;
        }


        /// <summary>
        /// Initializes the database.
        /// </summary>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        internal void InitializeDatabase()
        {
            this.BXConfig.Clear();
            this.TXConfig.Clear();
        }


        /// <summary>
        /// Gets actual connection.
        /// </summary>
        /// <param name="database">.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>


        internal void GetActualConnection(int database)
        {
            try
            {
                using (List<WDDBConfig>.Enumerator enumerator = GetDatabaseConfig(database).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            Connections connections = new Connections(current.ServerName, current.DatabaseName, dummyId, dummyPasswd);
                            try
                            {
                                WD.DataAccess.Context.DbContext dbContext = new Context.DbContext(new Connect() { DbProvider = current.dbProvider, ConnectionString = connections.ConnectionString() });
                                DataTable dataTable = dbContext.ICommands.ExecuteDataTable("SELECT USER_ID, PASSWD FROM SECURITY_TBL");
                                if ((dataTable.Rows.Count == 0))
                                {
                                    current.Userid = dummyId;
                                    current.Password = dummyPasswd;
                                }
                                else
                                {
                                    current.Userid = HelperUtility.Decrypt(dataTable.Rows[0]["USER_ID"].ToString());
                                    current.Password = HelperUtility.Decrypt(dataTable.Rows[0]["PASSWD"].ToString());
                                }
                                dbContext = null;
                                this.connectionString = new Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString();
                                this.dbProvider = current.dbProvider;
                            }
                            catch
                            {
                                //throw;
                            }
                        }
                        else
                        {

                            current.Userid = string.Empty;
                            current.Password = string.Empty;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        internal void GetConnection(int database)
        {
            try
            {
                using (List<WDDBConfig>.Enumerator enumerator = GetDatabaseConfig(database).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WDDBConfig current = enumerator.Current;
                        Connections connections = new Connections(current.ServerName, current.DatabaseName, dummyId, dummyPasswd);
                        try
                        {
                            WD.DataAccess.Context.DbContext dbContext = new Context.DbContext(new Connect() { DbProvider = current.dbProvider, ConnectionString = connections.ConnectionString() });
                            DataTable dataTable = dbContext.ICommands.ExecuteDataTable("SELECT USER_ID, PASSWD FROM SECURITY_TBL");
                            if ((dataTable.Rows.Count == 0))
                            {
                                current.Userid = dummyId;
                                current.Password = dummyPasswd;
                            }
                            else
                            {
                                current.Userid = HelperUtility.Decrypt(dataTable.Rows[0]["USER_ID"].ToString());
                                current.Password = HelperUtility.Decrypt(dataTable.Rows[0]["PASSWD"].ToString());
                            }
                            dbContext = null;
                            this.connectionString = new Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString();
                            this.dbProvider = current.dbProvider;
                            break;
                        }
                        catch(Exception exc)
                        {
                            ILogger.Fatal(exc);
                            //throw;
                        }

                    }
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Sets a connection.
        /// </summary>
        /// <param name="database">.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>

        internal void SetActualConnection(int database)
        {
            try
            {
                using (List<WDDBConfig>.Enumerator enumerator = GetDatabaseConfig(database).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WDDBConfig current = enumerator.Current;
                        current.ErrorMessages = string.Empty;
                        if (current.ActiveFlag && (current.Userid != null) && (current.Userid.Trim().Length > 0))
                        {
                            try
                            {
                                Connections connection = (current.Userid.Trim().Length == 0) ? new Connections(current.ServerName, current.DatabaseName) : new Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                this.connectionString = connection.ConnectionString();
                                this.dbProvider = current.dbProvider;
                                this.ICommands = new DbContext(new Connect() { ConnectionString = this.connectionString, DbProvider = this.dbProvider }, this.BXConfig, this.TXConfig).ICommands;
                                break;
                            }
                            catch(Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion


    }
}
