// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 06-06-2017
// ***********************************************************************
// <copyright file="IExecuteNonQuery.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
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
    
    /// <summary>   IExecuteNonQuery is an abstract class for ExecuteNonQuery methods. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>


    public abstract class IExecuteNonQuery : IExecuteDataTable
    {


        #region Constructor


        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>


        public IExecuteNonQuery() : base() { }
        #endregion


        /// <summary>   Returns Rows Affected for CommandText and optional Parameter Collection. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')");
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",aParams);
        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(string commandText, DBParameter[] aParams = null)
        {
            int rowAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowAffected = ExecuteNonQuery(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);


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
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')",con);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",con,aParams);
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
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(string commandText, IDbConnection connection, DBParameter[] aParams = null)
        {
            int rowAffected = 0;
            try
            {

                rowAffected = ExecuteNonQuery(commandText, CommandType.Text, connection, (IDbTransaction)null, aParams);
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
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction trans=con.BeginTransaction())
        ///   {
        ///   try
        ///   {
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')",trans);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",trans,aParams);
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
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(string commandText, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            int rowAffected = 0;
            try
            {
                rowAffected = ExecuteNonQuery(commandText, CommandType.Text, transaction.Connection, transaction, aParams);
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
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')",CommandType.Text);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",CommandType.Text,aParams);
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
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            int rowAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowAffected = ExecuteNonQuery(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);


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
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')",CommandType.Text,con);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",CommandType.Text,con,aParams);
        ///    }
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
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            int rowAffected = 0;
            try
            {
                rowAffected = ExecuteNonQuery(commandText, commandType, connection, (IDbTransaction)null, aParams);
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
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            int rowAffected = 0;
            try
            {
                rowAffected = ExecuteNonQuery(commandText, commandType, transaction.Connection, transaction, aParams);
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
        ///   using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction trans=con.BeginTransaction())
        ///   {
        ///   try
        ///   {
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')",trans);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery("INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)",CommandType.Text,trans,aParams);
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
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      Collection of Optional Parameters. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(string commandText, CommandType commandType, IDbConnection connection, IDbTransaction transaction, DBParameter[] aParams)
        {

            int rowsAffected = 0;
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
                    rowsAffected = dbCommand.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                ILogger.Fatal(exc);
                throw;
            }
            return rowsAffected;
        }


        /// <summary>   Executes the non query operation. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// SqlStatement[] sql=new  SqlStatement[1];
        /// sql[0]=new SqlStatement();
        /// sql[1].CommandText="INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')";
        /// sql[1].CommandType=CommandType.Text;
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery(sql);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    sql[1].CommandText="INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)";
        /// sql[1].CommandType=CommandType.Text;
        /// sql[1].Params=aParams;
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery(sql);

        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">    Collection of SQL Statements With CommandText,CommandType and Optional
        ///                         parameters. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(SqlStatement[] input)
        {
            int rowAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowAffected = ExecuteNonQuery(input, dbConnection, (IDbTransaction)null);


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
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        /// SqlStatement[] sql=new  SqlStatement[1];
        /// sql[0]=new SqlStatement();
        /// sql[1].CommandText="INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')";
        /// sql[1].CommandType=CommandType.Text;
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery(sql,con);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    sql[1].CommandText="INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)";
        /// sql[1].CommandType=CommandType.Text;
        /// sql[1].Params=aParams;
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery(sql,con);
        ///    }

        /// }
        /// }
        /// </code>
        ///</example>
        /// <param name="input">        Collection of SQL Statements With CommandText,CommandType and
        ///                             Optional parameters. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(SqlStatement[] input, IDbConnection connection)
        {
            int rowAffected = 0;
            try
            {
                rowAffected = ExecuteNonQuery(input, connection, (IDbTransaction)null);
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
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbTransaction trans= con.BeginTransaction())
        ///   {
        ///   try{
        /// SqlStatement[] sql=new  SqlStatement[1];
        /// sql[0]=new SqlStatement();
        /// sql[1].CommandText="INSERT INTO EMPLOYEE(FIRSTNAME) VALUES('XYX')";
        /// sql[1].CommandType=CommandType.Text;
        ///  int rowsEffected =dbContext.ICommands.ExecuteNonQuery(sql,con);
        ///   //or
        ///   WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="F";
        ///   aParams[0].ParameterValue="XXX";
        ///    sql[1].CommandText="INSERT INTO EMPLOYEE(FIRSTNAME) VALUES(@F)";
        /// sql[1].CommandType=CommandType.Text;
        /// sql[1].Params=aParams;
        ///    rowsEffected=dbContext.ICommands.ExecuteNonQuery(sql,con);
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
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="input">        Collection of SQL Statements With CommandText,CommandType and
        ///                             Optional parameters. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(SqlStatement[] input, IDbTransaction transaction)
        {
            int rowAffected = 0;
            try
            {
                rowAffected = ExecuteNonQuery(input, transaction.Connection, transaction);
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
        /// <param name="input">        Collection of SQL Statements With CommandText,CommandType and
        ///                             Optional parameters. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int ExecuteNonQuery(SqlStatement[] input, IDbConnection connection, IDbTransaction transaction)
        {

            int rowsAffected = 0;
            try
            {
                foreach (SqlStatement theSql in input)
                {

                    rowsAffected += ExecuteNonQuery(theSql.CommandText, theSql.CommandType, connection, transaction, theSql.Params);
                }
            }
            catch
            {

                throw;
            }
            return rowsAffected;

        }


        /// <summary>   Bulk insert. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select FirstName from employee");
        ///   <!--Dictionary<string,string> cols=new Dictionary<string,string>();-->//
        ///  <!-- cols.Add("FirstName","FirstName");-->//
        ///  //int rowsEffected =dbContext.ICommands.BulkInsert(dt,"Employee",100,30,cols);//
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dt">           . </param>
        /// <param name="tableName">    . </param>
        /// <param name="batchSize">    . </param>
        /// <param name="timeOut">      . </param>
        /// <param name="columnNames">  . </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int BulkInsert(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowsAffected = BulkInsert(dt, tableName, batchSize, timeOut, columnNames, dbConnection, (IDbTransaction)null);


                }
            }
            catch
            {

                throw;
            }
            return rowsAffected;
        }


        /// <summary>   Bulk insert. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select FirstName from employee",con);
        ///   <!--Dictionary<string,string> cols=new Dictionary<string,string>();-->//
        ///   <!-- cols.Add("FirstName","FirstName");-->//
        ///   //int rowsEffected =dbContext.ICommands.BulkInsert(dt,"Employee",100,30,cols,con);//
        ///  }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dt">           . </param>
        /// <param name="tableName">    . </param>
        /// <param name="batchSize">    . </param>
        /// <param name="timeOut">      . </param>
        /// <param name="columnNames">  . </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int BulkInsert(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = BulkInsert(dt, tableName, batchSize, timeOut, columnNames, connection, (IDbTransaction)null);
            }
            catch
            {

                throw;
            }
            return rowsAffected;
        }


        /// <summary>   Bulk insert. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        ///   WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   using(IDbConnection con=dbContext.ICommands.CreateConnection())
        ///   {
        ///   using(IDbConnection trans=con.BeginTransaction())
        ///   {
        ///   try{
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select FirstName from employee",trans);
        ///   <!--Dictionary<string,string> cols=new Dictionary<string,string>();-->//
        ///   <!-- cols.Add("FirstName","FirstName");-->//
        ///  //int rowsEffected =dbContext.ICommands.BulkInsert(dt,"Employee",100,30,cols,trans);//
        ///  trans.Commit();
        ///  }
        ///  catch{
        ///  trans.Rollback();
        ///  }
        ///  
        ///  }
        ///  }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dt">           . </param>
        /// <param name="tableName">    . </param>
        /// <param name="batchSize">    . </param>
        /// <param name="timeOut">      . </param>
        /// <param name="columnNames">  . </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        public virtual int BulkInsert(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDbTransaction transaction)
        {

            int rowsAffected = 0;
            try
            {
                rowsAffected = BulkInsert(dt, tableName, batchSize, timeOut, columnNames, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }


        /// <summary>   Bulk insert. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dt">           . </param>
        /// <param name="tableName">    . </param>
        /// <param name="batchSize">    . </param>
        /// <param name="timeOut">      . </param>
        /// <param name="columnNames">  . </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>


        protected virtual int BulkInsert(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDbConnection connection, IDbTransaction transaction)
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

                    string commandText = string.Empty;
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
        ///   Try
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select Id,FirstName,LastName from employee",connection);        
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;();
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///   int rowsEffected =dbContext.ICommands.BulkUpdate(dt,"Employee",100,30,columnNames,primaryColumns);//
        ///   transaction.Commit();
        ///   }
        ///  catch
        ///  {
        ///     trans.Rollback();
        ///  }
        /// } 
        /// } 
        /// }  
        /// </code>
        ///</example> 
        /// <remarks>  3/23/2018 . </remarks>
        ///
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param>
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated . </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkUpdate(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowsAffected = BulkUpdate(dt, tableName, batchSize, timeOut, columnNames, primaryColumns, dbConnection, (IDbTransaction)null);


                }
            }
            catch
            {

                throw;
            }
            return rowsAffected;
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
        ///   Try
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select Id,FirstName,LastName from employee",connection);
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;()
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///   int rowsEffected =dbContext.ICommands.BulkUpdate(dt,"Employee",100,30,columnNames,primaryColumns,connection);//
        ///   transaction.Commit();
        ///   }
        ///  catch
        ///  {
        ///     trans.Rollback();
        ///  }
        /// }       
        /// }  
        /// }  
        /// </code>
        ///</example> 
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param>
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated . </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>
        /// <param name="connection"> Active Connection Object . </param>        
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkUpdate(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = BulkUpdate(dt, tableName, batchSize, timeOut, columnNames, primaryColumns, connection, (IDbTransaction)null);
            }
            catch
            {

                throw;
            }
            return rowsAffected;
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
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;()
        ///   columnNames.Add("FirstName","FirstName");
        ///   columnNames.Add("LastName","LastName");
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///   int rowsEffected =dbContext.ICommands.BulkUpdate(dt,"Employee",100,30,columnNames,primaryColumns,transaction);//
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
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated . </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>        
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkUpdate(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns, IDbTransaction transaction)
        {

            int rowsAffected = 0;
            try
            {
                rowsAffected = BulkUpdate(dt, tableName, batchSize, timeOut, columnNames, primaryColumns, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;
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
        ///   Dictionary&lt;string,string&gt; columnNames=new Dictionary&lt;string,string&gt;()
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
        /// <param name="columnNames"> Column list which represents the Set options (columns) for records to be updated . </param>
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>
        /// <param name="connection"> Active Connection Object . </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkUpdate(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> columnNames, IDictionary<string, string> primaryColumns, IDbConnection connection, IDbTransaction transaction)
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

                    string commandText = string.Empty;
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
        ///   Try
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select Id from employee",connection);
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///   int rowsEffected =dbContext.ICommands.BulkDelete(dt,"Employee",100,30,primaryColumns);//       
        ///   }
        ///  catch
        ///  {        
        ///  }        
        /// }  
        /// } 
        /// }
        /// </code>
        ///</example> 
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param> 
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkDelete(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowsAffected = BulkDelete(dt, tableName, batchSize, timeOut, primaryColumns, dbConnection, (IDbTransaction)null);


                }
            }
            catch
            {

                throw;
            }
            return rowsAffected;
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
        ///   Try
        ///   {
        ///   DataTable dt =dbContext.ICommands.ExecuteDataTable("select Id from employee",connection);
        ///   Dictionary&lt;string, string&gt; primaryColumns = new Dictionary&lt;string, string&gt;();
        ///   primaryColumns.Add("Id", "Id");
        ///   int rowsEffected =dbContext.ICommands.BulkDelete(dt,"Employee",100,30,primaryColumns,connection);//

        ///   }
        ///  catch
        ///  {

        ///  }
        /// }         
        /// }  
        /// }  
        /// </code>
        ///</example> 
        /// <param name="dt"> Data Table which contains data. </param>
        /// <param name="tableName"> Name of the Table of database. </param>
        /// <param name="batchSize"> Size of batch to be processed. </param>
        /// <param name="timeOut">  Time out value in seconds for command execution. </param> 
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>
        /// <param name="connection"> Active Connection Object . </param>

        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkDelete(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = BulkDelete(dt, tableName, batchSize, timeOut, primaryColumns, connection, (IDbTransaction)null);
            }
            catch
            {

                throw;
            }
            return rowsAffected;
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
        ///   int rowsEffected =dbContext.ICommands.BulkDelete(dt,"Employee",100,30,primaryColumns,transaction);//
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
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>         
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkDelete(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = BulkDelete(dt, tableName, batchSize, timeOut, primaryColumns, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;
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
        /// <param name="primaryColumns"> Column list which represents the Where clause filters. </param>
        /// <param name="connection"> Active Connection Object . </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   No Of Rows Affected. </returns>
        public virtual int BulkDelete(DataTable dt, string tableName, int batchSize, int timeOut, IDictionary<string, string> primaryColumns, IDbConnection connection, IDbTransaction transaction)
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

                    string commandText = string.Empty;
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
    
    }
}
