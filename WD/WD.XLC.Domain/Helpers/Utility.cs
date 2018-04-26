// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 04-27-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="Utility.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WD.XLC.Domain.Entities;



// namespace: WD.XLC.Domain.Helpers
//
// summary:	.




// namespace: WD.XLC.Domain.Helpers
//
// summary:	.


namespace WD.XLC.Domain.Helpers
{

    /// <summary>
    /// Class PagingInfo./
    /// </summary>
    internal class PagingInfo : IDisposable
    {

        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>


        public PagingInfo() { }


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="pageIndex">        . </param>
        /// <param name="pageSize">         . </param>
        /// <param name="totalItemCount">   . </param>


        public PagingInfo(int pageIndex, int pageSize, int totalItemCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
        }


        /// <summary>   Gets or sets the zero-based index of the page. </summary>
        ///
        /// <value> The page index. </value>


        public int PageIndex { get; set; }


        /// <summary>   Gets or sets the size of the page. </summary>
        ///
        /// <value> The size of the page. </value>


        public int PageSize { get; set; }


        /// <summary>   Gets or sets the number of total items. </summary>
        ///
        /// <value> The total number of item count. </value>


        public int TotalItemCount { get; set; }


        /// <summary>   Gets the number of total pages. </summary>
        ///
        /// <value> The total number of page count. </value>


