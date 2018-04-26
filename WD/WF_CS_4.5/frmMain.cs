using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Configuration;

// Add Namespaces
using WD.DataAccess.Abstract;
using WD.DataAccess.Context;
using WD.DataAccess.Enums;
using WD.DataAccess.Parameters;


namespace WF_CS_4._5
{
    public partial class frmMain : Form
    {
        //DBProvider dbp;
        int dbp = 0;
        string connectionName = "";
        string connectionStr = "";
        string tokenName = "Token";

        public frmMain()
        {

            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            dbp = DBProvider.Sql;
            connectionName = ConfigurationManager.AppSettings["dbConnection"].ToString();
            connectionStr = ConfigurationManager.AppSettings["ConStr"].ToString();
            tokenName = ConfigurationManager.AppSettings["Token"].ToString();
        }

        private void btnExecNonQuery_Click(object sender, EventArgs e)
        {
            try
            {

                // Get the command object by initializing the DBContext.
                // Chosse which Database to connect. (In my case I am using SQL server so DBProvider.Sql)

                // --DbContext(): It will search App.Config for "DefaultConnection" 
                ICommands command = new DbContext().ICommands;

                // --DbContext(connectionName): It will search connection string from App.config with particular connection name.
                //ICommands command = new DbContext(connectionName).ICommands;

                // --DbContext(Connect aConnect): we will pass WD.DataAccess.Helpers.Connect object to create connection with database.
                // --WD.DataAccess.Helpers.Connect contains 3 properties.       
                // -- DbProvider
                // -- ConnectionString
                // -- ConnectionName
                //ICommands command = new DbContext(new WD.DataAccess.Helpers.Connect { ConnectionName = connectionName, DbProvider = DBProvider.Sql }).ICommands;

                // --DbContext(int dbProvider): Will get the connection string from settings
                //ICommands command = new DbContext(dbp).ICommands;

                // --DbContext(int dbProvider, string tokenName): Will get the connection string from settings
                //ICommands command = new DbContext(dbp,tokenName).ICommands;



                //---------mitec CONSTRUCTOR------------

                //ICommands command = new DbContext("DEFAULT", WD.DataAccess.Enums.Databases.BR).ICommands;
                //ICommands command = new DbContext(false).ICommands;

                //---------------------------------------

                WD.DataAccess.Logger.ILogger.Info(command.DBProvider.ToString());

                // ExecuteNonQuery() always return number of rows effected.
                // Put the Query into the textbox or pass directly to ExecuteNonQuery() Methode.
                int rowsAffectted = command.ExecuteNonQuery(txtExecNonQuery.Text);
                lblExecNonQueryStatus.Text = String.Format("Status: {0} rows effected", rowsAffectted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WD.DataAccess.Logger.ILogger.Error(ex);
            }

        }

        private void btnExecDataTable_Click(object sender, EventArgs e)
        {
            try
            {


                // Get the command object by initializing the DBContext.
                // Chosse which Database to connect. (In my case I am using SQL server so DBProvider.Sql)

                // --DbContext(): It will search App.Config for "DefaultConnection" 
                //ICommands command = new DbContext().ICommands;

                // --DbContext(connectionName): It will search connection string from App.config with particular connection name.
                //ICommands command = new DbContext(connectionName).ICommands;

                // --DbContext(Connect aConnect): we will pass WD.DataAccess.Helpers.Connect object to create connection with database.
                // --WD.DataAccess.Helpers.Connect contains 3 properties.       
                // -- DbProvider
                // -- ConnectionString
                // -- ConnectionName
                //ICommands command = new DbContext(new WD.DataAccess.Helpers.Connect { ConnectionName = connectionName, DbProvider = DBProvider.Sql }).ICommands;

                // --DbContext(int dbProvider): Will get the connection string from settings
                //ICommands command = new DbContext(dbp).ICommands;

                // --DbContext(int dbProvider, string tokenName): Will get the connection string from settings
                //ICommands command = new DbContext(dbp,tokenName).ICommands;

                //---------mitec CONSTRUCTOR------------

                //ICommands command = new DbContext("DEFAULT", WD.DataAccess.Enums.Databases.BR).ICommands;
                //ICommands command = new DbContext(false).ICommands;

                //---------------------------------------

                //ICommands command = new ICommands();
                
                dgExecuteDataTable.DataSource = null;
                DataTable dt = new DataTable();

                if (rbMitec.Checked)
                {
                    ICommands command = new DbContext("DEFAULT", WD.DataAccess.Enums.Databases.BR).ICommands;
                    dt = command.ExecuteDataTable(WD.DataAccess.Enums.Databases.BR, txtExecDataTable.Text);
                }
                else if (rbDbLog.Checked)
                {
                    ICommands command = new DbContext().ICommands;
                    dt = command.ExecuteDataTable( txtExecDataTable.Text);
                }

               

                

                //dt = command.ExecuteDataTable(txtExecNonQuery.Text, CommandType.Text);

                //DBParameter[] param = new DBParameter[1];
                //param[0] = new DBParameter() { ParameterName = "FirstName", ParameterValue = "Shahid" };
                //dt = command.ExecuteDataTable("prEmployeeList", CommandType.StoredProcedure, param);

                lblExecDataTableStatus.Text = String.Format("Status: Rows selected={0}", dt.Rows.Count);
                WD.DataAccess.Logger.ILogger.Info(lblExecDataTableStatus.Text);
                dgExecuteDataTable.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WD.DataAccess.Logger.ILogger.Error(ex);
            }
        }

        private void btnGetEntity_Click(object sender, EventArgs e)
        {
            try
            {

                // Get the command object by initializing the DBContext.
                // Chosse which Database to connect. (In my case I am using SQL server so DBProvider.Sql)

                // --DbContext(): It will search App.Config for "DefaultConnection" 
                //ICommands command = new DbContext().ICommands;

                // --DbContext(connectionName): It will search connection string from App.config with particular connection name.
                //ICommands command = new DbContext(connectionName).ICommands;

                // --DbContext(Connect aConnect): we will pass WD.DataAccess.Helpers.Connect object to create connection with database.
                // --WD.DataAccess.Helpers.Connect contains 3 properties.       
                // -- DbProvider
                // -- ConnectionString
                // -- ConnectionName
                ICommands command = new DbContext(new WD.DataAccess.Helpers.Connect { ConnectionName = connectionName, DbProvider = DBProvider.Sql }).ICommands;

                // --DbContext(int dbProvider): Will get the connection string from settings
                //ICommands command = new DbContext(dbp).ICommands;

                // --DbContext(int dbProvider, string tokenName): Will get the connection string from settings
                //ICommands command = new DbContext(dbp,tokenName).ICommands;

                //---------mitec CONSTRUCTOR------------

                //ICommands command = new DbContext("DEFAULT", WD.DataAccess.Enums.Databases.BR).ICommands;
                //ICommands command = new DbContext(false).ICommands;

                //---------------------------------------

                //Employee emp = command.GetEntity<Employee>(String.Format("Select * from tempEmployee Where EmployeeId={0}", txtEmpId.Text));

                DBParameter[] param = new DBParameter[1];
                param[0] = new DBParameter() { ParameterName = "EmpId", ParameterValue = txtEmpId.Text, Type=DbType.Int32, ParamDirection=ParameterDirection.Input };
                Employee emp = command.GetEntity<Employee>("prGetEmployeeById", CommandType.StoredProcedure, param);

                if (emp != null)
                {
                    lblGetEntityStatus.Text = String.Format("First Name: {0}, Last Name:{1}", emp.FirstName, emp.LastName);
                    WD.DataAccess.Logger.ILogger.Info(lblGetEntityStatus.Text);
                }
                else
                {
                    WD.DataAccess.Logger.ILogger.Info("No Record Fount for EmpId:" + txtEmpId.Text);
                }
                txtEmpId.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WD.DataAccess.Logger.ILogger.Error(ex);
            }

        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            try
            {

                // Get the command object by initializing the DBContext.
                // Chosse which Database to connect. (In my case I am using SQL server so DBProvider.Sql)

                // --DbContext(): It will search App.Config for "DefaultConnection" 
                //ICommands command = new DbContext().ICommands;

                // --DbContext(connectionName): It will search connection string from App.config with particular connection name.
                //ICommands command = new DbContext(connectionName).ICommands;

                // --DbContext(Connect aConnect): we will pass WD.DataAccess.Helpers.Connect object to create connection with database.
                // --WD.DataAccess.Helpers.Connect contains 3 properties.       
                // -- DbProvider
                // -- ConnectionString
                // -- ConnectionName
                //ICommands command = new DbContext(new WD.DataAccess.Helpers.Connect { ConnectionName = connectionName, DbProvider = DBProvider.Sql }).ICommands;

                // --DbContext(int dbProvider): Will get the connection string from settings
                ICommands command = new DbContext(dbp).ICommands;

                // --DbContext(int dbProvider, string tokenName): Will get the connection string from settings
                //ICommands command = new DbContext(dbp,tokenName).ICommands;

                //---------mitec CONSTRUCTOR------------

                //ICommands command = new DbContext("DEFAULT", WD.DataAccess.Enums.Databases.BR).ICommands;
                //ICommands command = new DbContext(false).ICommands;

                //---------------------------------------

                string strQ = "Select * from tempEmployee";

                //List<Employee> lst = command.GetList<Employee>(strQ);

                

                DBParameter[] param = new DBParameter[1];
                param[0] = new DBParameter() { ParameterName = "FirstName", ParameterValue = txtFirstName.Text, Type = DbType.String, ParamDirection = ParameterDirection.Input };
                List<Employee> lst = command.GetList<Employee>(String.Format("{0} Where FirstName like '%' + {1} + '%'", strQ, "@FirstName"),CommandType.Text,param);
                //List<Employee> lst = command.GetList<Employee>("prEmployeeListByName", CommandType.StoredProcedure, param);

                if (lst.Count > 0)
                {
                    lblGetEntityStatus.Text = String.Format("Employees Selected:{0}", lst.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WD.DataAccess.Logger.ILogger.Error(ex);
            }

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {

                // Get the command object by initializing the DBContext.
                // Chosse which Database to connect. (In my case I am using SQL server so DBProvider.Sql)

                // --DbContext(): It will search App.Config for "DefaultConnection" 
                //ICommands command = new DbContext().ICommands;

                // --DbContext(connectionName): It will search connection string from App.config with particular connection name.
                //ICommands command = new DbContext(connectionName).ICommands;

                // --DbContext(Connect aConnect): we will pass WD.DataAccess.Helpers.Connect object to create connection with database.
                // --WD.DataAccess.Helpers.Connect contains 3 properties.       
                // -- DbProvider
                // -- ConnectionString
                // -- ConnectionName
                //ICommands command = new DbContext(new WD.DataAccess.Helpers.Connect { ConnectionName = connectionName, DbProvider = DBProvider.Sql }).ICommands;

                // --DbContext(int dbProvider): Will get the connection string from settings
                //ICommands command = new DbContext(dbp).ICommands;

                // --DbContext(int dbProvider, string tokenName): Will get the connection string from settings
                ICommands command = new DbContext(dbp, tokenName).ICommands;

                //---------mitec CONSTRUCTOR------------

                //ICommands command = new DbContext("DEFAULT", WD.DataAccess.Enums.Databases.BR).ICommands;
                //ICommands command = new DbContext(false).ICommands;

                //---------------------------------------

                //string strq = String.Format("Insert Into tempEmployee(EmployeeId, FirstName, MiddleName, LastName, DateOfJoining,Provider) Values({0},'{1}','{2}','{3}','{4}','{5}')", txtEmployeeId.Text, txtFName.Text, txtMName.Text, txtLName.Text, dpDateOfJoining.Value.Date.ToShortDateString(), dbp.ToString());
                string strq = String.Format("Insert Into tempEmployee(EmployeeId, FirstName, MiddleName, LastName, DateOfJoining,Provider) Values({0},'{1}','{2}','{3}','{4}','{5}')", txtEmployeeId.Text, txtFName.Text, txtMName.Text, txtLName.Text, dpDateOfJoining.Value.Date.ToShortDateString(), dbp.ToString());


                int rowsAffectted = command.ExecuteNonQuery(strq);

                lblExecNonQueryStatus.Text = String.Format("Status: {0} rows effected", rowsAffectted);

                txtEmployeeId.Text = "";
                txtFName.Text = "";
                txtLName.Text = "";
                txtMName.Text = "";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                WD.DataAccess.Logger.ILogger.Error(ex);
            }
        }

        private void rbMitec_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMitec.Checked)
            {
                txtExecDataTable.Text = "Select * from testing1";
            }
        }

        private void rbDbLog_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDbLog.Checked)
            {
                txtExecDataTable.Text = "Select * from tempEmployee";
            }
        }


    }
}
