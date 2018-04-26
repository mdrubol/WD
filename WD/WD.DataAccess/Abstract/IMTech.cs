// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 03-21-2017
//
// Last Modified By : shahid_k, Asim Naeem 
// Last Modified On : 28-07-2017
// ***********************************************************************
// <copyright file="IMTech.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;
using WD.DataAccess.Parameters;
using WD.DataAccess.QueryProviders;



// namespace: WD.DataAccess.Abstract
//
// <summary>
// As Web developers, our lives revolve around working with data. We create databases to store the data, code to retrieve and modify it, and web pages to collect and summarize it. There are some cases were we need to switch between different databases and changes application code to make code working with the database connectors.
// Sometime that task is too cumbersome as it increases load on developers for writing more line of codes for different database connectors.To avoid such scenarios and to make all applications loosely coupled WD came up with an idea of having one core repository (DataAccess Layer (DAL) which can communicate with any kind of database (Right now we support SQL,Oracle,Db2 and Tera data).
// Using DAL developers only need to create Business logic for their application and use DAL for database communication.
// Following tutorials will give an overlook about DAL and we further discuss different scenarios and implementation of DAL in our client applications.
// </summary>


namespace WD.DataAccess.Abstract
{

    /// <summary>
    /// IMTech  is an abstract class for MITECH Solutions for
    /// ExecuteNonQuery,ExecuteScalar,
    /// ExecuteDataSet,ExecuteDataTable,
    /// ExecuteDataReader,ExecuteEntity,ExecuteRecordSet
    /// </summary>
    /// <seealso cref="WD.DataAccess.Abstract.IExecuteScalar"/>
    /// <remarks>Shahid K, 7/24/2017.</remarks>


    public abstract class IMTech : IExecuteScalar
    {
        private string arMessage = "Database flag inactive";
        #region Constructor


        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>


        public IMTech() : base() { }
        #endregion
        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        void DetermineIndex(ref int index)
        {
            index = index > 2 ? 1 : index < 1 ? 1 : index;
        }

