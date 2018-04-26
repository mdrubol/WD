// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-20-2017
// ***********************************************************************
// <copyright file="AppConfiguration.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using WD.DataAccess.Abstract;
using WD.DataAccess.Enums;
using WD.DataAccess.Factory;
using WD.DataAccess.Helpers;



// namespace: WD.DataAccess.Configurations
//
// summary:	.


namespace WD.DataAccess.Configurations
{
    
    /// <summary>   An application configuration. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public sealed  class AppConfiguration
    {

        
        /// <summary>   Creates data adapter. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   The new data adapter. </returns>
        internal static IDataAdapter CreateDataAdapter(int dbProvider)
        {
            IDataAdapter iDataAdapter;
            switch (dbProvider)
            {
                case DBProvider.Sql:
                    iDataAdapter = new SqlDataAdapter();
                    break;
                case DBProvider.Db2:
                case DBProvider.Oracle:
                case DBProvider.Oracle2:
                case DBProvider.PostgreSQL:
                case DBProvider.MySql:
                    iDataAdapter = GetFactory(dbProvider).CreateDataAdapter();
                    break;
                default:
                    iDataAdapter = new OleDbDataAdapter();
                    break;
            }
            return iDataAdapter;
        }
        /// <summary>   Gets connection string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionName">   . </param>
        ///
        /// <returns>   The connection string. </returns>
        

        internal string GetConnectionString(string connectionName)
        {
            string connectionValue = GetAppSetting(connectionName);
            if (string.IsNullOrEmpty(connectionValue))
            {
                
                return WebSecurityUtility.Decrypt(DataAccess.Properties.DataAccess.Default[connectionName.ToLower()].ToString(), true);
            }
            else
            {
                if (DataAccess.Properties.DataAccess.Default.SettingsKey.Equals(connectionValue))
                    return WebSecurityUtility.Decrypt(DataAccess.Properties.DataAccess.Default[connectionValue.ToLower()].ToString(), true);
                else
                    return Decrypt(connectionValue);
            }
        }

        
        /// <summary>   Gets connection string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect"> . </param>
        ///
        /// <returns>   The connection string. </returns>
        

        internal string GetConnectionString(Connect aConnect)
        {
            return string.IsNullOrEmpty(aConnect.ConnectionName) ? (String.IsNullOrEmpty(aConnect.ConnectionString) ? GetConnectionString(HelperUtility.GetDbProviderName(aConnect.DbProvider)) : aConnect.ConnectionString) : ConfigurationManager.ConnectionStrings[aConnect.ConnectionName].ToString();
        }

        
        /// <summary>   Gets application setting. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="appKey">   . </param>
        ///
        /// <returns>   The application setting. </returns>
        

        internal static string GetAppSetting(string appKey)
        {
            return ConfigurationManager.AppSettings[appKey];
        }

        
        /// <summary>   Decrypts. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="text"> . </param>
        ///
        /// <returns>   A string. </returns>
        

        internal static string Decrypt(string text)
        {
            string result = string.Empty;
            try
            {
               result = WebSecurityUtility.Decrypt(text, true);
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        
        /// <summary>   Gets database provider. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionName">   . </param>
        ///
        /// <returns>   The database provider. </returns>
        

        internal static int GetDbProvider(string connectionName)
        {
            int dbProvider = WD.DataAccess.Enums.DBProvider.Sql;
            switch (ConfigurationManager.ConnectionStrings[connectionName].ProviderName.ToLower())
           {
               case "system.data.sqlclient":
                   dbProvider = WD.DataAccess.Enums.DBProvider.Sql;
                   break;
                case "ibm.data.db2":
                   dbProvider = WD.DataAccess.Enums.DBProvider.Db2;
                    break;
                case "system.data.oracleclient":
                    dbProvider = WD.DataAccess.Enums.DBProvider.Oracle;
                    break;
                case "oracle.manageddataaccess.client":
                    dbProvider = WD.DataAccess.Enums.DBProvider.Oracle2;
                    break;
                case "npgsql":
                    dbProvider = WD.DataAccess.Enums.DBProvider.PostgreSQL;
                    break;
                case "mysql.data.client":
                    dbProvider = WD.DataAccess.Enums.DBProvider.MySql;
                    break;
                default:
                    dbProvider = WD.DataAccess.Enums.DBProvider.Access;
                    break;
           }
           return dbProvider;
        
        }

        
        /// <summary>   Gets default connection string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   The default connection string. </returns>
        

        internal static string GetDefaultConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        
        /// <summary>   Gets default connection string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionName">   . </param>
        ///
        /// <returns>   The default connection string. </returns>
        

        internal static string GetDefaultConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }



        /// <summary>   Creates a connection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   The new connection. </returns>


        public static IDbConnection CreateConnection(int dbProvider)
        {
            
            IDbConnection dbConnection;
            switch (dbProvider)
            {
                case DBProvider.Sql:
                    dbConnection = new SqlConnection();
                    break;
                case DBProvider.Db2:
                case DBProvider.Oracle:
                case DBProvider.Oracle2:
                case DBProvider.PostgreSQL:
                case DBProvider.MySql:
                    dbConnection = GetFactory(dbProvider).CreateConnection();
                    break;
                default:
                    dbConnection = new OleDbConnection();
                    break;
            }
            return dbConnection;
        }


        /// <summary>   Creates a connection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="providerName"> . </param>
        ///
        /// <returns>   The new connection. </returns>


        internal static IDbConnection CreateConnection(string providerName)
        {
            return DbProviderFactories.GetFactory(providerName).CreateConnection();
        }


        /// <summary>   Creates a command. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   The new command. </returns>

        private static DbProviderFactory GetFactory(int dbProvider)
        {
            DbProviderFactory _factory = null;
            switch (dbProvider)
            {
                case DBProvider.Sql:
                    _factory = null;
                    break;
                case DBProvider.Db2:
                    _factory = DbProviderFactories.GetFactory("IBM.Data.DB2");
                    break;
                case DBProvider.Oracle:
                    _factory = DbProviderFactories.GetFactory("System.Data.OracleClient");
                    break;
                case DBProvider.Oracle2:
                    _factory = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");
                    break;
                case DBProvider.PostgreSQL:
                    _factory = DbProviderFactories.GetFactory("Npgsql");
                    break;
                case DBProvider.MySql:
                    _factory = DbProviderFactories.GetFactory("mysql.data.client");
                    break;
                default:
                    _factory = null;
                    break;
            }
            return _factory;
        }
        internal static IDbCommand CreateCommand(int dbProvider)
        {
            IDbCommand dbCommand;
            switch (dbProvider)
            {
                case DBProvider.Sql:
                    dbCommand = new SqlCommand();
                    break;
                case DBProvider.Db2:
                case DBProvider.Oracle:
                case DBProvider.Oracle2:
                case DBProvider.PostgreSQL:
                case DBProvider.MySql:
                    dbCommand = GetFactory(dbProvider).CreateCommand();
                    break;
                default:
                    dbCommand = new OleDbCommand();
                    break;
            }
            return dbCommand;
        }

        /// <summary>   Gets the t. </summary>
        ///
        /// <value> The t. </value>
        ///
        /// ### <typeparam name="T">    . </typeparam>
        ///
        /// ### <param name="aConnect">         . </param>
        /// ### <param name="bxConnections">    . </param>
        /// ### <param name="txConnections">    . </param>


        internal static ICommands LoadFactory<T>(WD.DataAccess.Helpers.Connect aConnect,
          List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections)
          where T : FactoryBase, new()
        {
            var factory = new T();
            var data = factory.GetDataLayer(aConnect, bxConnections, txConnections);
            return data;
        }

        /// <summary>   Gets the t. </summary>
        ///
        /// <value> The t. </value>
        ///
        /// ### <typeparam name="T">    . </typeparam>
        ///
        /// ### <param name="connectionString"> . </param>
        /// ### <param name="dbProvider">       . </param>


        internal static ICommands LoadFactory<T>(string connectionString, int dbProvider) where T : FactoryBase, new()
        {
            var factory = new T();
            var data = factory.GetDataLayer(connectionString, dbProvider);
            return data;
        }

    }
}
