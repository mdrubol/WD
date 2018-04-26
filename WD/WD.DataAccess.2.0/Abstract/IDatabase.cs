// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="IDatabase.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using WD.DataAccess.Enums;
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
    /// IDatabase is an abstract class which helps in connections,transactions and commands
    /// </summary>
    public abstract class IDatabase
    {
        #region Constructor

        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        
        public IDatabase() : base() { }
        #endregion
        #region Connection
        
        /// <summary>
        /// Protected Connection Property.
        /// </summary>
        /// <value>Returns initialized Connection.</value>
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public abstract IDbConnection Connection { get; }

        
        /// <summary>
        /// Protected DBProvider Property.
        /// </summary>
        /// <value>The database provider.</value>
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public abstract int DBProvider { get; }
        /// <summary>
        /// Gets the bx connection.
        /// </summary>
        /// <value>The bx connection.</value>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal abstract System.Collections.Generic.List<WD.DataAccess.Mitecs.WDDBConfig> BXConnection { get; }
        /// <summary>
        /// Gets the tx connection.
        /// </summary>
        /// <value>The tx connection.</value>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal abstract System.Collections.Generic.List<WD.DataAccess.Mitecs.WDDBConfig> TXConnection { get; }

        
        /// <summary>
        /// Creates  instance of Connection.
        /// </summary>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        /// IDbConnection con=command.CreateConnection();
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
        /// 
        /// <returns>The new connection.</returns>
        
        

        public abstract IDbConnection CreateConnection();

        
        /// <summary>
        /// Creates instance of Open Connection.
        /// </summary>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        ///  IDbConnection con=command.CreateOpenConnection();
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
        /// <returns>The new open connection.</returns>
        
        

        public abstract IDbConnection CreateOpenConnection();

        
        /// <summary>
        /// Creates  instance of Open Connection for connection passed.
        /// </summary>
        /// <param name="con">Connection object.</param>
        /// <returns>The new open connection.</returns>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        ///  using(IDbConnection con=command.CreateConnection()){
        ///  ///it will open given connection
        ///  commad.CreateOpenConnect(con);
        ///  
        ///  }
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
        

        public abstract IDbConnection CreateOpenConnection(IDbConnection con);

        
        /// <summary>
        /// Creates  instance of  Connection for connection string passed.
        /// </summary>
        /// <param name="connectionString">.</param>
        /// <returns>The new connection.</returns>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        ///  using(IDbConnection con=command.CreateConnection("DataSource=XXX;UserId=XXX;Password=XXX")){
        ///  
        ///  }
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
        

        public abstract IDbConnection CreateConnection(string connectionString);

        
        /// <summary>
        /// Creates  instance of  Connection for connection string passed.
        /// </summary>
        /// <param name="connectionString">.</param>
        /// <returns>The new open connection.</returns>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        ///  using(IDbConnection con=command.CreateOpenConnection("DataSource=XXX;UserId=XXX;Password=XXX")){
        ///  
        ///  }
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
        

        public abstract IDbConnection CreateOpenConnection(string connectionString);

        
        /// <summary>
        /// Closes current connection.
        /// </summary>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        /// command.CloseConnection();
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
        

        public abstract void CloseConnection();
        #endregion
        #region Parameter

        
        /// <summary>
        /// Create Instance of Parameter.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterValue">Parameter Value.</param>
        /// <returns>DBParameter Object.</returns>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        /// DBParameter aParam=command.CreateParameter("FirstName","XXX");
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
        

        public virtual DBParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new DBParameter() { ParameterName = parameterName, ParameterValue = parameterValue };
        }

        
        /// <summary>
        /// Create Instance of Parameter.
        /// </summary>
        /// <returns>DBParameter Object.</returns>
        /// <example>
        ///  <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// WD.DataAccess.Abstract.ICommands command=new WD.DataAccess.Context.DbContext().ICommands;
        /// DBParameter aParam=command.CreateParameter();
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
        

        public virtual DBParameter CreateParameter()
        {

            return new DBParameter();
        }

        
        /// <summary>
        /// Create Instance of Parameter.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="command">Command Object having collection of parameter.</param>
        /// <returns>Parameter Value Object.</returns>
       
        

        public virtual object GetParameterValue(string parameterName, IDbCommand command)
        {

            object retValue = null;
            IDataParameter param = (IDataParameter)command.Parameters[parameterName];
            retValue = param.Value;
            return retValue;

        }

        
        /// <summary>
        /// Create Instance of Parameter.
        /// </summary>
        /// <param name="index">Parameter Index.</param>
        /// <param name="command">Command Object having collection of parameter.</param>
        /// <returns>Parameter Value Object.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual object GetParameterValue(int index, IDbCommand command)
        {
            object retValue = null;
            IDataParameter param = (IDataParameter)command.Parameters[index];
            retValue = param.Value;
            return retValue;
        }
        #endregion
        #region Transaction

        
        /// <summary>
        /// Creates Transaction instance for default connection.
        /// </summary>
        /// <returns>Transaction Object.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual IDbTransaction BeginTransaction()
        {
            return CreateOpenConnection().BeginTransaction();
        }
        
        /// <summary>
        /// Creates Transaction instance for passed connection.
        /// </summary>
        /// <param name="connection">Active Connection Object.</param>
        /// <returns>Transaction Object.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual IDbTransaction BeginTransaction(IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        
        /// <summary>
        /// Commits Transaction object for passed transaction.
        /// </summary>
        /// <param name="transaction">Active Transaction object.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual void CommitTransaction(IDbTransaction transaction)
        {
            try
            {
                transaction.Commit();
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
        }

        
        /// <summary>
        /// Rolls Backs passed Active Transaction object.
        /// </summary>
        /// <param name="transaction">Active Transaction object.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual void RollbackTransaction(IDbTransaction transaction)
        {
            try
            {
                transaction.Rollback();
            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
        }

        
        /// <summary>
        /// Disposed passed Active Transaction object.
        /// </summary>
        /// <param name="transaction">Active Transaction object.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual void DisposeTransaction(IDbTransaction transaction)
        {
            if (transaction == null)
                return;
            transaction.Dispose();
        }
        #endregion
        #region Command

        
        /// <summary>
        /// Creates Command Instance For active DBProvider.
        /// </summary>
        /// <returns>Active Command object.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual IDbCommand CreateCommand()
        {
            return WD.DataAccess.Configurations.AppConfiguration.CreateCommand(DBProvider);
        }

        
        /// <summary>
        /// Creates Command Instance For active DBProvider for CommandText, CommandType and Optional
        /// parameter.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) Collection of Optional parameter. </param>
        ///
        /// <returns>   Active Command object. </returns>
        

        public virtual IDbCommand CreateCommand(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            IDbCommand command = CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (aParams != null)
            {
                HelperUtility.AddParameters(command, aParams, DBProvider);
            }
            ILogger.Info(commandText);
            return command;
        }

        
        /// <summary>
        /// Creates Command Instance For active DBProvider for CommandText, CommandType, Connection and
        /// Optional parameter.
        /// </summary>
        /// <param name="commandText">Open Sql Statement or Procedure Name.</param>
        /// <param name="commandType">CommandType for Text or StoredProcedure (1 or 4)</param>
        /// <param name="connection">Active Connection Object.</param>
        /// <param name="aParams">(Optional) Collection of Optional parameter.</param>
        /// <returns>Active Command object.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            IDbCommand dbCommand = CreateCommand(commandText, commandType, aParams);
            dbCommand.Connection = connection;
            return dbCommand;
        }

        
        /// <summary>
        /// Creates Command Instance For active DBProvider for CommandText, CommandType, Transaction and
        /// Optional parameter.
        /// </summary>
        /// <param name="commandText">Open Sql Statement or Procedure Name.</param>
        /// <param name="commandType">CommandType for Text or StoredProcedure (1 or 4)</param>
        /// <param name="transaction">Active Transaction Object.</param>
        /// <param name="aParams">(Optional) Collection of Optional parameter.</param>
        /// <returns>Active Command object.</returns>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual IDbCommand CreateCommand(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {

            IDbCommand dbCommand = CreateCommand(commandText, commandType, transaction.Connection, aParams);
            dbCommand.Transaction = transaction;
            return dbCommand;
        }

        
        /// <summary>
        /// Disposed passed Active Command object.
        /// </summary>
        /// <param name="command">Active Command object.</param>
        /// <remarks>Shahid Kochak, 7/20/2017.</remarks>
        

        public virtual void DisposeCommand(IDbCommand command)
        {

            if (command == null)
                return;

            if (command.Connection != null)
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            command.Dispose();

        }
        #endregion
    }
}