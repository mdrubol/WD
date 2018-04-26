using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using MetroFramework.Controls;

namespace WD.XLC.WIN.Controls
{
    public partial class ConnectionControl : MetroUserControl
    {
        private readonly WD.XLC.Domain.Entities.ServerInfo info = null;
        public ConnectionControl()
        {
            this.info = new Domain.Entities.ServerInfo();
            InitializeComponent();
            switch (Properties.Settings.Default.DbProvider)
            {

                case  WD.DataAccess.Enums.DBProvider.Sql:
                    txtSqlServerPassword.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.Password, true);
                    txtSqlServerDBName.Text = Properties.Settings.Default.ServerName;
                    txtSqlServerUserID.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.UserID, true);
                    txtSqlServerInitialCat.Text =  Properties.Settings.Default.InitialCatalog;
                    info.IntegratedSecurity = Properties.Settings.Default.IntegratedSecurity;
                    cbxIntegratedSecurity.Checked = Properties.Settings.Default.IntegratedSecurity == 1;
                    metroTabControl1.SelectedIndex = 0;
                    sqlTab.ToolTipText = "current connection";
                    break;
                case WD.DataAccess.Enums.DBProvider.Access:
                    txtAccessProvider.Text = Properties.Settings.Default.ProviderString;
                    txtAccessPassword.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.Password, true);
                    txtAccessUserID.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.UserID, true);
                    txtAccessDBname.Text =  Properties.Settings.Default.ServerName;
                    metroTabControl1.SelectedIndex = 1;
                    accessTab.ToolTipText = "current connection";
                    break;
                case WD.DataAccess.Enums.DBProvider.Oracle:
                case WD.DataAccess.Enums.DBProvider.Oracle2:
                    txtOraclePassword.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.Password, true);
                    txtOracleUserID.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.UserID, true);
                    txtOracleDBname.Text =  Properties.Settings.Default.ServerName;
                    metroTabControl1.SelectedIndex = 2;
                    oracleTab.ToolTipText = "current connection";
                    break;
                case WD.DataAccess.Enums.DBProvider.Db2:
                    txtDb2Password.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.Password, true);
                    txtDb2UserId.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.UserID, true);
                    txtDb2ServerName.Text =  Properties.Settings.Default.ServerName;
                    txtDb2DatabaseName.Text = Properties.Settings.Default.DatabaseName;
                    txtDb2CurrentSchema.Text =  Properties.Settings.Default.CurrentSchema;
                    txtDb2Port.Text =  Properties.Settings.Default.ServerPort;
                    txtDb2Protocol.Text =  Properties.Settings.Default.CurrentProtocol;
                    metroTabControl1.SelectedIndex = 3;
                    db2Tab.ToolTipText = "current connection";
                    break;
                case WD.DataAccess.Enums.DBProvider.MySql:
                    txtMySQLPassword.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.Password, true);
                    txtMySqlDataBase.Text = Properties.Settings.Default.InitialCatalog;
                    txtMySQLUserId.Text = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.UserID, true);
                    txtMySQLServerName.Text =  Properties.Settings.Default.ServerName;
                    info.IntegratedSecurity = Properties.Settings.Default.IntegratedSecurity;
                    chkMySqlIntegratedSecurity.Checked = Properties.Settings.Default.IntegratedSecurity == 1;
                    metroTabControl1.SelectedIndex = 4;
                    MySqlTab.ToolTipText = "current connection";
                    break;
            }
        }
        #region SQL
        /// <summary>
        /// SQL Server 
        /// Configure for the use of integrated
        /// security
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            // if the user has checked the SQL Server connection
            // option to use integrated security, configure the
            //user ID and password controls accordingly

            if (cbxIntegratedSecurity.Checked == true)
            {
                txtSqlServerUserID.Text = string.Empty;
                txtSqlServerPassword.Text = string.Empty;

                txtSqlServerUserID.Enabled = false;
                txtSqlServerPassword.Enabled = false;
            }
            else
            {
                txtSqlServerUserID.Enabled = true;
                txtSqlServerPassword.Enabled = true;
            }
        }

        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSQLserverCancel_Click(object sender, EventArgs e)
        {
           //this.Dispose();
        }
        /// <summary>
        /// Test the SQL Server connection string
        /// based upon the user supplied settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSqlServerTest_Click(object sender, EventArgs e)
        {

            try
            {
                info.DbProvider = WD.DataAccess.Enums.DBProvider.Sql;
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                // Set the properties for the connection string
                info.Password = txtSqlServerPassword.Text;
                info.UserID = txtSqlServerUserID.Text;
                info.ServerName = txtSqlServerDBName.Text;
                info.InitialCatalog = txtSqlServerInitialCat.Text;

                // configure the connection string based upon the use
                // of integrated security
                if (cbxIntegratedSecurity.Checked == true)
                {
                    info.ConnString =
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog +
                        ";Integrated Security=SSPI";
                }
                else
                {
                    info.ConnString =
                        ";Password=" + info.Password +
                        ";User ID=" + info.UserID +
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog;
                }
            }
            catch (Exception ex)
            {
                // inform the user if the connection was not saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Test Connection
            if (info.ConnString != string.Empty)
            {

                try
                {
                    using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                    {
                        conn.ConnectionString = info.ConnString;
                        // test the connection with an open attempt
                        conn.Open();
                        MessageBox.Show("Connection Attempt Successful.", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // inform the user if the connection test failed
                    MessageBox.Show(ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }



        /// <summary>
        /// Persist and test an SQL Server connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSqlServerOK_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                info.DbProvider = WD.DataAccess.Enums.DBProvider.Sql;
                // Set the properties for the connection 
                info.Password = txtSqlServerPassword.Text;
                info.UserID = txtSqlServerUserID.Text;
                info.ServerName = txtSqlServerDBName.Text;
                info.InitialCatalog = txtSqlServerInitialCat.Text;

                info.DatabaseName = string.Empty;
                info.ServerPort = string.Empty;
                info.CurrentSchema = string.Empty;
                info.CurrentProtocol = string.Empty;
        
                // configure the connection string based upon
                // the use of integrated security
                if (cbxIntegratedSecurity.Checked == true)
                {
                    info.ConnString =
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog +
                        ";Integrated Security=SSPI";
                }
                else
                {
                    info.ConnString =
                        ";Password=" + info.Password +
                        ";User ID=" + info.UserID +
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog;
                }
            }
            catch (Exception ex)
            {
                // inform the user if the connection information was not saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Test Connection
            if (info.ConnString != string.Empty)
            {
                
                    try
                    {
                        using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                        {
                            conn.ConnectionString = info.ConnString;
                            // test the connection with an open attempt
                            conn.Open();
                            Properties.Settings.Default.DbProvider = info.DbProvider;
                            Properties.Settings.Default.ServerName = info.ServerName;
                            Properties.Settings.Default.InitialCatalog = info.InitialCatalog;
                            Properties.Settings.Default.Password = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.Password, true);
                            Properties.Settings.Default.UserID = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.UserID, true);
                            Properties.Settings.Default.ConnString = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.ConnString, true);
                            Properties.Settings.Default.IntegratedSecurity = info.IntegratedSecurity;
                            Properties.Settings.Default.Save();
                            MessageBox.Show("Changes saved successfully." + Environment.NewLine + "Please close instances to reflect connection changes.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //this.Dispose();
                        }

                    }
                    catch (Exception ex)
                    {
                        // inform the user if the connection was not saved
                        MessageBox.Show(ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }


        #endregion
        #region Access
        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccessCancel_Click(object sender, EventArgs e)
        {
           //this.Dispose();
        }



        /// <summary>
        /// Browse for an access database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "MS Access Database";
            openFile.DefaultExt = "mdb";
            openFile.Filter = "Access Database (*.mdb)|*mdb";
            openFile.ShowDialog();
            txtAccessDBname.Text = openFile.FileName;
        }




        /// <summary>
        /// Test an MS Access database connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccessTest_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                info.DbProvider = WD.DataAccess.Enums.DBProvider.Access;
                // Set the access database connection properties
                info.ProviderString = txtAccessProvider.Text;
                info.Password = txtAccessPassword.Text;
                info.UserID = txtAccessUserID.Text;
                info.ServerName = txtAccessDBname.Text;

                // Set the access database connection string
                info.ConnString = "Provider=" + info.ProviderString +
                                    ";Password=" + info.Password +
                                    ";User ID=" + info.UserID +
                                    ";Data Source=" + info.ServerName;



            }
            catch (Exception ex)
            {
                // inform the user if the connection could not be saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Test Connection
            if (info.ConnString != string.Empty)
            {

                try
                {
                    using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                    {
                        conn.ConnectionString = info.ConnString;
                        // test the connection with an open attempt
                        conn.Open();
                        MessageBox.Show("Access connection test successful", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // inform the user if the connection failed
                    MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }



        /// <summary>
        /// Persist and test an Access database connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccessOK_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                info.DbProvider = WD.DataAccess.Enums.DBProvider.Access;
                // Set the access database connection properties
                info.ProviderString = txtAccessProvider.Text;
                info.Password = txtAccessPassword.Text;
                info.UserID = txtAccessUserID.Text;
                info.ServerName = txtAccessDBname.Text;

                // Set the access database connection string
                info.ConnString = "Provider=" + info.ProviderString +
                ";Password=" + info.Password +
                ";User ID=" + info.UserID +
                ";Data Source=" + info.ServerName;


            }
            catch (Exception ex)
            {
                // Inform the user if the connection was not saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (info.ConnString != string.Empty)
            {
                
                    try
                    {
                        using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                        {
                            conn.ConnectionString = info.ConnString;
                            // test the connection with an open attempt
                            conn.Open();
                            Properties.Settings.Default.DbProvider = info.DbProvider;
                            Properties.Settings.Default.ProviderString = info.ProviderString;
                            Properties.Settings.Default.ServerName = info.ServerName;
                            Properties.Settings.Default.Password = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.Password, true);
                            Properties.Settings.Default.UserID = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.UserID, true);
                            Properties.Settings.Default.ConnString = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.ConnString, true);
                            Properties.Settings.Default.Save();
                            MessageBox.Show("Changes saved successfully." + Environment.NewLine + "Please close instances to reflect connection changes.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //this.Dispose();
                        }

                    }
                    catch (Exception ex)
                    {
                        // inform the user if the connection failed
                        MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }


        #endregion
        #region Oracle
        /// <summary>
        /// Store the Oracle settings and test the connection
        /// string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOracleOK_Click(object sender, EventArgs e)
        {
            // Future use; if a current data model and database
            // type need to be identified and saved with the connect
            // string to identify its purpose
            info.DbProvider = WD.DataAccess.Enums.DBProvider.Oracle;

            // Set the actual connection string properties into
            // the application settings
            info.Password = txtOraclePassword.Text;
            info.UserID = txtOracleUserID.Text;
            info.ServerName = txtOracleDBname.Text;

            // Set the connection string
            info.ConnString =    ";Password=" + info.Password +
                                 ";User ID=" + info.UserID +
                                 ";Data Source=" + info.ServerName;


            // Save the property settings

            //Test Connection
            if (info.ConnString != string.Empty)
            {

                try
                {
                    using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                    {
                        conn.ConnectionString = info.ConnString;
                        // test with an open attempt
                        conn.Open();
                        Properties.Settings.Default.DbProvider = info.DbProvider;
                        Properties.Settings.Default.ProviderString = info.ProviderString;
                        Properties.Settings.Default.ServerName = info.ServerName;
                        Properties.Settings.Default.Password = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.Password, true);
                        Properties.Settings.Default.UserID = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.UserID, true);
                        Properties.Settings.Default.ConnString = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.ConnString, true);
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Changes saved successfully." + Environment.NewLine + "Please close instances to reflect connection changes.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //this.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    // if the connection fails, inform the user
                    // so they can fix the properties
                    MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        /// <summary>
        /// Test the Oracle Connection String
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOracleTest_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                info.DbProvider = WD.DataAccess.Enums.DBProvider.Oracle;

                // Set the actual connection string properties into
                // the application settings
                info.UserID = txtOracleUserID.Text;
                info.ServerName = txtOracleDBname.Text;
                info.Password = txtOraclePassword.Text;
                // Set the connection string
                info.ConnString =   ";Password=" + info.Password +
                                    ";User ID=" + info.UserID +
                                    ";Data Source=" + info.ServerName;

                // Save the property settings


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (info.ConnString != string.Empty)
            {

                try
                {
                    using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                    {
                        conn.ConnectionString = info.ConnString;
                        // test the connection with an open attempt
                        conn.Open();

                        MessageBox.Show("Connection attempt successful.", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // inform the user if the connection fails
                    MessageBox.Show(ex.Message, "Connection Error");
                }
            }
        }
        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOracleCancel_Click(object sender, EventArgs e)
        {
           //this.Dispose();
        }
        #endregion
        #region DB2
        private void btnDb2Cancel_Click(object sender, EventArgs e)
        {
           //this.Dispose();
        }
        /// <summary>
        /// Persist and test an Db2 database connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDb2Test_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                info.DbProvider = WD.DataAccess.Enums.DBProvider.Db2;

                // Set the access database connection properties
                // Set the access database connection properties
                info.Password = txtDb2Password.Text;
                info.UserID = txtDb2UserId.Text;
                info.ServerName = txtDb2ServerName.Text;
                info.DatabaseName = txtDb2DatabaseName.Text;
                info.CurrentSchema = txtDb2CurrentSchema.Text;
                info.ServerPort = txtDb2Port.Text;
                info.CurrentProtocol = txtDb2Protocol.Text;
                // Set the access database connection string
                info.ConnString = "database=" + info.DatabaseName +
                ";Password=" + info.Password +
                ";User ID=" + info.UserID +
                ";server=" + info.ServerName + ":" + info.ServerPort +
                ";CurrentSchema=" + info.CurrentSchema +
                ";HostVarParameters=true";
            }
            catch (Exception ex)
            {
                // Inform the user if the connection was not saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Test Connection
            if (info.ConnString != string.Empty)
            {
               
                    try
                    {
                        using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                        {
                            conn.ConnectionString = info.ConnString;
                            conn.Open();
                            MessageBox.Show("Db2 connection test successful", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        // inform the user if the connection failed
                        MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }
        /// <summary>
        /// Persist and test an Db2 database connection
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnDb2OK_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                info.DbProvider = WD.DataAccess.Enums.DBProvider.Db2;
                // Set the access database connection properties
                info.Password = txtDb2Password.Text;
                info.UserID = txtDb2UserId.Text;
                info.ServerName = txtDb2ServerName.Text;
                info.DatabaseName = txtDb2DatabaseName.Text;
                info.CurrentSchema = txtDb2CurrentSchema.Text;
                info.ServerPort = txtDb2Port.Text;
                info.CurrentProtocol = txtDb2Protocol.Text;
                // Set the access database connection string
                info.ConnString = "database=" + info.DatabaseName +
                      ";Password=" + info.Password +
                      ";User ID=" + info.UserID +
                      ";server=" + info.ServerName + ":" + info.ServerPort +
                      ";CurrentSchema=" + info.CurrentSchema +
                      ";HostVarParameters=true";

            }
            catch (Exception ex)
            {
                // Inform the user if the connection was not saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Test Connection
            if (info.ConnString != string.Empty)
            {

                try
                {
                    using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                    {
                            conn.ConnectionString = info.ConnString;
                            conn.Open();
                            Properties.Settings.Default.DbProvider = info.DbProvider;
                            Properties.Settings.Default.ProviderString = info.ProviderString;
                            Properties.Settings.Default.Password = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.Password, true);
                            Properties.Settings.Default.UserID = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.UserID, true);
                            Properties.Settings.Default.ConnString = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.ConnString, true);
                            Properties.Settings.Default.ServerName = info.ServerName;
                            Properties.Settings.Default.DatabaseName = info.DatabaseName;
                            Properties.Settings.Default.CurrentSchema = info.CurrentSchema;
                            Properties.Settings.Default.ServerPort = info.ServerPort;
                            Properties.Settings.Default.CurrentProtocol = info.CurrentProtocol;
                            Properties.Settings.Default.Save();
                           MessageBox.Show("Changes saved successfully." + Environment.NewLine + "Please close instances to reflect connection changes.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // inform the user if the connection failed
                    MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        private void txtMySQLCancel_Click(object sender, EventArgs e)
        {

        }


        private void btnMySQLTest_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                // Set the properties for the connection 
                info.DbProvider = WD.DataAccess.Enums.DBProvider.MySql;
                info.Password = txtMySQLPassword.Text;
                info.UserID = txtMySQLUserId.Text;
                info.ServerName = txtMySQLServerName.Text;
                info.InitialCatalog = txtMySqlDataBase.Text;

                info.DatabaseName = string.Empty;
                info.ServerPort = string.Empty;
                info.CurrentSchema = string.Empty;
                info.CurrentProtocol = string.Empty;

                // configure the connection string based upon
                // the use of integrated security
                if (chkMySqlIntegratedSecurity.Checked == true)
                {
                    info.ConnString =
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog +
                        ";Integrated Security=SSPI" +
                       ";encrypt=No";
                }
                else
                {
                    info.ConnString =
                        ";Password=" + info.Password +
                        ";User ID=" + info.UserID +
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog +
                    ";User Id=" + info.UserID +
                     ";Password=" + info.Password +
                       ";encrypt=No";
                }


            }
            catch (Exception ex)
            {
                // inform the user if the connection information was not saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Test Connection
            if (info.ConnString != string.Empty)
            {

                try
                {
                    using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                    {
                        conn.ConnectionString = info.ConnString;
                        // test the connection with an open attempt
                        conn.Open();
                        MessageBox.Show("Connectedsuccessfully.", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    // inform the user if the connection was not saved
                    MessageBox.Show(ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnMySQLSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Future use; if a current data model and database
                // type need to be identified and saved with the connect
                // string to identify its purpose
                // Set the properties for the connection 
                info.DbProvider = WD.DataAccess.Enums.DBProvider.MySql;
                info.Password = txtMySQLPassword.Text;
                info.UserID = txtMySQLUserId.Text;
                info.ServerName = txtMySQLServerName.Text;
                info.InitialCatalog = txtMySqlDataBase.Text;

                info.DatabaseName = string.Empty;
                info.ServerPort = string.Empty;
                info.CurrentSchema = string.Empty;
                info.CurrentProtocol = string.Empty;

                // configure the connection string based upon
                // the use of integrated security
                if (chkMySqlIntegratedSecurity.Checked == true)
                {
                    info.ConnString =
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog +
                        ";Integrated Security=SSPI" +
                       ";User Id=" + info.UserID +
                       ";Password=" + info.Password +
                       ";encrypt=No";
                }
                else
                {
                    info.ConnString =
                        ";Password=" + info.Password +
                        ";User ID=" + info.UserID +
                        ";Data Source=" + info.ServerName +
                        ";Initial Catalog=" + info.InitialCatalog +
                    ";User Id=" + info.UserID +
                     ";Password=" + info.Password +
                       ";encrypt=No";
                }


            }
            catch (Exception ex)
            {
                // inform the user if the connection information was not saved
                MessageBox.Show(ex.Message, "Error saving connection information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Test Connection
            if (info.ConnString != string.Empty)
            {

                try
                {
                    using (IDbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(info.DbProvider))
                    {
                        conn.ConnectionString = info.ConnString;
                        // test the connection with an open attempt
                        conn.Open();
                        Properties.Settings.Default.DbProvider = info.DbProvider;
                        Properties.Settings.Default.Password = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.Password, true);
                        Properties.Settings.Default.UserID = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.UserID, true);
                        Properties.Settings.Default.ConnString = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.ConnString, true);
                        Properties.Settings.Default.ServerName = info.ServerName;
                        Properties.Settings.Default.InitialCatalog = info.InitialCatalog;
                        Properties.Settings.Default.IntegratedSecurity = info.IntegratedSecurity;
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Changes saved successfully." + Environment.NewLine + "Please close instances to reflect connection changes.", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    // inform the user if the connection was not saved
                    MessageBox.Show(ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkMySqlIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            // if the user has checked the SQL Server connection
            // option to use integrated security, configure the
            //user ID and password controls accordingly

            if (chkMySqlIntegratedSecurity.Checked == true)
            {
                txtMySQLUserId.Text = string.Empty;
                txtMySQLPassword.Text = string.Empty;

                txtMySQLUserId.Enabled = false;
                txtMySQLPassword.Enabled = false;
            }
            else
            {
                txtMySQLUserId.Enabled = true;
                txtMySQLPassword.Enabled = true;
            }
        }
      
    }
}
