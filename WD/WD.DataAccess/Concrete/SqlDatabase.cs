// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 06-05-2017
// ***********************************************************************
// <copyright file="SqlDatabase.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;
using WD.DataAccess.Abstract;
using WD.DataAccess.Parameters;



// namespace: WD.DataAccess.Concrete
//
// summary:	.


namespace WD.DataAccess.Concrete
{
    
    /// <summary>   A SQL database. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public sealed class SqlDatabase : ICommands, IDisposable
    {
       #region Constructor

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        

        public SqlDatabase(int  dbProvider) :
            base(dbProvider)
        {
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> . </param>
        /// <param name="dbProvider">       . </param>
        

        public SqlDatabase(string connectionString, int  dbProvider) :
            base(dbProvider, connectionString)
        {
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect"> . </param>
        

        public SqlDatabase(Connect aConnect) :
            base(aConnect)
        {

        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect">         . </param>
        /// <param name="bxConnections">    . </param>
        /// <param name="txConnections">    The transmit connections. </param>
        

        public SqlDatabase(Connect aConnect,
             List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections) :
            base(aConnect, bxConnections, txConnections)
        {

        }
        #endregion

        #region ExecuteNonQuery

        
        /// <summary>   Bulk insert. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dt">           . </param>
        /// <param name="tableName">    . </param>
        /// <param name="batchSize">    . </param>
        /// <param name="timeOut">      . </param>
        /// <param name="columnNames">  . </param>
        /// <param name="connection">   . </param>
        /// <param name="transaction">  . </param>
        ///
        /// <returns>   An int. </returns>
        

        protected override int BulkInsert(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDbConnection connection, IDbTransaction transaction)
        {
            int result = 0;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (SqlBulkCopy bCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.KeepIdentity, (SqlTransaction)transaction))
                {
                    bCopy.DestinationTableName = tableName;
                    bCopy.BatchSize = batchSize;
                    foreach (var column in columnNames)
                    {
                        SqlBulkCopyColumnMapping bulkColumnMap = new SqlBulkCopyColumnMapping();
                        bulkColumnMap.SourceColumn = column.Key;
                        bulkColumnMap.DestinationColumn = column.Value;
                        bCopy.ColumnMappings.Add(bulkColumnMap);
                    }
                    bCopy.BulkCopyTimeout = timeOut;
                    bCopy.WriteToServer(dt);
                    result = dt.Rows.Count;
                    WD.DataAccess.Logger.ILogger.Info("Bulk Copy---" + tableName);
                }
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
            return result;
        }
        #endregion
     
    }
   
}
