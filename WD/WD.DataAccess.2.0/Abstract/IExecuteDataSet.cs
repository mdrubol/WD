// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="IExecuteDataSet.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;
using System.Data.Common;
using WD.DataAccess.Helpers;
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
    
    /// <summary>   IExecuteDataSet is an abstract class for ExecuteDataSet methods. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public abstract class IExecuteDataSet : IExecuteDataReader
    {
              #region Constructor

        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public IExecuteDataSet() : base() { }
        #endregion

        
        /// <summary>
        /// Creates instance of DataSet for collection of commandText  and optional Collection of
        /// Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  DataSet ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName where FirstName=@FirstName",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(string commandText, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    dataSet = ExecuteDataSet(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);
                   
                }
            }
            catch 
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of commandText ,active connection and optional
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
        ///    DataSet ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName",dbConnection);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName where FirstName=@FirstName",dbConnection,aParams);
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
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(string commandText, IDbConnection connection, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            try
            {

                dataSet = ExecuteDataSet(commandText, CommandType.Text, connection, (IDbTransaction)null, aParams);
            }
            catch 
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of commandText ,active transaction and optional
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
        ///     DataSet ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName",trans);
        ///   //or
        ///    WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="FirstName";
        ///    aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName where FirstName=@FirstName",trans,aParams);
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
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(string commandText, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            try
            {
                dataSet = ExecuteDataSet(commandText, CommandType.Text, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of commandText ,commandType and optional
        /// Collection of Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  DataSet ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);
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
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    dataSet = ExecuteDataSet(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);
                   
                }
            }
            catch 
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of commandText ,commandType,  active connection
        /// and optional Collection of Parameters.
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
        ///    DataSet ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName",CommandTyp.Text,dbConnection);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,dbConnection,aParams);
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
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            try
            {
                dataSet = ExecuteDataSet(commandText, commandType, connection, (IDbTransaction)null, aParams);
            }
            catch 
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of commandText ,commandType,  active transaction
        /// and optional Collection of Parameters.
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
        ///     DataSet ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName",CommandType.Text,trans);
        ///   //or
        ///    WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///    aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    aParams[0].ParameterName="FirstName";
        ///    aParams[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,trans,aParams);
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
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            DataSet dataSet = new DataSet();
            try
            {
                dataSet = ExecuteDataSet(commandText, commandType, transaction.Connection, transaction, aParams);
            }
            catch 
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of commandText ,commandType,active connection,
        /// active transaction and optional Collection of Parameters.
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
        /// <returns>   Collection of Tables. </returns>
        

        private  DataSet ExecuteDataSet(string commandText, CommandType commandType, IDbConnection connection, IDbTransaction transaction, DBParameter[] aParams)
        {

            DataSet dataSet = new DataSet();
            try
            {
                
                using (IDbCommand dbCommand = CreateCommand(commandText,commandType))
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
                        while (!reader.IsClosed)
                            dataSet.Load(reader, LoadOption.PreserveChanges, string.Empty);
                    }
                }
                

            }
            catch(Exception exc)
            {
                Logger.ILogger.Fatal(exc);

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
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   SqlStatement[] input=new SqlStatement[1];
        ///   input.CommandText="Select columnNames from tableName";
        ///   input.CommandType=CommandType.Text;
        ///    DataSet ds=dbContext.ICommands.ExecuteDataSet(input);
        ///   //or
        ///   input=new SqlStatement[1];
        ///   input.CommandText="Select columnNames from tableName";
        ///   input.CommandType=CommandType.Text;
        ///   input.Params=new WD.DataAccess.Parameters.DBParameter[1];
        ///    WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///    input.Params[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    input.Params[0].ParameterName="FirstName";
        ///    input.Params[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet(input);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">    Object of Entity. </param>
        ///
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(SqlStatement[] input)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    dataSet = ExecuteDataSet(input, dbConnection, (IDbTransaction)null);
                   
                }
            }
            catch 
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of Sql Statements ,collection of  commandType, and
        /// active connection.
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
        ///   SqlStatement[] input=new SqlStatement[1];
        ///   input.CommandText="Select columnNames from tableName";
        ///   input.CommandType=CommandType.Text;
        ///    DataSet ds=dbContext.ICommands.ExecuteDataSet(input,dbConnection);
        ///   //or
        ///   input=new SqlStatement[1];
        ///   input.CommandText="Select columnNames from tableName";
        ///   input.CommandType=CommandType.Text;
        ///   input.Params=new WD.DataAccess.Parameters.DBParameter[1];
        ///    WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///    input.Params[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    input.Params[0].ParameterName="FirstName";
        ///    input.Params[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet(input,dbConnection);
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">        Object of Entity. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(SqlStatement[] input, IDbConnection connection)
        {
            DataSet dataSet = new DataSet();
            try
            {
                dataSet = ExecuteDataSet(input, connection);
            }
            catch 
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of Sql Statements ,collection of  commandType, and
        /// active transaction.
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
        ///    using(IDbTransaction trans=dbConnection.BeginTransaction())
        ///   {
        ///   try
        ///   {
        ///   SqlStatement[] input=new SqlStatement[1];
        ///   input.CommandText="Select columnNames from tableName";
        ///   input.CommandType=CommandType.Text;
        ///    DataSet ds=dbContext.ICommands.ExecuteDataSet(input,trans);
        ///   //or
        ///   input=new SqlStatement[1];
        ///   input.CommandText="Select columnNames from tableName";
        ///   input.CommandType=CommandType.Text;
        ///   input.Params=new WD.DataAccess.Parameters.DBParameter[1];
        ///    WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///    input.Params[0]=new WD.DataAccess.Parameters.DBParameter();
        ///    input.Params[0].ParameterName="FirstName";
        ///    input.Params[0].ParameterValue="first name";
        ///    ds=dbContext.ICommands.ExecuteDataSet(input,trans);
        ///    trans.Commit();
        ///    }
        ///    catch{
        ///    trans.Rollback();
        ///    }
        ///    }
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">        Object of Entity. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Collection of Tables. </returns>
        

        public virtual DataSet ExecuteDataSet(SqlStatement[] input, IDbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            try
            {
                dataSet = ExecuteDataSet(input, (IDbConnection)transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return dataSet;
        }

        
        /// <summary>
        /// Creates instance of DataSet for collection of Sql Statements ,collection of  commandType,
        /// active connection and active transaction.
        /// </summary>
        /// <param name="input">Object of Entity.</param>
        /// <param name="connection">Active Connection Object.</param>
        /// <param name="transaction">Active Transaction Object.</param>
        /// <returns>Collection of Tables.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        private   DataSet ExecuteDataSet(SqlStatement[] input, IDbConnection connection, IDbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            try
            {
                foreach (SqlStatement theSql in input)
                {
                    
                    using (IDbCommand dbCommand = CreateCommand(theSql.CommandText,theSql.CommandType))
                    {
                        dbCommand.Connection = connection;
                        dbCommand.Transaction = transaction;
                        if (theSql.Params != null)
                        {
                            WD.DataAccess.Helpers.HelperUtility.AddParameters(dbCommand, theSql.Params, DBProvider);
                        }
                        using (IDataReader reader = dbCommand.ExecuteReader())
                        {
                            while (!reader.IsClosed)
                                dataSet.Load(reader, LoadOption.PreserveChanges, string.Empty);
                        }

                    }
                    

                }
            }
            catch(Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
            return dataSet;
        }
    }
}
