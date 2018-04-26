// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 06-05-2017
// ***********************************************************************
// <copyright file="IExecuteScalar.cs" company="Western Digital">
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
    /// IExecuteScalar is an abstract class for executes calar methods.
    ///  </summary>
    /// <remarks>   Shahid K, 7/24/2017. </remarks>
    

    public abstract class IExecuteScalar : IExecuteRecordSet
    {
        #region Constructor

        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public IExecuteScalar() : base() { }
        #endregion

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  string firstName=(string)dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=1");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue=1;
        ///    firstName=dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=@F",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(string commandText, DBParameter[] aParams = null)
        {
            object result = null;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    result = ExecuteScalar(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);
                   

                }
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///  string firstName=(string)dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=1",con);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue=1;
        ///    firstName=dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=@F",con,aParams);
        ///    }
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
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(string commandText, IDbConnection connection, DBParameter[] aParams = null)
        {
            object result = null;
            try
            {

                result = ExecuteScalar(commandText, CommandType.Text, connection, (IDbTransaction)null, aParams);
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///   using(IDbTransaction trans=con.BeginTransaction()){
        ///   try{
        ///  string firstName=(string)dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=1",trans);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue=1;
        ///    firstName=dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=@F",trans,aParams);
        ///    trans.Commit();
        ///    }
        ///    catch{
        ///    trans.Rollback();
        ///    }
        ///    }
        ///    }
        /// }
        /// }
        /// </code>
        /// </example>  
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(string commandText, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            object result = null;
            try
            {
                result = ExecuteScalar(commandText, CommandType.Text, transaction.Connection, transaction, aParams);
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  string firstName=(string)dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=1",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue=1;
        ///    firstName=dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=@F",CommandType.Text,aParams);
        /// }
        /// }
        /// </code>
        /// </example>  
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            object result = null;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    result = ExecuteScalar(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);
                   

                }
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///  string firstName=(string)dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=1",CommandType.Text,con);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue=1;
        ///    firstName=dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=@F",CommandType.Text,con,aParams);
        ///    }
        /// }
        /// }
        /// </code>
        /// </example>  
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            object result = null;
            try
            {
                result = ExecuteScalar(commandText, commandType, connection, (IDbTransaction)null, aParams);
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection()){
        ///   using(IDbTransaction trans=con.BeginTransaction()){
        ///   try{
        ///  string firstName=(string)dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=1",CommandType.Text,trans);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="EmployeeId";
        ///   aParams[0].ParameterValue=1;
        ///    firstName=dbContext.ICommands.ExecuteScalar("Select FirstName from employee where EmployeeId=@F",CommandType.Text,trans,aParams);
        ///    trans.Commit();
        ///    }
        ///    catch{
        ///    trans.Rollback();
        ///    }
        ///    }
        ///    }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            object result = null;
            try
            {
                result = ExecuteScalar(commandText, commandType, transaction.Connection, transaction, aParams);
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
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
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(string commandText, CommandType commandType, IDbConnection connection, IDbTransaction transaction, DBParameter[] aParams)
        {

            object result = null;
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
                    result = dbCommand.ExecuteScalar();
                }

            }
            catch(Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query for CommandText and optional Parameters.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">    Collection of SQL Statments With CommandText,CommandType and Optional
        ///                         parameters. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(SqlStatement[] input)
        {
            object result = null;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    result = ExecuteScalar(input, dbConnection, (IDbTransaction)null);
                   

                }
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Object Item as per the scalar Query forCommandText,Command Type, optional Parameters
        /// and Active Connection Object.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">        Collection of SQL Statments With CommandText,CommandType and
        ///                             Optional parameters. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(SqlStatement[] input, IDbConnection connection)
        {
            object result = null;
            try
            {
                result = ExecuteScalar(input, connection, (IDbTransaction)null);
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Last Object Item from the collection of  Query for CommandText,Command Type, optional
        /// Parameters and Active transaction Object.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">        Collection of SQL Statments With CommandText,CommandType and
        ///                             Optional parameters. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        public virtual object ExecuteScalar(SqlStatement[] input, IDbTransaction transaction)
        {
            object result = null;
            try
            {
                result = ExecuteScalar(input, transaction.Connection, transaction);
            }
            catch 
            {
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Returns Last Object Item from the collection of  Query for CommandText,Command Type, optional
        /// Parameters , Active Connection Object and Active transaction Object.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">        Collection of SQL Statments With CommandText,CommandType and
        ///                             Optional parameters. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   object of any type. </returns>
        

        private object ExecuteScalar(SqlStatement[] input, IDbConnection connection, IDbTransaction transaction)
        {

            object result = null;
            try
            {
                foreach (SqlStatement theSql in input)
                {
                    result = ExecuteScalar(theSql.CommandText, theSql.CommandType, connection, transaction, theSql.Params);
                }
            }
            catch
            {
                throw;
            }
            return result;

        }
    }
}
