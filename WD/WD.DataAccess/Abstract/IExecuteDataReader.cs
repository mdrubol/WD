// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="IExecuteDataReader.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;
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
    /// <summary>
    /// IExecuteDataReader is an abstract class for ExecuteDataReader methods
    /// </summary>
    public abstract class IExecuteDataReader : IDatabase
    {
        #region Constructor

        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public IExecuteDataReader() : base() { }
        #endregion

        
        /// <summary>   Creates instance of DataReader for commandText and optional Parameters. </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName")){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName
        ///   where FirstName=@FirstName",aParams);{
        ///     //write your code
        ///      
        ///   }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Active DataReader object. </returns>
        

        public virtual IDataReader ExecuteDataReader(string commandText, DBParameter[] aParams = null)
        {
            IDataReader dataReader = null;
            try
            {
                IDbConnection dbConnection = CreateConnection();
                {
                    dataReader = ExecuteDataReader(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);
                }
            }
            catch
            {
                throw;
            }
            return dataReader;
        }

        
        /// <summary>
        /// Creates instance of DataReader for commandText, connection and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName",con)){
        ///     //write your code
        ///      
        ///   }
        ///   }
        ///   //or
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName
        ///   where FirstName=@FirstName",con,aParams);{
        ///     //write your code
        ///      
        ///   }
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
        /// <returns>   An IDataReader. </returns>
        

        public virtual IDataReader ExecuteDataReader(string commandText, IDbConnection connection, DBParameter[] aParams = null)
        {
            IDataReader dataReader = null;
            try
            {
                
                dataReader = ExecuteDataReader(commandText, CommandType.Text, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return dataReader;
        }

        
        /// <summary>
        /// Creates instance of DataReader for commandText, transaction and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction trans=connection.BeginTransaction())
        ///   {
        ///     try
        ///     {
        ///         using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName",trans))
        ///         {
        ///          //write your code
        ///      
        ///         }
        ///   
        ///         trans.Commit();
        ///      }
        ///    catch
        ///     {
        ///           trans.Rollback();
        ///     }
        ///   }
        ///  }
        ///   //or
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///     WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///       aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///     aParams[0].ParameterName="FirstName";
        ///     aParams[0].ParameterValue="first name";
        ///   using(IDbTransaction trans=connection.BeginTransaction())
        ///   {
        ///     try
        ///     {
        ///         using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName where FirstName=@FirstName",trans,aParams);
        ///          {
        ///         //write your code
        ///
        ///          }
        ///        trans.Commit();
        ///    }
        ///   catch{ 
        ///         trans.Rollback();
        ///         }
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
        /// <returns>   An IDataReader. </returns>
        

        public virtual IDataReader ExecuteDataReader(string commandText, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            IDataReader dataReader = null;
            try
            {
                dataReader = ExecuteDataReader(commandText, CommandType.Text, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return dataReader;
        }

        
        /// <summary>
        /// Creates instance of DataReader for commandText, commandType and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName",CommandType.Text)){
        ///     //write your code
        ///      
        ///   }
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);{
        ///     //write your code
        ///      
        ///   }
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
        /// <returns>   An IDataReader. </returns>
        

        public virtual IDataReader ExecuteDataReader(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            IDataReader dataReader = null;
            try
            {
                IDbConnection dbConnection = CreateConnection();
                {
                    dataReader = ExecuteDataReader(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);
                   
                }
            }
            catch
            {
                throw;
            }
            return dataReader;
        }

        
        /// <summary>
        /// Creates instance of DataReader for commandText,commandType,active connection and optional
        /// Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main(){
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName",CommandType.Text,con)){
        ///     //write your code
        ///      
        ///   }
        ///   }
        ///   //or
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,con,aParams);{
        ///     //write your code
        ///      
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
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   An IDataReader. </returns>
        

        public virtual IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            IDataReader dataReader = null;
            try
            {
                dataReader = ExecuteDataReader(commandText, commandType, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return dataReader;
        }

        
        /// <summary>
        /// Creates instance of DataReader for commandText,commandType, active transaction and optional
        /// Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass{
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction trans=connection.BeginTransaction())
        ///   {
        ///     try
        ///     {
        ///         using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName",CommandType.Text,trans))
        ///         {
        ///          //write your code
        ///      
        ///         }
        ///   
        ///         trans.Commit();
        ///      }
        ///    catch
        ///     {
        ///           trans.Rollback();
        ///     }
        ///   }
        ///  }
        ///   //or
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///     WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///       aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///     aParams[0].ParameterName="FirstName";
        ///     aParams[0].ParameterValue="first name";
        ///   using(IDbTransaction trans=connection.BeginTransaction())
        ///   {
        ///     try
        ///     {
        ///         using(IDataReader reader=dbContext.ICommands.ExecuteDataReader("Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,trans,aParams);
        ///          {
        ///         //write your code
        ///
        ///          }
        ///        trans.Commit();
        ///    }
        ///   catch{ 
        ///         trans.Rollback();
        ///         }
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
        /// <returns>   An IDataReader. </returns>
        

        public virtual IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            IDataReader dataReader = null;
            try
            {
                dataReader = ExecuteDataReader(commandText, commandType, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return dataReader;
        }

        
        /// <summary>
        /// Creates instance of DataReader for commandText,commandType, active connection, active
        /// transaction and optional Parameters.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   An IDataReader. </returns>
        

        private IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDbConnection connection, IDbTransaction transaction, DBParameter[] aParams = null)
        {

            IDataReader dataReader = null;
            try
            {

                using (IDbCommand dbCommand = CreateCommand(commandText, commandType))
                {

                    if (aParams != null)
                    {
                        HelperUtility.AddParameters(dbCommand, aParams, DBProvider);
                    }
                    dbCommand.Transaction = transaction;
                    dbCommand.Connection = connection;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }

            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return dataReader;

        }


        
    }
}
