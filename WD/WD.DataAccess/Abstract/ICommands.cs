// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="ICommands.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using WD.DataAccess.Abstract;
using WD.DataAccess.Configurations;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;
using WD.DataAccess.Mitecs;
using WD.DataAccess.Parameters;




// namespace: WD.DataAccess.Abstract
//
// <summary>
// As Web developers, our lives revolve around working with data. We create databases to store the data, code to retrieve and modify it, and web pages to collect and summarize it. There are some cases were we need to switch between different databases and changes application code to make code working with the database connectors.
// Sometime that task is too cumbersome as it increases load on developers for writing more line of codes for different database connectors.To avoid such scenarios and to make all applications loosely coupled WD came up with an idea of having one core repository (DataAccess Layer (DAL) which can communicate with any kind of database (Right now we support SQL,Oracle,Db2 and Tera data).
// Using DAL developers only need to create Business logic for their application and use DAL for database communication.
// Following tutorials will give an overlook about DAL and we further discuss different scenarios and implementation of DAL in our client applications.
// </summary>

///
namespace WD.DataAccess.Abstract
{
    
    /// <summary>
    /// Commands is main abstract class  which helps to communicate with databases (SQL,Oracle,Db2)
    /// 
    /// ICommands have few build in functions which are listed as below: ExecuteDataReader
    /// ExecuteDataSet ExecuteDataTable GetList GetEntity Insert Update Delete ExecuteNonQuery
    /// ExecuteRecordSet ExecuteScalar.
    /// </summary>
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public abstract class ICommands : IMTech
    {
        #region Property

        
        /// <summary>   Default Connection once DbContext gets initialized. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///     ICommands command=new DbContext().ICommands;
        ///     IDbConnection con =command.Connection;
        /// 
        /// }
        /// }
        /// </code>
        ///</example>
        /// <value> Returns initialized Connection. </value>
        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public override IDbConnection Connection
        {
            get { return this.connection; }
        }

        
        /// <summary>
        /// Default DBProvider once DbContext gets initialized.
        /// </summary>
        /// <example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///     ICommands command=new DbContext().ICommands;
        ///     int dbProvider =command.DBProvider;
        ///     ///  1 = Sql;
        ///     /// 2= DB2;
        ///     ///3 = Oracle ;Client required
        ///     /// 4= Oracle 2; Oracle managed access dll required
        ///     //5= TeraData
        /// }
        /// }
        /// </code>
        /// </example>
        /// <value>The database provider.</value>
        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public override int DBProvider
        {
            get { return this.dbProvider; }
        }

        
        /// <summary>
        /// Gets the bx connection.
        /// </summary>
        /// <value>The bx connection.</value>
        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal override List<WDDBConfig> BXConnection
        {
            get { return this.bxConnections; }

        }

        
        /// <summary>   Gets the tx connection. </summary>
        ///
        /// <value> The tx connection. </value>
        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal override List<WDDBConfig> TXConnection
        {
            get { return this.txConnections; }
        }

        #endregion
        #region Members
        /// <summary>   The bx connections. </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly List<WDDBConfig> bxConnections;
        /// <summary>   The tx connections. </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly List<WDDBConfig> txConnections;
        /// <summary>   Private connection String. </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly string connectionString;
        /// <summary>   Private Connection. </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly IDbConnection connection;
        /// <summary>
        /// Private DBProvider.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int dbProvider;
        #endregion
        #region Constructor

        
        /// <summary>   Initializes a new instance of the <see cref="ICommands" /> // class. </summary>
        /// <example>
        /// This sample shows how to initialize <see cref="ICommands" /> //.
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
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
       
        
        public ICommands() : base() { }
        
        /// <summary>   Initialized ICommands By DBProvider. </summary>
        /// <example>
        /// This sample shows how to initialize <see cref="ICommands(int)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext(WD.DataAccess.Enums.Sql).ICommands;
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>Default providers are maintains Connection Strings in DataAccess Layer.
        /// </remarks>
        /// <param name="dbProvider">   SQL,Oracle,Oracle2,Db2,TeraData. </param>
        

        public ICommands(int dbProvider)
        {
            this.dbProvider = dbProvider;
            this.connectionString = new AppConfiguration().GetConnectionString(dbProvider.ToString());
            this.connection = AppConfiguration.CreateConnection(dbProvider);
            this.connection.ConnectionString = this.connectionString;
        }

        
        /// <summary>   Initialized ICommands By DBProvider and ConnectionString. </summary>
        /// <example>
        /// This sample shows how to initialize <see cref="ICommands(int , string)" /> //.
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext(WD.DataAccess.Enums.Sql,"AppSettingKey").ICommands;
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
        /// <param name="dbProvider">       SQL,Oracle,Oracle2,Db2,TeraData. </param>
        /// <param name="connectionString"> Encrypted ConnectionString. </param>
        

        public ICommands(int dbProvider, string connectionString)
        {
            this.connectionString = connectionString;
            this.dbProvider = dbProvider;
            this.connection = AppConfiguration.CreateConnection(dbProvider);
            this.connection.ConnectionString = this.connectionString;
        }

        
        /// <summary>   Initialized ICommands By Connect Class. </summary>
        /// <example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Helpers.Connect aConnect=new WD.DataAccess.Helpers.Connect();
        /// aConnect.ConnectionName="testConnection";
        /// aConnect.DbProvider=WD.DataAccess.Enums.Sql;
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext(aConnect).ICommands;
        /// //or
        /// aConnect.ConnectionString="Data Source=XXXXX;User Id=XXX;Password=XXX";
        /// aConnect.DbProvider=WD.DataAccess.Enums.Sql;
        /// command =new WD.DataAccess.Context.DbContext(aConnect).ICommands;
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
        ///
        /// <param name="aConnect"> a connect. </param>
        

        public ICommands(Connect aConnect)
        {
            this.connectionString = new AppConfiguration().GetConnectionString(aConnect);
            this.dbProvider = aConnect.DbProvider;
            this.connection = AppConfiguration.CreateConnection(aConnect.DbProvider);
            this.connection.ConnectionString = this.connectionString;
        }

        
        /// <summary>   Initializes a new instance of the <see cref="ICommands" /> // class. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect">         a connect. </param>
        /// <param name="bxConnections">    The bx connections. </param>
        /// <param name="txConnections">    The tx connections. </param>
        

        public ICommands(Connect aConnect, List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections)
        {
            this.connectionString = aConnect.ConnectionString;
            this.dbProvider = aConnect.DbProvider;
            this.connection = AppConfiguration.CreateConnection(dbProvider);
            this.connection.ConnectionString = this.connectionString;
            this.bxConnections = bxConnections;
            this.txConnections = txConnections;
        }
        #endregion
        #region Connections

        
        /// <summary>   Creates instance of Open Connection. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateOpenConnection()){
        ///     //write code for the connection
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   The new open connection. </returns>
        

        public override IDbConnection CreateOpenConnection()
        {
            System.Data.Common.DbConnection connection = (System.Data.Common.DbConnection)CreateConnection();
            connection.Open();
            return connection;
        }

        
        /// <summary>   Creates the open connection. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///     //opens connection
        ///      dbContext.ICommands.CreateOpenConnection(con);
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connection">   The connection. </param>
        ///
        /// <returns>   IDbConnection. </returns>
        

        public override IDbConnection CreateOpenConnection(IDbConnection connection)
        {
            connection.Open();
            return connection;
        }

        
        /// <summary>   Closes current connection. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateOpenConnection()){
        ///     //closes default connection
        ///      dbContext.ICommands.CloseConnection();
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public override void CloseConnection()
        {

            if (connection != null)
            {
                connection.Close();
            }
        }

        
        /// <summary>   Creates  instance of Connection. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   The new connection. </returns>
        

        public override IDbConnection CreateConnection()
        {

            IDbConnection dbConnection = AppConfiguration.CreateConnection(this.dbProvider);
            dbConnection.ConnectionString = this.connectionString;
            return dbConnection;
        }

        
        /// <summary>   Creates  instance of  Connection for connection string passed. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection("DataSource=XXX;UserId=XXX;Password=XXX")){
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> . </param>
        ///
        /// <returns>   The new connection. </returns>
        

        public override IDbConnection CreateConnection(string connectionString)
        {

            IDbConnection dbConnection = AppConfiguration.CreateConnection(this.dbProvider);
            dbConnection.ConnectionString = connectionString;
            return dbConnection;
        }

        
        /// <summary>   Creates  instance of  Connection for connection string passed. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateOpenConnection("DataSource=XXX;UserId=XXX;Password=XXX")){
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> . </param>
        ///
        /// <returns>   The new open connection. </returns>
        

        public override IDbConnection CreateOpenConnection(string connectionString)
        {

            IDbConnection dbConnection = AppConfiguration.CreateConnection(this.dbProvider);
            dbConnection.ConnectionString = connectionString;
            dbConnection.Open();
            return dbConnection;
        }

        
        /// <summary>   Creates the adapter. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDataAdapter dbAdapter=dbContext.ICommands.CreateAdapter()){
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   IDataAdapter. </returns>
        

        public IDataAdapter CreateAdapter() {

            return AppConfiguration.CreateDataAdapter(this.dbProvider);
        }
        #endregion
        #region Dispose

        
        /// <summary>   Gets or sets a value indicating whether this instance is dispose. </summary>
        ///
        /// <value> <c>true</c> if this instance is dispose; otherwise, <c>false</c>. </value>
        

        public bool IsDispose { get; set; }

        
        /// <summary>   Disposes this instance. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
        /// <summary>   Releases unmanaged and - optionally - managed resources. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="disposing">    <c>true</c> to release both managed and unmanaged resources;
        ///                             <c>false</c> to release only unmanaged resources. </param>
        

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                //in future

            }
            IsDispose = disposing;
        }
        #endregion
    }
}
