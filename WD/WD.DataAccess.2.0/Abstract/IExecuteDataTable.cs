// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 06-05-2017
// ***********************************************************************
// <copyright file="IExecuteDataTable.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;
using System.Data.Common;
using WD.DataAccess.Logger;
using WD.DataAccess.Parameters;



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

    /// <summary>   IExecuteDataTable is an abstract class  for ExecuteDataTable methods. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>


    public abstract class IExecuteDataTable : IExecuteDataSet
    {
        #region Constructor


        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>


        public IExecuteDataTable() : base() { }
        #endregion


        /// <summary>
        /// Creates instance of DataTable for commandText  and optional Collection of Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  DataTable dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName where FirstName=@FirstName",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Records as Table. </returns>


        public virtual DataTable ExecuteDataTable(string commandText, DBParameter[] aParams = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    dataTable = ExecuteDataTable(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);

                }
            }
            catch
            {
                throw;
            }
            return dataTable;
        }


        /// <summary>
        /// Creates instance of DataTable for  commandText, active Connection  and optional Collection of
        /// Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        ///   {
        ///    DataTable dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName",dbConnection);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName where FirstName=@FirstName",dbConnection,aParams);
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Records as Table. </returns>


        public virtual DataTable ExecuteDataTable(string commandText, IDbConnection connection, DBParameter[] aParams = null)
        {
            DataTable dataTable = new DataTable();
            try
            {

                dataTable = ExecuteDataTable(commandText, CommandType.Text, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return dataTable;
        }


        /// <summary>
        /// Creates instance of DataTable for  commandText, active transaction  and optional Collection
        /// of Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction trans=dbConnection.BeginTransaction())
        ///   {
        /// try
        /// {
        ///     DataTable dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName",trans);
        ///   //or
        ///    WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="FirstName";
        ///    aParams[0].ParameterValue="first name";
        ///    dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName where FirstName=@FirstName",trans,aParams);
        ///    trans.Commit();
        ///  }
        ///  catch
        ///  {
        ///   trans.Rollback();
        ///  }
        ///  }
        ///  }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        /// <returns>   Collection of Records as Table. </returns>


        public virtual DataTable ExecuteDataTable(string commandText, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = ExecuteDataTable(commandText, CommandType.Text, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return dataTable;
        }


        /// <summary>
        /// Creates instance of DataTable for commandText,CommandType and optional Collection of
        /// Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  DataTable dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Records as Table. </returns>


        public virtual DataTable ExecuteDataTable(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    dataTable = ExecuteDataTable(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);

                }
            }
            catch
            {
                throw;
            }
            return dataTable;
        }


        /// <summary>
        /// Creates instance of DataTable for commandText, CommandType, active Connection and optional
        /// Collection of Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        ///   {
        ///    DataTable dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName",CommandTyp.Text,dbConnection);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,dbConnection,aParams);
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Records as Table. </returns>


        public virtual DataTable ExecuteDataTable(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = ExecuteDataTable(commandText, commandType, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return dataTable;
        }


        /// <summary>
        /// Creates instance of DataTable for commandText, CommandType, active Transaction  and optional
        /// Collection of Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction trans=dbConnection.BeginTransaction())
        ///   {
        ///   try{
        ///     DataTable dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName",CommandType.Text,trans);
        ///   //or
        ///    WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="FirstName";
        ///    aParams[0].ParameterValue="first name";
        ///    dt=dbContext.ICommands.ExecuteDataTable("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,trans,aParams);
        ///    trans.Commit();
        ///    }catch{
        ///    trans.Rollback();
        ///    }
        ///   }
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Records as Table. </returns>


        public virtual DataTable ExecuteDataTable(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = ExecuteDataTable(commandText, commandType, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return dataTable;
        }


        /// <summary>
        /// Creates instance of DataTable for collection of commandText and optional Collection of
        /// Parameters.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Records as Table. </returns>


        private DataTable ExecuteDataTable(string commandText, CommandType commandType, IDbConnection connection, IDbTransaction transaction, DBParameter[] aParams)
        {

            DataTable dataTable = new DataTable();
            try
            {
                
                using (IDbCommand dbCommand = CreateCommand(commandText, commandType))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (aParams != null)
                    {
                        WD.DataAccess.Helpers.HelperUtility.AddParameters(dbCommand, aParams, DBProvider);
                    }
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        dataTable.Load(reader, LoadOption.OverwriteChanges);
                    }
                }

            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataTable;

        }

    }
}
