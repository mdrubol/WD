// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 06-05-2017
// ***********************************************************************
// <copyright file="OracleDatabase.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using WD.DataAccess.Abstract;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using WD.DataAccess.Parameters;
using System.Data.Common;
using WD.DataAccess.Mitecs;
using WD.DataAccess.Logger;

using System.Reflection;


// namespace: WD.DataAccess.Concrete
//
// summary:	.


namespace WD.DataAccess.Concrete
{
    
    /// <summary>   An oracle database. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public sealed class OracleDatabase : ICommands, IDisposable
    {
          #region Constructor

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        

        public OracleDatabase(int  dbProvider) :
            base(dbProvider)
        {
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="dbProvider">       . </param>
        

        public OracleDatabase(string connectionString, int  dbProvider) :
            base(dbProvider, connectionString)
        {
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect"> . </param>
        

        public OracleDatabase(Connect aConnect) :
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
        

        public OracleDatabase(Connect aConnect,
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
                List<String> columnList = new List<string>();
                foreach (var col in columnNames)
                {
                    columnList.Add(col.Value);
                }
                string theSql = string.Format("INSERT INTO {0}{1} ({2}) ",
                            string.Empty,
                            tableName,
                            string.Join(",", columnList.ToArray())
                            );
                PagingInfo page = new PagingInfo(0, batchSize, dt.Rows.Count);
                int StartOfPage = 0;
                int EndOfPage = page.TotalItemCount >= page.PageSize ? page.PageSize - 1 : page.TotalItemCount - 1;
                int remainingItems = page.TotalItemCount;
                for (page.PageIndex = 0; page.PageIndex < page.TotalPageCount; page.PageIndex++)
                {

                    string commandText = "BEGIN ";
                    List<DBParameter> aParams = new List<DBParameter>();
                    for (int count = StartOfPage; count <= EndOfPage; count++)
                    {
                        List<string> valueList = new List<string>();
                        foreach (var col in columnNames)
                        {
                            DBParameter aParam = new DBParameter(col.Value + "_" + count.ToString(), dt.Rows[count][col.Key]);
                            valueList.Add(HelperUtility.Prefix(DBProvider) + aParam.ParameterName);
                            aParams.Add(aParam);
                        }
                        commandText += theSql + string.Format("VALUES ({0});", string.Join(",", valueList.ToArray()));
                        remainingItems--;
                    }
                    commandText += "END; ";
                    result += ExecuteNonQuery(commandText, CommandType.Text, connection, transaction, aParams.ToArray());
                    StartOfPage = EndOfPage + 1;
                    EndOfPage = EndOfPage + (remainingItems >= page.PageSize ? page.PageSize - 1 : remainingItems - 1);
                }
                WD.DataAccess.Logger.ILogger.Info("Bulk Copy---" + tableName);
            }
            catch(Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }

            return result;
        }


        /// <summary>   Bulk Update. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection connection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction transaction=con.BeginTransaction())
        ///   {  
        ///   Try
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select Id,FirstName,LastName from employee",connection);
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;();
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///   int rowsEffected =dbContext.ICommands.BulkUpdate(dt,"Employee",100,30,columnNames,primaryColumns,connection,transaction);//
        ///   transaction.Commit();
        ///   }
        ///  catch
        ///  {
        ///     trans.Rollback();
        ///  }
        /// }  
        /// } 
        /// }  
        /// }  
        /// </code>
        ///</example> 
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param>
        /// <param name="columnNames"> Represents the Set options for records to be updated. </param>
        /// <param name="primaryColumns"> Represents the Where clause filters. </param>
        /// <param name="connection"> Active Connection Object . </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public override int BulkUpdate(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns, IDbConnection connection, IDbTransaction transaction)
        {

            int result = 0;
            try
            {
                string theSql = string.Format("UPDATE {0} SET ", tableName);

                PagingInfo page = new PagingInfo(0, batchSize, dt.Rows.Count);
                int StartOfPage = 0;
                int EndOfPage = page.TotalItemCount >= page.PageSize ? page.PageSize - 1 : page.TotalItemCount - 1;
                int remainingItems = page.TotalItemCount;
                for (page.PageIndex = 0; page.PageIndex < page.TotalPageCount; page.PageIndex++)
                {
                    string commandText = "BEGIN ";
                    List<DBParameter> aParams = new List<DBParameter>();
                    for (int count = StartOfPage; count <= EndOfPage; count++)
                    {
                        List<string> valueList = new List<string>();
                        foreach (var col in columnNames)
                        {
                            DBParameter aParam = new DBParameter(col.Value + "_" + count.ToString(), dt.Rows[count][col.Key]);
                            valueList.Add(string.Format(" {0} = {1}", col.Value, HelperUtility.Prefix(DBProvider) + aParam.ParameterName));
                            aParams.Add(aParam);
                        }

                        List<string> WhereList = new List<string>();
                        foreach (var col in primaryColumns)
                        {
                            DBParameter aParam = new DBParameter(col.Value + "_" + count.ToString(), dt.Rows[count][col.Key]);
                            WhereList.Add(string.Format(" {0} = {1}", col.Value, HelperUtility.Prefix(DBProvider) + aParam.ParameterName));

                            if (aParams.FindIndex(item => item.ParameterName == aParam.ParameterName) < 0)
                            {
                                aParams.Add(aParam);
                            }
                        }

                        commandText += theSql + string.Format("{0}  WHERE {1};", string.Join(",", valueList.ToArray()), string.Join(" AND ", WhereList.ToArray()));
                        remainingItems--;
                    }
                    commandText += "END; ";
                    result += ExecuteNonQuery(commandText, CommandType.Text, connection, transaction, aParams.ToArray());
                    StartOfPage = EndOfPage + 1;
                    EndOfPage = EndOfPage + (remainingItems >= page.PageSize ? page.PageSize - 1 : remainingItems - 1);
                }
                WD.DataAccess.Logger.ILogger.Info("Bulk Copy---" + tableName);
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
            return result;
        }

        /// <summary>   Bulk Delete. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection connection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction transaction=con.BeginTransaction())
        ///   {  
        ///   Try
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select Id from employee",connection);
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///   int rowsEffected =dbContext.ICommands.BulkDelete(dt,"Employee",100,30,primaryColumns,connection,transaction);//
        ///   transaction.Commit();
        ///   }
        ///  catch
        ///  {
        ///     trans.Rollback();
        ///  }
        /// }  
        /// } 
        /// }  
        /// }  
        /// </code>
        ///</example> 
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="primaryColumns"> Represents the Where clause filters. </param>
        /// <param name="connection"> Active Connection Object . </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public override int BulkDelete(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns, IDbConnection connection, IDbTransaction transaction)
        {

            int result = 0;
            try
            {
                string theSql = string.Format("DELETE FROM {0}", tableName);

                PagingInfo page = new PagingInfo(0, batchSize, dt.Rows.Count);
                int StartOfPage = 0;
                int EndOfPage = page.TotalItemCount >= page.PageSize ? page.PageSize - 1 : page.TotalItemCount - 1;
                int remainingItems = page.TotalItemCount;
                for (page.PageIndex = 0; page.PageIndex < page.TotalPageCount; page.PageIndex++)
                {

                    string commandText = "BEGIN ";
                    List<DBParameter> aParams = new List<DBParameter>();
                    for (int count = StartOfPage; count <= EndOfPage; count++)
                    {

                        List<string> WhereList = new List<string>();
                        foreach (var col in primaryColumns)
                        {
                            DBParameter aParam = new DBParameter(col.Value + "_" + count.ToString(), dt.Rows[count][col.Key]);
                            WhereList.Add(string.Format(" {0} = {1}", col.Value, HelperUtility.Prefix(DBProvider) + aParam.ParameterName));
                            aParams.Add(aParam);
                        }

                        commandText += theSql + string.Format(" WHERE {0};", string.Join(" AND ", WhereList.ToArray()));
                        remainingItems--;
                    }
                    commandText += "END; ";
                    result += ExecuteNonQuery(commandText, CommandType.Text, connection, transaction, aParams.ToArray());
                    StartOfPage = EndOfPage + 1;
                    EndOfPage = EndOfPage + (remainingItems >= page.PageSize ? page.PageSize - 1 : remainingItems - 1);
                }
                WD.DataAccess.Logger.ILogger.Info("Bulk Copy---" + tableName);
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
