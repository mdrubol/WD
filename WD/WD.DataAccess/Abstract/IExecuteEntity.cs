// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-17-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="IExecuteEntity.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;
using WD.DataAccess.Parameters;
using WD.DataAccess.QueryProviders;
using System.Reflection;
using WD.DataAccess.Enums;

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
    
    /// <summary>   IExecuteEntity is an abstract class for Generic methods. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public abstract class IExecuteEntity : IExecuteDataTable
    {

           

        #region Constructor

        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public IExecuteEntity() : base() { }
        #endregion

        
        /// <summary>   Gets the list. </summary>
        ///
        /// <remarks>    </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   List &#60;Employee&#62; empList = dbContext.GetList&#60;Employee&#62;();
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    . </typeparam>
        ///
        /// <returns>   The list. </returns>
        

        public virtual List<T> GetList<T>()
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(dbConnection, (IDbTransaction)null);
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(dbConnection);
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="connection">   . </param>
        ///
        /// <returns>   The list. </returns>
        

        public virtual List<T> GetList<T>(IDbConnection connection)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(connection, (IDbTransaction)null);

            }
            catch
            {

                throw;
            }
            return ts;
        }

        
        /// <summary>   Gets a list. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        /// using(IDbTransaction trans=dbConnection.BeginTransaction())
        /// {
        /// try
        /// {
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(trans);
        /// trans.Commit();
        /// }
        /// catch
        /// {
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    . </typeparam>
        /// <param name="transaction">  . </param>
        ///
        /// <returns>   The list. </returns>
        

        public virtual List<T> GetList<T>(IDbTransaction transaction)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(transaction.Connection, transaction);

            }
            catch
            {

                throw;
            }
            return ts;
        }

        
        /// <summary>   Gets List of Entity object as per the where clause and active connection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="connection">   . </param>
        /// <param name="transaction">  . </param>
        ///
        /// <returns>   List of Entity. </returns>
        

        private List<T> GetList<T>(IDbConnection connection, IDbTransaction transaction)
        {
            List<T> ts = new List<T>();
            try
            {

                using (IDbCommand dbCommand = CreateCommand(Helpers.HelperUtility.QueryBuilder<T>().Select(), CommandType.Text))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (IDataReader dataReader = dbCommand.ExecuteReader(CommandBehavior.KeyInfo))
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
            catch
            {

                throw;
            }
            return ts;
        }

        
        /// <summary>   Gets List of Entity object as per the where clause. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(x=&gt;x.FirstName="XYZ");
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        ///
        /// <returns>   List of Entity. </returns>
        

        public virtual List<T> GetList<T>(Expression<Func<T, bool>> predicate)
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(predicate, dbConnection, (IDbTransaction)null);
                   
                }

            }
            catch
            {

                throw;
            }
            return ts;
        }

        
        /// <summary>   Gets List of Entity object as per the where clause and active connection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",dbConnection);
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   List of Entity. </returns>
        

        public virtual List<T> GetList<T>(Expression<Func<T, bool>> predicate, IDbConnection connection)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(predicate, connection, (IDbTransaction)null);

            }
            catch
            {

                throw;
            }
            return ts;
        }

        
        /// <summary>   Gets List of Entity object as per the where clause and active Transaction. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///  using(IDbTransaction trans=dbConnection.BeginTransaction())
        ///  {
        ///  try{
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",trans);
        /// trans.Commit();
        /// }
        /// catch{
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   List of Entity. </returns>
        

        public virtual List<T> GetList<T>(Expression<Func<T, bool>> predicate, IDbTransaction transaction)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(predicate, transaction.Connection, transaction);

            }
            catch
            {

                throw;
            }
            return ts;
        }

        
        /// <summary>
        /// Gets List of Entity object as per the where clause, active connection and active transaction.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   List of Entity. </returns>
        

        private List<T> GetList<T>(Expression<Func<T, bool>> predicate, IDbConnection connection, IDbTransaction transaction)
        {
            List<T> ts = new List<T>();
            try
            {

                using (IDbCommand dbCommand = CreateCommand(Helpers.HelperUtility.QueryBuilder<T>().Where(predicate).Select(), CommandType.Text))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (IDataReader dataReader = dbCommand.ExecuteReader(CommandBehavior.KeyInfo))
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
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return ts;
        }
      /// <summary>
      /// 
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="pageNumber"></param>
      /// <param name="pageSize"></param>
      /// <param name="totalRecordsQuery"></param>
      /// <param name="predicate"></param>
      /// <param name="orderBy"></param>
      /// <param name="sortBy"></param>
      /// <returns></returns>
        protected virtual string GetPagingQuery<T>(int pageNumber, int pageSize, out string totalRecordsQuery, System.Linq.Expressions.Expression<Func<T, bool>> predicate =null, SortOption sortBy = SortOption.ASC, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            WD.DataAccess.QueryProviders.QueryBuilder<T> qbe = new WD.DataAccess.QueryProviders.QueryBuilder<T>();
            qbe.Where(predicate);
            qbe.OrderBy(orderBy.ToArray());
            totalRecordsQuery = String.Format("SELECT Count(1) FROM {0} {1}", HelperUtility.GetTableName<T>(), qbe.Where());
            return String.Format("SELECT * FROM (SELECT  ROW_NUMBER() OVER ({0} {1}) R,{2}) A WHERE  R BETWEEN {3} AND {4}", qbe.OrderBy(), sortBy.ToString(), qbe.Select().Replace(qbe.OrderBy(), string.Empty).Replace("SELECT", string.Empty), ((pageNumber - 1) * pageSize) + 1, pageNumber * pageSize);
        }


        #region Orderby

        /// <summary>   Gets the list. </summary>
        ///
        /// <remarks>    </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   List &#60;Employee&#62; empList = dbContext.GetList&#60;Employee&#62;(WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    . </typeparam>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        ///
        /// <returns>   The list. </returns>

        public virtual List<T> GetList<T>(WD.DataAccess.Enums.SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(orderBy, sortBy, dbConnection, (IDbTransaction)null);
                }

            }
            catch
            {

                throw;
            }
            return ts;
        }

        /// <summary>   Gets List of Entity object as per the where clause and active connection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="connection">   . </param>
        /// <param name="transaction">  . </param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        /// <returns>   List of Entity. </returns>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(dbConnection,WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="connection">   . </param>
        ///<param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        /// 
        /// <returns>   The list. </returns>


        public virtual List<T> GetList<T>(IDbConnection connection,WD.DataAccess.Enums.SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(orderBy, sortBy, connection, (IDbTransaction)null);

            }
            catch
            {

                throw;
            }
            return ts;
        }


        /// <summary>   Gets a list. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        /// using(IDbTransaction trans=dbConnection.BeginTransaction())
        /// {
        /// try
        /// {
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(trans,WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// trans.Commit();
        /// }
        /// catch
        /// {
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    . </typeparam>
        /// <param name="transaction">  . </param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        ///
        /// <returns>   The list. </returns> object>>[]


        public virtual List<T> GetList<T>(IDbTransaction transaction,WD.DataAccess.Enums.SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy )
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(orderBy, sortBy, transaction.Connection, transaction);

            }
            catch
            {

                throw;
            }
            return ts;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderBy"></param>
        /// <param name="sortBy"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        protected List<T> GetList<T>(System.Linq.Expressions.Expression<Func<T, object>>[] orderBy, WD.DataAccess.Enums.SortOption sortBy, IDbConnection connection, IDbTransaction transaction)
        {
            List<T> ts = new List<T>();
            try
            {

                using (IDbCommand dbCommand = CreateCommand(Helpers.HelperUtility.QueryBuilder<T>().OrderBy(orderBy).Select() + " " + sortBy.ToString(), CommandType.Text))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (IDataReader dataReader = dbCommand.ExecuteReader(CommandBehavior.KeyInfo))
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
            catch
            {

                throw;
            }
            return ts;
        }

       

        /// <summary>   Gets List of Entity object as per the where clause. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        ///
        /// <returns>   List of Entity. </returns>


        public virtual List<T> GetList<T>(Expression<Func<T, bool>> predicate, WD.DataAccess.Enums.SortOption sortBy,  params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(predicate, orderBy, sortBy, dbConnection, (IDbTransaction)null);

                }

            }
            catch
            {

                throw;
            }
            return ts;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="sortBy"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        protected List<T> GetList<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] orderBy, SortOption sortBy, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            List<T> ts = new List<T>();
            try
            {

                using (IDbCommand dbCommand = CreateCommand(Helpers.HelperUtility.QueryBuilder<T>().Where(predicate).OrderBy(orderBy).Select() + " " + sortBy.ToString(), CommandType.Text))
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.Transaction = dbTransaction;
                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                    }
                    using (IDataReader dataReader = dbCommand.ExecuteReader(CommandBehavior.KeyInfo))
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
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return ts;
        }


        /// <summary>   Gets List of Entity object as per the where clause and active connection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",dbConnection,WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName);
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        ///
        /// <returns>   List of Entity. </returns>


        public virtual List<T> GetList<T>(IDbConnection dbConnection,Expression<Func<T, bool>> predicate, SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(predicate,orderBy, sortBy, dbConnection, (IDbTransaction)null);

            }
            catch
            {

                throw;
            }
            return ts;
        }


        /// <summary>   Gets List of Entity object as per the where clause and active Transaction. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///  using(IDbTransaction trans=dbConnection.BeginTransaction())
        ///  {
        ///  try{
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",trans,WD.DataAccess.Enums.SortOption.ASC,x=&gt;x.FirstName );
        /// trans.Commit();
        /// }
        /// catch{
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy"></param>
        ///
        /// <returns>   List of Entity. </returns>


        public virtual List<T> GetList<T>(IDbTransaction dbTransaction,Expression<Func<T, bool>> predicate, SortOption sortBy, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy )
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(predicate, orderBy, sortBy,dbTransaction.Connection, dbTransaction);

            }
            catch
            {

                throw;
            }
            return ts;
        }


        #endregion

        #region Paging

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
        ///     ICommands com = new DbContext().ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(1, 10, out totalCount);
        /// }
        /// } 
        /// </code>
        /// </example>

        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        

        /// <returns></returns>
        public virtual List<T> GetList<T>(int pageNumber, int pageSize, out int totalCount)
        {
            List<T> ts = new List<T>();
            string totalRecordsQuery = "";
            string theSQL = GetPagingQuery<T>(pageNumber, pageSize, out totalRecordsQuery);
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(theSQL, dbConnection);
                    using (IDbCommand dbCommand = CreateCommand(totalRecordsQuery, CommandType.Text))
                    {
                        dbCommand.Connection = dbConnection;
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }
                        totalCount = Convert.ToInt32(dbCommand.ExecuteScalar());
                    }

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
        ///     ICommands com = new DbContext().ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(1, 10, out totalCount, X =&gt; X.Address == "KL");
        /// }
        /// } 
        /// </code>
        /// </example>

        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>

        /// <returns></returns>
        public virtual List<T> GetList<T>(int pageNumber, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            List<T> ts = new List<T>();
            string totalRecordsQuery = "";
            string theSQL = GetPagingQuery<T>(pageNumber, pageSize, out totalRecordsQuery, predicate);
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(theSQL, dbConnection);
                    using (IDbCommand dbCommand = CreateCommand(totalRecordsQuery, CommandType.Text))
                    {
                        dbCommand.Connection = dbConnection;
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }
                        totalCount = Convert.ToInt32(dbCommand.ExecuteScalar());
                    }

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
        ///     ICommands com = new DbContext().ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(1, 10, out totalCount, X =&gt; X.Address == "KL",WD.DataAccess.Enums.SortOption.ASC,y=&gt;y.Country);
        /// }
        /// } 
        /// </code>
        /// </example>
        
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy">Expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int pageNumber, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> predicate,SortOption sortBy,params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            string totalRecordsQuery = "";
            string theSQL = GetPagingQuery<T>(pageNumber, pageSize, out totalRecordsQuery, predicate, sortBy ,orderBy);
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(theSQL, dbConnection);
                    using (IDbCommand dbCommand = CreateCommand(totalRecordsQuery, CommandType.Text))
                    {
                        dbCommand.Connection = dbConnection;
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }
                        totalCount = Convert.ToInt32(dbCommand.ExecuteScalar());
                    }
  
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
        ///     ICommands com = new DbContext().ICommands;   
        ///     List&lt;Employee&gt; empLst = com.GetList&lt;Employee&gt;(1, 10, out totalCount, WD.DataAccess.Enums.SortOption.ASC,y=&gt;y.Country);
        /// }
        /// } 
        /// </code>
        /// </example>

        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber">Number of page which needs to be shown</param>
        /// <param name="pageSize"> Indicates Number of rows per page </param>
        /// <param name="totalCount">Total Number of records</param>
        /// <param name="predicate">Filter expression</param>
        /// <param name="sortBy">value of WD.DataAccess.Enums.SortOption. ASC or DESC</param>
        /// <param name="orderBy">Expression</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(int pageNumber, int pageSize, out int totalCount, SortOption sortBy, params System.Linq.Expressions.Expression<Func<T, object>>[] orderBy)
        {
            List<T> ts = new List<T>();
            string totalRecordsQuery = "";
            string theSQL = GetPagingQuery<T>(pageNumber, pageSize, out totalRecordsQuery, null, sortBy, orderBy);
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(theSQL, dbConnection);
                    using (IDbCommand dbCommand = CreateCommand(totalRecordsQuery, CommandType.Text))
                    {
                        dbCommand.Connection = dbConnection;
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }
                        totalCount = Convert.ToInt32(dbCommand.ExecuteScalar());
                    }

                }

            }
            catch
            {

                throw;
            }
            return ts;
        }
        #endregion

        /// <summary>   Gets Entity object as per the where clause. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.FirstName="XYZ");
        /// 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        ///
        /// <returns>   Entity Object. </returns>
        

        public virtual T GetEntity<T>(Expression<Func<T, bool>> predicate)
        {
            T t = default(T);
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    t = GetEntity<T>(predicate, dbConnection, (IDbTransaction)null);
                   
                }
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>   Gets Entity object as per the where clause and active connection. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///    Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",dbConnection);
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   The entity. </returns>
        

        public virtual T GetEntity<T>(Expression<Func<T, bool>> predicate, IDbConnection connection)
        {
            T t = default(T);
            try
            {
                t = GetEntity<T>(predicate, connection, (IDbTransaction)null);
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>   Gets Entity object as per the where clause and active transaction. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        /// using(IDbTransaction trans=dbConnection.BeginTransaction())
        /// {
        /// try{
        ///    Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",trans);
        ///   trans.Commit();
        ///   }
        ///   catch{
        ///   trans.Rollback();
        ///   }
        ///  }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Entity Object. </returns>
        

        public virtual T GetEntity<T>(Expression<Func<T, bool>> predicate, IDbTransaction transaction)
        {
            T t = default(T);
            try
            {
                t = GetEntity<T>(predicate, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>
        /// Gets Entity object as per the where clause, active connection and active transaction.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Entity Object. </returns>
        

        private T GetEntity<T>(Expression<Func<T, bool>> predicate, IDbConnection connection, IDbTransaction transaction)
        {
            T t = default(T);
            try
            {
              
                using (IDbCommand dbCommand = CreateCommand(Helpers.HelperUtility.QueryBuilder<T>().Where(predicate).Select(), CommandType.Text))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (IDataReader dataReader = dbCommand.ExecuteReader())
                    {

                        using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                        {
                            while (dataReader.Read())
                            {
                                t = (mapper.MapFrom(dataReader));
                            }
                        }
                    }

                }

            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return t;
        }

        
        /// <summary>   Get an Item for CommandText and Optional Parameters. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// 
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee");
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",aParams);
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Entity. </returns>
        

        public virtual T GetEntity<T>(string commandText, DBParameter[] aParams = null)
        {
            T t = default(T);
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    t = GetEntity<T>(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);
                   
                }
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>   Get an Item  for CommandText, active Connection and Optional Parameters. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///    Employee emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee",dbConnection);
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",dbConnection,aParams);
        ///   }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Entity. </returns>
        

        public virtual T GetEntity<T>(string commandText, IDbConnection connection, DBParameter[] aParams = null)
        {
            T t = default(T);
            try
            {

                t = GetEntity<T>(commandText, CommandType.Text, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>   Get an Item  for CommandText, active Transaction and Optional Parameters. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///  using(IDbTransaction trans=dbConnection.BeginTransaction())
        /// {
        /// try{
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee",trans);
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",trans,aParams);
        ///   trans.Commit();
        ///   }
        ///  catch{
        ///  trans.Rollback();
        ///  }
        ///  }
        ///   }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Entity. </returns>
        

        public virtual T GetEntity<T>(string commandText, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            T t = default(T);
            try
            {
                t = GetEntity<T>(commandText, CommandType.Text, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>   Get an Item  for CommandText, CommandType and Optional Parameters. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee",CommandType.Text);
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,aParams);
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Entity. </returns>
        

        public virtual T GetEntity<T>(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            T t = default(T);
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    t = GetEntity<T>(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);
                   
                }
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>
        /// Get an Item  for CommandText, CommandType, active Connection and Optional Parameters.
        /// </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee",CommandType.Text,dbConnection);
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   emp=dbContext.GetEntity&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,dbConnection,aParams);
        ///   }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Entity. </returns>
        

        public virtual T GetEntity<T>(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            T t = default(T);
            try
            {
                t = GetEntity<T>(commandText, commandType, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>
        /// Get an Item  for CommandText, CommandType, active Transaction and Optional Parameters.
        /// </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        /// using(IDbTransaction trans=dbConnection.BeginTransaction())
        /// {
        /// try{
        ///    Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.FirstName="XYZ",trans);
        ///   trans.Commit();
        ///   }
        ///   catch{
        ///   trans.Rollback();
        ///   }
        ///  }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   Entity. </returns>
        

        public virtual T GetEntity<T>(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            T t = default(T);
            try
            {
                t = GetEntity<T>(commandText, commandType, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return t;
        }

        
        /// <summary>
        /// Get an Item  for CommandText, CommandType, active Connection, active Transaction and
        /// Parameters.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      Collection of Optional Parameters. </param>
        ///
        /// <returns>   Entity. </returns>
        

        private T GetEntity<T>(string commandText, CommandType commandType, IDbConnection connection, IDbTransaction transaction, DBParameter[] aParams)
        {

            T t = default(T);
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
                    using (IDataReader dataReader = dbCommand.ExecuteReader(CommandBehavior.KeyInfo))
                    {

                        using (var mapper = new Helpers.DataReaderMapper<T>(dataReader))
                        {
                            while (dataReader.Read())
                            {
                                t = (mapper.MapFrom(dataReader));
                            }
                        }
                    }

                }


            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);

                throw;
            }
            return t;
        }

        
        /// <summary>   Get List of Items for CommandText and Optional Parameters. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee")
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",aParams);
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   List of Entities. </returns>
        

        public virtual List<T> GetList<T>(string commandText, DBParameter[] aParams = null)
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(commandText, CommandType.Text, dbConnection, (IDbTransaction)null, aParams);
                   
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        
        /// <summary>
        /// Get List of Items for CommandText, active Connection and Optional Parameters.
        /// </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee",dbConnection);
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",dbConnection,aParams);
        ///   }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   List of Entities. </returns>
        

        public virtual List<T> GetList<T>(string commandText, IDbConnection connection, DBParameter[] aParams = null)
        {
            List<T> ts = new List<T>();
            try
            {

                ts = GetList<T>(commandText, CommandType.Text, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return ts;
        }

        
        /// <summary>
        /// Get List of Items for CommandText, active Transaction and Optional Parameters.
        /// </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///  using(IDbTransaction trans=dbConnection.BeginTransaction())
        /// {
        /// try{
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee",trans)
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///  empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",trans,aParams);
        ///   trans.Commit();
        ///   }
        ///  catch{
        ///  trans.Rollback();
        ///  }
        ///  }
        ///   }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   List of Entities. </returns>
        

        public virtual List<T> GetList<T>(string commandText, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(commandText, CommandType.Text, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return ts;
        }

        
        /// <summary>   Get List of Items for CommandText, CommandType and Optional Parameters. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee",CommandType.Text);
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,aParams);
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   List of Entities. </returns>
        

        public virtual List<T> GetList<T>(string commandText, CommandType commandType, DBParameter[] aParams = null)
        {
            List<T> ts = new List<T>();
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    ts = GetList<T>(commandText, commandType, dbConnection, (IDbTransaction)null, aParams);
                   
                }
            }
            catch
            {
                throw;
            }
            return ts;
        }

        
        /// <summary>
        /// Get List of Items for CommandText, CommandType, active Connection and Optional Parameters.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   List of Entities. </returns>
        

        public virtual List<T> GetList<T>(string commandText, CommandType commandType, IDbConnection connection, DBParameter[] aParams = null)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(commandText, commandType, connection, (IDbTransaction)null, aParams);
            }
            catch
            {
                throw;
            }
            return ts;
        }
        
        /// <summary>
        /// Get List of Items for CommandText, CommandType, active Transaction and Optional Parameters.
        /// </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using(IDbConnection dbConnection=dbContext.ICommands.CreateConnection())
        /// {
        ///  using(IDbTransaction trans=dbConnection.BeginTransaction())
        /// {
        /// try{
        ///   List&lt;Employee&gt; empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee",CommandType.Text,trans);
        ///   or
        ///      WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        ///   aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        ///   aParams[0].ParameterName="FirstName";
        ///   aParams[0].ParameterValue="first name";
        ///   empList=dbContext.GetList&lt;Employee&gt;("SELECT columnNames from tempEmployee WHERE FirstName like @F",CommandType.Text,trans,aParams);
        ///   trans.Commit();
        ///   }
        ///  catch{
        ///    trans.Rollback();
        ///    }
        ///  }
        ///   }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      (Optional) Collection of Optional Parameters. </param>
        ///
        /// <returns>   List of Entities. </returns>
        

        public virtual List<T> GetList<T>(string commandText, CommandType commandType, IDbTransaction transaction, DBParameter[] aParams = null)
        {
            List<T> ts = new List<T>();
            try
            {
                ts = GetList<T>(commandText, commandType, transaction.Connection, transaction, aParams);
            }
            catch
            {
                throw;
            }
            return ts;
        }

        
        /// <summary>
        /// Get List of Items for CommandText, CommandType,  active Connection, active Transaction and
        /// Parameters.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="commandText">  Open Sql Statement or Procedure Name. </param>
        /// <param name="commandType">  CommandType for Text or StoredProcedure (1 or 4) </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        /// <param name="aParams">      Collection of Optional Parameters. </param>
        ///
        /// <returns>   List of Entities. </returns>
        

        public virtual List<T> GetList<T>(string commandText, CommandType commandType, IDbConnection connection, IDbTransaction transaction, DBParameter[] aParams)
        {

            List<T> ts = new List<T>();
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
                    using (IDataReader dataReader = dbCommand.ExecuteReader(CommandBehavior.KeyInfo))
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
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return ts;
        }

        
        /// <summary>   Insert item for an Entity. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  Employee emp=new Employee(){
        ///     FirstName ="ABCD",
        ///     MiddleName ="",
        ///     LastName ="ACBD",
        ///     DateOfBirth =  new DateTime(1986,07,07)
        ///     
        ///  };
        ///  int record=dbContext.ICommands.Insert(emp);
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Class. </typeparam>
        /// <param name="input">    Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Insert<T>(T input)
        {
            int rowsAffected = 0;
            try
            {
              
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowsAffected = Insert<T>(input, dbConnection, (IDbTransaction)null);
                   
                }
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>   Insert item for an Entity and active Connection. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  Employee emp=new Employee(){
        ///     FirstName ="ABCD",
        ///     MiddleName ="",
        ///     LastName ="ACBD",
        ///     DateOfBirth =  new DateTime(1986,07,07)
        ///     
        ///  };
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
        ///  int record=dbContext.ICommands.Insert(emp,con);
        ///  }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Insert<T>(T input, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Insert<T>(input, connection, (IDbTransaction)null);
            }
            catch
            {
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Insert item for an Entity and active Transaction. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  Employee emp=new Employee(){
        ///     FirstName ="ABCD",
        ///     MiddleName ="",
        ///     LastName ="ACBD",
        ///     DateOfBirth =  new DateTime(1986,07,07)
        ///     
        ///  };
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
        ///  using(IDbTransaction trans=con.BeginTransaction())
        ///  {
        ///  try{
        ///  int record=dbContext.ICommands.Insert(emp,con);
        ///  trans.Commit();
        ///  }
        ///  catch{
        ///  trans.Rollback();
        ///  }
        ///  }
        ///  }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Insert<T>(T input, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Insert<T>(input, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Insert item for an Entity, active Connection and active Transaction. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        private int Insert<T>(T input, IDbConnection connection, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {

                using (IDbCommand dbCommand = Helpers.HelperUtility.InsertCommandText<T>(input, CreateCommand(), DBProvider))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    rowsAffected = dbCommand.ExecuteNonQuery();
                    WD.DataAccess.Logger.ILogger.Info(dbCommand.CommandText);
                }


            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Update item or Items for an Entity and where clause. </summary>
        ///
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1); 
        ///   int record =dbContext.ICommands.Update(emp,x=&gt;x.EmployeeId=1);
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="predicate">    where clause. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Update<T>(T input, Expression<Func<T, bool>> predicate)
         {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowsAffected = Update<T>(input, predicate, dbConnection, (IDbTransaction)null);
                   
                }
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>   Update item or Items for an Entity, where clause and active Connection. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
        ///      Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1,con); 
        ///      int record =dbContext.ICommands.Update&lt;Employee&gt;(emp,x=&gt;x.EmployeeId=1,con);
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Update<T>(T input, Expression<Func<T, bool>> predicate, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Update<T>(input, predicate, connection, (IDbTransaction)null);
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>   Update item or Items for an Entity, where clause and active Transaction. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
        ///  using(IDbTransaction trans =con.BeginTransaction())
        ///  {
        ///  try{
        ///      Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1,trans);  
        ///      int record =dbContext.ICommands.Update&lt;Employee&gt;(emp,x=&gt;x.EmployeeId=1,trans); 
        /// trans.Commit();
        /// }catch{
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="predicate">    where clause. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Update<T>(T input, Expression<Func<T, bool>> predicate, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Update<T>(input, predicate, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>
        /// Update item or Items for an Entity, where clause, active Connection and active Transaction.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        private int Update<T>(T input, Expression<Func<T, bool>> predicate, IDbConnection connection, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {

                using (IDbCommand dbCommand = Helpers.HelperUtility.UpdateCommandText<T>(input, predicate, CreateCommand(), DBProvider))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    rowsAffected = dbCommand.ExecuteNonQuery();
                    WD.DataAccess.Logger.ILogger.Info(dbCommand.CommandText);
                }
            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);

                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>   Update item for Schema, TableName, where clause and Entity. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///      Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1,trans); 
        ///      int record =dbContext.ICommands.Update&lt;Employee&gt;(&quot;dbo&quot;, &quot;Employee&quot;, &quot;EmployeeId=1&quot;, emp); 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="schemaName">   . </param>
        /// <param name="tableName">    . </param>
        /// <param name="whereClause">  . </param>
        /// <param name="input">        Object of Entity. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public int Update<T>(string schemaName, string tableName, string whereClause, T input)
        {
            int rowsAffected = 0;
            try
            {

                using (IDbConnection dbConnection = CreateConnection())
                {
                    using (IDbCommand dbCommand = Helpers.HelperUtility.UpdateCommandText<T>(input, CreateCommand(), whereClause, DBProvider))
                    {
                        dbCommand.Connection = dbConnection;
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }
                        rowsAffected = dbCommand.ExecuteNonQuery();
                        WD.DataAccess.Logger.ILogger.Info(dbCommand.CommandText);
                    }
                   
                }

            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);

                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Update item for an Entity. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
       
        ///      Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1); 
        ///      int record =dbContext.ICommands.Update&lt;Employee&gt;(emp); 
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">    Object of Entity. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Update<T>(T input)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    rowsAffected = Update<T>(input, connection, (IDbTransaction)null);
                }
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>   Update item for an Entity and active Connection. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
      
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1,con); 
        ///   int record =dbContext.ICommands.Update&lt;Employee&gt;(emp,con); 
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, Asim Naeem, 8/15/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Update<T>(T input, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Update<T>(input, connection, (IDbTransaction)null);
            }
            catch
            {
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Update item for an Entity and active Transaction. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
        ///  using(IDbTransaction trans =con.BeginTransaction())
        ///  {
        ///  try{
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1,trans); 
        ///   int record =dbContext.ICommands.Update(emp,trans); 
        /// trans.Commit();
        /// }catch{
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Update<T>(T input, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Update<T>(input, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Update item for an Entity, active Connection and active Transaction. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        private int Update<T>(T input, IDbConnection connection, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {

                using (IDbCommand dbCommand = Helpers.HelperUtility.UpdateCommandText<T>(input, CreateCommand(), DBProvider))
                {
                    dbCommand.Connection = connection;
                    dbCommand.CommandType = CommandType.Text;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    rowsAffected = dbCommand.ExecuteNonQuery();
                    WD.DataAccess.Logger.ILogger.Info(dbCommand.CommandText);
                }


            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Deletes item for dynamic Id. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
       
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1); 
        ///   int record =dbContext.ICommands.Delete&lt;Employee&gt;(1); 
       
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="id">   . </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Delete<T>(object id)
        {
            int rowsAffected = 0;
            try
            {

                using (IDbConnection dbConnection = CreateConnection())
                {
                    using (IDbCommand dbCommand = CreateCommand(string.Format("DELETE FROM {0} WHERE {1}=" + HelperUtility.Prefix(DBProvider) + "{2}",
                            HelperUtility.GetTableName<T>(),
                            HelperUtility.GetPrimaryColumn<T>(), HelperUtility.GetPrimaryColumn<T>()), CommandType.Text))
                    {

                        IDataParameter dbParam = dbCommand.CreateParameter();
                        dbParam.ParameterName = HelperUtility.Prefix(DBProvider) + HelperUtility.GetPrimaryColumn<T>();
                        dbParam.Value = id;
                        dbCommand.Parameters.Add(dbParam);
                        dbCommand.Connection = dbConnection;
                        if (dbConnection.State != ConnectionState.Open)
                        {
                            dbConnection.Open();
                        }
                        rowsAffected = dbCommand.ExecuteNonQuery();
                       
                    }
                   
                }

            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>   Deletes entity. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1); 
        ///   int record =dbContext.ICommands.Delete&lt;Employee&gt;(emp); 
      
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">    Object of Entity. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Delete<T>(T input)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowsAffected = Delete<T>(input, dbConnection, (IDbTransaction)null);
                   
                }
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>   Deletes entity for active Connection. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
        
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1,con); 
        ///   int record =dbContext.ICommands.Delete&lt;Employee&gt;(emp,con); 
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Delete<T>(T input, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Delete<T>(input, connection, (IDbTransaction)null);
            }
            catch
            {
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Deletes entity for active Transaction. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        ///  using (IDbConnection con=dbContext.ICommands.CreateConnection())
        ///  {
        ///  using(IDbTransaction trans =con.BeginTransaction())
        ///  {
        ///  try{
        ///   Employee emp=dbContext.GetEntity&lt;Employee&gt;(x=&gt;x.EmployeeId=1,trans); 
        ///   int record =dbContext.ICommands.Delete&lt;Employee&gt;(emp,trans); 
        /// trans.Commit();
        /// }catch{
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Delete<T>(T input, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Delete<T>(input, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Deletes entity for active Connection and active Transaction. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="input">        Object of Entity. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        private int Delete<T>(T input, IDbConnection connection, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbCommand dbCommand = Helpers.HelperUtility.DeleteCommandText<T>(input, CreateCommand(), DBProvider))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    rowsAffected = dbCommand.ExecuteNonQuery();
                    WD.DataAccess.Logger.ILogger.Info(dbCommand.CommandText);
                }
            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);
                throw;
            }
            return rowsAffected;

        }

        
        /// <summary>   Deletes entity or collection of entities for where clause. </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
         ///   int record =dbContext.ICommands.Delete&lt;Employee&gt;(x=&gt;x.EmployeeId==1); 
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Delete<T>(Expression<Func<T, bool>> predicate)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    rowsAffected = Delete<T>(predicate, dbConnection, (IDbTransaction)null);
                   
                }
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>
        /// Deletes entity or collection of entities for where clause and active Connection.
        /// </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection con=dbContext.ICommands.CreateConnection())
        /// {
        ///   int record =dbContext.ICommands.Delete&lt;Employee&gt;(x=&gt;x.EmployeeId==1,con); 
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Delete<T>(Expression<Func<T, bool>> predicate, IDbConnection connection)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Delete<T>(predicate, connection, (IDbTransaction)null);
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>
        /// Deletes entity or collection of entities for where clause and active Transaction.
        /// </summary>
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
        /// WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
        /// using(IDbConnection con =dbContext.ICommands.CreateConnection())
        /// {
        ///  using(IDbTransaction trans=con.BeginTransaction())
        ///  {
        ///  
        ///  try{
        ///        int record =dbContext.ICommands.Delete&lt;Employee&gt;(x=&gt;x.EmployeeId==1,trans); 
        /// trans.Commit();
        /// }
        /// catch{
        /// trans.Rollback();
        /// }
        /// }
        /// }
        /// }
        /// } 
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        public virtual int Delete<T>(Expression<Func<T, bool>> predicate, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = Delete<T>(predicate, transaction.Connection, transaction);
            }
            catch
            {
                throw;
            }
            return rowsAffected;
        }

        
        /// <summary>
        /// Deletes entity or collection of entities for where clause, active Connection and active
        /// Transaction.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    Entity. </typeparam>
        /// <param name="predicate">    where clause. </param>
        /// <param name="connection">   Active Connection Object. </param>
        /// <param name="transaction">  Active Transaction Object. </param>
        ///
        /// <returns>   Rows Affected. </returns>
        

        private int Delete<T>(Expression<Func<T, bool>> predicate, IDbConnection connection, IDbTransaction transaction)
        {
            int rowsAffected = 0;
            try
            {

                using (IDbCommand dbCommand = CreateCommand(Helpers.HelperUtility.DeleteCommandText<T>(predicate, DBProvider), CommandType.Text))
                {
                    dbCommand.Connection = connection;
                    dbCommand.Transaction = transaction;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    rowsAffected = dbCommand.ExecuteNonQuery();
                    WD.DataAccess.Logger.ILogger.Info(dbCommand.CommandText);
                   
                }

            }
            catch (Exception exc)
            {
                Logger.ILogger.Fatal(exc);

                throw;
            }
            return rowsAffected;
        }
       
    }

  


}