        public int TotalPageCount
        {
            get
            {
                return (int)Math.Ceiling((double)TotalItemCount / (double)PageSize);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    /// <summary>   A result value. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>


    public class ResultValue
    {

        /// <summary>   Gets or sets the pathname of the directory. </summary>
        ///
        /// <value> The pathname of the directory. </value>


        public string Directory { get; set; }


        /// <summary>   Gets or sets the full pathname of the file. </summary>
        ///
        /// <value> The full pathname of the file. </value>


        public string FilePath { get; set; }


        /// <summary>   Gets or sets the full pathname of the destination file. </summary>
        ///
        /// <value> The full pathname of the destination file. </value>


        public string DestinationPath { get; set; }


        /// <summary>   Gets or sets the records inserted. </summary>
        ///
        /// <value> The records inserted. </value>


        public int RecordsInserted { get; set; }


        /// <summary>   Gets or sets the records updated. </summary>
        ///
        /// <value> The records updated. </value>


        public int RecordsUpdated { get; set; }


        /// <summary>   Gets or sets the records corrupt. </summary>
        ///
        /// <value> The records corrupt. </value>


        public int RecordsCorrupt { get; set; }

    }


    /// <summary>   (Serializable) information about the in active. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>


    [Serializable]
    [XmlRootAttribute("InActiveRecord", Namespace = "", IsNullable = true)]
    public class InActiveRecord
    {


        /// <summary>   Gets or sets the identifier of the record. </summary>
        ///
        /// <value> The identifier of the record. </value>


        public string RecId { get; set; }


        /// <summary>   Gets or sets the number of.  </summary>
        ///
        /// <value> The count. </value>


        public int Count { get; set; }
    }


    /// <summary>   A loading on time. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>


    public class LoadingOnTime
    {

        /// <summary>   The next run time. </summary>
        private DateTime _nextRunTime;


        /// <summary>   Executes the periodically operation. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="action">       The action. </param>
        /// <param name="startTime">    The start time. </param>
        /// <param name="interval">     The interval. </param>
        /// <param name="token">        The token. </param>
        ///
        /// <returns>   The asynchronous result. </returns>


        public async Task RunPeriodically(Action action, DateTime startTime, TimeSpan interval, CancellationToken token)
        {
            _nextRunTime = startTime;
            while (true)
            {
                TimeSpan delay = _nextRunTime - DateTime.Now;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, token);
                }
                action();
                _nextRunTime += interval;
            }


        }


        /// <summary>   Executes the periodically operation. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="action">   The action. </param>
        /// <param name="interval"> The interval. </param>
        /// <param name="token">    The token. </param>
        ///
        /// <returns>   The asynchronous result. </returns>


        public async Task RunPeriodically(Action action, TimeSpan interval, CancellationToken token)
        {
            while (true)
            {
                action();
                await Task.Delay(interval, token);
            }
        }


        /// <summary>   Round current to next five minutes. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <returns>   A DateTime. </returns>


        public DateTime RoundCurrentToNextFiveMinutes()
        {
            DateTime now = DateTime.Now,
                 result = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);

            return result.AddMinutes(((now.Minute / 1) + 1) * 1);
        }
    }
    public enum Level
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Verbose = 4,
        Debug = 5
    };
    public class TimeCalculation
    {
        public TimeSpan TheDuration { get; set; }
    }
    public class LogEvent
    {
        public LogEvent(Level level, string message)
        {
            EventTime = DateTime.Now;
            Level = level;
            Message = message;
        }

        public readonly DateTime EventTime;

        public readonly Level Level;
        public readonly string Message;
    }


    public class Utility
    {

        /// <summary>   Gets an instance. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="max">  The maximum. </param>
        ///
        /// <returns>   The instance. </returns>


        public static MyInstance GetInstance(int max)
        {
            return WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader").Instances.Find(x => x.Index == max);
        }


        /// <summary>   Saves an instance. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="input">    The input. </param>


        public static void SaveInstance(MyInstance input)
        {
            MyApplication my = WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            my.Instances.Find(x => x.Index == input.Index).IsActive = input.IsActive;
            my.Instances.Find(x => x.Index == input.Index).Index = input.Index;
            my.Instances.Find(x => x.Index == input.Index).Inbox = input.Inbox;
            my.Instances.Find(x => x.Index == input.Index).Folders = input.Folders;
            my.Instances.Find(x => x.Index == input.Index).Templates = input.Templates;
            WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Save(my, System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            my = null;
        }


        /// <summary>   Resets the configuration. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>


        public static void ResetConfig()
        {
            if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader"))
            {
                WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Save(new MyApplication() { Instances = new List<MyInstance>() }, System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            }

            MyApplication my = WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            if (my.Instances.Count > 0)
            {
                foreach (var m in my.Instances)
                {
                    m.IsActive = false;
                }
            }
            WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Save(my, System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            my = null;
        }


        /// <summary>   Dispose configuration. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="max">  The maximum. </param>


        public static void DisposeConfig(int max)
        {
            MyApplication my = WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            if (my.Instances.Count > 0)
            {
                my.Instances.Find(x => x.Index == max).IsActive = false;
            }
            WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Save(my, System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            my = null;
        }


        /// <summary>   Initiate configuration. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <returns>   An int. </returns>


        public static int InitiateConfig()
        {
            string noOfProcess = string.Empty;
            int max = 1;
            if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader"))
            {
                WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Save(new MyApplication(), System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            }
            MyApplication my = WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            if (my.Instances.Count == 0)
            {
                MyInstance i = new MyInstance { Index = max, IsActive = true, Inbox = System.Configuration.ConfigurationManager.AppSettings["Inbox"] };
                List<MyInstance> iList = new List<MyInstance>();
                iList.Add(i);
                my = new MyApplication()
                {
                    Instances = iList
                };
                iList = null;
                i = null;
            }
            else
            {
                var totalList = (from m in my.Instances
                                 select m.Index
                               ).ToList();
                List<int> currentList = (from m in my.Instances
                                         where m.IsActive == true
                                         select m.Index
                               ).ToList();
                var missingItems = totalList.Except(currentList);
                bool found = false;
                foreach (int index in missingItems)
                {
                    MyInstance instance = (from m in my.Instances where m.Index == index select m).FirstOrDefault();
                    if (instance != null)
                    {

                        my.Instances.Find(x => x.Index == index).IsActive = true;
                        max = index;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    max = my.Instances.Max(x => x.Index);
                    max = max + 1;
                    my.Instances.Add(new MyInstance() { Index = max, IsActive = true, Inbox = System.Configuration.ConfigurationManager.AppSettings["Inbox"] });
                }
                totalList = null;
                currentList = null;
                missingItems = null;
            }
            WD.DataAccess.Helpers.ObjectXMLSerializer<MyApplication>.Save(my, System.AppDomain.CurrentDomain.BaseDirectory + @"\xlcloader.xlcloader");
            my = null;
            CreateFolders(max);
            DeleteFiles(max);
            return max;
        }

        /// <summary>   Creates the folders. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="max">  The maximum. </param>


        private static void CreateFolders(int max)
        {
            string[] folders = { "Working", "Pending", "Bad", "Archive", "Process", "Ignore" };
            if (!Directory.Exists(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString())))
            {
                Directory.CreateDirectory(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString()));
            }
            Parallel.For(0, folders.Length, (index) =>
            {
                if (!Directory.Exists(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,max.ToString(), folders[index])))
                {
                    Directory.CreateDirectory(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), folders[index]));
                }
            });
            CopyTemp(max);
        }

        private static void CopyTemp(int max)
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Temp", max.ToString()));
            if (info.Exists)
            {
                foreach (var dir in info.GetDirectories())
                {
                    try
                    {
                        if (File.Exists(Path.Combine(dir.FullName, "schema.ini")))
                        {
                            File.Delete(Path.Combine(dir.FullName, "schema.ini"));
                        }
                        foreach (var file in info.GetFiles("*", SearchOption.AllDirectories).ToArray())
                        {
                            file.MoveTo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), "Working", Path.GetFileName(file.FullName)));
                        }
                    }
                    catch
                    {
                    }
                }
                Directory.Delete(info.FullName, true);
            }
        }


        /// <summary>   Starts a load. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="max">              The maximum. </param>
        /// <param name="folderLocation">   The folder location. </param>
        ///
        /// <returns>   An int. </returns>


        public static int StartLoad(int max, string folderLocation)
        {
            int count = 0;
            try
            {
                DirectoryInfo info = new DirectoryInfo(folderLocation);
                if (info.Exists)
                {
                    int fileCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FileCount"]);
                    int actualFileCount = Directory.GetFiles(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,max.ToString(), "Working")).Count();
                    int copyFiles = fileCount - actualFileCount;
                    copyFiles = fileCount > copyFiles ? copyFiles : fileCount;
                    foreach (FileInfo finfos in info.GetFiles("*").Skip((max - 1) * copyFiles).Take(copyFiles))
                    {
                        string fileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, max.ToString(), "Working", Path.GetFileName(finfos.FullName));
                        finfos.CopyTo(fileName, true);
                        File.Delete(finfos.FullName);
                        count++;
                        fileName = string.Empty;
                    }
                }
            }
            catch (Exception exc)
            {

                System.Windows.Forms.MessageBox.Show(exc.Message);

            }

            return count;
        }


        /// <summary>   Recursions. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="pageIndex">    Zero-based index of the page. </param>
        /// <param name="info">         [in,out] The information. </param>
        /// <param name="copyFiles">    [in,out] The copy files. </param>
        ///
        /// <returns>   A FileInfo[]. </returns>


        public static FileInfo[] Recursion(int pageIndex, ref DirectoryInfo info, ref int copyFiles)
        {

            FileInfo[] finfos = info.GetFiles("*", SearchOption.AllDirectories).Skip((pageIndex - 1) * copyFiles).Take(copyFiles).ToArray();
            if (finfos.Count() > 0)
            {
                return finfos;
            }
            else
            {
                pageIndex--;
                return Recursion(pageIndex, ref info, ref copyFiles);
            }
        }


        /// <summary>   Query if 'strSourcePath' is file locked. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="strSourcePath">    Full pathname of the source file. </param>
        ///
        /// <returns>   True if file locked, false if not. </returns>


        public static bool IsFileLocked(string strSourcePath)
        {
            try
            {
                using (Stream stream = new FileStream(strSourcePath, FileMode.Open))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>   Gets the t. </summary>
        ///
        /// <value> The t. </value>
        ///
        /// ### <param name="input">    The input. </param>


        public static string ConvertObjectToJson<T>(T input) where T : new()
        {
            return JsonConvert.SerializeObject(input, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            });
        }


        /// <summary>   Gets the t. </summary>
        ///
        /// <value> The t. </value>


        public static T ConvertJsonToObject<T>(string jsonString) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }


        /// <summary>   Converts this object to a pivot table. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <typeparam name="T">        Generic type parameter. </typeparam>
        /// <typeparam name="TColumn">  Type of the column. </typeparam>
        /// <typeparam name="TRow">     Type of the row. </typeparam>
        /// <typeparam name="TData">    Type of the data. </typeparam>
        /// <param name="source">           Source for the. </param>
        /// <param name="columnSelector">   The column selector. </param>
        /// <param name="rowSelector">      The row selector. </param>
        /// <param name="dataSelector">     The data selector. </param>
        ///
        /// <returns>   The given data converted to a DataTable. </returns>


        public static DataTable ToPivotTable<T, TColumn, TRow, TData>(IEnumerable<T> source, Func<T, TColumn> columnSelector, Expression<Func<T, TRow>> rowSelector, Func<IEnumerable<T>, TData> dataSelector)
        {
            DataTable table = new DataTable();
            var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            table.Columns.Add(new DataColumn(rowName));
            var columns = source.Select(columnSelector).Distinct();

            foreach (var column in columns)
                table.Columns.Add(new DataColumn(column.ToString()));

            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             });

            foreach (var row in rows)
            {
                var dataRow = table.NewRow();
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                dataRow.ItemArray = items.ToArray();
                table.Rows.Add(dataRow);
            }

            return table;
        }


        /// <summary>   Gets anonymous object. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="columns">  The columns. </param>
        /// <param name="values">   The values. </param>
        ///
        /// <returns>   The anonymous object. </returns>


        public static dynamic GetAnonymousObject(IEnumerable<string> columns, IEnumerable<object> values)
        {
            IDictionary<string, object> eo = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            int i;
            for (i = 0; i < columns.Count(); i++)
            {
                eo.Add(columns.ElementAt<string>(i), values.ElementAt<object>(i));
            }
            return eo;
        }


        /// <summary>   Gets database type. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="p">    A string to process. </param>
        ///
        /// <returns>   The database type. </returns>


        public static DbType GetDbType(string p)
        {
            DbType dbType = DbType.String;
            switch (p.ToLower())
            {
                case "datetime": dbType = DbType.DateTime; break;
                case "int": dbType = DbType.Int32; break;
                case "int32": dbType = DbType.Int32; break;
                case "int64": dbType = DbType.Int64; break;
                case "long": dbType = DbType.Int64; break;
                case "double":
                case "float":
                case "decimal": dbType = DbType.Double; break;
                case "byte": dbType = DbType.Byte; break;
                case "bool": dbType = DbType.Boolean; break;
                default: dbType = DbType.String; break;
            }
            return dbType;
        }

        private static void DeleteFiles(int max)
        {
            try {

                string batchFile = "batch_" + max.ToString() + ".bat";
                if (File.Exists(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile)))
                {
                    File.Delete(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile));
                }
                foreach (string folder in System.Configuration.ConfigurationManager.AppSettings.Get("DeleteFolders").Split(','))
                {
                    batchFile = "delete_" + folder + "_" + max.ToString() + ".bat";
                    if (File.Exists(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile)))
                    {
                        File.Delete(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, batchFile));
                    }
                }

            }
            catch
            { 
            
            
            }
        }
    }
}
