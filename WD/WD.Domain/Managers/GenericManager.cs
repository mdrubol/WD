using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using WD.DataAccess.Abstract;
using WD.DataAccess.Configurations;
using WD.DataAccess.Context;
using WD.DataAccess.Enums;
using WD.Domain.Abstract;
using WD.Domain.Helpers;
using Newtonsoft.Json;
using WD.DataAccess.Helpers;
using WD.DataAccess.Parameters;
using WD.DataAccess.Logger;

namespace WD.Domain.Managers
{
    public class GenericManager :DbContext, IGenericCommands
    {
        #region Constructor

        public GenericManager():base()
        { 
        }

        public GenericManager(int dbType)
            : base(dbType) {
       
        }
        public GenericManager(int dbType, string connectionName)
            : base(dbType, connectionName)
        {

        }
        public GenericManager(Connect aConnect)
            : base(aConnect)
        {

        }
        #endregion

        public void ASPConnection(string conString, int provider=DBProvider.Sql)
        {

            Constructor(conString, provider);
            //new GenericManager(new Connect() { dbProvider = provider, ConnectionString = conString });
        }

        #region Select Table
        /// <summary>
        /// This function will return data table
        /// </summary>
        /// <param name="query">Enter complete sql statement</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns>Datatable</returns>
        public DataTable Select_Generic_Table(string query, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    DataTable dt = ICommands.ExecuteDataTable(query, CommandType.Text);
                    return dt;
                }
            }
            
            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
                throw ;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
                throw ;
                //return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// This function will return data table
        /// </summary>
        /// <param name="table">Enter table name to query data </param>
        /// <param name="where">Enter where clause e.g. CustomerID=1001 or Name=Peter</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns>Datatable</returns>
        public DataTable Select_Generic_Table(string table, string where, ref string errorNumber, ref string errorMessage)
        {
            //put yoiur code here
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataTable("SELECT " + "*" + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
                throw ;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
                throw ;
                //return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// This function will return data table
        /// </summary>
        /// <param name="table">Enter table name to query data </param>
        /// <param name="where">Enter where clause e.g. CustomerID=1001 or Name=Peter</param>
        /// <param name="columns">Enter comma separated list of column names to select e.g. FirstName, MiddleName, LastName</param>      
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns>DataTable</returns>
        public DataTable Select_Generic_Table(string table, string where, string columns, ref string errorNumber, ref string errorMessage)
        {
            //put yoiur code here
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataTable("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
                throw ;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
                throw ;
                //return null;
            }
            finally
            {

            }
        }
        /// <summary>
        /// This function will return data table
        /// </summary>
        /// <param name="table">Enter table name to query data</param>
        /// <param name="where">Enter where clause e.g. CustomerID=1001 or Name=Peter</param>
        /// <param name="columns">Enter comma separated list of column names to select e.g. FirstName, MiddleName, LastName</param>
        /// <param name="orderBy">Enter orderBy e.g. columnName ASC or columnName DESC</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns>DataTable</returns>
        public DataTable Select_Generic_Table(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataTable("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) + (string.IsNullOrEmpty(orderBy) ? "" : " ORDER BY " + orderBy), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
                throw ;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
                throw ;
                //return null;
            }
            finally
            {

            }

        }
        #endregion

        #region DataSet
        /// <summary>
        /// This function will return DataSet
        /// </summary>
        /// <param name="query">Enter complete sql statement</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns>DataSet</returns>
        public DataSet Select_Generic_DataSet(string query, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet(query, CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error( exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// This function will return DataSet
        /// </summary>
        /// <param name="table">Enter table name to query data </param>
        /// <param name="where">Enter where clause e.g. CustomerID=1001 or Name=Peter</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns>DataSet</returns>
        public DataSet Select_Generic_DataSet(string table, string where, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet("SELECT " + "*" + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error( exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error( exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public DataSet Select_Generic_DataSet(string table, string where, string columns, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public DataSet Select_Generic_DataSet(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) + (string.IsNullOrEmpty(orderBy) ? "" : " ORDER BY " + orderBy), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error( exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        #endregion

        #region List<T>

        public List<T> Select_Generic_Object<T>(string query, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.GetList<T>(query, CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        public List<T> Select_Generic_Object<T>(string table, string where, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.GetList<T>("SELECT " + "*" + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        public List<T> Select_Generic_Object<T>(string table, string where, string columns, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.GetList<T>("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        public List<T> Select_Generic_Object<T>(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.GetList<T>("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) + (string.IsNullOrEmpty(orderBy) ? "" : " ORDER BY " + orderBy), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        #endregion

        #region XML
        public string Select_Generic_XML(string query, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet(query, CommandType.Text).GetXml();
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error( exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error( exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public string Select_Generic_XML(string table, string where, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet("SELECT " + "*" + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text).GetXml();
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error( exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public string Select_Generic_XML(string table, string where, string columns, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text).GetXml();
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public string Select_Generic_XML(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataSet("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) + (string.IsNullOrEmpty(orderBy) ? "" : " ORDER BY " + orderBy), CommandType.Text).GetXml();
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error( exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        #endregion

        #region Serial Number
        // Serial Number
        public DataTable Select_Generic_TableBySN(string table, string SERIAL_NUMBER, string columns, string orderBy, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(SERIAL_NUMBER))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    return ICommands.ExecuteDataTable("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + " WHERE SERIAL_NUMBER = " + SERIAL_NUMBER + (string.IsNullOrEmpty(orderBy) ? "" : " ORDER BY " + orderBy), CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }

        }
        #endregion

        #region Jason
        //Jason
        public string Select_Generic_JSon(string query, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    DataSet ds = ICommands.ExecuteDataSet(query, CommandType.Text);
                    return JsonConvert.SerializeObject(ds);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public string Select_Generic_JSon(string table, string where, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    DataSet ds = ICommands.ExecuteDataSet("SELECT " + "*" + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                    return JsonConvert.SerializeObject(ds);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public string Select_Generic_JSon(string table, string where, string columns, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    DataSet ds = ICommands.ExecuteDataSet("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), CommandType.Text);
                    return JsonConvert.SerializeObject(ds);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        public string Select_Generic_JSon(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    DataSet ds = ICommands.ExecuteDataSet("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) + (string.IsNullOrEmpty(orderBy) ? "" : " ORDER BY " + orderBy), CommandType.Text);
                    return JsonConvert.SerializeObject(ds);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        #endregion

        public ADODB.Recordset Select_Generic_Recordset(string query, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    errorNumber = "-1";
                    errorMessage = "One or more mandatory parameters are missing.";
                    return null;
                }
                else
                {
                    errorNumber = "0";
                    errorMessage = "Successful";
                    //DataSet ds = ICommands.ExecuteDataSet(query, CommandType.Text);
                    return ICommands.ExecuteRecordSet(query, CommandType.Text);
                }
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
                throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
                throw;
                //return null;
            }
            finally
            {

            }
        }

        #region RecordSet

        #endregion

        #region SP
        public int Execute_SP(string spName, DBParameter[] parm, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                errorNumber = "0";
                errorMessage = "Successful";
                return ICommands.ExecuteNonQuery(spName, CommandType.StoredProcedure, parm);

            }
            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// This function will insert data
        /// </summary>k
        /// <param name="query">Enter complete sql statement</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        public int Insert_Generic_Table(string query, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                errorNumber = "0";
                errorMessage = "Successful";
                return ICommands.ExecuteNonQuery(query,CommandType.Text);
            }

            
            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// This function will insert data
        /// </summary>
        /// <param name="o">Enter object name</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        public int Insert_Generic_Object(object o, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                errorNumber = "0";
                errorMessage = "Successful";
                return ICommands.Insert<object>(o);
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }

        }
        #endregion

        #region Update

        /// <summary>
        /// This function will update data
        /// </summary>k
        /// <param name="query">Enter complete sql statement</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        public int Update_Generic_Table(string query, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                errorNumber = "0";
                errorMessage = "Successful";
                return ICommands.ExecuteNonQuery(query);
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        /// <summary>
        /// This function will update data
        /// </summary>
        /// <param name="o">Enter object name</param>
        /// <param name="where">Enter where clause e.g. CustomerID=1001 or Name=Peter</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        public int Update_Generic_Object(object o, string where, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                errorNumber = "0";
                errorMessage = "Successful";


                return ICommands.Update(string.Empty, o.GetType().Name, " WHERE " + where, o);
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        /// <summary>
        /// This function will update data
        /// </summary>
        /// <param name="o">Enter object name</param>
        /// <param name="SERIAL_NUMBER">Enter serial number</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns></returns>
        public int Update_Generic_ObjectBySN(object o, string SERIAL_NUMBER, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                errorNumber = "0";
                errorMessage = "Successful";

                string where = " WHERE SERIAL_NUMBER = " + SERIAL_NUMBER;

                return ICommands.Update(string.Empty, o.GetType().Name, where, o);
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        #endregion

        #region Delete

        /// <summary>
        /// This function will delete data
        /// </summary>
        /// <param name="query">Enter complete sql statement</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        public int Delete_Generic_Table(string query, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                errorNumber = "0";
                errorMessage = "Successful";
                return ICommands.ExecuteNonQuery(query);
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        /// <summary>
        /// This function will delete data
        /// </summary>
        /// <param name="o">Enter object name e.g. typeof(CUSTOMER)</param>
        /// <param name="where">Enter where clause e.g. CustomerID=1001 or Name=Peter</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        public int Delete_Generic_Object(Type o, string where, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                errorNumber = "0";
                errorMessage = "Successful";

                StringBuilder sql = new StringBuilder();

                sql.Append("DELETE FROM ").Append(o.Name);
                sql.Append(" WHERE " + where);

                return ICommands.ExecuteNonQuery(sql.ToString());
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        /// <summary>
        /// This function will delete data
        /// </summary>
        /// <param name="o">Enter object name e.g. typeof(CUSTOMER)</param>
        /// <param name="SERIAL_NUMBER">Enter serial number</param>
        /// <param name="errorNumber">Define errorNumber as string and pass it e.g. string errorNumber = string.Empty;</param>
        /// <param name="errorMessage">Define errorMessage as string and pass it e.g. string errorMessage = string.Empty;</param>
        /// <returns></returns>
        public int Delete_Generic_ObjectBySN(Type o, string SERIAL_NUMBER, ref string errorNumber, ref string errorMessage)
        {

            try
            {
                errorNumber = "0";
                errorMessage = "Successful";

                StringBuilder sql = new StringBuilder();

                sql.Append("DELETE FROM ").Append(o.Name);
                sql.Append(" WHERE SERIAL_NUMBER = " + SERIAL_NUMBER);

                return ICommands.ExecuteNonQuery(sql.ToString());
            }

            catch (System.Data.SqlClient.SqlException exp) // SQL Server
            {
                ILogger.Error(exp);
                errorNumber = exp.Number.ToString();
                errorMessage = exp.Message;
               throw;
                //return null;
            }

            catch (Exception exp) // DB2 and Oracle
            {
                ILogger.Error(exp);
                Helper.GetErrorNumberAndErrorMessage(exp, ref errorNumber, ref errorMessage);
               throw;
                //return null;
            }
            finally
            {

            }
        }
        #endregion


      
    }
}
