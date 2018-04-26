using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Linq.Expressions;

using WD.DataAccess.Abstract;
using WD.DataAccess.Helpers;
using WD.DataAccess.Context;
using WD.DataAccess.Attributes;
using System.Reflection;
using System.Data.SqlClient;

namespace TestApp
{
    class Program
    {
        //static void Main()
        //{
        //    WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);



        //    ICommands comm = new WD.DataAccess.Context.DbContext("OracleCon").ICommands;


        //    //OPEN QUERY
        //    string theSql = "select * from Test111";
        //    DataTable dt = comm.ExecuteDataTable(theSql);
        //    Dictionary<string, string> col = new Dictionary<string, string>();
        //    col.Add("Alpha", "Alpha");
        //    int rows = dbContext.ICommands.BulkInsert(dt, "Test111", 100, 20, col);

        //    //theSql = " Select a as sourceColumn from testing1";
        //    //DataTable dt = comm.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, theSql);
        //    //Dictionary<string, string> col = new Dictionary<string, string>();
        //    //col.Add("sourceColumn", "a");
        //    //int rows = dbContext.ICommands.BulkInsert(WD.DataAccess.Enums.Databases.BR, dt, "testing1", 100, 20, col);


        //    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);

        //    // databaseand command object
        //    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, dbCommand))
        //    {
        //        //your code here
        //    }
        //    //or
        //    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        //    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1, dbCommand))
        //    {
        //        //your code here
        //    }

        //    //QUERY WITH PARAMETER
        //    theSql = "Select * from employee where EmployeeId=@EmployeeId";

        //    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        //    aParams[0] = new WD.DataAccess.Parameters.DBParameter();

        //    aParams[0].ParameterName = "EmployeeId";
        //    // Or
        //    aParams[0].ParameterName = "@EmployeeId";
        //    aParams[0].ParameterValue = "1";
        //    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);

        //    dbCommand = new System.Data.SqlClient.SqlCommand(theSql);
        //    dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter { ParameterName = "@EmployeeId", Value = "1", DbType = DbType.Int32 });

        //    // databaseand command object
        //    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, dbCommand))
        //    {
        //        //your code here
        //    }
        //    //or
        //    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        //    using (IDataReader reader = dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 2, dbCommand))
        //    {
        //        //your code here
        //    }

        //}


        //static void Main()
        //{
        //    WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(true);

        //    //OPEN QUERY
        //    string theSql = "select * from tempEmployee Where FirstName='abc'";
        //    IDbCommand dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text);

        //    int rows = dbContext.ICommands.ExecuteNonQuery(WD.DataAccess.Enums.Databases.BR, 1, theSql, CommandType.Text, WD.DataAccess.Enums.Transaction.None);
            
        //    // database and command object
        //    Employee emp = dbContext.ICommands.GetEntity<Employee>(WD.DataAccess.Enums.Databases.BR, dbCommand);
        //    //OR
        //    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        //    emp = dbContext.ICommands.GetEntity<Employee>(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);

        //    //QUERY WITH PARAMETER

        //    theSql = "SELECT columnNames from tempEmployee WHERE FirstName like @FirstName";

        //    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[1];
        //    aParams[0] = new WD.DataAccess.Parameters.DBParameter();

        //    aParams[0].ParameterName = "FirstName";
        //    // Or
        //    aParams[0].ParameterName = "@FirstName";
        //    aParams[0].ParameterValue = "abc";
        //    dbCommand = dbContext.ICommands.CreateCommand(theSql, CommandType.Text, aParams);
        //    //Or
        //    dbCommand = new System.Data.SqlClient.SqlCommand(theSql);
        //    dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter { ParameterName = "@FirstName", Value = "abc", DbType = DbType.Int32 });

        //    // database and command object
        //    emp = dbContext.ICommands.GetEntity<Employee>(WD.DataAccess.Enums.Databases.BR, dbCommand);
        //    //OR
        //    // databse (BR/TX), Databse instance(1=BR1/TX1 or 2=BR2/TX2) and command object
        //    emp = dbContext.ICommands.GetEntity<Employee>(WD.DataAccess.Enums.Databases.BR, 1, dbCommand);

        // }

        //==================================================
         //static void Main(){
         //  WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);
         //  using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName",CommandType.Text)){
         //    //write your code
              