        /// <summary>   Gets a transaction. </summary>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        /// <param name="isTransaction">        [in,out] True if this object is transaction. </param>
        /// <param name="isTransactionScope">   [in,out] True if this object is transaction scope. </param>
        /// <param name="transaction">          (Optional) </param>
        internal void GetTransaction(ref bool isTransaction, ref bool isTransactionScope, int transaction = Transaction.None)
        {
            switch (transaction)
            {
                case Transaction.Yes: isTransactionScope = false; isTransaction = true; break;
                case Transaction.TS: isTransactionScope = true; isTransaction = false; break;
                case Transaction.TSNT: isTransactionScope = true; isTransaction = true; break;
                default: isTransaction = false; isTransactionScope = false; break;
            }

        }
        /// <summary>   Executes the non query operation. </summary>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        /// <param name="ex">               [in,out] Details of the exception. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="commandText">      . </param>
        /// <param name="commandType">      CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">          . </param>
        /// <param name="isTransaction">    (Optional) True if this object is transaction. </param>
        /// <returns>   No Of Rows Affected. </returns>
        internal int ExecuteNonQuery(ref Exception ex, WD.DataAccess.Mitecs.Connections connection, string commandText, CommandType commandType, DBParameter[] aParams, bool isTransaction = false)
        {
            int rowAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateOpenConnection(connection.ConnectionString()))
                {
                    if (isTransaction)
                    {
                        using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                rowAffected = ExecuteNonQuery(commandText, CommandType.Text, dbConnection, dbTransaction, aParams);
                                dbTransaction.Commit();
                            }
                            catch (Exception exc)
                            {
                                dbTransaction.Rollback();
                                ex = exc;
                            }
                        }

                    }
                    else
                    {
                        rowAffected = ExecuteNonQuery(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);
                    }
                }
            }
            catch (Exception exc)
            {
                ex = exc;
            }
            return rowAffected;
        }


        /// <summary>   Bulk insert. </summary>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        /// <param name="ex">               [in,out] Details of the exception. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="dt">               . </param>
        /// <param name="tableName">        . </param>
        /// <param name="batchSize">        . </param>
        /// <param name="timeOut">          . </param>
        /// <param name="columnNames">      . </param>
        /// <param name="isTransaction">    (Optional) True if this object is transaction. </param>
        /// <returns>   An int. </returns>
        internal int BulkInsert(ref Exception ex, WD.DataAccess.Mitecs.Connections connection, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, bool isTransaction = false)
        {
            int rowAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateOpenConnection(connection.ConnectionString()))
                {
                    if (isTransaction)
                    {
                        using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                rowAffected = BulkInsert(dt, tableName, batchSize, timeOut, columnNames, dbConnection, dbTransaction);
                                dbTransaction.Commit();
                            }
                            catch (Exception exc)
                            {
                                dbTransaction.Rollback();
                                ex = exc;
                            }
                        }

                    }
                    else
                    {
                        rowAffected = BulkInsert(dt, tableName, batchSize, timeOut, columnNames, dbConnection);
                    }
                }
            }
            catch (Exception exc)
            {
                ex = exc;
            }
            return rowAffected;
        }
        
         /// <summary>   Bulk Update. </summary>      
        /// <param name="ex">               [in,out] Details of the exception. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="dt">               . </param>
        /// <param name="tableName">        . </param>
        /// <param name="batchSize">        . </param>
        /// <param name="timeOut">          . </param>
        /// <param name="columnNames">      . </param>
        /// <param name="primaryColumns">      . </param>
        /// <param name="isTransaction">    (Optional) True if this object is transaction. </param>
        /// <returns>   An int. </returns>
        internal int BulkUpdate(ref Exception ex, WD.DataAccess.Mitecs.Connections connection, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns, bool isTransaction = false)
        {
            int rowAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateOpenConnection(connection.ConnectionString()))
                {
                    if (isTransaction)
                    {
                        using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                rowAffected = BulkUpdate(dt, tableName, batchSize, timeOut, columnNames,primaryColumns,dbConnection, dbTransaction);
                                dbTransaction.Commit();
                            }
                            catch (Exception exc)
                            {
                                dbTransaction.Rollback();
                                ex = exc;
                            }
                        }

                    }
                    else
                    {
                        rowAffected = BulkUpdate(dt, tableName, batchSize, timeOut, columnNames,primaryColumns, dbConnection);
                    }
                }
            }
            catch (Exception exc)
            {
                ex = exc;
            }
            return rowAffected;
        }

        /// <summary>   Bulk delete. </summary>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        /// <param name="ex">               [in,out] Details of the exception. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="dt">               . </param>
        /// <param name="tableName">        . </param>
        /// <param name="batchSize">        . </param>
        /// <param name="timeOut">          . </param>
        /// <param name="primaryColumns">      . </param>
        /// <param name="isTransaction">    (Optional) True if this object is transaction. </param>
        /// <returns>   An int. </returns>
        internal int BulkDelete(ref Exception ex, WD.DataAccess.Mitecs.Connections connection, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns, bool isTransaction = false)
        {
            int rowAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateOpenConnection(connection.ConnectionString()))
                {
                    if (isTransaction)
                    {
                        using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                rowAffected = BulkDelete(dt, tableName, batchSize, timeOut, primaryColumns, dbConnection, dbTransaction);
                                dbTransaction.Commit();
                            }
                            catch (Exception exc)
                            {
                                dbTransaction.Rollback();
                                ex = exc;
                            }
                        }

                    }
                    else
                    {
                        rowAffected = BulkDelete(dt, tableName, batchSize, timeOut, primaryColumns, dbConnection);
                    }
                }
            }
            catch (Exception exc)
            {
                ex = exc;
            }
            return rowAffected;
        }
       
        #endregion
        #region ExecuteNonQuery
        /// <summary>   Returns Rows Affected for CommandText</summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')");
        ///
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database, string commandText)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection connection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += ExecuteNonQuery(commandText, CommandType.Text, connection);
                                }
                            }
                            catch (Exception exc) {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead) {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Returns Rows Affected for CommandText and Parameter Collection. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///   int rowsEffected=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database, string commandText,DBParameter[] aParams)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection connection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += ExecuteNonQuery(commandText, CommandType.Text, connection, aParams);
                                }
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Returns Rows Affected for CommandText</summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')");
        ///
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database,int index, string commandText)
        {
            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && current.Index==index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection connection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += ExecuteNonQuery(commandText, CommandType.Text, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                                }
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Returns Rows Affected for CommandText and Parameter Collection. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///   int rowsEffected=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database,int index, string commandText, DBParameter[] aParams)
        {
            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && current.Index==index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection connection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += ExecuteNonQuery(commandText, CommandType.Text, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()), aParams);
                                }
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Executes the non query operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')",CommandType.Text);
        ///   
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>

        public virtual int ExecuteNonQuery(int database, string commandText,CommandType commandType)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += ExecuteNonQuery(commandText, commandType, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Executes the non query operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///   int rowsEffected=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database, string commandText, CommandType commandType, DBParameter[] aParams)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += ExecuteNonQuery(commandText, commandType, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()), aParams);
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Executes the non query operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')",CommandType.Text);
        ///   
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database, int index, string commandText,CommandType commandType)
        {
            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && current.Index == index)
                        {
                            try
                            {
                                rowAffected += ExecuteNonQuery(commandText, commandType, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Executes the non query operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///   int rowsEffected=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,"INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database, int index, string commandText, CommandType commandType, DBParameter[] aParams)
        {
            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && current.Index == index)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += ExecuteNonQuery(commandText, commandType, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()), aParams);
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Executes the non query operation. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        /// <param name="transaction">  (Optional) </param>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// string theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES('12121')";
        /// 
        /// 
        /// //No Transaction while inserting record or set of records
        /// int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,null,WD.DataAccess.Enums.Transaction.None);
        /// or
        /// use Transaction while inserting record or set of records
        ///  returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,null,WD.DataAccess.Enums.Transaction.Yes);
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,null,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,null,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        ///   //QUERY WITH PARAMETER
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES(WD.Helpers.HelperUtility.Prefix(dbContext.ICommands.DbProvider)+'aColName')";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams= new WD.DataAccess.Parameters.DBParameter[1]
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="aColName";
        ///    aParams[0].ParameterValue="abcd";
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,aParams,WD.DataAccess.Enums.Transaction.Yes);
        /// }
        /// }
        /// </code>
        ///</example>
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
        public virtual int ExecuteNonQuery(int database, string commandText, DBParameter[] aParams , int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>
        ///  Executes the non query operation.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// 
        ///   //QUERY WITH PARAMETER
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES(@aColName)";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams= new WD.DataAccess.Parameters.DBParameter[1]
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="aColName";
        ///    aParams[0].ParameterValue="abcd";
        ///   int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,aParams,WD.DataAccess.Enums.Transaction.Yes);
        ///   
        /// //No Transaction while inserting record or set of records
        ///    returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,aParams,WD.DataAccess.Enums.Transaction.None);
        ///
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,aParams,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,aParams,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        /// 
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database, int index, string commandText, DBParameter[] aParams , int transaction = Transaction.None)
        {

            int rowAffected = 0;
            DetermineIndex(ref index);
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>
        ///  Executes the non query operation.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// 
        ///   //QUERY WITH PARAMETER
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES(@aColName)";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams= new WD.DataAccess.Parameters.DBParameter[1]
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="aColName";
        ///    aParams[0].ParameterValue="abcd";
        ///   int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.Yes);
        ///   
        /// //No Transaction while inserting record or set of records
        ///    returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.None);
        ///
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        /// 
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database, int index, string commandText, CommandType commandType, DBParameter[] aParams, int transaction = Transaction.None)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();

                        }

                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                            }
                        }

                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return rowAffected;

        }
        /// <summary>
        ///  Executes the non query operation.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// 
        ///   //QUERY WITH PARAMETER
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES(@aColName)";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams= new WD.DataAccess.Parameters.DBParameter[1]
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="aColName";
        ///    aParams[0].ParameterValue="abcd";
        ///   int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.Yes);
        ///   
        /// //No Transaction while inserting record or set of records
        ///    returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.None);
        ///
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,aParams,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        /// 
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        /// <param name="transaction">  (Optional) </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int ExecuteNonQuery(int database, string commandText, CommandType commandType,  DBParameter[] aParams,  int transaction = Transaction.None)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();

                        }

                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, aParams, isTransaction);
                            }
                        }

                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return rowAffected;

        }


        /// <summary>   Executes the non query operation. </summary>
        ///
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="transaction">  (Optional) </param>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// string theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES('12121')";
        /// 
        /// 
        /// //No Transaction while inserting record or set of records
        /// int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,WD.DataAccess.Enums.Transaction.None);
        /// or
        /// use Transaction while inserting record or set of records
        ///  returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,WD.DataAccess.Enums.Transaction.Yes);
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR,theSql,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        ///   
        /// }
        /// }
        /// </code>
        ///</example>
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
        public virtual int ExecuteNonQuery(int database, string commandText,  int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>
        ///  Executes the non query operation.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// 
        ///   
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES(@aColName)";
        ///    
        ///   int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,WD.DataAccess.Enums.Transaction.Yes);
        ///   
        /// //No Transaction while inserting record or set of records
        ///    returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,WD.DataAccess.Enums.Transaction.None);
        ///
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        /// 
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(int database,int index, string commandText,  int transaction = Transaction.None)
        {

            int rowAffected = 0;
            DetermineIndex(ref index);
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>
        ///  Executes the non query operation.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// 
        ///   
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES('abc')";
        ///   int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.Yes);
        ///   
        /// //No Transaction while inserting record or set of records
        ///    returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.None);
        ///
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1,theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        /// 
        /// }
        /// }
        /// </code>
        ///</example>
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
       /// <param name="database"></param>
       /// <param name="index"></param>
       /// <param name="commandText"></param>
       /// <param name="commandType"></param>
       /// <param name="transaction"></param>
       /// <returns></returns>
        public virtual int ExecuteNonQuery(int database,int index, string commandText, CommandType commandType,  int transaction = Transaction.None)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                            }
                        }

                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return rowAffected;

        }
        /// <summary>
        ///  Executes the non query operation.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        /// //OPEN QUERY
        /// 
        ///   
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES('abc')";
        ///   int returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.Yes);
        ///   
        /// //No Transaction while inserting record or set of records
        ///    returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.None);
        ///
        ///  or
        ///  use Transaction scope while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.TS);
        ///     or
        ///     No Transaction scope and database connection transaction while inserting record or set of records
        ///   returnRows=dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, theSql, CommandType.Text,WD.DataAccess.Enums.Transaction.TSNT);
        ///   
        /// 
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="transaction">  (Optional) Value of WD.DataAccess.Enums.Transaction </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int ExecuteNonQuery(int database, string commandText, CommandType commandType, int transaction = Transaction.None)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();

                        }

                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += ExecuteNonQuery(ref ex, connection, commandText, CommandType.Text, null, isTransaction);
                            }
                        }

                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return rowAffected;

        }

        /// <summary>   Bulk insert. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select FirstName from employee",,WD.DataAccess.Transactions.None);
        ///   Dictionary&lt;string,string&gt; cols=new Dictionary&lt;string,string&gt;();
        ///   <!--cols.Add("FirstName","FirstName");-->
        ///  int rowsEffected =dbContext.ICommands.BulkInsert(WD.DataAccess.Databases.BR, 1,dt,"Employee",100,30,cols);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <param name="database">     . </param>
        /// <param name="index" >       . </param>
        /// <param name="dt">           . </param>
        /// <param name="tableName">    . </param>
        /// <param name="batchSize">    . </param>
        /// <param name="timeOut">      . </param>
        /// <param name="columnNames">  . </param>
        /// <returns>   An int. </returns>
        public virtual int BulkInsert(int database, int index, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames)
        {

            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += BulkInsert(dt, tableName, batchSize, timeOut, columnNames, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc) {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Bulk insert. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select FirstName from employee",,WD.DataAccess.Transactions.None);
        ///   Dictionary&lt;string,string&gt; cols=new Dictionary&lt;string,string&gt;();
        ///   <!--cols.Add("FirstName","FirstName");-->
        ///  int rowsEffected =dbContext.ICommands.BulkInsert(WD.DataAccess.Databases.BR,dt,"Employee",100,30,cols);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        /// <param name="batchSize"></param>
        /// <param name="timeOut"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public virtual int BulkInsert(int database, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += BulkInsert(dt, tableName, batchSize, timeOut, columnNames,CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc) {

                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk insert. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select FirstName from employee",,WD.DataAccess.Transactions.None);
        ///   Dictionary&lt;string,string&gt; cols=new Dictionary&lt;string,string&gt;();
        ///   <!--cols.Add("FirstName","FirstName");-->
        ///  int rowsEffected =dbContext.ICommands.BulkInsert(WD.DataAccess.Databases.BR, 1,dt,"Employee",100,30,cols,WD.DataAccess.Transactions.None);
        /// }
        /// }
        /// </code>
        ///</example>
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
       /// <param name="database"></param>
       /// <param name="index"></param>
       /// <param name="dt"></param>
       /// <param name="tableName"></param>
       /// <param name="batchSize"></param>
       /// <param name="timeOut"></param>
       /// <param name="columnNames"></param>
       /// <param name="transaction"></param>
       /// <returns></returns>
        public virtual int BulkInsert(int database,int index, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += BulkInsert(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += BulkInsert(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Bulk insert. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select FirstName from employee",,WD.DataAccess.Transactions.None);
        ///   Dictionary&lt;string,string&gt; cols=new Dictionary&lt;string,string&gt;();
        ///   <!--cols.Add("FirstName","FirstName");-->
        ///  int rowsEffected =dbContext.ICommands.BulkInsert(WD.DataAccess.Databases.BR,dt,"Employee",100,30,cols,WD.DataAccess.Transactions.None);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        /// <param name="batchSize"></param>
        /// <param name="timeOut"></param>
        /// <param name="columnNames"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int BulkInsert(int database, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += BulkInsert(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += BulkInsert(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        
         
        /// <summary>   Bulk update. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection connection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id,FirstName,LastName from employee",connection);
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;();
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///  int rowsEffected =dbContext.ICommands.BulkUpdate(WD.DataAccess.Databases.BR, 1,dt,"Employee",100,30,columnNames,primaryColumns);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>
        /// <param name="index" >  index number of database. </param>
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated. </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param> 
        /// <returns>   An int. </returns>
        public virtual int BulkUpdate(int database, int index, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns)
        {

            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += BulkUpdate(dt, tableName, batchSize, timeOut, columnNames, primaryColumns, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk update. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection connection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id,FirstName,LastName from employee",connection);
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;();
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///  int rowsEffected =dbContext.ICommands.BulkUpdate(WD.DataAccess.Databases.BR,dt,"Employee",100,30,columnNames,primaryColumns);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>       
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated. </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param> 
        /// <returns>   An int. </returns>
        public virtual int BulkUpdate(int database, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += BulkUpdate(dt, tableName, batchSize, timeOut, columnNames,primaryColumns, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc)
                            {

                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk update. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection connection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id,FirstName,LastName from employee",connection);
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;();
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///  int rowsEffected =dbContext.ICommands.BulkUpdate(WD.DataAccess.Databases.BR, 1,dt,"Employee",100,30,columnNames,primaryColumns,WD.DataAccess.Transactions.None);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>
        /// <param name="index" >  index number of database. </param>
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated. </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param> 
        /// <param name="transaction"> (Optional)No Transaction for Database </param>
        /// <returns>   An int. </returns>
        public virtual int BulkUpdate(int database, int index, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += BulkUpdate(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames,primaryColumns, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += BulkUpdate(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames,primaryColumns, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk update. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection connection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id,FirstName,LastName from employee",connection);
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;();
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///  int rowsEffected =dbContext.ICommands.BulkUpdate(WD.DataAccess.Databases.BR,dt,"Employee",100,30,columnNames,primaryColumns,WD.DataAccess.Transactions.None);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>       
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated. </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param> 
        /// <param name="transaction"> (Optional)No Transaction for Database </param>
        /// <returns>   An int. </returns>
        public virtual int BulkUpdate(int database, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += BulkUpdate(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames,primaryColumns, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += BulkUpdate(ref ex, connection, dt, tableName, batchSize, timeOut, columnNames, primaryColumns, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk delete. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id from employee");
        ///   Dictionary&lt;string,string&gt; primaryColumns=new Dictionary&lt;string,string&gt;();
        ///   primaryColumns.Add("Id","Id");
        ///  int rowsEffected =dbContext.ICommands.BulkDelete(WD.DataAccess.Databases.BR,1,dt,"Employee",100,30,primaryColumns);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>
        /// <param name="index" > index number of database. </param>
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>   
        /// <returns></returns>
        public virtual int BulkDelete(int database, int index, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns)
        {

            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += BulkDelete(dt, tableName, batchSize, timeOut, primaryColumns, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk delete. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id from employee");
        ///   Dictionary&lt;string,string&gt; primaryColumns=new Dictionary&lt;string,string&gt;();
        ///   primaryColumns.Add("Id","Id");
        ///  int rowsEffected =dbContext.ICommands.BulkDelete(WD.DataAccess.Databases.BR,dt,"Employee",100,30,primaryColumns);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>  
        /// <returns></returns>
        public virtual int BulkDelete(int database, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                rowAffected += BulkDelete(dt, tableName, batchSize, timeOut, primaryColumns, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()));
                            }
                            catch (Exception exc)
                            {

                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk delete. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id from employee");
        ///   Dictionary&lt;string,string&gt; primaryColumns=new Dictionary&lt;string,string&gt;();
        ///   primaryColumns.Add("Id","Id");
        ///  int rowsEffected =dbContext.ICommands.BulkDelete(WD.DataAccess.Databases.BR,1,dt,"Employee",100,30,primaryColumns,WD.DataAccess.Transactions.None);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>
        /// <param name="index" >  index number of database. </param>
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>      
        /// <param name="transaction"> (Optional)No Transaction for Database </param>
        /// <returns></returns>
        public virtual int BulkDelete(int database, int index, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            DetermineIndex(ref index);
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += BulkDelete(ref ex, connection, dt, tableName, batchSize, timeOut, primaryColumns, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += BulkDelete(ref ex, connection, dt, tableName, batchSize, timeOut, primaryColumns, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Bulk delete. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"select Id from employee");
        ///   Dictionary&lt;string,string&gt; primaryColumns=new Dictionary&lt;string,string&gt;();
        ///   primaryColumns.Add("Id","Id");
        ///  int rowsEffected =dbContext.ICommands.BulkDelete(WD.DataAccess.Databases.BR,dt,"Employee",100,30,primaryColumns,WD.DataAccess.Transactions.None);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> WD.DataAccess.Enums.</param>
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>      
        /// <param name="transaction"> (Optional)No Transaction for Database </param>
        /// <returns></returns>
        public virtual int BulkDelete(int database, DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    rowAffected += BulkDelete(ref ex, connection, dt, tableName, batchSize, timeOut, primaryColumns, isTransaction);
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                rowAffected += BulkDelete(ref ex, connection, dt, tableName, batchSize, timeOut, primaryColumns, isTransaction);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
       
        #endregion
        #region ExecuteScalar


        /// <summary>   Executes the Scalar operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   string firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, "Select FirstName from employee where EmployeeId=1");
        ///   
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue="1";
        ///   firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, "Select FirstName from employee where EmployeeId=@EmployeeId", aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="aParams">      (Optional) </param>
        /// <returns>   An object. </returns>


        public virtual object ExecuteScalar(int database, string commandText, DBParameter[] aParams = null)
        {

            object result = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteScalar(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;


        }
        /// <summary>   Executes the Scalar operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   string firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1, "Select FirstName from employee where EmployeeId=1");
        ///   
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue="1";
        ///   firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1, "Select FirstName from employee where EmployeeId=@EmployeeId", aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(int database,int index, string commandText, DBParameter[] aParams = null)
        {

            object result = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteScalar(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;


        }
        /// <summary>   Executes the Scalar operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   string firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR,  "Select FirstName from employee where EmployeeId=1", CommandType.Text);
        ///   
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue="1";
        ///   firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR,  "Select FirstName from employee where EmployeeId=@EmployeeId", CommandType.Text, aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  . </param>
        /// <param name="aParams">      (Optional) </param>
        /// <returns>   An object. </returns>
        public virtual object ExecuteScalar(int database, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            object result = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteScalar(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);
                                }
                            }
                            catch 
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return result;


        }
        /// <summary>   Executes the Scalar operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   string firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1, "Select FirstName from employee where EmployeeId=1", CommandType.Text);
        ///   
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue="1";
        ///   firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1, "Select FirstName from employee where EmployeeId=@EmployeeId", CommandType.Text, aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(int database,int index, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            object result = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteScalar(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return result;


        }
        #endregion
        #region ExecuteDataTable


        /// <summary>   Executes the data table operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataTable dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"Select columnNames from tableName");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Databases.BR,"Select columnNames from tableName where FirstName=@FirstName",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="aParams">      (Optional) </param>
        /// <returns>   A DataTable. </returns>


        public virtual DataTable ExecuteDataTable(int database, string commandText, DBParameter[] aParams = null)
        {

            DataTable dataTable = new DataTable();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataTable = ExecuteDataTable(commandText, CommandType.Text, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataTable;


        }
        /// <summary>   Executes the data Table operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataTable dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual DataTable ExecuteDataTable(int database,int index, string commandText, DBParameter[] aParams = null)
        {

            DataTable dataTable = new DataTable();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataTable = ExecuteDataTable(commandText, CommandType.Text, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataTable;


        }
        /// <summary>   Executes the data Table operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataTable dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR,"Select columnNames from tableName",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        ///
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  . </param>
        /// <param name="aParams">      (Optional) </param>
        /// <returns>   A DataTable. </returns>


        public virtual DataTable ExecuteDataTable(int database, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            DataTable dataTable = new DataTable();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataTable = ExecuteDataTable(commandText, commandType, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return dataTable;


        }
        /// <summary>   Executes the data Table operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataTable dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual DataTable ExecuteDataTable(int database,int index, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            DataTable dataTable = new DataTable();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataTable = ExecuteDataTable(commandText, commandType, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return dataTable;


        }
        #endregion
        #region ExecuteDataSet


        /// <summary>   Executes the data set operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataSet ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,"Select columnNames from tableName");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,"Select columnNames from tableName where FirstName=@FirstName",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        ///
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="aParams">      (Optional) </param>
        /// <returns>   A DataSet. </returns>


        public virtual DataSet ExecuteDataSet(int database, string commandText, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataSet = ExecuteDataSet(commandText, CommandType.Text, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return dataSet;


        }

        /// <summary>   Executes the data set operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataSet ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enum.Databases.BR, 1,"Select columnNames from tableName");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(int database,int index, string commandText, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataSet = ExecuteDataSet(commandText, CommandType.Text, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }

            return dataSet;


        }
        /// <summary>   Executes the data set operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataSet ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,"Select columnNames from tableName",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  . </param>
        /// <param name="aParams">      (Optional) </param>
        /// <returns>   A DataSet. </returns>


        public virtual DataSet ExecuteDataSet(int database, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataSet = ExecuteDataSet(commandText, commandType, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataSet;


        }
        /// <summary>   Executes the data set operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  DataSet ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(int database,int index, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    dataSet = ExecuteDataSet(commandText, commandType, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataSet;


        }
        #endregion
        #region ExecuteRecordSet


        /// <summary>   Executes the record set operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Databases.BR,"Select columnNames from tableName")){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Databases.BR,"Select columnNames from tableName
        ///   where FirstName=@FirstName",aParams);{
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="aParams">      (Optional) </param>
        /// <returns>   An ADODB.Recordset. </returns>


        public virtual ADODB.Recordset ExecuteRecordSet(int database, string commandText, DBParameter[] aParams = null)
        {

            ADODB.Recordset result = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteRecordSet(commandText, CommandType.Text, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;


        }
        /// <summary>   Executes the record set operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName")){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName
        ///   where FirstName=@FirstName",aParams);{
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
       /// <param name="database"></param>
       /// <param name="index"></param>
       /// <param name="commandText"></param>
       /// <param name="aParams"></param>
       /// <returns></returns>
        public virtual ADODB.Recordset ExecuteRecordSet(int database,int index, string commandText, DBParameter[] aParams = null)
        {

            ADODB.Recordset result = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteRecordSet(commandText, CommandType.Text, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;


        }
        /// <summary>   Executes the record set operation. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Databases.BR,"Select columnNames from tableName",CommandType.Text)){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Databases.BR,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);{
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  . </param>
        /// <param name="aParams">      (Optional) </param>
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



        public virtual ADODB.Recordset ExecuteRecordSet(int database, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            ADODB.Recordset result = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteRecordSet(commandText, commandType, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;


        }
        /// <summary>   Executes the record set operation. </summary>
        ///
        /// <remarks>    </remarks>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName",CommandType.Text)){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(ADODB.Recordset recordSet=dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);{
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual ADODB.Recordset ExecuteRecordSet(int database,int index, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            ADODB.Recordset result = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag&&index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    result = ExecuteRecordSet(commandText, commandType, dbConnection, aParams);

                                }
                            }
                            catch
                            {
                                throw;
                            }

                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;


        }
        #endregion
        #region ExecuteDataReader


        /// <summary>   Executes the data reader operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Databases.BR,"Select columnNames from tableName")){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Databases.BR,"Select columnNames from tableName
        ///   where FirstName=@FirstName",aParams);{
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="aParams">      (Optional) . </param>
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


        public virtual IDataReader ExecuteDataReader(int database, string commandText, DBParameter[] aParams = null)
        {

            IDataReader dataReader = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                dataReader = ExecuteDataReader(commandText, CommandType.Text, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()), aParams);
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataReader;


        }
        /// <summary>   Executes the data reader operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName")){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",aParams)){
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteDataReader(int database,int index, string commandText, DBParameter[] aParams = null)
        {

            IDataReader dataReader = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                dataReader = ExecuteDataReader(commandText, CommandType.Text, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()), aParams);
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataReader;


        }
        /// <summary>   Executes the data reader operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, "Select columnNames from tableName ",CommandType.Text)){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, "Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams)){
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        ///
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) . </param>
        /// <returns>   An IDataReader. </returns>


        public virtual IDataReader ExecuteDataReader(int database, string commandText,CommandType commandType, DBParameter[] aParams = null)
        {

            IDataReader dataReader = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                dataReader = ExecuteDataReader(commandText, commandType, CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()), aParams);
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataReader;


        }
        /// <summary>   Executes the data reader operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName",CommandType.Text)){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams)){
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteDataReader(int database,int index, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            IDataReader dataReader = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag&& index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                dataReader = ExecuteDataReader(commandText, commandType, CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()), aParams);
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataReader;


        }
        #endregion
        #region Connections


        /// <summary>   Creates instance of Connection MITEC database </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection(WD.DataAccess.Databases.BR)){
        ///     //write code for the connection
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        ///
        /// <param name="database"> TX OR BR. Value of WD.DataAccess.Enums.Databases </param>
        ///
        /// <returns>   The new connection. </returns>


        public virtual IDbConnection CreateConnection(int database)
        {
            IDbConnection dbConnection = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString());

                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dbConnection;
        }
        /// <summary>   Creates instance of Connection MITEC database </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection(WD.DataAccess.Databases.Enums.BR, 1)){
        ///     //write code for the connection
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        ///
        /// <param name="database"> TX OR BR. Value of WD.DataAccess.Enums.Databases </param>
        /// <param name="index"> Databse instance: 1=BR1/TX1 or 2=BR2/TX2 </param>
        /// <returns></returns>
        public virtual IDbConnection CreateConnection(int database,int index)
        {
            IDbConnection dbConnection = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString());

                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dbConnection;
        }
        /// <summary>   Creates open connection. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection con=dbContext.ICommands.CreateOpenConnection(WD.DataAccess.Databases.BR)){
        ///     //write code for the connection
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        ///
        /// <param name="database"> TX OR BR. Value of WD.DataAccess.Enums.Databases </param>
        ///
        /// <returns>   The new open connection. </returns>


        public virtual IDbConnection CreateOpenConnection(int database)
        {
            IDbConnection dbConnection = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                dbConnection = CreateOpenConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString());

                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dbConnection;
        }

        /// <summary>   Creates open connection. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   using(IDbConnection con=dbContext.ICommands.CreateOpenConnection(WD.DataAccess.Databases.BR)){
        ///     //write code for the connection
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
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
        /// <param name="database"> TX OR BR. Value of WD.DataAccess.Enums.Databases </param>
        /// <param name="index">Databse instance: 1=BR1/TX1 or 2=BR2/TX2 </param>
        /// <returns></returns>
        public virtual IDbConnection CreateOpenConnection(int database,int index)
        {
            IDbConnection dbConnection = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                dbConnection = CreateOpenConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString());

                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dbConnection;
        }
        #endregion
        #region IExecuteEntity

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee")
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee WHERE FirstName like @F",aParams);
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
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="aParams">      (Optional) . </param>
        /// <returns>   The list. </returns>
        public virtual List<T> GetList<T>(int database, string commandText, DBParameter[] aParams = null)
        {

            List<T> ts = new List<T>();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(commandText, CommandType.Text, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,"SELECT columnNames from tempEmployee")
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,"SELECT columnNames from tempEmployee WHERE FirstName like @F",aParams);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database,int index, string commandText, DBParameter[] aParams = null)
        {

            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag&& index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(commandText, CommandType.Text, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee",CommandType.Text)
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetList&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,aParams);
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
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) . </param>
        /// <returns>   The list. </returns>
        public virtual List<T> GetList<T>(int database, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            List<T> ts = new List<T>();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(commandText, commandType, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,"SELECT columnNames from tempEmployee",CommandType.Text)
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,"SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,aParams);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index,string commandText, CommandType commandType, DBParameter[] aParams = null)
        {

            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(commandText, commandType, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.LastName="XYZ");
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="predicate">    The predicate. </param>
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
        /// <returns>   The list. </returns>
        public virtual List<T> GetList<T>(int database, Expression<Func<T, bool>> predicate)
        {

            List<T> ts = new List<T>();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(predicate,dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.LastName="XYZ");
        /// 
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
       /// <typeparam name="T"></typeparam>
       /// <param name="database"></param>
       /// <param name="index"></param>
       /// <param name="predicate"></param>
       /// <returns></returns>
        public virtual List<T> GetList<T>(int database,int index, Expression<Func<T, bool>> predicate)
        {

            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(predicate, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }

        #region OrderBy
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Databases.BR,WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="predicate">    The predicate. </param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
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
        /// <returns>   The list. </returns>
        public virtual List<T> GetList<T>(int database, WD.DataAccess.Enums.SortOption sortBy, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {

            List<T> ts = new List<T>();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(orderBy, sortBy, dbConnection, (IDbTransaction)null);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// 
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="predicate"></param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index, WD.DataAccess.Enums.SortOption sortBy, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {

            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(orderBy, sortBy, dbConnection, (IDbTransaction)null);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.LastName="XYZ",WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="predicate">    The predicate. </param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
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
        /// <returns>   The list. </returns>
        public virtual List<T> GetList<T>(int database, Expression<Func<T, bool>> predicate,WD.DataAccess.Enums.SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {

            List<T> ts = new List<T>();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(predicate, orderBy, sortBy, dbConnection, (IDbTransaction)null);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   List&lt;Employee&gt; empList=dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.LastName="XYZ",WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// 
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="predicate"></param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index, Expression<Func<T, bool>> predicate,WD.DataAccess.Enums.SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {

            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    ts = GetList<T>(predicate, orderBy, sortBy, dbConnection, (IDbTransaction)null);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        #endregion

        #region paging

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, 10, out totalCount);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int pageNumber, int pageSize, out int totalCount)
        {
            List<T> ts = new List<T>();
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount = "";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, 1, 10, out totalCount);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index, int pageNumber, int pageSize, out int totalCount)
        {
            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount = "";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, 10, out totalCount, X =&gt; X.Address == "KL");
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int pageNumber, int pageSize, out int totalCount, Expression<Func<T, bool>> predicate)
        {
            List<T> ts = new List<T>();
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount = "";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount, predicate);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, 1, 10, out totalCount, X =&gt; X.Address == "KL");
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <param name="orderBy">Order by expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index, int pageNumber, int pageSize, out int totalCount, Expression<Func<T, bool>> predicate)
        {
            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount = "";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount, predicate);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 10, 1, out totalCount, X =&gt; X.Address == "KL",WD.DataAccess.Enums.SortOption.ASC,y=&gt;y.Country);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy">Order by expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int pageNumber, int pageSize, out int totalCount, Expression<Func<T, bool>> predicate,SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy )
        {
            List<T> ts = new List<T>();
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount="";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount, predicate,  sortBy,orderBy);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, 10, 1, out totalCount, X =&gt; X.Address == "KL",WD.DataAccess.Enums.SortOption.ASC,y=&gt;y.Country);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy">Order by expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index, int pageNumber, int pageSize, out int totalCount, Expression<Func<T, bool>> predicate,SortOption sortBy, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount = "";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount, predicate, sortBy,orderBy);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 10, 1, out totalCount, WD.DataAccess.Enums.SortOption.ASC,y=&gt;y.Country);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy">Order by expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int pageNumber, int pageSize, out int totalCount, SortOption sortBy, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount = "";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount, null, sortBy, orderBy);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="EMPLOYEE")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Id")]
        ///             [Required]
        ///             public int? ID { get; set; }

        ///             [Display(Name = "Name")]
        ///             [Required]
        ///             [CustomAttribute(Name = "NAME")]
        ///             public string Name { get; set; }

        ///             [Display(Name = "Country")]
        ///             [Required]
        ///             [CustomAttribute(Name = "COUNTRY")]
        ///             public string Country { get; set; }

        ///             [Display(Name = "Location")]
        ///             [Required]
        ///             [CustomAttribute(Name = "LOCATION")]
        ///             public string Location { get; set; }

        ///             [Display(Name = "Address")]
        ///             [Required]
        ///             [CustomAttribute(Name = "Address")]
        ///             public string Address { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        /// {
        ///     int totalCount = 0;
        ///     ICommands com = new DbContext(true).ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, 10, 1, out totalCount,WD.DataAccess.Enums.SortOption.ASC,y=&gt;y.Country);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy">Order by expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index, int pageNumber, int pageSize, out int totalCount, SortOption sortBy, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            totalCount = 0;
            string theSQL = "", theSQLCount = "";
            theSQL = GetPagingQuery<T>(pageNumber, pageSize, out theSQLCount, null, sortBy, orderBy);
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index == current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    totalCount = Convert.ToInt32(ExecuteScalar(theSQLCount, dbConnection));
                                    ts = GetList<T>(theSQL, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        #endregion

        /// <summary>   Gets an entity. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee")
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee WHERE FirstName like @F",aParams);
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
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) . </param>
        /// <returns>   The entity. </returns>
        public virtual T GetEntity<T>(int database, string commandText, DBParameter[] aParams = null)
        {
            T t = default(T);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    t = GetEntity<T>(commandText, CommandType.Text, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return t;

        }
        /// <summary>   Gets an entity. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR, 1,"SELECT columnNames from tempEmployee")
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,"SELECT columnNames from tempEmployee WHERE FirstName like @F",aParams);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public virtual T GetEntity<T>(int database,int index, string commandText, DBParameter[] aParams = null)
        {
            T t = default(T);
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    t = GetEntity<T>(commandText, CommandType.Text, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return t;

        }
        /// <summary>   Gets an entity. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee",CommandType.Text)
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,"SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,aParams);
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
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="commandText">  . </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) . </param>
        /// <returns>   The entity. </returns>
        public virtual T GetEntity<T>(int database, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            T t = default(T);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    t = GetEntity<T>(commandText, commandType, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return t;

        }
        /// <summary>   Gets an entity. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,"SELECT columnNames from tempEmployee",CommandType.Text)
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,"SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,aParams);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="aParams"></param>
        /// <returns>Returns the Entity of Type T</returns>
        public virtual T GetEntity<T>(int database,int index, string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            T t = default(T);
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    t = GetEntity<T>(commandText, commandType, dbConnection, aParams);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return t;

        }
        /// <summary>   Gets an entity. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.FirstName="XYZ");
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="predicate">    The predicate. </param>
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
        /// <returns>   The entity. </returns>
        public T GetEntity<T>(int database, Expression<Func<T, bool>> predicate)
        {
            T t = default(T);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    t = GetEntity<T>(predicate, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch { throw; }
            return t;
        }
        /// <summary>   Gets an entity. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.FirstName="XYZ");
        /// 
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
        /// <returns>   The entity. </returns>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="predicate"></param>
        
        public T GetEntity<T>(int database,int index, Expression<Func<T, bool>> predicate)
        {
            T t = default(T);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag&&index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    t = GetEntity<T>(predicate, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch { throw; }
            return t;
        }
        /// <summary>   Inserts. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=new Employee(){
        ///     FirstName ="ABCD",
        ///     MiddleName ="",
        ///     LastName ="ACBD",
        ///     DateOfBirth =  new DateTime(1986,07,07)
        ///     
        ///  };
        ///  int record=dbContext.ICommands.Insert(WD.DataAccess.Databases.BR,emp);
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database"> TX OR BX FOR MITEC. </param>
        /// <param name="input">    The input. </param>
        ///
        /// <returns>   An int. </returns>
        public virtual int Insert<T>(int database, T input)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {

                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Insert<T>(input, dbConnection);
                                }
                            }
                            catch(Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                    if (!isRead)
                    {
                        throw new ArgumentException(arMessage);
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        
        /// <summary>   Inserts. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=new Employee(){
        ///     FirstName ="ABCD",
        ///     MiddleName ="",
        ///     LastName ="ACBD",
        ///     DateOfBirth =  new DateTime(1986,07,07)
        ///     
        ///  };
        ///  int record=dbContext.ICommands.Insert(WD.DataAccess.Databases.BR, 1,emp);
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual int Insert<T>(int database,int index, T input)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Insert<T>(input, dbConnection);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Databases.BR,emp); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database"> TX OR BX FOR MITEC. </param>
        /// <param name="input">    The input. </param>
        ///
        /// <returns>   An int. </returns>
        public virtual int Update<T>(int database, T input)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Update<T>(input, dbConnection);
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Databases.BR,emp, WD.DataAccess.Enums.Transaction.None); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database"> TX OR BX FOR MITEC. </param>
        /// <param name="input">    The input. </param>
        /// <param name="transaction">    Value of WD.DataAccess.Enums.Transaction </param>
        /// <returns>   An int. </returns>
        public virtual int Update<T>(int database, T input, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                        isRead = true;
                                        WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                        try
                                        {
                                            using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                            {
                                               
                                                if (isTransaction)
                                                {
                                                    dbConnection.Open();
                                                    IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                                    try
                                                    {
                                                        rowAffected += Update<T>(input, dbTrans);
                                                        dbTrans.Commit();
                                                    }
                                                    catch
                                                    {
                                                        dbTrans.Rollback();
                                                        throw;

                                                    }
                                                    dbConnection.Close();
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        rowAffected += Update<T>(input, dbConnection);
                                                    }
                                                    catch
                                                    {
                                                        throw;

                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception exc)
                                        {
                                            ILogger.Fatal(exc);
                                            ex = exc;
                                        }
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                try
                                {
                                    using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                    {
                                        if (isTransaction)
                                        {
                                            dbConnection.Open();
                                            IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                            try
                                            {
                                                rowAffected += Update<T>(input, dbTrans);
                                                dbTrans.Commit();
                                            }
                                            catch
                                            {
                                                dbTrans.Rollback();
                                                throw;

                                            }
                                            dbConnection.Close();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                rowAffected += Update<T>(input, dbConnection);
                                            }
                                            catch
                                            {
                                                throw;

                                            }
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    ILogger.Fatal(exc);
                                    ex = exc;
                                }
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        
        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Enums.Databases.BR, 1,emp); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual int Update<T>(int database, int index, T input)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Update<T>(input, dbConnection);
                                }
                            }
                            catch 
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Enums.Databases.BR, 1,emp, WD.DataAccess.Enums.Transaction.None); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="input"></param>
        /// <param name="transaction">    Value of WD.DataAccess.Enums.Transaction </param>
        /// <returns></returns>
        public virtual int Update<T>(int database, int index, T input, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    try
                                    {
                                        using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                        {

                                            if (isTransaction)
                                            {
                                                dbConnection.Open();
                                                IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                                try
                                                {
                                                    rowAffected += Update<T>(input, dbTrans);
                                                    dbTrans.Commit();
                                                }
                                                catch
                                                {
                                                    dbTrans.Rollback();
                                                    throw;

                                                }
                                                dbConnection.Close();
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    rowAffected += Update<T>(input, dbConnection);
                                                }
                                                catch
                                                {
                                                    throw;

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        ILogger.Fatal(exc);
                                        ex = exc;
                                    }
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                try
                                {
                                    using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                    {
                                        if (isTransaction)
                                        {
                                            dbConnection.Open();
                                            IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                            try
                                            {
                                                rowAffected += Update<T>(input, dbTrans);
                                                dbTrans.Commit();
                                            }
                                            catch
                                            {
                                                dbTrans.Rollback();
                                                throw;

                                            }
                                            dbConnection.Close();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                rowAffected += Update<T>(input, dbConnection);
                                            }
                                            catch
                                            {
                                                throw;

                                            }
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    ILogger.Fatal(exc);
                                    ex = exc;
                                }
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        
        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Databases.BR,emp,x=&gt;x.EmployeeId=1); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="input">        The input. </param>
        /// <param name="predicate">    The predicate. </param>
        ///
        /// <returns>   An int. </returns>
        public virtual int Update<T>(int database, T input, Expression<Func<T, bool>> predicate)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Update<T>(input, predicate, dbConnection);
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Databases.BR,emp,x=&gt;x.EmployeeId=1, WD.DataAccess.Enums.Transaction.None); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="input">        The input. </param>
        /// <param name="predicate">    The predicate. </param>
        /// <param name="transaction">    Value of WD.DataAccess.Enums.Transaction </param>
        /// <returns>   An int. </returns>
        public virtual int Update<T>(int database, T input, Expression<Func<T, bool>> predicate, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag)
                                {
                                    isRead = true;
                                    //WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    try
                                    {
                                        using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                        {

                                            if (isTransaction)
                                            {
                                                dbConnection.Open();
                                                IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                                try
                                                {
                                                    rowAffected += Update<T>(input,predicate, dbTrans);
                                                    dbTrans.Commit();
                                                }
                                                catch
                                                {
                                                    dbTrans.Rollback();
                                                    throw;

                                                }
                                                dbConnection.Close();
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    rowAffected += Update<T>(input,predicate, dbConnection);
                                                }
                                                catch
                                                {
                                                    throw;

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        ILogger.Fatal(exc);
                                        ex = exc;
                                    }
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                try
                                {
                                    using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                    {
                                        if (isTransaction)
                                        {
                                            dbConnection.Open();
                                            IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                            try
                                            {
                                                rowAffected += Update<T>(input, predicate, dbTrans);
                                                dbTrans.Commit();
                                            }
                                            catch
                                            {
                                                dbTrans.Rollback();
                                                throw;

                                            }
                                            dbConnection.Close();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                rowAffected += Update<T>(input, predicate, dbConnection);
                                            }
                                            catch
                                            {
                                                throw;

                                            }
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    ILogger.Fatal(exc);
                                    ex = exc;
                                }
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Enums.Databases.BR, 1,emp,x=&gt;x.EmployeeId=1); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="input"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Update<T>(int database, int index, T input, Expression<Func<T, bool>> predicate)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag&& index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Update<T>(input, predicate, dbConnection);
                                }
                            }
                            catch 
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }

        /// <summary>   Updates this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Update(WD.DataAccess.Enums.Databases.BR, 1,emp,x=&gt;x.EmployeeId=1, WD.DataAccess.Enums.Transaction.None); 
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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="input"></param>
        /// <param name="predicate"></param>
        /// <param name="transaction">    Value of WD.DataAccess.Enums.Transaction </param>
        /// <returns></returns>
        public virtual int Update<T>(int database, int index, T input, Expression<Func<T, bool>> predicate, int transaction = Transaction.None)
        {

            int rowAffected = 0;
            Exception ex = null;
            bool isTransaction = false;
            bool isTransactionScope = false;
            GetTransaction(ref isTransaction, ref isTransactionScope, transaction);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {

                    if (isTransactionScope)
                    {
                        using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                        {
                            while (enumerator.MoveNext())
                            {
                                WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                                if (current.ActiveFlag && index == current.Index)
                                {
                                    isRead = true;
                                    WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                    try
                                    {
                                        using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                        {

                                            if (isTransaction)
                                            {
                                                dbConnection.Open();
                                                IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                                try
                                                {
                                                    rowAffected += Update<T>(input, predicate, dbTrans);
                                                    dbTrans.Commit();
                                                }
                                                catch
                                                {
                                                    dbTrans.Rollback();
                                                    throw;

                                                }
                                                dbConnection.Close();
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    rowAffected += Update<T>(input, predicate, dbConnection);
                                                }
                                                catch
                                                {
                                                    throw;

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        ILogger.Fatal(exc);
                                        ex = exc;
                                    }
                                }
                            }
                            if (rowAffected == 0 && ex != null)
                            {
                                throw ex;
                            }
                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                            if (current.ActiveFlag && index == current.Index)
                            {
                                isRead = true;
                                WD.DataAccess.Mitecs.Connections connection = new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password);
                                try
                                {
                                    using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                    {
                                        if (isTransaction)
                                        {
                                            dbConnection.Open();
                                            IDbTransaction dbTrans = dbConnection.BeginTransaction();
                                            try
                                            {
                                                rowAffected += Update<T>(input, predicate, dbTrans);
                                                dbTrans.Commit();
                                            }
                                            catch
                                            {
                                                dbTrans.Rollback();
                                                throw;

                                            }
                                            dbConnection.Close();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                rowAffected += Update<T>(input, predicate, dbConnection);
                                            }
                                            catch
                                            {
                                                throw;

                                            }
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    ILogger.Fatal(exc);
                                    ex = exc;
                                }
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        
        /// <summary>   Deletes this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Databases.BR,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Delete&lt;Employee&gt;(WD.DataAccess.Databases.BR,emp); 

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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database"> TX OR BX FOR MITEC. </param>
        /// <param name="input">    The input. </param>
        ///
        /// <returns>   An int. </returns>
        public virtual int Delete<T>(int database, T input)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Delete<T>(input, dbConnection);
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Deletes this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///  Employee emp=dbContext.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.EmployeeId=1); 
        ///   int record=dbContext.ICommands.Delete&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,emp); 

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
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
       /// <typeparam name="T"></typeparam>
       /// <param name="database"></param>
       /// <param name="index"></param>
       /// <param name="input"></param>
       /// <returns></returns>
        public virtual int Delete<T>(int database,int index, T input)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag && index==current.Index)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Delete<T>(input, dbConnection);
                                }
                            }
                            catch 
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Deletes this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   int record=dbContext.ICommands.Delete&lt;Employee&gt;(x=&gt;x.EmployeeId==1); 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="database">     TX OR BX FOR MITEC. </param>
        /// <param name="predicate">    The predicate. </param>
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
        /// <returns>   An int. </returns>
        public virtual int Delete<T>(int database, Expression<Func<T, bool>> predicate)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection( new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Delete<T>(predicate, dbConnection);
                                }
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Deletes this object. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main(){
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
        ///   int record=dbContext.ICommands.Delete&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1,x=&gt;x.EmployeeId==1); 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
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
        /// 
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Delete<T>(int database,int index, Expression<Func<T, bool>> predicate)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    rowAffected += Delete<T>(predicate, dbConnection);
                                }
                            }
                            catch 
                            {
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        #endregion
        #region Command
        #region ExecuteNonQuery
        /// <summary>   Executes the non query operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="command">The command.</param>
        /// <returns>System.Int32.</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "INSERT INTO TABLE XYX(XYCOLUMN) VALUES('12121')";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // databaseand command object
        ///    int returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES(@aColName)";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "aColName";
        ///    aParams[0].ParameterValue = "abcd";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        public virtual int ExecuteNonQuery(int database, IDbCommand command)
        {
            int rowAffected = 0;
            Exception ex = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        rowAffected += dbCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ex = exc;
                                ILogger.Fatal(ex);
                            }
                        }
                    }
                }
                if (rowAffected == 0 && ex != null)
                {
                    throw ex;
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>   Executes the non query operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="index">The index.</param>
        /// <param name="command">The command.</param>
        /// <returns>System.Int32.</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "INSERT INTO TABLE XYX(XYCOLUMN) VALUES('12121')";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // databaseand command object
        ///    int returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql ="INSERT INTO TABLE XYX(XYCOLUMN) VALUES(@aColName)";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "aColName";
        ///    aParams[0].ParameterValue = "abcd";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    returnRows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        public virtual int ExecuteNonQuery(int database, int index, IDbCommand command)
        {
            int rowAffected = 0;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        rowAffected += dbCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return rowAffected;
        }
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>System.Int32.</returns>
        public virtual int ExecuteNonQuery(IDbCommand command, IDbTransaction dbTransaction) {
            int rowAffected = 0;
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbTransaction.Connection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbCommand.Parameters!= null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    
                    rowAffected = dbCommand.ExecuteNonQuery();
                }
                
            }
            catch(Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return rowAffected;
        }
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>System.Int32.</returns>
        public virtual int ExecuteNonQuery(IDbCommand command, IDbConnection dbConnection)
        {
            int rowAffected = 0;
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbConnection;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                    }
                    rowAffected = dbCommand.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return rowAffected;
        }
        #endregion
        #region ExecuteScalar

        /// <summary>   Executes the Scalar operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="command">The command.</param>
        /// <returns>Object</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select FirstName from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    string firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select FirstName from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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


        public virtual object ExecuteScalar(int database, IDbCommand command)
        {
            object result = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        result = dbCommand.ExecuteScalar();
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>   Executes the Scalar operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="index">The index.</param>
        /// <param name="command">The command.</param>
        /// <returns>Object</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select FirstName from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    string firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select FirstName from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        /// 
        public virtual object ExecuteScalar(int database, int index, IDbCommand command)
        {
            object result = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        result = dbCommand.ExecuteScalar();
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return result;
        }


        public virtual object ExecuteScalar(IDbCommand command, IDbTransaction dbTransaction)
        {
            object result = null;
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbTransaction.Connection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                   
                    result = dbCommand.ExecuteScalar();
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return result;
        }


        public virtual object ExecuteScalar(IDbCommand command, IDbConnection dbConnection)
        {
            object result = null;
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbConnection;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                    }
                    result = dbCommand.ExecuteScalar();
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return result;
        }
        #endregion
        #region ExecuteDataTable
        /// <summary>   Executes the Data Table operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="command">The command.</param>
        /// <returns>DataTable</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    DataTable dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        public virtual DataTable ExecuteDataTable(int database, IDbCommand command)
        {
            DataTable dataTable = new DataTable();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }

                                        using (IDataReader reader = dbCommand.ExecuteReader())
                                        {
                                            while (!reader.IsClosed)
                                                dataTable.Load(reader, LoadOption.PreserveChanges);
                                        }
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataTable;
        }
        /// <summary>   Executes the Data Table operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="index">The index.</param>
        /// <param name="command">The command.</param>
        /// <returns>DataTable</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    DataTable dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // database and command object
        ///    dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        /// 
        public virtual DataTable ExecuteDataTable(int database, int index, IDbCommand command)
        {
            DataTable dataTable = new DataTable();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader reader = dbCommand.ExecuteReader())
                                        {
                                            while (!reader.IsClosed)
                                                dataTable.Load(reader, LoadOption.PreserveChanges);
                                        }
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataTable;
        }
        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>DataTable.</returns>
        public virtual DataTable ExecuteDataTable(IDbCommand command, IDbTransaction dbTransaction)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbTransaction.Connection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (!reader.IsClosed)
                            dataTable.Load(reader, LoadOption.PreserveChanges);
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataTable;
        }
        /// <summary>
        /// Executes the data table.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>DataTable.</returns>
        public virtual DataTable ExecuteDataTable(IDbCommand command, IDbConnection dbConnection)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbConnection;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (!reader.IsClosed)
                            dataTable.Load(reader, LoadOption.PreserveChanges);
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataTable;
        }
        #endregion
        #region ExecuteDataSet
        /// <summary>   Executes the Data Set operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="command">The command.</param>
        /// <returns>Dataset</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    DataSet dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        public virtual DataSet ExecuteDataSet(int database, IDbCommand command)
        {
            DataSet dataSet = new DataSet();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader reader = dbCommand.ExecuteReader())
                                        {
                                            while (!reader.IsClosed)
                                                dataSet.Load(reader, LoadOption.PreserveChanges, string.Empty);
                                        }
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataSet;
        }
        /// <summary>   Executes the Data Set operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="index">The index.</param>
        /// <param name="command">The command.</param>
        /// <returns>Dataset</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    DataSet dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR,dbCommand);
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    dt = dbContext.ICommands.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, 1,dbCommand);
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        public virtual DataSet ExecuteDataSet(int database, int index, IDbCommand command)
        {
            DataSet dataSet = new DataSet();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader reader = dbCommand.ExecuteReader())
                                        {
                                            while (!reader.IsClosed)
                                                dataSet.Load(reader, LoadOption.PreserveChanges, string.Empty);
                                        }
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                ILogger.Fatal(exc);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataSet;
        }
        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>DataSet.</returns>
        public virtual DataSet ExecuteDataSet(IDbCommand command, IDbTransaction dbTransaction)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbTransaction.Connection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (!reader.IsClosed)
                            dataSet.Load(reader, LoadOption.PreserveChanges, string.Empty);
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataSet;
        }
        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>DataSet.</returns>
        public virtual DataSet ExecuteDataSet(IDbCommand command, IDbConnection dbConnection)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbConnection;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                    }
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (!reader.IsClosed)
                            dataSet.Load(reader, LoadOption.PreserveChanges, string.Empty);
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataSet;
        }
        #endregion
        #region ExecuteRecordSet
        /// <summary>   Executes the record set operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="command">The command.</param>
        /// <returns>ADODB.Recordset</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    }     
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        /// 
        public virtual ADODB.Recordset ExecuteRecordSet(int database, IDbCommand command)
        {

            ADODB.Recordset recordSet = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader reader = dbCommand.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            while (!reader.IsClosed)
                                                dataTable.Load(reader, LoadOption.PreserveChanges);
                                            recordSet = Helpers.HelperUtility.ToRecordset(dataTable);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return recordSet;


        }
        /// <summary>   Executes the record set operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="index">The index.</param>
        /// <param name="command">The command.</param>
        /// <returns>ADODB.Recordset</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    }     
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (ADODB.Recordset recordSet = dbContext.ICommands.ExecuteRecordSet(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        public virtual ADODB.Recordset ExecuteRecordSet(int database, int index, IDbCommand command)
        {

            ADODB.Recordset recordSet = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader reader = dbCommand.ExecuteReader())
                                        {
                                            DataTable dataTable = new DataTable();
                                            while (!reader.IsClosed)
                                                dataTable.Load(reader, LoadOption.PreserveChanges);
                                            recordSet = Helpers.HelperUtility.ToRecordset(dataTable);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return recordSet;


        }
        #endregion
        #region ExecuteDataReader

        /// <summary>   Executes the data reader operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="command">The command.</param>
        /// <returns>IDataReader</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    }     
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        /// 
        public virtual IDataReader ExecuteDataReader(int database, IDbCommand command)
        {

            IDataReader dataReader = null;
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbCommand dbCommand = command)
                                {
                                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                    dbCommand.Connection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString());
                                    if (dbCommand.Parameters!= null)
                                    {
                                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                    }
                                    if (dbCommand.Connection.State != ConnectionState.Open)
                                    {
                                        dbCommand.Connection.Open();
                                    }
                                    dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataReader;


        }

        /// <summary>   Executes the data reader operation. </summary>
        /// <param name="database">The database.</param>
        /// <param name="index">The index.</param>
        /// <param name="command">The command.</param>
        /// <returns>IDataReader</returns>
        /// <remarks>   Asim Naeem, 7/28/2017. </remarks>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        ///<example>
        /// <code>
        /// class Program
        ///{
        ///  static void Main()
        ///  {
        ///     WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///     //OPEN QUERY
        ///     string theSql = "Select columnNames from employee where EmployeeId=1";
        ///
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///
        ///    // database and command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    }     
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///    //QUERY WITH PARAMETER
        ///    theSql = "Select columnNames from employee where EmployeeId=@EmployeeId";
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName = "EmployeeId";
        ///    aParams[0].ParameterValue = "1";
        ///
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    // databaseand command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///    //or
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        ///    {
        ///         //Your code here
        ///    } 
        ///
        ///  }
        ///
        ///}
        /// </code>
        ///</example>
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
        /// 
        public virtual IDataReader ExecuteDataReader(int database, int index, IDbCommand command)
        {

            IDataReader dataReader = null;
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString());
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbCommand.Connection.State != ConnectionState.Open)
                                        {
                                            dbCommand.Connection.Open();
                                        }
                                        dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                                    }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return dataReader;


        }

        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>IDataReader.</returns>
       public virtual IDataReader ExecuteDataReader(IDbCommand command, IDbTransaction dbTransaction)
        {
            IDataReader dataReader = null;
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbTransaction.Connection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataReader;
        }        
        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>System.Data.IDataReader.</returns>
        public virtual IDataReader ExecuteDataReader(IDbCommand command, IDbConnection dbConnection)
        {
            IDataReader dataReader = null;
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbConnection;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }
                    dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataReader;
        }
        #endregion
        #region ExecuteEntity
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        ///{
        ///    WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);

        ///    //OPEN QUERY
        ///    string theSql = "select * from tempEmployee";
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///    
        ///    // database and command object
        ///    List&lt;Employee&gt; empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///    //QUERY WITH PARAMETER
        ///
        ///    theSql = "SELECT columnNames from tempEmployee WHERE FirstName like @FirstName";
        ///
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///
        ///    aParams[0].ParameterName = "FirstName";
        ///    // Or
        ///    aParams[0].ParameterName = "@FirstName";
        ///    aParams[0].ParameterValue = "abc";
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    //Or
        ///    dbCommand = new System.Data.SqlClient.SqlCommand(theSql);
        ///    dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter { ParameterName = "@FirstName", Value = "abc", DbType = DbType.Int32 });
        ///
        ///    // database and command object
        ///    empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database,  IDbCommand command)
        {
            List<T> ts = new List<T>();
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }

                                        using (IDataReader dataReader = command.ExecuteReader())
                                        {
                                            using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                                            {
                                                while (dataReader.Read())
                                                {
                                                    ts.Add(mapper.MapFrom(dataReader));
                                                }
                                            }
                                        }

                                    }


                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets a list. </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        ///{
        ///    WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);

        ///    //OPEN QUERY
        ///    string theSql = "select * from tempEmployee";
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///    
        ///    // database and command object
        ///    List&lt;Employee&gt; empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///    //QUERY WITH PARAMETER
        ///
        ///    theSql = "SELECT columnNames from tempEmployee WHERE FirstName like @FirstName";
        ///
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///
        ///    aParams[0].ParameterName = "FirstName";
        ///    // Or
        ///    aParams[0].ParameterName = "@FirstName";
        ///    aParams[0].ParameterValue = "abc";
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    //Or
        ///    dbCommand = new System.Data.SqlClient.SqlCommand(theSql);
        ///    dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter { ParameterName = "@FirstName", Value = "abc", DbType = DbType.Int32 });
        ///
        ///    // database and command object
        ///    empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    empList = dbContext.ICommands.GetList&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int database, int index, IDbCommand command)
        {

            List<T> ts = new List<T>();
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader dataReader = command.ExecuteReader())
                                        {
                                            using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                                            {
                                                while (dataReader.Read())
                                                {
                                                    ts.Add(mapper.MapFrom(dataReader));
                                                }
                                            }
                                        }
                                       
                                    }
                                    
                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public virtual List<T> GetList<T>(IDbCommand command, IDbTransaction dbTransaction)
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbTransaction.Connection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    using (IDataReader dataReader = command.ExecuteReader())
                    {
                        using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                        {
                            while (dataReader.Read())
                            {
                                ts.Add(mapper.MapFrom(dataReader));
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return ts;
        }
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public virtual List<T> GetList<T>(IDbCommand command, IDbConnection dbConnection)
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbConnection;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }
                    using (IDataReader dataReader = command.ExecuteReader())
                    {
                        using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                        {
                            while (dataReader.Read())
                            {
                                ts.Add(mapper.MapFrom(dataReader));
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return ts;
        }
        /// <summary>   Gets an Entity </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        ///{
        ///    WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///
        ///    //OPEN QUERY
        ///    string theSql = "select * from tempEmployee Where FirstName='abc'";
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///    
        ///    // database and command object
        ///    Employee emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///    //QUERY WITH PARAMETER
        ///
        ///    theSql = "SELECT columnNames from tempEmployee WHERE FirstName like @FirstName";
        ///
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///
        ///    aParams[0].ParameterName = "FirstName";
        ///    // Or
        ///    aParams[0].ParameterName = "@FirstName";
        ///    aParams[0].ParameterValue = "abc";
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    //Or
        ///    dbCommand = new System.Data.SqlClient.SqlCommand(theSql);
        ///    dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter { ParameterName = "@FirstName", Value = "abc", DbType = DbType.Int32 });
        ///
        ///    // database and command object
        ///    emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual T GetEntity<T>(int database, IDbCommand command)
        {
            T ts = default(T);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader dataReader = command.ExecuteReader())
                                        {
                                            using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                                            {
                                                while (dataReader.Read())
                                                {
                                                    ts = mapper.MapFrom(dataReader);
                                                }
                                            }
                                        }

                                    }


                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }
        /// <summary>   Gets an Entity </summary>
        ///<example>
        /// <code>
        ///       [CustomAttribute(Name="TempEmployee")]
        ///        public class Employee
        ///        {
        ///
        ///            [CustomAttribute(IsPrimary = true)]
        ///            [Display(Name = "Employee Id")]
        ///            [Required]
        ///            public int? EmployeeId { get; set; }
        ///            [Display(Name = "First Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "FIRSTNAME")]
        ///            public string FirstName { get; set; }
        ///            [Display(Name = "Middle Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "MIDDLENAME")]
        ///            public string MiddleName { get; set; }
        ///            [Display(Name = "Last Name")]
        ///            [Required]
        ///            [CustomAttribute(Name = "LASTNAME")]
        ///            public string LastName { get; set; }
        ///            [Display(Name = "Date Of Joining")]
        ///            [CustomAttribute(Name = "DATEOFJOINING")]
        ///            public DateTime DateOfJoining { get; set; }
        ///            [Required]
        ///            public string Provider { get; set; }
        ///            public virtual Customer12 Customer { get; set; }
        ///
        ///        }
        /// class TestClass{
        /// static void Main()
        ///{
        ///    WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
        ///
        ///    //OPEN QUERY
        ///    string theSql = "select * from tempEmployee Where FirstName='abc'";
        ///    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);
        ///    
        ///    // database and command object
        ///    Employee emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
        ///    //QUERY WITH PARAMETER
        ///
        ///    theSql = "SELECT columnNames from tempEmployee WHERE FirstName like @FirstName";
        ///
        ///    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        ///
        ///    aParams[0].ParameterName = "FirstName";
        ///    // Or
        ///    aParams[0].ParameterName = "@FirstName";
        ///    aParams[0].ParameterValue = "abc";
        ///    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        ///    //Or
        ///    dbCommand = new System.Data.SqlClient.SqlCommand(theSql);
        ///    dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter { ParameterName = "@FirstName", Value = "abc", DbType = DbType.Int32 });
        ///
        ///    // database and command object
        ///    emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, dbCommand);
        ///    //OR
        ///    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        ///    emp = dbContext.ICommands.GetEntity&lt;Employee&gt;(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);
        ///
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
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="index"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual T GetEntity<T>(int database, int index, IDbCommand command)
        {

            T ts = default(T);
            DetermineIndex(ref index);
            bool isRead = false;
            try
            {
                using (List<WD.DataAccess.Mitecs.WDDBConfig>.Enumerator enumerator = database == WD.DataAccess.Enums.Databases.BR ? this.BXConnection.GetEnumerator() : this.TXConnection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        WD.DataAccess.Mitecs.WDDBConfig current = enumerator.Current;
                        if (index == current.Index && current.ActiveFlag)
                        {
                            isRead = true;
                            try
                            {
                                using (IDbConnection dbConnection = CreateConnection(new WD.DataAccess.Mitecs.Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString()))
                                {
                                    using (IDbCommand dbCommand = command)
                                    {
                                        HelperUtility.PrefixCommand(dbCommand, DBProvider);
                                        dbCommand.Connection = dbConnection;
                                        if (dbCommand.Parameters!= null)
                                        {
                                            HelperUtility.PrefixParameters(dbCommand, DBProvider);
                                        }
                                        if (dbConnection.State != ConnectionState.Open)
                                        {
                                            dbConnection.Open();
                                        }
                                        using (IDataReader dataReader = command.ExecuteReader())
                                        {
                                            using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                                            {
                                                while (dataReader.Read())
                                                {
                                                    ts = mapper.MapFrom(dataReader);
                                                }
                                            }
                                        }

                                    }


                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger.Fatal(ex);
                                throw;
                            }
                            break;
                        }
                    }
                }
                if (!isRead)
                {
                    throw new ArgumentException(arMessage);
                }
            }
            catch
            {
                throw;
            }
            return ts;

        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>T.</returns>
        public virtual T GetEntity<T>(IDbCommand command, IDbTransaction dbTransaction)
        {
            T ts = default(T);
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbTransaction.Connection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    using (IDataReader dataReader = command.ExecuteReader())
                    {
                        using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                        {
                            while (dataReader.Read())
                            {
                                ts = mapper.MapFrom(dataReader);
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return ts;
        }
        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>T.</returns>
        public virtual T GetEntity<T>(IDbCommand command, IDbConnection dbConnection)
        {
            T ts = default(T);
            try
            {
                using (IDbCommand dbCommand = command)
                {
                    HelperUtility.PrefixCommand(dbCommand, DBProvider);
                    dbCommand.Connection = dbConnection;
                    if (dbCommand.Parameters != null)
                    {
                        HelperUtility.PrefixParameters(dbCommand, DBProvider);
                    }
                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }
                    using (IDataReader dataReader = command.ExecuteReader())
                    {
                        using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                        {
                            while (dataReader.Read())
                            {
                                ts = mapper.MapFrom(dataReader);
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Fatal(exc);
                throw;
            }
            return ts;
        }
        #endregion
        #endregion
    }
}
