using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFramework.Controls;
using System.Configuration;
using System.IO;
using WD.XLC.Domain.Entities;
using WD.XLC.Domain.Helpers;
using WD.DataAccess.Helpers;
using WD.DataAccess.Enums;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using WD.DataAccess.Context;
using System.Data.Common;
using System.Windows.Forms;
namespace WD.XLC.WIN.Controls
{
    /// <summary>   A loader control. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    public partial class LoaderControl : MetroUserControl
    {
        public delegate bool AddFolder(FolderStructure fs);
        public AddFolder OnAddFolder;
        public delegate void DeleteFolder(string id);
        public DeleteFolder OnDeleteFolder;
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="max">  The maximum. </param>
        public LoaderControl(int max, ServerInfo serverInfo)
        {
            this.serverInfo = serverInfo;
            this.max = max;
            InitializeComponent();
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instance = Utility.GetInstance(max);
            this.timerLoad.Interval = HelperUtility.ConvertTo<int>(String.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeInterval"]) ? "1" : ConfigurationManager.AppSettings["TimeInterval"], 1);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public string GetDate(int dbProvider)
        {


            string returnValue = string.Empty;
            switch (dbProvider)
            {
                case DBProvider.Oracle:
                case DBProvider.Oracle2:
                    returnValue = "sysdate";
                    break;
                case DBProvider.Db2:
                    returnValue = "CURRENT TIMESTAMP";
                    break;
                case DBProvider.MySql:
                case DBProvider.Access:
                case DBProvider.PostgreSQL:
                    returnValue = "now()";
                    break;
                default:
                    returnValue = "getdate()";
                    break;
            }
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public static string GetDateFormatted(string format, int dbProvider)
        {
            string returnValue = string.Empty;
            switch (dbProvider)
            {
                case DBProvider.Oracle:
                case DBProvider.Oracle2:
                    returnValue = "to_date(to_char(sysdate,'" + format + "'),'" + format + "')";
                    break;
                default:
                    returnValue = "CAST(getdate() AS Date)";
                    break;
            }
            return returnValue;
        }

        public void LoadMenu()
        {

            try
            {
                this.cmbxLog.Items.Clear();
                string log4netPath = ConfigurationManager.AppSettings["log4net.Config"];
                if (!string.IsNullOrEmpty(log4netPath))
                {
                    this.cmbxLog.Items.AddRange((from r in WD.DataAccess.Helpers.ObjectXMLSerializer<WD.DataAccess.Logger.Log4netConfiguration>.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, log4netPath)).Log4net.Appenders select r.Name).ToArray());
                }
            }
            catch (Exception ex)
            {
                LogRecord(Level.Info, ex.Message);

            }
        }
        internal void LoadFolder(List<FolderStructure> folderList)
        {
            grdFolderList.Rows.Clear();
            grdFolderList.AutoGenerateColumns = false;
            foreach (FolderStructure f in folderList)
            {
                bool flag = this.instance.Folders.Count > 0 ? (from ff in this.instance.Folders
                                                                 where ff == f.FolderName
                                                               select f).FirstOrDefault() != null ? true : false : false;
                System.Windows.Forms.DataGridViewRow row = new System.Windows.Forms.DataGridViewRow();
                row.CreateCells(grdFolderList, new object[] { f.Id, flag, f.FolderName,f.FolderExtension });
                grdFolderList.Rows.AddRange(row);
            }
        }
        /// <summary>   Loads the grid. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        public void LoadFolder()
        {
            grdFolderList.Rows.Clear();
            grdFolderList.AutoGenerateColumns = false;
            DirectoryInfo info = new DirectoryInfo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString()));
            if (info.Exists)
            {
                GetDirectories(info.GetDirectories());
            }
            info = new DirectoryInfo(Utility.GetInstance(max).Inbox);
            if (info.Exists)
            {
                System.Windows.Forms.DataGridViewRow row = new System.Windows.Forms.DataGridViewRow();
                row.CreateCells(grdFolderList, new object[] { "-1", flag, info.FullName, "*.*" });
                grdFolderList.Rows.AddRange(row);
                grdFolderList.Rows[grdFolderList.Rows.Count - 1].Cells[2].ReadOnly = false;
                grdFolderList.Rows[grdFolderList.Rows.Count - 1].Cells[1].ReadOnly = true;
            }
            else
            {
                MessageBox.Show(String.Format("Folder [{0}] not present.Please create folder or change path location in App.Config file and delete xlcloader file.", Utility.GetInstance(max).Inbox));
            }
        }
        /// <summary>   Gets the directories. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="subDirs">  The sub directories. </param>
        private void GetDirectories(DirectoryInfo[] subDirs)
        {
            foreach (DirectoryInfo subDir in subDirs)
            {
                System.Windows.Forms.DataGridViewRow row = new System.Windows.Forms.DataGridViewRow();
                row.CreateCells(grdFolderList, new object[] { "0",subDir.FullName.Contains("Working") ? true : false, subDir.FullName,"*.*" });
                row.ReadOnly = subDir.FullName.Contains("Working") ? false : true;
                row.DefaultCellStyle.BackColor = subDir.FullName.Contains("Working") ? Color.White : Color.LightGray;
                grdFolderList.Rows.AddRange(row);
            }
        }
        /// <summary>   Loads the grid. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="iList">    Zero-based index of the list. </param>
        public void LoadGrid(List<AppConfig> iList)
        {
            grdTemplateList.Rows.Clear();
            grdTemplateList.AutoGenerateColumns = false;

            foreach (AppConfig list in iList)
            {
                System.Windows.Forms.DataGridViewRow row = new System.Windows.Forms.DataGridViewRow();
                bool flag = this.instance.Templates.Count > 0 ? (from f in this.instance.Templates
                                                                 where f == list.RecId
                                                                 select f).FirstOrDefault() != null ? true : false : true;
                row.CreateCells(grdTemplateList, new object[] {flag, list.RecId, list.Id, list.Config, list.TargetTableName, list.ConnString, list.DbProvider.ToString()});
                grdTemplateList.Rows.AddRange(row);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void header_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in grdTemplateList.Rows.Cast<DataGridViewRow>())
            {
                row.Cells[0].Value = state;
            }
        }
        #region Members
        /// <summary> /// /// </summary>
        /// <summary>   True to flag. </summary>
        private bool flag = false;
        /// <summary>   The start time. </summary>
        private DateTime startTime { get; set; }
        /// <summary>   The no of records. </summary>
        int noOfRecords =0;
        /// <summary>   The no of corrupt records. </summary>
         int noOfCorruptRecords = 0;
        /// <summary>   The no of updates. </summary>
         int noOfUpdates = 0;
        /// <summary>   Number of files. </summary>
         int fileCount = 0;
        /// <summary>   Number of totals. </summary>
         int totalCount = 0;
        #endregion
        /// <summary>   Gets database type. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="p">    A string to process. </param>
        ///
        /// <returns>   The database type. </returns>
        private ItemSpecification.JetDataType GetDbType(string p)
        {
            ItemSpecification.JetDataType dbType = ItemSpecification.JetDataType.Text;
            switch (p.ToLower())
            {
                case "datetime":
                    dbType = ItemSpecification.JetDataType.DateTime;
                    break;
                case "string":
                    dbType = ItemSpecification.JetDataType.Text;
                    break;
                case "int":
                case "int32":
                case "int64":
                case "long":
                    dbType = ItemSpecification.JetDataType.Text;
                    break;
                case "bool":
                    dbType = ItemSpecification.JetDataType.Bit;
                    break;
                case "double":
                case "decimal":
                case "float":
                    dbType = ItemSpecification.JetDataType.Double;
                    break;
                case "byte":
                    dbType = ItemSpecification.JetDataType.Byte;
                    break;
                default:
                    dbType = ItemSpecification.JetDataType.Text;
                    break;
            }
            return dbType;
        }
        /// <summary>   Event handler. Called by grdFolderList for cell leave events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Data grid view cell event information. </param>
        private void grdFolderList_CellLeave(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (grdFolderList.Rows[e.RowIndex].Cells[0].Value.ToString() != "0")
                {
                    string folderLocation = grdFolderList.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString();
                    DirectoryInfo info = new DirectoryInfo(folderLocation);
                    if (info.Exists)
                    {
                        instance.Inbox = info.FullName;
                        Utility.SaveInstance(instance);
                    }
                    else
                    {
                        MessageBox.Show("Please enter valid folder location.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        /// <summary>
        /// Event handler. Called by toggleStartProcess for checked changed events.
        /// </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        public void Toggle()
        {
           toggleStartProcess.Checked = true;
        }
        private void toggleStartProcess_CheckedChanged(object sender, EventArgs e)
        {
            if (!toggleStartProcess.Checked)
            {
                LogRecord(Level.Info, "***Timer End*** \n");
            }
            else
            {
                if (!flag)
                {
                    if (grdFolderList.Rows.Cast<System.Windows.Forms.DataGridViewRow>().Where(x => x.Cells[1].Value.ToString() == "True").FirstOrDefault() == null)
                    {

                        MessageBox.Show("Please select folder", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        toggleStartProcess.Checked = false;
                    }
                    else if (grdTemplateList.Rows.Cast<System.Windows.Forms.DataGridViewRow>().Where(x => x.Cells[0].Value.ToString() == "True").FirstOrDefault() == null)
                    {
                        MessageBox.Show("Please select template", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        toggleStartProcess.Checked = false;
                    }
                    else
                    {
                        noOfUpdates = 0;
                        noOfRecords = 0;
                        noOfCorruptRecords = 0;
                        totalCount = 0;
                        fileCount = 0;
                        startTime = DateTime.Now;
                        instance.Templates = (from c in grdTemplateList.Rows.Cast<System.Windows.Forms.DataGridViewRow>().Where(x => x.Cells[0].Value.ToString() == "True")
                                              select c.Cells[1].Value.ToString()).ToList();
                        instance.Folders = (from c in grdFolderList.Rows.Cast<System.Windows.Forms.DataGridViewRow>().Where(x => x.Cells[1].Value.ToString() == "True")
                                            select c.Cells[2].Value.ToString()).ToList(); ;
                        Utility.SaveInstance(instance);
                        instance.Folders = new List<string>();
                        instance.Templates = new List<string>();
                        LogRecord(Level.Info, "***Timer Start*** \n");
                        timerLoad.Enabled = true;


                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!flag)
            {
                flag = true;
                StartLoad(this.max, Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), "Pending"), "*.*");
                foreach (DataGridViewRow row in grdFolderList.Rows.Cast<System.Windows.Forms.DataGridViewRow>().Where(x => x.Cells[1].Value.ToString() == "True"))
                {
                    StartLoad(this.max, row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString());
                }
                Run();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        /// <param name="folderLocation"></param>
        private void StartLoad(int max, string folderLocation, string fileExtension)
        {

            try
            {
                DirectoryInfo info = new DirectoryInfo(folderLocation);
                if (info.Exists)
                {
                   
                        string batchFile = "batch_" + max.ToString() + ".bat";
                        if (!File.Exists(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile)))
                        {
                            string command = Properties.Settings.Default.CopyFiles;
                            command = command.Replace("#baseDir#", folderLocation);
                            command = command.Replace("#destDir#", Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), "Working"));
                            command = command.Replace("#fileExtension#", fileExtension);
                            command = command.Replace("#maxFiles#", System.Configuration.ConfigurationManager.AppSettings["FileCount"]);
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
                                    FileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile),
                                    UseShellExecute = true,
                                    Verb = "runas",
                                    CreateNoWindow = true,
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    Domain = Environment.UserDomainName,
                                };
                                process.Start();
                                process.WaitForExit();
                                process.Close();
                            }
                            File.Delete(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile));
                        }
                }
            }
            catch (Exception exc)
            {
                WriteMessage(3, "", "", exc);
            }
        }
        /// <summary>   Runs this object. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        private async void Run()
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    #region Folders
                    string baseFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), "Working");
                    string[] files = Directory.GetFiles(baseFolder);
                    if (files.Count() > 0)
                    {
                        totalCount += files.Count();
                        List<TimeCalculation> myCollection = new List<TimeCalculation>();
                        if (files.Count() < System.Environment.ProcessorCount)
                        {
                            foreach (string filePath in files)
                            {
                                Stopwatch sw = Stopwatch.StartNew();
                                ResultValue r = ReadFileData(filePath,false);
                                try
                                {
                                    myCollection.Add(new TimeCalculation() { TheDuration = new TimeSpan(sw.ElapsedTicks) });
                                    File.Copy(r.FilePath, r.DestinationPath, true);
                                    File.Delete(r.FilePath);
                                    FileProcess(1);
                                }
                                catch { }
                                finally
                                {
                                    sw.Stop();
                                    sw = null;
                                    fileCount = fileCount + 1;
                                    WriteMessage(4, string.Empty, r.FilePath +"=>"+r.DestinationPath, null, r.RecordsInserted, r.RecordsUpdated, r.RecordsCorrupt);
                                    r = null;
                                }
                            }
                            WriteAverage(String.Format("Avg Time:{0} sec(s) / {1} file(s)", new TimeSpan((long)myCollection.Average(x => x.TheDuration.Ticks)).TotalSeconds, files.Count()));
                        }
                        else
                        {
                            Parallel.ForEach(files, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, (filePath) =>
                            {
                                Stopwatch sw = Stopwatch.StartNew();
                                ResultValue r = ReadFileData(filePath,true);
                                try
                                {
                                    myCollection.Add(new TimeCalculation() { TheDuration = new TimeSpan(sw.ElapsedTicks) });
                                    File.Copy(r.FilePath, r.DestinationPath, true);
                                    File.Delete(r.FilePath);
                                    FileProcess(1);
                                }
                                catch { }
                                finally
                                {
                                    sw.Stop();
                                    sw = null;
                                    Interlocked.Increment(ref fileCount);
                                    WriteMessage(4, string.Empty, r.FilePath + "=>" + r.DestinationPath, null, r.RecordsInserted, r.RecordsUpdated, r.RecordsCorrupt);
                                    r = null;
                                }
                            });
                            WriteAverage(String.Format("Avg Time:{0} sec(s) / {1} file(s)", new TimeSpan((long)myCollection.Average(x => x.TheDuration.Ticks)).TotalSeconds, files.Count()));
                        }
                        myCollection = null;
                    }
                    files = null;
                    #endregion
                }
                catch (Exception exc)
                {

                    LogRecord(Level.Error, exc.Message);
                }
                finally
                {
                    timerLoad.Enabled = toggleStartProcess.Checked;
                    flag = false;
                    Clear();
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// 
        private ResultValue ReadFileData(string filePath,bool isSync)
        {
            string connbit = string.Empty;
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            string destFileName = string.Empty;
            string theSQL = string.Empty;
            int rCount = 0;
            int uCount = 0;
            int cCount = 0;
            try
            {

                baseDirectory += this.max;
                List<string> expected = (from line in File.ReadAllLines(filePath)
                                         let data = line.Split(',')
                                         orderby data[0] ascending
                                         select data[0]).Distinct().ToList();
                foreach (AppConfig input in (from row in grdTemplateList.Rows.Cast<System.Windows.Forms.DataGridViewRow>().Where(x => x.Cells[0].Value.ToString().Equals("True"))
                                             join e in expected on row.Cells[1].Value.ToString() equals e
                                             select new AppConfig()
                                             {
                                                 RecId = row.Cells[1].Value.ToString(),
                                                 Id = row.Cells[2].Value.ToString(),
                                                 Config = row.Cells[3].Value.ToString(),
                                                 TargetTableName = row.Cells[4].Value.ToString(),
                                                 ConnString = row.Cells[5].Value.ToString(),
                                                 DbProvider = HelperUtility.ConvertTo<int>(row.Cells[6].Value, 1),
                                             }))
                {
                    if (isSync)
                        AsyncSaveFileData(input, filePath, baseDirectory, ref  rCount, ref  uCount, ref  cCount);
                    else
                        SaveFileData(input, filePath, baseDirectory, ref  rCount, ref  uCount, ref  cCount);
                }
                expected = null;
            }
            catch (Exception exc)
            {
                WriteMessage(5, string.Empty, String.Empty, exc);
            }
            finally
            {
                if (rCount == 0 && uCount == 0)
                    destFileName = Path.Combine(baseDirectory, "Ignore", Path.GetFileName(filePath));
                else if (rCount > 0 || uCount > 0)
                    destFileName = Path.Combine(baseDirectory, "Process", Path.GetFileNameWithoutExtension(filePath) + DateTime.UtcNow.ToString("MMddyyyy") + Path.GetExtension(filePath));
                else
                    destFileName = Path.Combine(baseDirectory, "Pending", Path.GetFileName(filePath));
                connbit = string.Empty;
                baseDirectory = string.Empty;
                theSQL = string.Empty;
            }
            return new ResultValue() { RecordsCorrupt = cCount, RecordsInserted = rCount, RecordsUpdated = uCount, Directory = Path.GetDirectoryName(filePath), FilePath = filePath, DestinationPath = destFileName };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filePath"></param>
        /// <param name="baseDirectory"></param>
        /// <param name="rCount"></param>
        /// <param name="uCount"></param>
        /// <param name="cCount"></param>
        private void SaveFileData(AppConfig input, string filePath, string baseDirectory, ref int rCount, ref int uCount, ref int cCount)
        {
            try
            {
                Mapping mapper = Utility.ConvertJsonToObject<Mapping>(input.Config);
                string connbit = string.Empty;
                int dbProvider = 1;
                if (serverInfo != null)
                {
                    connbit = serverInfo.ConnString;
                    dbProvider = serverInfo.DbProvider;
                }
                else
                {
                    connbit = input.ConnString;
                    dbProvider = input.DbProvider;
                }
                #region Command
                using (IDbConnection tempConnection = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(dbProvider))
                {
                    tempConnection.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(connbit, true);
                    tempConnection.Open();
                    using (var cmd = tempConnection.CreateCommand())
                    {

                        foreach (var reader in (from line in File.ReadAllLines(filePath)
                                                let data = line.Split(',')
                                                where data[0] == input.RecId
                                                orderby data[0] ascending
                                                select data))
                        {
                            try
                            {
                                #region SQLStatement
                                StringBuilder updateSQL = new StringBuilder();
                                updateSQL.AppendLine("UPDATE " + input.TargetTableName + " SET ");
                                StringBuilder whereClause = new StringBuilder();
                                whereClause.AppendLine(" WHERE ");
                                #endregion
                                #region Parameters
                                List<object> values = new List<object>();
                                List<string> columns = new List<string>();
                                foreach (var c in mapper.Columns)
                                {
                                    try
                                    {
                                        object value = reader[Convert.ToInt32(c.ColumnName.Replace("F", String.Empty)) - 1];
                                        if (!string.IsNullOrEmpty(value.ToString()))
                                        {
                                            #region Switch
                                            switch (c.DataType.ToString().ToLower())
                                            {
                                                case "decimal":
                                                case "float":
                                                case "long":
                                                case "double":
                                                    columns.Add(c.FieldName);
                                                    values.Add(value);
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "=" + value + ",");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "=" + value + " AND ");
                                                    break;
                                                case "int":
                                                case "int16":
                                                case "int32":
                                                case "int64":
                                                    columns.Add(c.FieldName);
                                                    values.Add(Math.Round(Convert.ToDouble(value)));
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "=" + Math.Round(Convert.ToDouble(value)) + ",");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "=" + Math.Round(Convert.ToDouble(value)) + " AND ");
                                                    break;
                                                case "sByte":
                                                case "byte":
                                                    columns.Add(c.FieldName);
                                                    values.Add("'" + value + "'");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "='" + value + "',");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "='" + value + "' AND ");
                                                    break;
                                                case "datetime":
                                                    columns.Add(c.FieldName);
                                                    values.Add(HelperUtility.DbDateTime(Convert.ToDateTime(value), "{0:MM/dd/yyyy HH:mm:ss}", dbProvider));
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "=" + HelperUtility.DbDateTime(Convert.ToDateTime(value), "{0:MM/dd/yyyy HH:mm:ss}", dbProvider) + ",");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "=" + HelperUtility.DbDateTime(Convert.ToDateTime(value), "{0:MM/dd/yyyy HH:mm:ss}", dbProvider) + " AND ");
                                                    break;
                                                case "single":
                                                case "string":
                                                default:
                                                    columns.Add(c.FieldName);
                                                    values.Add("'" + HelperUtility.Truncate(value.ToString(), c.iLength).Replace("'", "''") + "'");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "='" + HelperUtility.Truncate(value.ToString(), c.iLength).Replace("'", "''") + "',");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "='" + HelperUtility.Truncate(value.ToString(), c.iLength).Replace("'", "''") + "' AND ");
                                                    break;
                                            }
                                            #endregion
                                        }
                                    }
                                    catch
                                    {


                                    }
                                }
                                #region Extra Elements
                                string UpdateDateTimeColumnName = ConfigurationManager.AppSettings.Get("UpdateDateTimeColumnName").ToUpper();
                                if (string.IsNullOrEmpty(UpdateDateTimeColumnName) == false && UpdateDateTimeColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == UpdateDateTimeColumnName).Count() == 0)
                                    {
                                        columns.Add(UpdateDateTimeColumnName);
                                        values.Add(GetDate(dbProvider));
                                    }
                                }

                                string PDateColumnName = ConfigurationManager.AppSettings.Get("PDateColumnName").ToUpper();
                                if (string.IsNullOrEmpty(PDateColumnName) == false && PDateColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == PDateColumnName).Count() == 0)
                                    {
                                        columns.Add(PDateColumnName);
                                        values.Add(GetDateFormatted("MM/dd/yyyy", dbProvider));
                                    }
                                }
                                
                                string CreateDateTimeColumnName = ConfigurationManager.AppSettings.Get("CreateDateTimeColumnName").ToUpper();
                                if (string.IsNullOrEmpty(CreateDateTimeColumnName) == false && CreateDateTimeColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == CreateDateTimeColumnName).Count() == 0)
                                    {
                                        columns.Add(CreateDateTimeColumnName);
                                        values.Add(GetDate(dbProvider));
                                    }
                                }

                                string UpdateColumnName = ConfigurationManager.AppSettings.Get("UpdateDateColumnName").ToUpper();
                                if (string.IsNullOrEmpty(UpdateColumnName) == false && UpdateColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == UpdateColumnName).Count() == 0)
                                    {
                                        columns.Add(UpdateColumnName);
                                        values.Add(GetDateFormatted("MM/dd/yyyy", dbProvider));
                                    }
                                }
                                
                                string PTNKeyColumnName = ConfigurationManager.AppSettings.Get("PTNKeyColumnName").ToUpper();
                                if (string.IsNullOrEmpty(PTNKeyColumnName) == false && PTNKeyColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == PTNKeyColumnName).Count() == 0)
                                    {
                                        columns.Add(PTNKeyColumnName);
                                        values.Add(DateTime.Today.Day);
                                    }
                                }

                                updateSQL.AppendLine(UpdateDateTimeColumnName + "=" + GetDate(dbProvider) + ",");
                                // updateSQL.AppendLine("ORA_UPDATE_DATE=" + HelperUtility.GetDateFormatted("MM/dd/yyyy", dbProvider) + ",");
                                #endregion
                                #endregion
                                bool isUpdate = false;
                                #region Insert
                                StringBuilder theSQL = new StringBuilder();
                                try
                                {
                                    theSQL.AppendLine("INSERT INTO " + input.TargetTableName + "(");
                                    theSQL.AppendLine(string.Join(",", columns.ToArray()));
                                    theSQL.AppendLine(")");
                                    theSQL.AppendLine(" VALUES (");
                                    theSQL.AppendLine(string.Join(",", values.ToArray()));
                                    theSQL.AppendLine(")");

                                    cmd.CommandText = theSQL.ToString();
                                    cmd.CommandType = CommandType.Text;
                                    WD.DataAccess.Logger.ILogger.Info(cmd.CommandText);
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        //WD.DataAccess.Logger.ILogger.Info(cmd.CommandText);
                                         rCount++;
                                       noOfRecords++;
                                        WriteMessage(1, input.RecId, filePath, null);
                                    }

                                }
                                catch (Exception exc)
                                {
                                    WriteMessage(3, input.RecId, filePath, exc);
                                    isUpdate = true;
                                }
                                #endregion
                                #region Update
                                if (isUpdate)
                                {
                                    try
                                    {
                                        updateSQL.Remove(updateSQL.Length - 3, 3);
                                        whereClause.Remove(whereClause.Length - 7, 7);
                                        updateSQL.AppendLine(whereClause.ToString());

                                        cmd.CommandText = updateSQL.ToString();
                                        cmd.CommandType = CommandType.Text;
                                        WD.DataAccess.Logger.ILogger.Info(cmd.CommandText);
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {
                                            
                                            uCount++;
                                            noOfUpdates++;
                                            WriteMessage(2, input.RecId, filePath, null);
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        using (FileStream stream = File.Open(baseDirectory + @"\Bad\" + input.RecId + Path.GetFileName(filePath), FileMode.Append, FileAccess.Write, FileShare.Write))
                                        {
                                            StreamWriter outfile = new StreamWriter(stream);
                                            {
                                                outfile.WriteLine(String.Join(",", reader));
                                                outfile.Flush();
                                            }
                                        }
                                        cCount++;
                                        noOfCorruptRecords++;
                                        WriteMessage(3, input.RecId, filePath, exc);
                                    }
                                }
                                #endregion
                                theSQL = null;
                                updateSQL = null;
                                whereClause = null;
                                values = null;
                                columns = null;
                            }
                            catch (Exception exc)
                            {
                                WriteMessage(3, input.RecId, filePath, exc);
                                using (FileStream stream = File.Open(baseDirectory + @"\Bad\" + input.RecId + Path.GetFileName(filePath), FileMode.Append, FileAccess.Write, FileShare.Write))
                                {
                                    StreamWriter outfile = new StreamWriter(stream);
                                    {
                                        outfile.WriteLine(String.Join(",", reader));
                                        outfile.Flush();
                                    }
                                }
                            }
                        }//end of foreach loop
                    }
                }
                #endregion
                mapper = null;
                connbit = string.Empty;
            }
            catch (Exception exc)
            {
                rCount = -1;
                uCount = -1;
                WriteMessage(3, input.RecId, filePath, exc);
            }
            finally
            {
                input = null;
                Clear();
            }
        }
        private void AsyncSaveFileData(AppConfig input, string filePath, string baseDirectory, ref int rCount, ref int uCount, ref int cCount)
        {
            try
            {
                Mapping mapper = Utility.ConvertJsonToObject<Mapping>(input.Config);
                string connbit = string.Empty;
                int dbProvider = 1;
                if (serverInfo != null)
                {
                    Interlocked.Exchange(ref connbit, serverInfo.ConnString);
                    Interlocked.Exchange(ref dbProvider, serverInfo.DbProvider);
                }
                else
                {
                    Interlocked.Exchange(ref connbit, input.ConnString);
                    Interlocked.Exchange(ref dbProvider, input.DbProvider);
                }
                #region Command
                using (IDbConnection tempConnection = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(dbProvider))
                {
                    tempConnection.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(connbit, true);
                    tempConnection.Open();
                    using (var cmd = tempConnection.CreateCommand())
                    {

                        foreach (var reader in (from line in File.ReadAllLines(filePath)
                                                let data = line.Split(',')
                                                where data[0] == input.RecId
                                                orderby data[0] ascending
                                                select data))
                        {
                            try
                            {
                                #region SQLStatement
                                StringBuilder updateSQL = new StringBuilder();
                                updateSQL.AppendLine("UPDATE " + input.TargetTableName + " SET ");
                                StringBuilder whereClause = new StringBuilder();
                                whereClause.AppendLine(" WHERE ");
                                #endregion
                                #region Parameters
                                List<object> values = new List<object>();
                                List<string> columns = new List<string>();
                                foreach (var c in mapper.Columns)
                                {
                                    try
                                    {
                                        object value = reader[Convert.ToInt32(c.ColumnName.Replace("F", String.Empty)) - 1];
                                        if (!string.IsNullOrEmpty(value.ToString()))
                                        {
                                            #region Switch
                                            switch (c.DataType.ToString().ToLower())
                                            {
                                                case "decimal":
                                                case "float":
                                                case "long":
                                                case "double":
                                                    columns.Add(c.FieldName);
                                                    values.Add(value);
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "=" + value + ",");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "=" + value + " AND ");
                                                    break;
                                                case "int":
                                                case "int16":
                                                case "int32":
                                                case "int64":
                                                    columns.Add(c.FieldName);
                                                    values.Add(Math.Round(Convert.ToDouble(value)));
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "=" + Math.Round(Convert.ToDouble(value)) + ",");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "=" + Math.Round(Convert.ToDouble(value)) + " AND ");
                                                    break;
                                                case "sByte":
                                                case "byte":
                                                    columns.Add(c.FieldName);
                                                    values.Add("'" + value + "'");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "='" + value + "',");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "='" + value + "' AND ");
                                                    break;
                                                case "datetime":
                                                    columns.Add(c.FieldName);
                                                    values.Add(HelperUtility.DbDateTime(Convert.ToDateTime(value), "{0:MM/dd/yyyy HH:mm:ss}", dbProvider));
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "=" + HelperUtility.DbDateTime(Convert.ToDateTime(value), "{0:MM/dd/yyyy HH:mm:ss}", dbProvider) + ",");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "=" + HelperUtility.DbDateTime(Convert.ToDateTime(value), "{0:MM/dd/yyyy HH:mm:ss}", dbProvider) + " AND ");
                                                    break;
                                                case "single":
                                                case "string":
                                                default:
                                                    columns.Add(c.FieldName);
                                                    values.Add("'" + HelperUtility.Truncate(value.ToString(), c.iLength).Replace("'", "''") + "'");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == false) != null)
                                                        updateSQL.AppendLine(c.FieldName + "='" + HelperUtility.Truncate(value.ToString(), c.iLength).Replace("'", "''") + "',");
                                                    if (mapper.Columns.Find(x => x.FieldName == c.FieldName && x.IsPrimary == true) != null)
                                                        whereClause.AppendLine(c.FieldName + "='" + HelperUtility.Truncate(value.ToString(), c.iLength).Replace("'", "''") + "' AND ");
                                                    break;
                                            }
                                            #endregion
                                        }
                                    }
                                    catch
                                    {


                                    }
                                }
                                #region Extra Elements

                                ////string UpdateColumnName = ConfigurationManager.AppSettings.Get("UpdateDateColumnName").ToUpper();
                                ////if (!string.IsNullOrEmpty(UpdateColumnName) && columns.FindAll(x => x.ToUpper() == UpdateColumnName).Count() == 0)
                                ////{
                                ////    columns.Add(UpdateColumnName);
                                ////    values.Add(GetDate(dbProvider));
                                ////}
                                //////if (columns.FindAll(x => x.ToUpper() == "P_DATE").Count() == 0)
                                //////{
                                //////    columns.Add("P_DATE");
                                //////    values.Add(HelperUtility.GetDateFormatted("MM/dd/yyyy", dbProvider));
                                //////}
                                ////string CreateColumnName = ConfigurationManager.AppSettings.Get("CreateDateColumnName").ToUpper();
                                ////if (!string.IsNullOrEmpty(CreateColumnName) && columns.FindAll(x => x.ToUpper() == CreateColumnName).Count() == 0)
                                ////{
                                ////    columns.Add(CreateColumnName);
                                ////    values.Add(GetDate(dbProvider));
                                ////}
                                //////if (columns.FindAll(x => x.ToUpper() == "ORA_UPDATE_DATE").Count() == 0)
                                //////{
                                //////    columns.Add("ORA_UPDATE_DATE");
                                //////    values.Add(HelperUtility.GetDateFormatted("MM/dd/yyyy", dbProvider));
                                //////}
                                ////////string PTNKeyColumnName = ConfigurationManager.AppSettings.Get("PTNKeyColumnName").ToUpper();
                                ////////if (!string.IsNullOrEmpty(PTNKeyColumnName) && columns.FindAll(x => x.ToUpper() == PTNKeyColumnName).Count() == 0)
                                ////////{
                                ////////    columns.Add(PTNKeyColumnName);
                                ////////    values.Add(DateTime.Today.Day);
                                ////////}

                                string UpdateDateTimeColumnName = ConfigurationManager.AppSettings.Get("UpdateDateTimeColumnName").ToUpper();
                                if (string.IsNullOrEmpty(UpdateDateTimeColumnName) == false && UpdateDateTimeColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == UpdateDateTimeColumnName).Count() == 0)
                                    {
                                        columns.Add(UpdateDateTimeColumnName);
                                        values.Add(GetDate(dbProvider));
                                    }
                                }

                                string PDateColumnName = ConfigurationManager.AppSettings.Get("PDateColumnName").ToUpper();
                                if (string.IsNullOrEmpty(PDateColumnName) == false && PDateColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == PDateColumnName).Count() == 0)
                                    {
                                        columns.Add(PDateColumnName);
                                        values.Add(GetDateFormatted("MM/dd/yyyy", dbProvider));
                                    }
                                }

                                string CreateDateTimeColumnName = ConfigurationManager.AppSettings.Get("CreateDateTimeColumnName").ToUpper();
                                if (string.IsNullOrEmpty(CreateDateTimeColumnName) == false && CreateDateTimeColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == CreateDateTimeColumnName).Count() == 0)
                                    {
                                        columns.Add(CreateDateTimeColumnName);
                                        values.Add(GetDate(dbProvider));
                                    }
                                }

                                string UpdateColumnName = ConfigurationManager.AppSettings.Get("UpdateDateColumnName").ToUpper();
                                if (string.IsNullOrEmpty(UpdateColumnName) == false && UpdateColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == UpdateColumnName).Count() == 0)
                                    {
                                        columns.Add(UpdateColumnName);
                                        values.Add(GetDateFormatted("MM/dd/yyyy", dbProvider));
                                    }
                                }

                                string PTNKeyColumnName = ConfigurationManager.AppSettings.Get("PTNKeyColumnName").ToUpper();
                                if (string.IsNullOrEmpty(PTNKeyColumnName) == false && PTNKeyColumnName.Length > 0)
                                {
                                    if (columns.FindAll(x => x.ToUpper() == PTNKeyColumnName).Count() == 0)
                                    {
                                        columns.Add(PTNKeyColumnName);
                                        values.Add(DateTime.Today.Day);
                                    }
                                }

                                updateSQL.AppendLine(UpdateDateTimeColumnName + "=" + GetDate(dbProvider) + ",");
                                // updateSQL.AppendLine("ORA_UPDATE_DATE=" + HelperUtility.GetDateFormatted("MM/dd/yyyy", dbProvider) + ",");

                                //================================================
                               // if (columns.FindAll(x => x.ToUpper() == "DB_UPDATE_DATE_TIME").Count() == 0)
                               // {
                               //     columns.Add("DB_UPDATE_DATE_TIME");
                               //     values.Add(HelperUtility.GetDate(dbProvider));
                               // }
                               // //if (columns.FindAll(x => x.ToUpper() == "P_DATE").Count() == 0)
                               // //{
                               // //    columns.Add("P_DATE");
                               // //    values.Add(HelperUtility.GetDateFormatted("MM/dd/yyyy", dbProvider));
                               // //}

                               // if (columns.FindAll(x => x.ToUpper() == "DB_CREATE_DATE_TIME").Count() == 0)
                               // {
                               //     columns.Add("DB_CREATE_DATE_TIME");
                               //     values.Add(HelperUtility.GetDate(dbProvider));
                               // }
                               // //if (columns.FindAll(x => x.ToUpper() == "ORA_UPDATE_DATE").Count() == 0)
                               // //{
                               // //    columns.Add("ORA_UPDATE_DATE");
                               // //    values.Add(HelperUtility.GetDateFormatted("MM/dd/yyyy", dbProvider));
                               // //}

                               // if (columns.FindAll(x => x.ToUpper() == "DB_PTN_KEY").Count() == 0)
                               // {
                               //     columns.Add("DB_PTN_KEY");
                               //     values.Add(DateTime.Today.Day);
                               // }

                               // updateSQL.AppendLine("DB_UPDATE_DATE_TIME=" + HelperUtility.GetDate(dbProvider) + ",");
                               //// updateSQL.AppendLine("ORA_UPDATE_DATE=" + HelperUtility.GetDateFormatted("MM/dd/yyyy", dbProvider) + ",");
                                #endregion
                                #endregion
                                bool isUpdate = false;
                                #region Insert
                                StringBuilder theSQL = new StringBuilder();
                                try
                                {
                                    
                                    theSQL.AppendLine("INSERT INTO " + input.TargetTableName + "(");
                                    theSQL.AppendLine(string.Join(",", columns.ToArray()));
                                    theSQL.AppendLine(")");
                                    theSQL.AppendLine(" VALUES (");
                                    theSQL.AppendLine(string.Join(",", values.ToArray()));
                                    theSQL.AppendLine(")");
                                    cmd.CommandText = theSQL.ToString();
                                    cmd.CommandType = CommandType.Text;
                                    WD.DataAccess.Logger.ILogger.Info(cmd.CommandText);
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        
                                        Interlocked.Increment(ref rCount);
                                        Interlocked.Increment(ref noOfRecords);
                                        WriteMessage(1, input.RecId, filePath, null);
                                    }

                                }
                                catch (Exception exc)
                                {
                                    WriteMessage(3, input.RecId, filePath, exc);

                                    isUpdate = true;
                                }
                                #endregion
                                #region Update
                                if (isUpdate)
                                {
                                    try
                                    {
                                        updateSQL.Remove(updateSQL.Length - 3, 3);
                                        whereClause.Remove(whereClause.Length - 7, 7);
                                        updateSQL.AppendLine(whereClause.ToString());

                                        cmd.CommandText = updateSQL.ToString();
                                        cmd.CommandType = CommandType.Text;
                                        WD.DataAccess.Logger.ILogger.Info(cmd.CommandText);
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {
                                            
                                            Interlocked.Increment(ref uCount);
                                            Interlocked.Increment(ref noOfUpdates);
                                            WriteMessage(2, input.RecId, filePath, null);
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        using (FileStream stream = File.Open(baseDirectory + @"\Bad\" + input.RecId + Path.GetFileName(filePath), FileMode.Append, FileAccess.Write, FileShare.Write))
                                        {
                                            StreamWriter outfile = new StreamWriter(stream);
                                            {
                                                outfile.WriteLine(String.Join(",", reader));
                                                outfile.Flush();
                                            }
                                        }
                                        Interlocked.Increment(ref cCount);
                                        Interlocked.Increment(ref noOfCorruptRecords);
                                        WriteMessage(3, input.RecId, filePath, exc);
                                    }
                                }
                                #endregion
                                theSQL = null;
                                updateSQL = null;
                                whereClause = null;
                                values = null;
                                columns = null;
                            }
                            catch (Exception exc)
                            {
                                WriteMessage(3, input.RecId, filePath, exc);
                                using (FileStream stream = File.Open(baseDirectory + @"\Bad\" + input.RecId + Path.GetFileName(filePath), FileMode.Append, FileAccess.Write, FileShare.Write))
                                {
                                    StreamWriter outfile = new StreamWriter(stream);
                                    {
                                        outfile.WriteLine(String.Join(",", reader));
                                        outfile.Flush();
                                    }
                                }
                            }
                        }//end of foreach loop
                    }
                }
                #endregion
                mapper = null;
                connbit = string.Empty;
            }
            catch (Exception exc)
            {
                Interlocked.Exchange(ref rCount,-1);
                Interlocked.Exchange(ref uCount,-1);
                WriteMessage(3, input.RecId, filePath, exc);
            }
            finally
            {
                input = null;
                Clear();
            }
        }
        /// <summary>   File process. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="afileCount">   Number of afiles. </param>
        private  void FileProcess(int afileCount)
        {
          SaveFileCount(afileCount);
        }
        #region WriteLogs
        /// <summary>   Writes a bad data to a file. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="row">              The row. </param>
        /// <param name="baseDirectory">    Pathname of the base directory. </param>
        /// <param name="recId">            Identifier for the record. </param>
        /// <param name="filePath">         Full pathname of the file. </param>
        private void WriteBad(string row, string baseDirectory, string recId, string filePath)
        {
            using (FileStream stream = File.Open(Path.Combine(baseDirectory, "Bad", recId + "_" + Path.GetFileNameWithoutExtension(filePath) + Path.GetExtension(filePath)), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            {
                StreamWriter outfile = new StreamWriter(stream);
                {
                    outfile.WriteLine(row);
                    outfile.Flush();
                }
            }
        }
        /// <summary>   Writes a message. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="op">           The operation. </param>
        /// <param name="recId">        (Optional) Identifier for the record. </param>
        /// <param name="filePath">     (Optional) Full pathname of the file. </param>
        /// <param name="exc">          (Optional) The exc. </param>
        /// <param name="fileCount">    (Optional) Number of files. </param>
        /// <param name="rCount">       (Optional) Number of. </param>
        /// <param name="uCount">       (Optional) Number of. </param>
        /// <param name="cCount">       (Optional) Number of. </param>
        /// 
        private void WriteMessage(int op, string recId = "", string filePath = "", Exception exc = null, int rCount = 0, int uCount = 0, int cCount = 0)
        {
            try
            {
                switch (op)
                {
                    case 1:
                        LogRecord(Level.Info, String.Format("{0}:{1} {2} \n", recId, Path.GetFileName(filePath), "Record inserted"));
                        break;
                    case 2:
                        LogRecord(Level.Info, String.Format("{0}:{1} {2} \n", recId, Path.GetFileName(filePath), "Record updated"));
                        break;
                    case 3:
                        LogRecord(Level.Error, recId + ": " + Path.GetFileName(filePath) + ":" + exc.Message);
                        break;
                    case 4:
                        LogRecord(Level.Info, string.Format("{0} =>File Processed => Records I:{1}; U:{2}; C:{3} \n",
                        filePath,
                        rCount,
                        uCount,
                        cCount));
                        break;
                    case 5:
                        LogRecord(Level.Error,exc.Message);
                        break;
                    default:
                        break;
                }
                WriteRecord();
            }
            catch { }
        }
        private void WriteRecord()
        {
            this.InvokeAndClose((MethodInvoker)delegate
            {

                lblInfo.Text = String.Format("Records I:{0}; U:{1}; C:{2}", noOfRecords, noOfUpdates, noOfCorruptRecords);
                lblTotalFiles.Text = String.Format("Processing Files: {0}/{1}", totalCount, fileCount);
                TimeSpan t = DateTime.Now - startTime;
                lblTimer.Text = String.Format("Start Time: {0}; End Time: {1}", startTime.ToString("MM/dd/yyyy hh:mm:ss tt"), DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
                lblEventLog.Text = String.Format("Time Elapsed: {0}:{1}:{2}:{3}", t.Days, t.Hours, t.Minutes, t.Seconds);
            });
        }
        private void WriteAverage(string text)
        {
            this.InvokeAndClose((MethodInvoker)delegate
            {
                lblAverageTime.Text = text;
            });
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileToOpen"></param>
        /// <param name="isFolder"></param>
        private void Open(WD.DataAccess.Logger.Appender fileToOpen, bool isFolder = false)
        {
            try
            {
                using (Process process = new Process())
                {
                   
                    process.StartInfo = new ProcessStartInfo()
                    {
                        FileName = isFolder ? Path.GetDirectoryName(fileToOpen.File.FullPath) : fileToOpen.File.FullPath,
                        Domain = Environment.UserDomainName,
                    };
                    process.Start();
                   // process.WaitForExit();
                    process.Close();
                }
            }
            catch(Exception exc){
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsRunning() { return toggleStartProcess.Checked; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbxLog_SelectedIndexChanged(object sender, System.EventArgs e)
        {
                try
                {
                    string log4netPath = ConfigurationManager.AppSettings.Get("log4net.Config");
                    if (!string.IsNullOrEmpty(log4netPath))
                    {
                        var fileToOpen = WD.DataAccess.Helpers.ObjectXMLSerializer<WD.DataAccess.Logger.Log4netConfiguration>.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, log4netPath)).Log4net.Appenders.Find(x => x.Name == this.cmbxLog.SelectedItem.ToString());
                        if (fileToOpen != null)
                        {
                            Open(fileToOpen, false);
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogRecord(Level.Error, exc.Message);
                }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem == btnBrowse)
            {
                try
                {
                    string log4netPath = ConfigurationManager.AppSettings.Get("log4net.Config");
                    if (!string.IsNullOrEmpty(log4netPath))
                    {
                        var fileToOpen = WD.DataAccess.Helpers.ObjectXMLSerializer<WD.DataAccess.Logger.Log4netConfiguration>.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, log4netPath)).Log4net.Appenders.FirstOrDefault();
                        if (fileToOpen != null)
                        {
                            Open(fileToOpen, true);
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogRecord(Level.Error, exc.Message);
                }
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="messageFormat"></param>
        /// <returns></returns>
        private string FormatALogEventMessage(LogEvent logEvent, string messageFormat)
        {
            string message = logEvent.Message ?? "<NULL>";
            return string.Format(messageFormat,
                /* {0} */ logEvent.EventTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                /* {1} */ logEvent.EventTime.ToString("yyyy-MM-dd HH:mm:ss"),
                /* {2} */ logEvent.EventTime.ToString("yyyy-MM-dd"),
                /* {3} */ logEvent.EventTime.ToString("HH:mm:ss.fff"),
                /* {4} */ logEvent.EventTime.ToString("HH:mm:ss"),

                                 /* {5} */ LevelName(logEvent.Level)[0],
                /* {6} */ LevelName(logEvent.Level),
                /* {7} */ (int)logEvent.Level,

                                 /* {8} */ message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private string LevelName(Level level)
        {
            switch (level)
            {
                case Level.Critical: return "Critical";
                case Level.Error: return "Error";
                case Level.Warning: return "Warning";
                case Level.Info: return "Info";
                case Level.Verbose: return "Verbose";
                case Level.Debug: return "Debug";
                default: return string.Format("<value={0}>", (int)level);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="text"></param>
        private void LogRecord(Level level, string text)
        {
            Color color;
            text = FormatALogEventMessage(new LogEvent(level, text), "{4} [{5}] : {8}");
            bool doWrite = true;
            switch (level)
            {
                case Level.Critical:
                    color = Color.Red;
                    WD.DataAccess.Logger.ILogger.Fatal(text);
                    doWrite = false;
                    break;
                case Level.Error:
                    WD.DataAccess.Logger.ILogger.Error(text);
                    color = Color.Red;
                    doWrite = false;
                    break;
                case Level.Warning:
                    color = Color.Goldenrod;
                    WD.DataAccess.Logger.ILogger.Warn(text);
                    break;
                case Level.Info:
                    color = Color.Green;
                    WD.DataAccess.Logger.ILogger.Info(text);
                    break;
                case Level.Verbose:
                    color = Color.Blue;
                    WD.DataAccess.Logger.ILogger.Info(text);
                    break;
                default:
                    color = Color.Black;
                    WD.DataAccess.Logger.ILogger.Debug(text);
                    break;
            }
            if (doWrite)
            {
                this.InvokeAndClose((MethodInvoker)delegate
                {
                    txtLog.ForeColor = color;
                    txtLog.Text += text;
                    txtLog.ForeColor = color;
                });
            }
            text = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Clear()
        {
            this.InvokeAndClose((MethodInvoker)delegate
            {
                txtLog.Text = string.Empty;
                txtLog.Clear();
            });
        }
        private void grdFolderList_MouseClick(object sender, MouseEventArgs e)
        {
            if (!toggleStartProcess.Checked)
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu m = new ContextMenu();
                    m.MenuItems.Add(new MenuItem("Add Folder", btn_addFolder));
                    m.MenuItems.Add(new MenuItem("Delete Folder", btn_deleteFolder));
                    m.Show(grdFolderList, new Point(e.X, e.Y));
                }
            }
        }
        private void btn_addFolder(object sender, EventArgs e)
        {
            WD.XLC.WIN.PopUps.FormFolder ff = new PopUps.FormFolder();
            ff.OnAddFolder += addFolder;
            ff.ShowDialog();
        }
        private bool addFolder(FolderStructure fs)
        {
            bool result = false;
            try
            {
                result = OnAddFolder(fs);
            }
            catch 
            {
                throw;
            }
            return result;
        }
        private void btn_deleteFolder(object sender, EventArgs e)
        {
            if (grdFolderList.SelectedCells[0].OwningRow.Cells[0].Value.ToString() != "0" && grdFolderList.SelectedCells[0].OwningRow.Cells[0].Value.ToString() != "-1")
            {
                OnDeleteFolder(grdFolderList.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            }
            else
            {
                MessageBox.Show("Cannot delete setting folders!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

      
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="func"></param>
        public static void InvokeAndClose(this Control self, MethodInvoker func)
        {
            try
            {
                IAsyncResult result = self.BeginInvoke(func);
                self.EndInvoke(result);
                var handle = result.AsyncWaitHandle;
                if (handle.SafeWaitHandle != null && !handle.SafeWaitHandle.IsInvalid && !handle.SafeWaitHandle.IsClosed)
                {
                    ((IDisposable)handle).Dispose();
                }
            }
            catch { }
           
        }
    }


    public class FileRecord {

        public FileRecord() {
            rCount = 0;
            cCount = 0;
            uCount = 0;
        }
        public int rCount { get; set; }
        public int uCount { get; set; }
        public int cCount { get; set; }
    }
}

