using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using WD.DataAccess.Attributes;
using WD.DataAccess.Context;
using WD.DataAccess.Helpers;
using WD.DataAccess.Mitecs;
using WD.XLC.Domain.Entities;
using WD.XLC.Domain.Enums;
using WD.XLC.Domain.Helpers;
using WD.XLC.WIN.Controls;

// namespace: WD.XLC.WIN
//
// summary:	.
namespace WD.XLC.WIN
{

    /// <summary>   Form for viewing the dashboard. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    public partial class DashboardForm : Form
    {
        private readonly int dbProvider = 0;
        private readonly string conString = string.Empty;
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="max">  The maximum. </param>
        public DashboardForm(int max, bool runLoader)
           
        {
            try
            {
                this.max = max;
                this.runLoader = runLoader;
                InitializeComponent();
                this.backgroundWorker.WorkerSupportsCancellation = true;
                this.backgroundWorker.WorkerReportsProgress = true;
                this.Text += "(" + this.max.ToString() + ")";
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
                this.dbProvider = Properties.Settings.Default.DbProvider;
                this.TimeInterval();
                this.conString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(Properties.Settings.Default.ConnString, true);

            }
            catch { }


        }
        /// <summary>   Event handler. Called by Dashboard for load events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        void Dashboard_Load(object sender, System.EventArgs e)
        {
              ProgressBar();
        }
        /// <summary>   Event handler. Called by Dashboard for resize events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        void Dashboard_Resize(object sender, System.EventArgs e)
        {


        }
        #region Progress Bar and Background
        /// <summary>   Calculates. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="i">    Zero-based index of the. </param>
        void Calculate(int i)
        {
            double pow = Math.Pow(i, i);
        }
        /// <summary>   Progress bar. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        void ProgressBar()
        {
                if (!backgroundWorker.IsBusy)
                    backgroundWorker.RunWorkerAsync();
        }
        /// <summary>   Event handler. Called by backgroundWorker for do work events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Do work event information. </param>
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            Parallel.Invoke(() =>
            {
                for (int j = 0; j <= 100; j++)
                {
                    Task.Delay(((j + 1) * 10) / 2);
                    backgroundWorker.ReportProgress(j);
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }, () =>
            {
                this.InvokeAndClose((MethodInvoker)delegate
                {
                    ClearControls();
                    switch (dashBoardControl.SelectedIndex)
                    {
                        case 0:
                            LoadConfiguration();
                            Notification("Global Configuration");
                            break;
                        case 1:
                            LoadServer();
                            Notification("Destination Server");
                            break;
                        case 2:
                            LoadTemplate();
                            LoadTemplate(new AppConfig());
                            Notification("Template Configuration");
                            break;
                        case 3:
                            LoadMapping();
                            Notification("Mapping");
                            break;
                        case 4:
                            LoadLoader();
                            Notification("Loader");
                            break;
                        case 5:
                        default:
                            LoadInstance();
                            Notification("Connection");
                            break;
                    }

                });
            });
        }   
        /// <summary>   Event handler. Called by backgroundWorker for progress changed events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Progress changed event information. </param>
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
        /// <summary>
        /// Event handler. Called by backgroundWorker for run worker completed events.
        /// </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Run worker completed event information. </param>
        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // An error occurred
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // The process was cancelled
                MessageBox.Show("Job cancelled.","Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
               
            }
            this.backgroundWorker.CancelAsync();
        }
        /// <summary>   Notifications. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="message">  The message. </param>
        private void Notification(string message)
        {
            this.notifyIconDashBoard.BalloonTipText = message;
            this.notifyIconDashBoard.ShowBalloonTip(100);
            this.notifyIconDashBoard.Text = message;
            
        }
        #endregion
        #region Global Configuration
        void LoadConfiguration() {

            this.connectionSplitContainer.Panel1.Controls.Clear();
            this.connectionSplitContainer.Panel1.Controls.Add(new ConnectionControl());
        }
        #endregion
        #region Template Tab
        /// <summary>   Loads the home. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        public void LoadTemplate()
        {
            try
            {
                this.dashBoardSplitContainer.Panel2.Controls.Clear();
                TemplateListControl tV = new TemplateListControl();
                tV.Name = "loadTemplate";
                tV.OnDoubleClicked += OnDoubleClick;
                tV.OnDeleteGrid += OnDeleteTemplate;
                tV.OnCopyGrid += OnCopyTemplate;
                tV.LoadGrid(GetList());
                this.dashBoardSplitContainer.Panel2.Controls.Add(tV);
                //tV.OnDoubleClicked -= OnDoubleClick;
                //tV.OnDeleteGrid -= OnDeleteTemplate;
                //tV.OnCopyGrid -= OnCopyTemplate;
                tV = null;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadTemplate(AppConfig input)
        {
            try
            {

                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                dashBoardSplitContainer.Panel1.Controls.Clear();
                CreateOrEditTemplateControl cT = new CreateOrEditTemplateControl(input, this.max);
                cT.CheckTable += OnCheckTable;
                cT.SaveChanges += SaveChanges;
                cT.ResetChanges += ResetChanges;
                cT.CheckDuplicate += CheckDuplicate;
                List<ServerInfo> info = new List<ServerInfo>();
                info.Add(new ServerInfo() { ServerName = "Choose Server" });
                info.AddRange(dbContext.ICommands.GetList<ServerInfo>());
                cT.LoadServer(info);
                if (!string.IsNullOrEmpty(input.ServerId))
                {
                    ServerInfo si = dbContext.ICommands.GetEntity<ServerInfo>(x => x.Id.Equals(input.ServerId));
                    if (si != null)
                    {
                        ((ComboBox)cT.Controls["cbxServer"]).SelectedIndex = ((ComboBox)cT.Controls["cbxServer"]).FindStringExact(si.ServerName);
                    }
                    cT.LoadTable();
                    ((ComboBox)cT.Controls["cbxTable"]).SelectedIndex = ((ComboBox)cT.Controls["cbxTable"]).FindStringExact(input.TargetTableName);
                    si = null;
                }
                dashBoardSplitContainer.Panel1.Controls.Add(cT);
                //cT.CheckTable -= OnCheckTable;
                //cT.SaveChanges -= SaveChanges;
                //cT.ResetChanges -= ResetChanges;
                //cT.CheckDuplicate -= CheckDuplicate;
                cT = null;
                info = null;
                dbContext.Dispose();
                dbContext = null;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnCopyTemplate(string Id, string templateName)
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                AppConfig input = dbContext.ICommands.GetEntity<AppConfig>(x => x.Id == Id);
                input.RecId = templateName;
                input.Id = Guid.NewGuid().ToString();
                input.CreatedBy = Environment.UserName;
                input.CreatedOn = DateTime.UtcNow;
                dbContext.ICommands.Insert<AppConfig>(input);
                input = null;
                dbContext.Dispose();
                dbContext = null;
                ((TemplateListControl)this.dashBoardSplitContainer.Panel2.Controls["loadTemplate"]).LoadGrid(GetList());
                MessageBox.Show("Template Copied successfully!", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>   Executes the delete template action. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="Id">   The identifier. </param>
        private void OnDeleteTemplate(string Id)
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                dbContext.ICommands.Delete<AppConfig>(x => x.Id == Id);
                dbContext.Dispose();
                dbContext = null;
                ((TemplateListControl)this.dashBoardSplitContainer.Panel2.Controls["loadTemplate"]).LoadGrid(GetList());
                MessageBox.Show("Template Deleted successfully!", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>   Loads a template. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="input">    The input. </param>
        private bool CheckDuplicate(AppConfig info)
        {
            bool result = false;
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                if (string.IsNullOrEmpty(info.Id))
                {
                    result = dbContext.ICommands.GetEntity<AppConfig>(x => x.RecId.Equals(info.RecId)) == null ? false : true;
                }
                else
                {
                    result = dbContext.ICommands.GetEntity<AppConfig>(x => x.RecId.Equals(info.RecId) && x.Id != info.Id) == null ? false : true;
                }
                dbContext.Dispose();
                dbContext = null;
            }
            catch { }
            return result;
        }
        private object[] OnCheckTable(string connString,int dbProvider)
        {
            object[] result = null;
            try
            {

                using (System.Data.Common.DbConnection conn = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(dbProvider) as System.Data.Common.DbConnection)
                {
                    conn.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(connString, true);
                    conn.Open();
                    if (dbProvider == WD.DataAccess.Enums.DBProvider.Oracle || dbProvider == WD.DataAccess.Enums.DBProvider.Oracle2)
                    {
                          using (IDbCommand command = conn.CreateCommand())
                          {
                            command.CommandText = "SELECT Table_Name FROM user_tables";
                            command.CommandType = CommandType.Text;
                            using (IDataReader reader = command.ExecuteReader())
                            {
                                DataTable dtOne = new DataTable();
                                while (!reader.IsClosed)
                                    dtOne.Load(reader);
                                result = (from DataRow r in dtOne.Rows select r["Table_Name"].ToString()).ToArray();
                            }
                        }
                    }
                    else {
                         DataTable dt = conn.GetSchema("Tables");
                         result = (from DataRow r in dt.Rows select r["Table_Name"].ToString()).ToArray();
                    }
                }

            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        /// <summary>   Saves the changes. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="input">    The input. </param>
        private void SaveChanges(AppConfig input)
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                if (string.IsNullOrEmpty(input.Id))
                {

                    try
                    {
                        Mapping mapper = Utility.ConvertJsonToObject<Mapping>(input.Config);
                        using (IDbConnection con = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(input.DbProvider))
                        {
                            con.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(input.ConnString, true);
                            using (IDbCommand com = con.CreateCommand())
                            {
                                com.CommandText = string.Format("select * from {0}", input.TargetTableName); ;
                                com.CommandType = CommandType.Text;
                                con.Open();
                                DataTable dt =  com.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();
                                if (dt.Rows.Count > 0)
                                {
                                    int count = 1;
                                    mapper.Columns = (from DataRow row in dt.Rows
                                                      select new ColumnConfig
                                                      {
                                                          Index = count,
                                                          ColumnName = "F" + (count++),
                                                          DataType = row["DATATYPE"].ToString().Replace("System.", string.Empty).Replace("Boolean", "bool").ToLower(),
                                                          DefaultValue = string.Empty,
                                                          FieldName = row["COLUMNNAME"].ToString(),
                                                          Format = string.Empty,
                                                          Length = String.IsNullOrEmpty(row["COLUMNSIZE"].ToString()) ? "0" : row["COLUMNSIZE"].ToString(),
                                                          IsPrimary = ((row["IsKey"].ToString() == "True") || (row["IsUnique"].ToString() == "True")) ? true : false
                                                      }).ToList();

                                    input.Config = Utility.ConvertObjectToJson<Mapping>(mapper).Trim();
                                }
                            }
                        }
                        input.Id = Guid.NewGuid().ToString();
                        input.CreatedBy = Environment.UserName;
                        input.CreatedOn = DateTime.UtcNow;
                        input.UpdatedBy = string.Empty;
                        dbContext.ICommands.Insert<AppConfig>(input);
                        mapper = null;
                        ((TemplateListControl)this.dashBoardSplitContainer.Panel2.Controls["loadTemplate"]).LoadGrid(GetList());
                        LoadTemplate(new AppConfig());
                        MessageBox.Show("Changes saved successfully!", "Save Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        input.Id = string.Empty;
                        throw;
                    }
                }
                else
                {

                    try
                    {
                        if (dbContext.ICommands.GetEntity<AppConfig>(x => x.Id != input.Id && x.RecId == input.RecId) == null)
                        {
                            Mapping mapper = Utility.ConvertJsonToObject<Mapping>(input.Config);
                            using (IDbConnection con = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(input.DbProvider))
                            {
                                con.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(input.ConnString, true);
                                using (IDbCommand com = con.CreateCommand())
                                {
                                    com.CommandText = String.Format("select * from {0}", input.TargetTableName);
                                    com.CommandType = CommandType.Text;
                                    con.Open();
                                    DataTable dt = com.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();
                                    if (dt.Rows.Count > 0)
                                    {
                                        int count = 1;
                                        mapper.Columns = (from DataRow row in dt.Rows
                                                          select new ColumnConfig
                                                          {
                                                              Index = count,
                                                              ColumnName = "F" + (count++),
                                                              DataType = row["DATATYPE"].ToString().Replace("System.", string.Empty).Replace("Boolean", "bool").ToLower(),
                                                              DefaultValue = string.Empty,
                                                              FieldName = row["COLUMNNAME"].ToString(),
                                                              Format = string.Empty,
                                                              Length = String.IsNullOrEmpty(row["COLUMNSIZE"].ToString()) ? "0" : row["COLUMNSIZE"].ToString(),
                                                              IsPrimary = ((row["IsKey"].ToString() == "True") || (row["IsUnique"].ToString() == "True")) ? true : false
                                                          }).ToList();

                                        input.Config = Utility.ConvertObjectToJson<Mapping>(mapper).Trim();
                                    }
                                }
                            }
                            input.UpdatedBy = Environment.UserName;
                            input.UpdatedOn = DateTime.UtcNow;
                            dbContext.ICommands.Update<AppConfig>(input, x => x.Id == input.Id);
                            LoadTemplate();
                            ((TemplateListControl)this.dashBoardSplitContainer.Panel2.Controls["loadTemplate"]).LoadGrid(GetList());
                             MessageBox.Show("Changes saved successfully!", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            LoadTemplate(input);
                            MessageBox.Show("Template name already present.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
                dbContext.Dispose();
                dbContext = null;
            }
            catch (Exception exc)
            {
                LoadTemplate(input);
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                input = null;
            }
        }
        /// <summary>   Resets the changes. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        private void ResetChanges()
        {
            LoadTemplate(new AppConfig());
        }
        /// <summary>   Raises the data grid view cell event. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information to send to registered event handlers. </param>
        private void OnDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                try
                {
                    DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                    DataGridViewRow row = ((DataGridView)sender).Rows[e.RowIndex];
                    string id = row.Cells[0].Value.ToString();
                    LoadTemplate(dbContext.ICommands.GetEntity<AppConfig>(x => x.Id == id));
                    row.Selected = true;
                    dbContext.Dispose();
                    dbContext = null;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private  void dashBoardControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProgressBar();
        }
        #endregion
        #region Mapping Tab
        /// <summary>   Loads the mapping. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        private void LoadMapping()
        {
            try
            {
                this.mappingPage.Controls.Clear();
                MappingControl mC = new MappingControl(this.max);
                mC.AfterSelected += AfterSelected;
                mC.SaveChanges += SaveChanges;
                mC.LoadTree(GetList());
                this.mappingPage.Controls.Add(mC);
                //mC.AfterSelected -= AfterSelected;
                //mC.SaveChanges -= SaveChanges;
                mC = null;
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>   Saves the changes. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="input_rows">   The input rows. </param>
        /// <param name="id">           The identifier. </param>


        private void SaveChanges(List<ColumnConfig> input_rows, string id)
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                AppConfig appConfig = dbContext.ICommands.GetEntity<AppConfig>(x => x.Id == id);
                Mapping config = Utility.ConvertJsonToObject<Mapping>(appConfig.Config);
                config.Columns = input_rows;
                appConfig.Config = Utility.ConvertObjectToJson<Mapping>(config);
                appConfig.UpdatedBy = Environment.UserName;
                appConfig.UpdatedOn = DateTime.UtcNow;
                dbContext.ICommands.Update<AppConfig>(appConfig, x => x.Id == id);
                dbContext.Dispose();
                dbContext = null;
                MessageBox.Show("Changes saved successfully!", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                input_rows = null;
                id = string.Empty;
            }
        }

        /// <summary>   After selected. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Tree view event information. </param>
        /// <param name="mc">       The mc. </param>


        private AppConfig AfterSelected(object sender, TreeViewEventArgs e)
        {
            AppConfig input = null;
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                string id = e.Node.Name;
                input = dbContext.ICommands.GetEntity<AppConfig>(x => x.Id == id);
                dbContext.Dispose();
                dbContext = null;
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return input;
        }
        #endregion
        #region Loader Tab
        public  void ChangeTab(int index) {

            try
            {
                
                if (runLoader)
                    this.dashBoardControl.SelectedIndex = index;
            }
            catch
            {


            }
        }
        private bool isLoaded = false;
        /// <summary>   Loads the loader. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        private void LoadLoader()
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                if (!isLoaded)
                {
                   
                    LoaderControl loadingControl = new LoaderControl(this.max, dbContext.ICommands.GetEntity<ServerInfo>("SELECT  S.* FROM (XLC_ServerProcess sp LEFT OUTER JOIN  XLC_ServerInfo  S ON S.ID = sp.ServerId) WHERE sp.ProcessId =" + this.max.ToString() + " AND sp.MachineId='" + Environment.MachineName + "'"));
                    loadingControl.Name = "loadingControl";
                    loadingControl.SaveFileCount += SaveFileCount;
                    loadingControl.OnAddFolder += AddFolder;
                    loadingControl.OnDeleteFolder += DeleteFolder;
                    loadingControl.LoadFolder(dbContext.ICommands.GetList<FolderStructure>(x => x.IsActive == 1));
                    loadingControl.LoadGrid(GetList(x => x.IsActive == 1));
                    LoaderPage.Controls.Add(loadingControl);
                    if (runLoader) {
                        loadingControl.Toggle();
                    }
                    loadingControl.LoadMenu();
                    //loadingControl.SaveFileCount -= SaveFileCount;
                    //loadingControl.OnAddFolder -= AddFolder;
                    //loadingControl.OnDeleteFolder -= DeleteFolder;
                    loadingControl = null;
                    isLoaded = true;
                }
                else
                {
                    
                    ((LoaderControl)LoaderPage.Controls["loadingControl"]).LoadGrid(GetList(x => x.IsActive == 1));
                }
                dbContext.Dispose();
                dbContext = null;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteFolder(string id)
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                dbContext.ICommands.Delete<FolderStructure>(x => x.Id == id);
                if (MessageBox.Show("Folder deleted successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                    ((LoaderControl)LoaderPage.Controls["loadingControl"]).LoadFolder(dbContext.ICommands.GetList<FolderStructure>(x => x.IsActive == 1));
                dbContext.Dispose();
                dbContext = null;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool AddFolder(FolderStructure fs)
        {
            bool result = false;
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                if (dbContext.ICommands.GetEntity<FolderStructure>(x => x.FolderName == fs.FolderName) != null)
                {
                    MessageBox.Show(fs.FolderName + " alread added!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else 
                {
                    dbContext.ICommands.Insert<FolderStructure>(fs);
                    ((LoaderControl)LoaderPage.Controls["loadingControl"]).LoadFolder(dbContext.ICommands.GetList<FolderStructure>(x => x.IsActive == 1));
                    result = true;
                }
                dbContext.Dispose();
                dbContext = null;
            }
            catch 
            {
                throw;
            }
            return result;
        }
        private void SaveFileCount(int aCount)
        {
            try
            {
                if (ConfigurationManager.AppSettings.Get("EnableFileProcessedCount").ToLower()=="true")
                {

                    DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                    string currentDate = DateTime.UtcNow.ToString("yyyyMMdd");
                    FileProcess fp = dbContext.ICommands.GetEntity<FileProcess>(x => x.CreatedOn == currentDate);
                    if (fp != null)
                    {

                        fp.FileCount = fp.FileCount + aCount;
                        dbContext.ICommands.Update<FileProcess>(fp, x => x.CreatedOn == currentDate);
                    }
                    else
                    {
                        dbContext.ICommands.Insert<FileProcess>(new FileProcess() { CreatedOn = currentDate, FileCount = aCount });
                    }
                    fp = null;
                    dbContext.Dispose();
                    dbContext = null;

                }
            }
            catch
            { }
                
        }
        #endregion
        #region Server
        private void LoadServer()
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                this.serverPageSplitContainer.Panel2.Controls.Clear();
                ServerInfoListControl sLV = new ServerInfoListControl(this.max);
                sLV.LoadGrid(dbContext.ICommands.GetList<ServerInfo>());
                sLV.OnServerDeleteGrid += OnServerDeleteGrid;
                sLV.OnServerDoubleClicked += OnServerDoubleClicked;
                this.serverPageSplitContainer.Panel2.Controls.Add(sLV);
                //sLV.OnServerDeleteGrid -= OnServerDeleteGrid;
                //sLV.OnServerDoubleClicked -= OnServerDoubleClicked;
                sLV = null;
                dbContext.Dispose();
                dbContext = null;
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnServerDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                try
                {
                    DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                    DataGridViewRow row = ((DataGridView)sender).Rows[e.RowIndex];
                    string id = row.Cells[0].Value.ToString();
                    var f = new FormConnect(dbContext.ICommands.GetEntity<ServerInfo>(x => x.Id == id));
                    f.SaveChanges += ServerChanges;
                    f.ShowDialog();
                    row.Selected = true;
                    dbContext.Dispose();
                    dbContext = null;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void OnServerDeleteGrid(string id)
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                if (dbContext.ICommands.GetEntity<AppConfig>(x => x.ServerId == id) != null)
                {
                    MessageBox.Show("Cannot delete server details as it's assigned to templates.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (dbContext.ICommands.GetEntity<ServerProcess>(x => x.ServerId == id) != null)
                {
                    MessageBox.Show("Cannot delete server details as it's used by instance(s) as override connection for templates.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    dbContext.ICommands.Delete<ServerInfo>(x => x.Id == id);
                    MessageBox.Show("Server details deleted successfully!", "Server Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadServer();
                }
                dbContext.Dispose();
                dbContext = null;
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAddServer_Click(object sender, EventArgs e)
        {
            var f = new FormConnect(new ServerInfo());
            f.SaveChanges += ServerChanges;
            f.ShowDialog();
        }
        private void ServerChanges(ServerInfo info)
        {
            try
            {
                info.Password = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.Password, true);
                info.UserID = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.UserID, true);
                info.ConnString = WD.DataAccess.Helpers.WebSecurityUtility.Encrypt(info.ConnString, true);
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                if (string.IsNullOrEmpty(info.Id))
                {
                    info.Id = Guid.NewGuid().ToString();
                    info.CreatedBy = Environment.UserName;
                    info.CreatedOn = DateTime.UtcNow;
                    if (dbContext.ICommands.GetList<ServerInfo>(x => x.ServerName.Equals(info.ServerName)).Count() > 0)
                    {
                        if (MessageBox.Show("Server already added.Do you want to add duplicate record?", "Server Change", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            dbContext.ICommands.Insert<ServerInfo>(info);
                        }
                    }
                    else
                    {
                        
                        dbContext.ICommands.Insert<ServerInfo>(info);
                        MessageBox.Show("Changes saved successfully!", "Save Changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    info.UpdatedBy = Environment.UserName;
                    info.UpdatedOn = DateTime.UtcNow;
                    if (dbContext.ICommands.GetEntity<AppConfig>(x => x.ServerId == info.Id)!=null)
                    {

                        if (MessageBox.Show("Changes will effect all templates assigned with the current server.Do you want to continue?", "Server Change", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            dbContext.ICommands.ExecuteNonQuery("UPDATE XLC_AppConfig SET ConnString='" + info.ConnString + "',DbProvider='" + info.DbProvider + "',ServerName='" + info.ServerName + "' WHERE ServerId='" + info.Id + "'");
                            // Cancel the Closing event from closing the form.
                            dbContext.ICommands.Update<ServerInfo>(info, x => x.Id == info.Id);
                            MessageBox.Show("Changes saved successfully!", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        dbContext.ICommands.Update<ServerInfo>(info, x => x.Id == info.Id);
                        MessageBox.Show("Changes saved successfully!", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                dbContext.Dispose();
                dbContext = null;
                LoadServer();

            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Instance Config
        private void LoadInstance()
        {
            try
            {
              
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                instanceSplitContainer.Panel1.Controls.Clear();
                InstanceConfigControl ic = new InstanceConfigControl(this.max);
                ic.Bind(dbContext.ICommands.GetEntity<ServerProcess>(x => x.ProcessId == this.max && x.MachineId == Environment.MachineName));
                ic.LoadGrid(dbContext.ICommands.ExecuteDataTable("SELECT sp.ProcessId, sp.MachineId, S.ID as ServerId, S.ServerName FROM (XLC_ServerInfo S LEFT OUTER JOIN XLC_ServerProcess sp ON S.ID = sp.ServerId)"));
                ic.SaveConfig += SaveConfig;
                ic.DeleteConfig += DeleteConfig;
                instanceSplitContainer.Panel1.Controls.Add(ic);
                //ic.SaveConfig -= SaveConfig;
                //ic.DeleteConfig -= DeleteConfig;
                ic = null;
                dbContext.Dispose();
                dbContext = null;
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteConfig()
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                dbContext.ICommands.Delete<ServerProcess>(x => x.MachineId.Equals(Environment.MachineName) && x.ProcessId.Equals(this.max));
                dbContext.Dispose();
                dbContext = null;
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveConfig(string id)
        {
            try
            {
                DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                dbContext.ICommands.Delete<ServerProcess>(x => x.MachineId.Equals(Environment.MachineName) && x.ProcessId.Equals(this.max));
                dbContext.ICommands.Insert<ServerProcess>(new ServerProcess() { MachineId = Environment.MachineName, ProcessId = this.max, ServerId = id });
                dbContext.Dispose();
                dbContext = null;
                MessageBox.Show("Changes saved successfully!", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Clear Controls
        void ClearControls() {

            this.connectionSplitContainer.Panel1.Controls.Clear();
            this.serverPageSplitContainer.Panel2.Controls.Clear();
            this.mappingPage.Controls.Clear();
            this.instanceSplitContainer.Panel1.Controls.Clear();
            this.dashBoardSplitContainer.Panel2.Controls.Clear();
            this.dashBoardSplitContainer.Panel1.Controls.Clear();
            
        }
        #endregion
        #region AppConfig
        public virtual List<AppConfig> GetList(Expression<Func<AppConfig, bool>> predicate = null)
        {
            List<AppConfig> iList = new List<AppConfig>();
            try
            {
                    DbContext dbContext = new DbContext(new Connect() { ConnectionString = this.conString, DbProvider = dbProvider });
                    iList = dbContext.ICommands.GetList<AppConfig>(predicate, WD.DataAccess.Enums.SortOption.ASC, x => x.RecId);
                    dbContext.Dispose();
                    dbContext = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return iList;
        }
        #endregion
        #region Delete 
        private async void ScheduleAction(Action action, int delay)
        {
            try
            {
                await Task.Delay(delay);
                action();
                WD.DataAccess.Logger.ILogger.Debug(String.Format("******************End: {0}**********************************", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));
                TimeInterval();
            }
            catch(Exception exc)  {
                WD.DataAccess.Logger.ILogger.Debug(exc.Message);
            }
        }
        private void Execute(Action action, DateTime ExecutionTime)
        {
            Task WaitTask = Task.Delay((int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds);
            WaitTask.ContinueWith(_ => action);
            WaitTask.Start();
        }
        void TimeInterval()
        {
            try
            {
                DateTime startTime = DateTime.ParseExact(ConfigurationManager.AppSettings.Get("DeleteTime"), "hh:mm", System.Globalization.CultureInfo.InvariantCulture);
                int hrs = startTime.Hour;
                int mins = startTime.Minute;
                int secs = startTime.Second;
                int delay = (hrs + mins + secs) * 1000;
                if (startTime < DateTime.Now)
                {
                      startTime = startTime.AddDays(1);
                      hrs = (24 + startTime.Hour - DateTime.Now.Hour) * 60;
                      mins = startTime.Minute - DateTime.Now.Minute;
                      secs = startTime.Second - DateTime.Now.Second;
                      delay = (((hrs + mins) * 60) + secs) * 1000;
                }
                else
                {
                    hrs = (hrs - DateTime.Now.Hour) * 60 * 60;
                    mins = (mins - DateTime.Now.Minute) * 60;
                    delay = (hrs + mins - DateTime.Now.Second) * 1000;
                }
                WD.DataAccess.Logger.ILogger.Debug(String.Format("******************Start: {0}********************************", DateTime.Now.AddMilliseconds(delay).ToString("MM/dd/yyyy hh:mm:ss tt")));
                ScheduleAction(() => { DeleteProcess(); }, delay);
            }
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
        }
        private  void DeleteProcess()
        {
            try
            {
                #region Delete Files
                foreach (string folder in ConfigurationManager.AppSettings.Get("DeleteFolders").Split(','))
                {
                    WD.DataAccess.Logger.ILogger.Debug(String.Format("Started deleting files from {0} at {1}.", folder, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));
                    WD.DataAccess.Logger.ILogger.Debug("**********************************************************************");
                    DirectoryInfo info = new DirectoryInfo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), folder));
                    if (info.Exists)
                    {
                        string batchFile = "delete_" + folder + "_" + max.ToString() + ".bat";
                        {
                            string command = ConfigurationManager.AppSettings.Get("CreateDeleteLog").ToLower() == "true" ? Properties.Settings.Default.DeleteFiles : Properties.Settings.Default.DeleteFilesWithoutLog;
                            command = command.Replace("#baseDir#", info.FullName);
                            command = command.Replace("#logDir#", Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), folder + DateTime.Now.ToString("MMddyyyy") + ".log"));
                            command = command.Replace("#days#", ConfigurationManager.AppSettings.Get("ArchiveDay"));
                            WD.DataAccess.Logger.ILogger.Debug(command);
                            using (FileStream stream = File.Open(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                StreamWriter outfile = new StreamWriter(stream);
                                {
                                    outfile.WriteLine(command);
                                    outfile.Flush();
                                }
                            }
                            using (Process process = new Process())
                            {
                                process.StartInfo = new ProcessStartInfo()
                                {
                                    WorkingDirectory =System.AppDomain.CurrentDomain.BaseDirectory,
                                    FileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile),
                                    UseShellExecute = true,
                                    Verb = "runas",
                                    CreateNoWindow = true,
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    Domain = Environment.UserDomainName,
                                };
                                process.Start();
                               // process.WaitForExit();
                            }
                        }
                    }
                    WD.DataAccess.Logger.ILogger.Debug(String.Format("Completed deleting files from {0} at {1}.", folder, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));
                    WD.DataAccess.Logger.ILogger.Debug("**********************************************************************");
                }
                #endregion
            } 
            catch (Exception exc)
            {
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
        }
        #endregion
    }
}