         //  }
         //  //or
         //  WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
         //  aParams[0]=new WD.DataAccess.Parameters.DBParameter();
         //  aParams[0].ParameterName="FirstName";
         //  aParams[0].ParameterValue="first name";
         //  using(IDataReader reader=dbContext.ICommands.ExecuteDataReader(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams)){
         //    //write your code
              
         //  }

        //=========================================================

        //static void Main()
        //{
        //    //WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext(false);

           

        //    //DataTable dt = com.ExecuteDataTable(WD.DataAccess.Enums.Databases.TX, "Select * from TX_MM2_VIF_CMNLSYSTEM");
        //    string sqlStatementText = "INSERT INTO TX_MM2_VIF_CmnLSystem (StatusCode,Key1,Key2,Message1,Message2,LogTime,TimeStamp2) VALUES ('I5301001','VENUS IF','Main.69','***** Venus IF Receive Main START.','Version : 1.4.6577.31022',@P1,@P2)";//"Select * from TX_MM2_VIF_CMNLSYSTEM";

        //    WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[2];
        //    aParams[0] = new WD.DataAccess.Parameters.DBParameter();
        //    aParams[0].ParameterName = "P1";
        //    aParams[0].ParameterValue = "3/1/2018 18:05:00";

        //    aParams[1] = new WD.DataAccess.Parameters.DBParameter();
        //    aParams[1].ParameterName = "P2";
        //    aParams[1].ParameterValue = "3/1/2018 18:05:00";

        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText,aParams);
        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText, aParams);
        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText, aParams);
        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText, aParams);
        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText, aParams);
        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText, aParams);
        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText, aParams);
        //    Execute(WD.DataAccess.Enums.Databases.TX, sqlStatementText, aParams);


        //    int totalCount = 0;
        //    ICommands com = new DbContext(true).ICommands;

        //    List<Employee> empLst = com.GetList<Employee>(WD.DataAccess.Enums.Databases.BR, 10, 1, com.DBProvider, out totalCount, X => X.Country == "MY");





        //    //============================================================

        //    //string countQ = "";
        //    //string thesql = WD.DataAccess.Helpers.HelperUtility.GetPagingQuery<Employee>(10, 1, WD.DataAccess.Enums.DBProvider.Sql,out countQ, x => x.Address == "KL");

        //    //ICommands com = new DbContext(true).ICommands;

        //    //int totalRecords = Convert.ToInt32(com.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, countQ));
        //    //List<Employee> empLst = com.GetList<Employee>(thesql);


        //    //=========================================================

        //    //List<Employee> empLst = GetListByPage<Employee>(x => x.Address == "KL", 2, 2);
        //    //empLst = GetListByPage<Employee>( 2, 4);
        //    if (empLst.Count > 0)
        //    {
        //        Console.WriteLine(empLst.Count.ToString());
        //    }



        //    ////DataTable dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName",CommandType.Text);

        //    //string firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1, "Select FirstName from employee where EmployeeId=1", CommandType.Text);
        //    ////or
        //    //WD.DataAccess.Parameters.DBParameter[] aParams=new WD.DataAccess.Parameters.DBParameter[1];
        //    //aParams[0]=new WD.DataAccess.Parameters.DBParameter();
        //    //aParams[0].ParameterName = "EmployeeId";
        //    //aParams[0].ParameterValue = "1";
        //    //firstName = (string)dbContext.ICommands.ExecuteScalar(WD.DataAccess.Enums.Databases.BR, 1, "Select FirstName from employee where EmployeeId=@EmployeeId", CommandType.Text, aParams);
        //    ////dt=dbContext.ICommands.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, 1,"Select columnNames from tableName where FirstName=@FirstName",CommandType.Text,aParams);
        //}

        //=====================================================

        static void Main()
        {
            int totalCount = 0;
            //var filterExpressions = new List<Expression<Func<Employee, object>>> { x => x.Country, y => y.Location };
            ICommands comm = new DbContext().ICommands;
            //MasterUser obj = new MasterUser();
            //obj.EmployeeId = "V0020265";
            //obj.EmployeeName = "Muhammad Asim Naeem";
            //obj.SupervisorID = "abc";
            //obj.SupervisorName = "CK";
            //obj.SupervisorEmail = "ck@wdc.com";
            //comm.Insert<MasterUser>(WD.DataAccess.Enums.Databases.BR, obj);
            //WD.DataAccess.Context.DbContext dbContext = new WD.DataAccess.Context.DbContext(false);
            //=======================================

            //EmailSubscription obj = new EmailSubscription();
            //obj.ID = 1;
            //obj.AppCode = "MVX";
            //obj.TIMESTAMP2 = DateTime.Now;
            //obj.ModuleID = 25;
            //obj.EmployeeID = "V002356";
            //obj.AttributeKey = "TABLENAME";
            ////obj.TableName = "MVX_APP_Filter_RULE";

            //List<EmailSubscription> lst = comm.GetList<EmailSubscription>(WD.DataAccess.Enums.Databases.BR, x => x.AppCode == obj.AppCode 
            //    && x.ModuleID == obj.ModuleID && x.TableName == obj.TableName && x.AttributeKey == obj.AttributeKey);

            //if (lst != null)
            //{
            //    Console.WriteLine("Number of Records: " + lst.Count.ToString());
            //}





            //comm.Insert<EmailSubscription>(WD.DataAccess.Enums.Databases.BR, obj);

            //==============================================================

            //List<Employee> empLst = comm.GetList<Employee>(x => x.Location == "LHE", WD.DataAccess.Enums.SortOption.ASC, y => y.Country);
            //foreach(Employee emp in empLst)
            //{
            //    emp.Address = "SETAPAK";
            //    comm.Update<Employee>(WD.DataAccess.Enums.Databases.BR,emp,x=>x.Location=="LHE" && x.ID==emp.ID,WD.DataAccess.Enums.Transaction.TSNT);
            //}

            //List<SiteInfo> entity = comm.GetList<SiteInfo>(WD.DataAccess.Enums.Databases.BR, x => x.AppCode == "MVX" && x.LocationID == "DEV" && x.HostIP == "172.22.22.72" && x.HostType == "SECONDARY" && x.StatusCode == true);

            //List<MasterModule> empLst = comm.GetList<MasterModule>(WD.DataAccess.Enums.Databases.BR, x => x.AppCode == "MVX" && x.ID == 4, WD.DataAccess.Enums.SortOption.ASC, y => y.ID);

            Guid objG = Guid.NewGuid();

            IDbConnection dbConnection = new SqlConnection("Database=WDMBR1;Server=172.22.24.156\\MYBR1;User Id=mesapps;Password=borabora2002;Connect Timeout=30;");

            //IDbConnection dbConnection = new SqlConnection("Database=TESTDB;Server=MYIT00359;User Id=sa;Password=myrep0rt@wdvrtheB3ST;Connect Timeout=30;");

            Console.WriteLine("DB connection Created");
            //List<Employee> empLst = comm.GetList<Employee>(x=>x.Name=="Asim" ,dbConnection);


            //Console.WriteLine("GetList<>");

            ///GetEmployees();

            //int rows = dbContext.ICommands.BulkInsert

            

            MasterTestPlan objM = new MasterTestPlan();
            ///objM.MTP_ID = 1;

            //comm.Update<MasterTestPlan>(objM);
            //comm.Insert<MasterTestPlan>(objM);
            //comm.Delete<MasterTestPlan>(objM);
          //  Dictionary<string, string> columnList = new Dictionary<string, string>();
          //  //columnList.Add("Id","ID");
          // // columnList.Add("Name", "NAME");
          //  columnList.Add("Address", "ADDRESS");
          //  //columnList.Add("Date", "DATE");

          //  Dictionary<string, string> whereList = new Dictionary<string, string>();
          //  whereList.Add("Id", "ID");
          //  whereList.Add("Name", "NAME");
          ////comm.BulkDelete(GetEmployees(), "Employee", 2, 60, whereList, dbConnection, null);

           
           // whereList.Add("FirstName", "FIRSTNAME");
            //comm.BulkDelete(GetEmployees(), "Employee", 2, 60, whereList, dbConnection, null);

            /// <summary>   Bulk Update. </summary>
            /// comm.BulkUpdate(GetEmployees(), "Employee", 5, 60, columnList, whereList, dbConnection, null);
            //comm.BulkInsert(GetEmployees(), "Employee", 5, 60, columnList, dbConnection);
            //comm.BulkInsert(
            //List<MasterTestPlan> mstPlan = comm.GetList<MasterTestPlan>(x => x.MTP_ID == objG, dbConnection);
            //List<SNRangeAttribute> empLst = comm.GetList<SNRangeAttribute>(x => x.DEID == "666", dbConnection);




            //Console.WriteLine("List generated");
            //foreach (SNRangeAttribute emp in empLst)
            //{
            //    Console.WriteLine("List Item:" + emp.DEID.ToString());
            //    //emp.Hits += 1 ;
            //    //comm.Update<MasterModule>(WD.DataAccess.Enums.Databases.BR, emp, x => x.AppCode == "MVX" && x.ID == emp.ID, WD.DataAccess.Enums.Transaction.TSNT);
            //    //emp.AUDIT_VERSION += 1;
            //    //comm.Update<SNRangeAttribute>(WD.DataAccess.Enums.Databases.BR, emp, x => x.DEID == emp.DEID, WD.DataAccess.Enums.Transaction.TS);
            //    emp.REMARKS = "20180220";
            //    //update<SNRangeAttribute>(emp, x => x.Name == emp.Name && x.Address == emp.Address && x.Country == emp.Country);
            //    Console.WriteLine("Upadte");
            //    update<SNRangeAttribute>(emp, x => x.DEID=="666");
            //}

            //if (empLst.Count > 0)
            //{
            //    Console.WriteLine("Total Records" + empLst.Count.ToString());
            //}
            /// <returns>   No Of Rows Affected. </returns>
            /// 


              











            
            DateTime dt = DateTime.Now;
            Console.WriteLine("Start Time :" + dt);

            //*********************************************************************************************Insert Update Delete************************************************************
            Dictionary<string, string> columnList = new Dictionary<string, string>();
         columnList.Add("Id", "Id"); //Comment during Update 
            columnList.Add("FirstName", "FirstName");
            columnList.Add("LastName", "LastName");
            columnList.Add("Address", "Address");
            columnList.Add("Phone", "Phone");
            columnList.Add("Country", "Country");
            columnList.Add("City", "City");
            columnList.Add("Age", "Age");
            columnList.Add("DoB", "DoB");
        columnList.Add("ICNumber", "ICNumber");//Comment during Update 
            //*********************************************************************************************Insert Update Delete**
            Dictionary<string, string> whereList = new Dictionary<string, string>();
            whereList.Add("Id", "ID");



//            WD.DataAccess.Context.DbContext dbContext=new WD.DataAccess.Context.DbContext();
//            using (IDbConnection con = dbContext.ICommands.CreateConnection())
//            {
////                DataTable dtFinal = dbContext.ICommands.ExecuteDataTable(@"select MVX_APP_MASTER_MENU.CreatedBy, MVX_APP_MASTER_MENU.CreatedDate, MVX_APP_MASTER_MENU.Remarks,
////                         MVX_APP_MASTER_MENU.AppCode, MVX_APP_MASTER_MENU.ID ID, MVX_APP_MASTER_MENU.Name,
////                        MVX_APP_MASTER_MENU.ParentMenuID as ParentMenuID, MSTMP.Name ParentMenuName, MVX_APP_MASTER_MENU.SortOrder,
////                        MVX_APP_MASTER_MENU.StatusCode, MVX_APP_MASTER_MENU.Timestamp2
////                         from MVX_APP_MASTER_MENU  with (nolock)
////                        left join MVX_APP_MASTER_MENU MSTMP with (nolock) on MVX_APP_MASTER_MENU.ParentMenuID = MSTMP.ID 
////                         where MVX_APP_MASTER_MENU.StatusCode = 1 
////                          order by MVX_APP_MASTER_MENU.TIMESTAMP2 desc", con);
//                con.Open();
//                DataTable dtFinal = dbContext.ICommands.ExecuteDataTable(@"select * from MVX_APP_MASTER_MENU ", con);
//            }
             //    Dictionary<string, string> cols = new Dictionary<string, string>();
             //    cols.Add("FirstName", "FirstName");
             //    int rowsEffected = dbContext.ICommands.BulkUpdate(dtFinal, "EmployeeTest", 200, 60, cols, whereList, dbConnection, null);

             //}


            // comm.BulkInsert(GetEmployees(), "EmployeeTest", 100, 60, columnList, dbConnection);
             Console.WriteLine("Insert Finishid ");
             using (DbContext _dbContext = new DbContext())
             {
                 //DataTable dtFromDB = _dbContext.ICommands.ExecuteDataTable("SELECT * FROM MVX_APP_MASTER_MENU", dbConnection);
                 DataTable dtFromDB = _dbContext.ICommands.ExecuteDataTable(@"select MVX_APP_MASTER_MENU.CreatedBy, MVX_APP_MASTER_MENU.CreatedDate, MVX_APP_MASTER_MENU.Remarks,
                                                                              MVX_APP_MASTER_MENU.AppCode, MVX_APP_MASTER_MENU.ID ID, MVX_APP_MASTER_MENU.Name,
                                                                              MVX_APP_MASTER_MENU.ParentMenuID as ParentMenuID, MSTMP.Name ParentMenuName, MVX_APP_MASTER_MENU.SortOrder,
                                                                              MVX_APP_MASTER_MENU.StatusCode, MVX_APP_MASTER_MENU.Timestamp2
                                                                              from MVX_APP_MASTER_MENU  with (nolock)
                                                                              left join MVX_APP_MASTER_MENU MSTMP with (nolock) on MVX_APP_MASTER_MENU.ParentMenuID = MSTMP.ID 
                                                                              where MVX_APP_MASTER_MENU.StatusCode = 1 
                                                                              order by MVX_APP_MASTER_MENU.TIMESTAMP2 desc", dbConnection);
             }
                //comm.BulkUpdate(GetEmployees(), "EmployeeTest", 200, 60, columnList, whereList, dbConnection, null);
                //Console.WriteLine("Update Finishid ");

                //comm.BulkDelete(GetEmployees(), "EmployeeTest", 200, 60, whereList, dbConnection, null);
                //Console.WriteLine("Delete Finishid ");

                //*********************************************************************************************Insert Update Delete************************************************************

                DateTime dt2 = DateTime.Now;

                TimeSpan ts = (dt2 - dt);
                Console.WriteLine("End Time :" + dt2);
                Console.WriteLine("Total Time :" + ts);
                Console.ReadLine();

            //}
        }
     
        static DataTable GetEmployees()
        {
            Guid A = Guid.NewGuid();
            // Here we create a DataTable with columns.
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("Country", typeof(string));
            table.Columns.Add("City", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Columns.Add("DoB", typeof(DateTime));
            table.Columns.Add("ICNumber", typeof(Guid));

            for (int i = 1; i <= 100; i++)
            {
                string insert = "Insert" + i.ToString();
                string update = "Update" + i.ToString();

                int status =1;

                string iu = (status == 1) ? insert : update;

                table.Rows.Add(i, "FirstName" + iu, "LastNam" + iu, "Address" + iu, "Phone" + iu, "Country" + iu, "City" + iu, 5, "03-03-2000", Guid.NewGuid());
                //table.Rows.Add(i);
            }
            //table.Rows.Add(3,"NAME6", Guid.NewGuid());
            //table.Rows.Add(6, "NAME6", "ADDRESS60");
            //table.Rows.Add("Rubol2Update", "Cyber JayaUpdate");

            return table;
        }
        static void update<T>(T input, Expression<Func<T, bool>> predicate)
        {
            try
            {

            
            int rowAffected=0;
            ICommands comm = new DbContext().ICommands;
            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
            {
                Console.WriteLine("Transaction Scope created");
                using (IDbConnection dbConnection = new SqlConnection("Database=WDMBR1;Server=172.22.24.156\\MYBR1;User Id=mesapps;Password=borabora2002;Connect Timeout=30;"))
                {
                    Console.WriteLine("1-Dbconnection Created");
                    dbConnection.Open();
                    Console.WriteLine("1-Connection opened");
                    using (IDbTransaction dbTrans = dbConnection.BeginTransaction())
                    {
                        Console.WriteLine("1-Begin Transaction");
                        try
                        {
                            rowAffected += comm.Update<T>(input, predicate, dbTrans);
                            Console.WriteLine("1-Rows updated=" + rowAffected.ToString());
                            dbTrans.Commit();
                            Console.WriteLine("1-Transaction committed");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("1-Exception:" + ex.Message);
                            dbTrans.Rollback();
                        }
                        
                    }
                    
                }
                Console.WriteLine("Second Connection");
                using (IDbConnection dbConnection = new SqlConnection("Database=WDMBR2;Server=172.22.24.156\\MYBR2;User Id=mesapps;Password=borabora2002;Connect Timeout=30;"))
                {
                    Console.WriteLine("2-Dbconnection Created");
                    dbConnection.Open();
                    Console.WriteLine("2-Connection opened");
                    using (IDbTransaction dbTrans = dbConnection.BeginTransaction())
                    {
                        Console.WriteLine("2-Begin Transaction");
                        try
                        {

                            rowAffected += comm.Update<T>(input, predicate, dbTrans);
                            Console.WriteLine("2-Rows updated=" + rowAffected.ToString());
                            dbTrans.Commit();
                            Console.WriteLine("2-Transaction committed");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("2-Exception:" + ex.Message);
                            dbTrans.Rollback();
                        }

                    }
                    //rowAffected += comm.Update<T>(input, predicate, dbConnection);
                }
                Console.WriteLine("connection closed");

                //using (IDbConnection dbConnection = new SqlConnection("Database=BR2;Server=MYIT00023\\SQLEXPRESS;User Id=performa;Password=performa;Connect Timeout=30;"))
                //{
                //    dbConnection.Open();
                //    using (IDbTransaction dbTrans = dbConnection.BeginTransaction())
                //    {
                //        try
                //        {
                //            rowAffected += comm.Update<T>(input, predicate, dbTrans);
                //            dbTrans.Commit();
                //        }
                //        catch (Exception)
                //        {

                //            dbTrans.Rollback();
                //        }

                //    }
                //    //rowAffected += comm.Update<T>(input, predicate, dbConnection);
                //}
                transactionScope.Complete();
                Console.WriteLine("Transaction Scope completed");
            }
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    Console.WriteLine("Exception:" + ex.Message);
                }
                
            }
        }

        static void Execute(int dbIndex, string query, WD.DataAccess.Parameters.DBParameter[] arrParameter)
        {
            using (DbContext _dbContext = new DbContext(false))
            {
                //DataTable dt = _dbContext.ICommands.ExecuteDataTable(db, sqlStatement);
                int ReturnVal = _dbContext.ICommands.ExecuteNonQuery(dbIndex, query, arrParameter);
            }
        }

         static List<T> GetListByPage<T>(Expression<Func<T, bool>> predicate, int pageindex, int pageSize)
         {
             
             // Calculate paging info - Starting row and End row
             int startrow = ((pageindex - 1) * pageSize) + 1;
             int endrow = pageindex * pageSize;
                         
             //Create parameters
             WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[2];
             aParams[0] = new WD.DataAccess.Parameters.DBParameter();
             aParams[0].ParameterName = "startrow";
             aParams[0].ParameterValue = startrow;

             aParams[1] = new WD.DataAccess.Parameters.DBParameter();
             aParams[1].ParameterName = "endrow";
             aParams[1].ParameterValue = endrow;

             string theSQL = GetpagingQuery<T>(predicate);
             
             ICommands cmd = new DbContext(true).ICommands;
             //DataSet ds = cmd.ExecuteDataSet(WD.DataAccess.Enums.Databases.BR, "Select * from Employee");

             List<T> empLst = cmd.GetList<T>(theSQL, aParams);

             return empLst;
         }


         static List<T> GetListByPage<T>(int pageindex, int pageSize)
         {
            


             // Calculate paging info - Starting row and End row
             int startrow = ((pageindex - 1) * pageSize) + 1;
             int endrow = pageindex * pageSize;

             

             //Create parameters
             WD.DataAccess.Parameters.DBParameter[] aParams = new WD.DataAccess.Parameters.DBParameter[2];
             aParams[0] = new WD.DataAccess.Parameters.DBParameter();
             aParams[0].ParameterName = "startrow";
             aParams[0].ParameterValue = startrow;

             aParams[1] = new WD.DataAccess.Parameters.DBParameter();
             aParams[1].ParameterName = "endrow";
             aParams[1].ParameterValue = endrow;
             string theSQL = GetpagingQuery<T>();
             ICommands cmd = new DbContext(false).ICommands;

             List<T> empLst = cmd.GetList<T>(theSQL, aParams);

             return empLst;
         }

         static string GetpagingQuery<T>(Expression<Func<T, bool>> predicate = null)
         {
             // Create object of Query Builder
             WD.DataAccess.QueryProviders.QueryBuilder<T> qbe = new WD.DataAccess.QueryProviders.QueryBuilder<T>();

             //Get list of columns
             string cols = qbe.Projection();

             //Get Table name from entity type
             var dnAttribute = typeof(T).GetCustomAttributes(typeof(CustomAttribute), true).FirstOrDefault() as CustomAttribute;
             string tableName = (dnAttribute != null ? dnAttribute.Name ?? typeof(T).Name : typeof(T).Name);

             // Get the primary column name
             string primaryCol = "";

             // Get Where clause of SQL query
             string whereClause = "";
             if (predicate != null)
             {
                 whereClause = qbe.Where(predicate).Where();
             }

             List<string> lst = (from p in (typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty))
                            .Select(prop => new
                            {
                                prop,
                                attr = prop.GetCustomAttributes(typeof(CustomAttribute)).FirstOrDefault()
                            })
                            .Select(x => new
                            {
                                name = x.attr == null ? x.prop.Name : ((CustomAttribute)x.attr).Name ?? x.prop.Name,
                                x.prop
                            }).Where(x => x.prop.GetMethod.IsVirtual == false).Where(x => ((CustomAttribute)x.prop.GetCustomAttribute(typeof(CustomAttribute), true)) != null && ((CustomAttribute)x.prop.GetCustomAttribute(typeof(CustomAttribute), true)).IsPrimary == true)
                                 select p.name).ToList();

             if (lst.Count > 0)
             {
                 primaryCol = lst.ElementAt<string>(0).ToString();
             }
             else
             {
                 //In case no primary column then first column will be considered.
                 primaryCol = cols.Split(',')[0];
             }

             string theSQL = String.Format("Select {0} FROM (Select  ROW_NUMBER() OVER ( ORDER BY {1} ) RowNum, E.* From {2} E {3} ) A WHERE  RowNum BETWEEN @startrow AND @endrow", cols, primaryCol, tableName, whereClause);

             return theSQL;
         }

         //static DataTable GetEmployees()
         //{
         //    Guid A = Guid.NewGuid();
         //    // Here we create a DataTable with columns.
         //    DataTable table = new DataTable();
         //    table.Columns.Add("Id", typeof(int));
         //    table.Columns.Add("FirstName", typeof(string));
         //    table.Columns.Add("LastName", typeof(string));
         //    table.Columns.Add("Address", typeof(string));
         //    table.Columns.Add("Phone", typeof(string));
         //    table.Columns.Add("Country", typeof(string));
         //    table.Columns.Add("City", typeof(string));
         //    table.Columns.Add("Age", typeof(int));
         //    table.Columns.Add("DoB", typeof(DateTime));
         //   table.Columns.Add("ICNumber", typeof(Guid));   

         //    for (int i = 1; i <= 100; i++)
         //    {
         //       string insert = "Insert" + i.ToString();
         //       string update = "Update" + i.ToString();
         //       int status = 2;
         //       string iu = (status == 1) ? insert : update;

         //       //table.Rows.Add(i, "FirstName" + iu, "LastNam" + iu, "Address" + iu, "Phone" + iu, "Country" + iu, "City" + iu, 5, "03-03-2000", Guid.NewGuid());
         //        //update
         //       table.Rows.Add(i, "FirstName" + iu, "LastNam" + iu, "Address" + iu, "Phone" + iu, "Country" + iu, "City" + iu, 5, "03-03-2000", Guid.NewGuid());
         //    }
         //    //table.Rows.Add(3,"NAME6", Guid.NewGuid());
         //    //table.Rows.Add(6, "NAME6", "ADDRESS60");
         //    //table.Rows.Add("Rubol2Update", "Cyber JayaUpdate");
            
         //    return table;
         //}


    }
}
 