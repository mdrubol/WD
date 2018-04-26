using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using WD.DataAccess.Abstract;
using WD.DataAccess.Context;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using WD.DataAccess.Mtech;

namespace WD.DataAccess.Mtech
{
    /// <summary>
    /// 
    /// </summary>
    public  class DBHandler
    {
        private DBProvider dbType;
        private string ConnectionString = string.Empty;
        private List<WDDBConfig> BRConfig;
        private List<WDDBConfig> TXConfig;
        private Databases databases;
        public const string DummyID = "secaccess";
        public const string DummyPasswd = "ruworthy";
        protected ICommands ICommand;
        public DBHandler(string siteName)
        {
            BRConfig = new List<WDDBConfig>();
            TXConfig = new List<WDDBConfig>();
            this.LoadFromXML(siteName);
            this.GetActualConnection(Databases.BR);
            this.ICommand = new DbContext(new Connect() { DbType = dbType, ConnectionString = this.ConnectionString }).ICommands;
            this.QueryDatabaseStatus();
            this.GetActualConnection(Databases.BR);
            this.ICommand = new DbContext(new Connect() { DbType = dbType, ConnectionString = this.ConnectionString }).ICommands;
        }
        private void QueryDatabaseStatus()
        {
            try
            {
                string commandText = "SELECT TX1_FLAG, TX2_FLAG, BR1_FLAG, BR2_FLAG, TX1_SERVER, TX1_DBNAME, TX2_SERVER, TX2_DBNAME, BR1_SERVER, BR1_DBNAME, BR2_SERVER, BR2_DBNAME FROM LU_DATABASE_STATUS";
                 DataTable dataTable = ICommand.ExecuteDataTable(commandText);
                bool flag = dataTable.Rows.Count > 0;
                if (flag)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    if (this.BRConfig.Count > 0)
                    {
                        bool bx = HelperUtility.ConvertTo<bool>(
                            (((!this.BRConfig[0].ServerName.Equals((dataRow["BR1_SERVER"]))
                            | !this.BRConfig[0].DatabaseName.Equals((dataRow["BR1_DBNAME"]))
                            ) && (
                            this.BRConfig[0].ActiveFlag.Equals(dataRow["BR1_FLAG"]) == false
                            ))|
                            ((!this.BRConfig[1].ServerName.Equals((dataRow["BR2_SERVER"]))
                            | !this.BRConfig[1].DatabaseName.Equals((dataRow["BR2_DBNAME"]))
                            ) && (
                            this.BRConfig[1].ActiveFlag.Equals(dataRow["BR2_FLAG"]) == false
                            ))), false);
                       if (flag)
                        {
                            this.BRConfig.Clear();
                            this.BRConfig=new List<WDDBConfig>();
                            this.BRConfig.Add(new WDDBConfig(HelperUtility.ConvertTo<string>(dataRow["BR1_SERVER"], string.Empty),
                                                                HelperUtility.ConvertTo<string>(dataRow["BR1_DBNAME"], string.Empty),
                                                                dataRow["BR1_FLAG"] == "1" ? true : false,
                                                               DBProvider.Sql));
                            this.BRConfig.Add(new WDDBConfig(HelperUtility.ConvertTo<string>(dataRow["BR2_SERVER"],string.Empty),
                                                             HelperUtility.ConvertTo<string>(dataRow["BR2_DBNAME"],string.Empty),
                                                            dataRow["BR2_FLAG"] == "1" ? true : false,
                                                             DBProvider.Sql));
                        }
                    }
                     if (this.TXConfig.Count > 0)
                    {
                        bool tx = HelperUtility.ConvertTo<bool>(
                            (((!this.TXConfig[0].ServerName.Equals((dataRow["TX1_SERVER"]))
                            | !this.TXConfig[0].DatabaseName.Equals((dataRow["TX1_DBNAME"]))
                            ) && (
                            this.TXConfig[0].ActiveFlag.Equals(dataRow["TX1_FLAG"]) == false
                            ))|
                            ((!this.TXConfig[1].ServerName.Equals((dataRow["TX2_SERVER"]))
                            | !this.TXConfig[1].DatabaseName.Equals((dataRow["TX2_DBNAME"]))
                            ) && (
                            this.TXConfig[1].ActiveFlag.Equals(dataRow["TX2_FLAG"]) == false
                            ))), false);
                        if (tx)
                        {
                            this.TXConfig.Clear();
                            this.TXConfig = new List<WDDBConfig>();
                            this.TXConfig.Add(new WDDBConfig(HelperUtility.ConvertTo<string>(dataRow["TX1_SERVER"], string.Empty),
                                                                HelperUtility.ConvertTo<string>(dataRow["TX1_DBNAME"], string.Empty),
                                                               dataRow["TX1_FLAG"] == "1" ? true : false,
                                                               DBProvider.Sql));
                            this.TXConfig.Add(new WDDBConfig(HelperUtility.ConvertTo<string>(dataRow["TX2_SERVER"], string.Empty),
                                                             HelperUtility.ConvertTo<string>(dataRow["TX2_DBNAME"], string.Empty),
                                                            dataRow["TX2_FLAG"] == "1" ? true : false,
                                                             DBProvider.Sql));
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }
           
        }
        private void LoadFromXML(string OverloadDefaultDBSite)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            DataSet dataSet = this.OpenFileData();
            str = this.GetDbSite(OverloadDefaultDBSite, HelperUtility.ConvertTo<String>(dataSet.Tables["DBCONFIG"].Rows[0]["DEFAULT_DB_SITE"], string.Empty));
            this.InitializeDatabase();
            checked
            {
                try
                {
                    str2 = dataSet.Relations["DBCONFIG_INSTANCES"].ChildTable.Select("NAME='" + str + "'")[0]["INSTANCES_Id"].ToString();
                    DataRow[] array = dataSet.Relations["INSTANCES_DB"].ChildTable.Select("INSTANCES_Id='" + str2 + "'");
                    for (int i = 0; i < array.Length; i++)
                    {
                        DataRow dataRow = array[i];
                        GetDatabaseConfig(HelperUtility.ConvertTo<String>(dataRow["ID"], string.Empty)).Add(new WDDBConfig(
                            HelperUtility.ConvertTo<String>(dataRow["SERVER"], string.Empty), HelperUtility.ConvertTo<String>(dataRow["DBName"], string.Empty), true,
                           HelperUtility.GetDBProvider(HelperUtility.ConvertTo<String>(dataRow["DBType"], string.Empty))
                          ));
                    }
                }
                catch
                {

                    throw;
                }
               
            }
        }
        private string GetDbSite(string OverloadDefaultDBSite, string defaultSite)
        {
            bool flag = OverloadDefaultDBSite.Length > 0;
            string result;
            if (flag)
            {
                result = OverloadDefaultDBSite;
            }
            else
            {
                result = defaultSite;
            }
            return result;
        }
        private DataSet OpenFileData()
        {
            DataSet dataSet = new DataSet();
            string text = ConfigurationManager.AppSettings["ConnectionsFile"];
            bool flag = File.Exists(text);
            if (flag)
            {
                dataSet.ReadXml(text);
                return dataSet;
            }
            throw new ApplicationException("OpenFile : Could not locate this file ");
        }
        private List<WDDBConfig> GetDatabaseConfig(string DBType)
        {
            return String.Compare(DBType.ToString().Substring(0, 2), Databases.BR.ToString(), false) == 0 ? this.BRConfig : this.TXConfig;
        }
        private List<WDDBConfig> GetDatabaseConfig(Databases DBType)
        {
            return DBType == Databases.BR ? this.BRConfig : this.TXConfig;
        }
        private void InitializeDatabase()
        {
            this.BRConfig.Clear();
            this.TXConfig.Clear();
        }
        private void GetActualConnection(Databases Database)
        {
            List<WDDBConfig>.Enumerator enumerator;
            try
            {
                enumerator = GetDatabaseConfig(Database).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    WDDBConfig current = enumerator.Current;
                    bool flag = current.ActiveFlag;
                    if (flag)
                    {
                        Connections connections = new Connections(current.ServerName, current.DatabaseName, "secaccess", "ruworthy");
                        try
                        {
                            WD.DataAccess.Context.DbContext dbContext = new Context.DbContext(new Connect() { DbType = current.DBType, ConnectionString = connections.ConnectionString() });
                            DataTable dataTable = dbContext.ICommands.ExecuteDataTable("SELECT USER_ID, PASSWD FROM SECURITY_TBL");
                            flag = (dataTable.Rows.Count == 0);
                            if (flag)
                            {
                                current.Userid = "secaccess";
                                current.Password = "ruworthy";
                            }
                            else
                            {
                                current.Userid = HelperUtility.Decrypt(dataTable.Rows[0]["USER_ID"].ToString());
                                current.Password = HelperUtility.Decrypt(dataTable.Rows[0]["PASSWD"].ToString());
                            }
                            dbContext = null;
                            this.dbType = current.DBType;
                            this.ConnectionString = new Connections(current.ServerName, current.DatabaseName, current.Userid, current.Password).ConnectionString();
                        }
                        catch
                        {
                            //throw;
                        }
                       
                    }
                }
            }
            catch 
            {
                throw;
            }
           
        }

    }
}