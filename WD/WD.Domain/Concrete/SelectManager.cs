using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using WD.DataAccess.Abstract;
using WD.DataAccess.Configurations;
using WD.DataAccess.Context;
using WD.DataAccess.Enums;
using WD.Domain.Abstract;
using Newtonsoft.Json;
using WD.DataAccess.Helpers;
using WD.DataAccess.Parameters;

namespace WD.Domain.Concrete
{
   public class SelectManager:DbContext,ISelect
   {
        #region Constructor
        public SelectManager(int dbType)
            : base(dbType) {
       
        }
        public SelectManager(int dbType, string connectionName)
            : base(dbType, connectionName)
        {

        }
        public SelectManager(Connect aConnect)
            : base(aConnect)
        {

        }
        #endregion

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
                    return ICommands.ExecuteDataTable(query, CommandType.Text);
                }
            }

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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
                    return ICommands.ExecuteDataTable("SELECT " + "*" + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) , CommandType.Text);
                }
            }

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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
                    return ICommands.ExecuteDataTable("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) , CommandType.Text);
                }
            }

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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
            
            catch (Exception exp) 
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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
                    return ICommands.ExecuteDataSet("SELECT " + "*" + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) , CommandType.Text);
                }
            }

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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
                    return ICommands.ExecuteDataSet("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) , CommandType.Text);
                }
            }

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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
                    return ICommands.GetList<T>("SELECT " + (string.IsNullOrEmpty(columns) ? "*" : columns) + " FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where) , CommandType.Text);
                }
            }

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
            }
            finally
            {

            }
        }
        #endregion

        #region 
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
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

            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return null;
            }
            finally
            {

            }
        }

        #endregion

        #region SP
        public int Execute_SP(string spName, DBParameter[] aParams, ref string errorNumber, ref string errorMessage)
        {
            try
            {
                 errorNumber = "0";
                 errorMessage = "Successful";
                 return ICommands.ExecuteNonQuery(spName, CommandType.StoredProcedure,aParams);

            }
            catch (Exception exp)
            {
                errorNumber = "-1";
                errorMessage = exp.Message;
                return -1;
            }
        }
        #endregion
   }
}
