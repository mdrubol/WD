// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="HelperUtility.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using WD.DataAccess.Abstract;
using WD.DataAccess.Attributes;
using WD.DataAccess.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using WD.DataAccess.Parameters;
using WD.DataAccess.Enums;
using System.Collections;
using WD.DataAccess.Mitecs;
using System.IO;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   A helper utility. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public static class HelperUtility
    {
        
        /// <summary>   Truncates. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="value">        . </param>
        /// <param name="maxLength">    . </param>
        ///
        /// <returns>   A string. </returns>
        

        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)||maxLength==0) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        
        /// <summary>   Validates the given entity. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="entity">   . </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        

        public static bool Validate(object entity)
        {
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(entity, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(entity, context, results, true);
        }

        
        /// <summary>   Gets the version. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   The version. </returns>
        

        public static string GetVersion()
        {
            return Assembly.GetAssembly(typeof(HelperUtility)).GetName().Version.ToString();
        }

        
        /// <summary>   Prefixes. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   A string. </returns>
        

        public static string Prefix(int dbProvider)
        {
            string result = string.Empty;
            switch (dbProvider) { 
                case DBProvider.Oracle:
                case DBProvider.Oracle2:
                    result = ":";
                    break;
                default:
                    result = "@";
                    break;
            
            }
            return result;
        }

        
        /// <summary>   Convert to. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="value">    . </param>
        /// <param name="input">    . </param>
        ///
        /// <returns>   to converted. </returns>
        

        public static T ConvertTo<T>(object value, T input)
        {
            if (Convert.IsDBNull(value))
            {
                return input;
            }
            return (T)Convert.ChangeType(value, input.GetType());
        }

        
      

        
        
        /// <summary>
        /// The prefix pattern
        /// </summary>
        static string prefixPattern = "^(@|:)";
        
        /// <summary>   Adds the parameters. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbCommand">    . </param>
        /// <param name="aParams">      . </param>
        /// <param name="dbProvider">   . </param>
        
        internal static void AddParameters(IDbCommand dbCommand, DBParameter[] aParams, int dbProvider)
        {

            foreach (DBParameter db in aParams)
            {
                IDataParameter dbParam = (IDataParameter)dbCommand.CreateParameter();
                if (dbCommand.CommandType == CommandType.StoredProcedure)
                {
                    switch (dbProvider)
                    {
                        case 3:
                        case 4:
                            if (System.Text.RegularExpressions.Regex.IsMatch(db.ParameterName, prefixPattern))
                                dbParam.ParameterName = db.ParameterName.Replace("@", string.Empty).Replace(":", string.Empty);
                            else
                                dbParam.ParameterName = db.ParameterName;
                            break;
                        default:
                            if (System.Text.RegularExpressions.Regex.IsMatch(db.ParameterName, prefixPattern))
                                dbParam.ParameterName = db.ParameterName.Replace("@", Prefix(dbProvider)).Replace(":", Prefix(dbProvider));
                            else
                            dbParam.ParameterName = Prefix(dbProvider) + db.ParameterName;
                            break;
                    }
                }
                else
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(db.ParameterName, prefixPattern))
                        dbParam.ParameterName = db.ParameterName.Replace("@", Prefix(dbProvider)).Replace(":", Prefix(dbProvider));
                    else
                    dbParam.ParameterName = Prefix(dbProvider) + db.ParameterName;
                }
                dbParam.Value = db.ParameterValue;
                dbParam.Direction = db.ParamDirection;
                dbParam.DbType = db.Type;
                dbCommand.Parameters.Add(dbParam);
                dbParam = null;
            }
        }
        internal static void PrefixCommand(IDbCommand dbCommand, int dbProvider)
        {
            dbCommand.CommandText = dbCommand.CommandText; //.Replace("@", Prefix(dbProvider)).Replace(":", Prefix(dbProvider));
        }
        internal static void PrefixParameters(IDbCommand dbCommand, int dbProvider)
        {
            foreach (IDbDataParameter dbParam in dbCommand.Parameters)
            {
                if (dbCommand.CommandType == CommandType.StoredProcedure)
                {
                    switch (dbProvider)
                    {
                        case 3:
                        case 4:
                            if (System.Text.RegularExpressions.Regex.IsMatch(dbParam.ParameterName, prefixPattern))
                                dbParam.ParameterName = dbParam.ParameterName.Replace("@", string.Empty).Replace(":", string.Empty);
                            break;
                        default:
                            if (System.Text.RegularExpressions.Regex.IsMatch(dbParam.ParameterName, prefixPattern))
                                dbParam.ParameterName = dbParam.ParameterName.Replace("@", Prefix(dbProvider)).Replace(":", Prefix(dbProvider));
                            else
                                dbParam.ParameterName = Prefix(dbProvider) + dbParam.ParameterName;
                            break;
                    }
                }
                else
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(dbParam.ParameterName, prefixPattern))
                        dbParam.ParameterName = dbParam.ParameterName.Replace("@", Prefix(dbProvider)).Replace(":", Prefix(dbProvider));
                    else
                        dbParam.ParameterName = Prefix(dbProvider) + dbParam.ParameterName;
                    break;
                }
            }
        }
        /// <summary>   Adds the parameters to 'input'. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="dbCommand">    . </param>
        /// <param name="input">        . </param>
        

        internal static void AddParameters<T>(IDbCommand dbCommand, T input)
        {
            foreach (PropertyInfo propertyInfo in input.GetType().GetProperties().ToList<PropertyInfo>())
            {
                if (!String.IsNullOrEmpty(ColumnName(propertyInfo)))
                {
                    IDataParameter dbParam = dbCommand.CreateParameter();
                    dbParam.ParameterName = ColumnName(propertyInfo);
                    dbParam.Value = propertyInfo.GetValue(input, null);
                    dbCommand.Parameters.Add(dbParam);
                    dbParam = null;
                }
            }
        }

        
        /// <summary>   Inserts a command text. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="input">        . </param>
        /// <param name="dbCommand">    [in,out]. </param>
        /// <param name="dbProvider">   . </param>
        ///
        /// ### <returns>   . </returns>


        internal static IDbCommand InsertCommandText<T>(T input, IDbCommand dbCommand, int dbProvider)
        {
            List<String> columnList = new List<string>();
            List<String> valueList = new List<string>();
            foreach (PropertyInfo propertyInfo in input.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetValue(input, null) != null).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                if (!IsPrimary(propertyInfo))
                {
                    object value = propertyInfo.GetValue(input, null);
                    if (value != null)
                    {
                        IDataParameter dbParam = dbCommand.CreateParameter();
                        switch (propertyInfo.PropertyType.ToString())
                        {
                            case "System.DateTime":
                                if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                                {
                                    columnList.Add(ColumnName(propertyInfo));
                                    dbParam = dbCommand.CreateParameter();
                                    dbCommand.CreateParameter();
                                    dbParam.ParameterName = Prefix(dbProvider) + ColumnName(propertyInfo);
                                    dbParam.Value = dbProvider ==6 ?  ((DateTime)propertyInfo.GetValue(input, null)).ToString("G"):(propertyInfo.GetValue(input, null)) ;
                                    dbParam.DbType = DbType.DateTime;
                                    dbCommand.Parameters.Add(dbParam);
                                    valueList.Add(dbParam.ParameterName);
                                }
                                break;
                            default:
                                columnList.Add(ColumnName(propertyInfo));
                                dbParam = dbCommand.CreateParameter();
                                dbCommand.CreateParameter();
                                dbParam.ParameterName = Prefix(dbProvider) + ColumnName(propertyInfo);
                                dbParam.Value = propertyInfo.GetValue(input, null);
                                dbCommand.Parameters.Add(dbParam);
                                valueList.Add(dbParam.ParameterName);
                                break;
                        }
                    }
                }

            }
            dbCommand.CommandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                           GetTableName<T>(),
                            string.Join(",", columnList.ToArray()),
                            string.Join(",", valueList.ToArray())
                            );
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        
        /// <summary>   Updates the command text. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="input">        . </param>
        /// <param name="predicate">    . </param>
        /// <param name="dbCommand">    [in,out]. </param>
        /// <param name="dbProvider">   . </param>
        ///
        /// ### <returns>   . </returns>
        

        internal static IDbCommand UpdateCommandText<T>(T input, Expression<Func<T, bool>> predicate,  IDbCommand dbCommand, int dbProvider)
        {
            List<String> columnList = new List<string>();
            string whereClause = HelperUtility.ConvertExpressionToStringWithProvider(predicate.Body, dbProvider);
            string columnName = string.Empty;
            foreach (PropertyInfo propertyInfo in input.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                IDataParameter dbParam = dbCommand.CreateParameter();
                object value = propertyInfo.GetValue(input, null);
                if (value != null)
                {
                    //IDataParameter dbParam = dbCommand.CreateParameter();
                    switch (propertyInfo.PropertyType.ToString())
                    {
                        case "System.DateTime":
                            if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                            {
                                dbParam = dbCommand.CreateParameter();
                                columnName = ColumnName(propertyInfo);
                                dbParam.ParameterName = Prefix(dbProvider) + columnName;
                                dbParam.Value = dbProvider == 6 ? ((DateTime)propertyInfo.GetValue(input, null)).ToString("G") : (propertyInfo.GetValue(input, null));
                                dbParam.DbType = DbType.DateTime;
                                dbCommand.Parameters.Add(dbParam);
                                if (!IsPrimary(propertyInfo))
                                    columnList.Add(columnName + "=" + dbParam.ParameterName);
                            }
                            break;
                        default:
                            dbParam = dbCommand.CreateParameter();
                            columnName = ColumnName(propertyInfo);
                            dbParam.ParameterName = Prefix(dbProvider) + columnName;
                            dbParam.Value = propertyInfo.GetValue(input, null);
                            dbCommand.Parameters.Add(dbParam);
                            if (!IsPrimary(propertyInfo))
                                columnList.Add(columnName + "=" + dbParam.ParameterName);
                            break;
                    }
                }
                else
                {
                    dbParam = dbCommand.CreateParameter();
                    columnName = ColumnName(propertyInfo);
                    dbParam.ParameterName = Prefix(dbProvider) + columnName;
                    dbParam.Value = DBNull.Value;
                    dbCommand.Parameters.Add(dbParam);
                    if (!IsPrimary(propertyInfo))
                        columnList.Add(columnName + "=" + dbParam.ParameterName);
                }

            }
            columnName = string.Empty;
            dbCommand.CommandText = string.Format("Update {0} SET {1} WHERE {2}",
                            GetTableName<T>(),
                            string.Join(",", columnList.ToArray()),
                            whereClause
                            );
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        
        /// <summary>   Updates the command text. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="input">        . </param>
        /// <param name="dbCommand">    [in,out]. </param>
        /// <param name="whereClause">  . </param>
        /// <param name="dbProvider">   . </param>
        ///
        /// ### <returns>   . </returns>


        internal static IDbCommand UpdateCommandText<T>(T input, IDbCommand dbCommand, string whereClause, int dbProvider)
        {
            List<String> columnList = new List<string>();
            string columnName = string.Empty;
            foreach (PropertyInfo propertyInfo in input.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetValue(input, null) != null).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                object value = propertyInfo.GetValue(input, null);
                if (value != null)
                {
                    IDataParameter dbParam = dbCommand.CreateParameter();
                    switch (propertyInfo.PropertyType.ToString())
                    {
                        case "System.DateTime":
                            if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                            {
                                columnName = ColumnName(propertyInfo);
                                dbParam.ParameterName = Prefix(dbProvider) + columnName;
                                dbParam.Value = dbProvider == 6 ? ((DateTime)propertyInfo.GetValue(input, null)).ToString("G") : (propertyInfo.GetValue(input, null));
                                dbParam.DbType = DbType.DateTime;
                                dbCommand.Parameters.Add(dbParam);
                                if (!IsPrimary(propertyInfo))
                                    columnList.Add(columnName + "=" + dbParam.ParameterName);
                            }
                            break;
                        default:
                            columnName = ColumnName(propertyInfo);
                            dbParam.ParameterName = Prefix(dbProvider) + columnName;
                            dbParam.Value = propertyInfo.GetValue(input, null);
                            dbCommand.Parameters.Add(dbParam);
                            if (!IsPrimary(propertyInfo))
                                columnList.Add(columnName + "=" + dbParam.ParameterName);
                            break;
                    }
                }

            }
            columnName = string.Empty;
            dbCommand.CommandText = !string.IsNullOrEmpty(whereClause) ? string.Format("Update {0} SET {1} WHERE {2}",
                            GetTableName<T>(),
                            string.Join(",", columnList.ToArray()),
                            whereClause
                            ) : string.Empty;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        
        /// <summary>   Updates the command text. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="input">        . </param>
        /// <param name="dbCommand">    [in,out]. </param>
        /// <param name="dbProvider">   . </param>
        ///
        /// ### <returns>   . </returns>


        internal static IDbCommand UpdateCommandText<T>(T input, IDbCommand dbCommand, int dbProvider)
        {
            List<String> columnList = new List<string>();
            string whereClause = string.Empty;
            string columnName = string.Empty;
            foreach (PropertyInfo propertyInfo in input.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetValue(input, null) != null).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                object value = propertyInfo.GetValue(input, null);
                if (value != null)
                {
                    IDataParameter dbParam = dbCommand.CreateParameter();
                    switch (propertyInfo.PropertyType.ToString())
                    {
                        case "System.DateTime":
                            if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                            {
                                dbParam = dbCommand.CreateParameter();
                                columnName = ColumnName(propertyInfo);
                                dbParam.ParameterName = Prefix(dbProvider) + columnName;
                                dbParam.Value = dbProvider == 6 ? ((DateTime)propertyInfo.GetValue(input, null)).ToString("G") : (propertyInfo.GetValue(input, null));
                                dbParam.DbType = DbType.DateTime;
                                dbCommand.Parameters.Add(dbParam);
                                if (!IsPrimary(propertyInfo))
                                    columnList.Add(columnName + "=" + dbParam.ParameterName);
                                else
                                {
                                    whereClause += String.IsNullOrEmpty(whereClause) ? whereClause : " AND ";
                                    whereClause += columnName + "=" + dbParam.ParameterName;
                                }
                            }
                            break;
                        default:
                            dbParam = dbCommand.CreateParameter();
                            columnName = ColumnName(propertyInfo);
                            dbParam.ParameterName = Prefix(dbProvider) + columnName;
                            dbParam.Value = propertyInfo.GetValue(input, null);
                            dbCommand.Parameters.Add(dbParam);
                            if (!IsPrimary(propertyInfo))
                                columnList.Add(columnName + "=" + dbParam.ParameterName);
                            else
                            {
                                whereClause += String.IsNullOrEmpty(whereClause) ? whereClause : " AND ";
                                whereClause += columnName + "=" + dbParam.ParameterName;
                            }
                            break;
                    }
                }
            }
            columnName = string.Empty;
            dbCommand.CommandText = !string.IsNullOrEmpty(whereClause) ? string.Format("Update {0} SET {1} WHERE {2}",
                            GetTableName<T>(),
                            string.Join(",", columnList.ToArray()),
                            whereClause
                            ) : string.Empty;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        
        /// <summary>   Deletes the command text. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="predicate">    . </param>
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   A string. </returns>
        

        internal static string DeleteCommandText<T>(Expression<Func<T, bool>> predicate,int dbProvider)
        {
            return string.Format("DELETE FROM {0} WHERE {1}",
                            GetTableName<T>(),
                            HelperUtility.ConvertExpressionToString(predicate.Body)
                            );
        }

        
        /// <summary>   Deletes the command text. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="input">        . </param>
        /// <param name="dbCommand">    [in,out]. </param>
        /// <param name="dbProvider">   . </param>
        ///
        /// ### <returns>   . </returns>
        

        internal static IDbCommand DeleteCommandText<T>(T input,  IDbCommand dbCommand, int dbProvider)
        {
            List<String> columnList = new List<string>();
            string whereClause = string.Empty;
            string columnName = string.Empty;
            foreach (var propertyInfo in input.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetValue(input, null) != null).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                object value = propertyInfo.GetValue(input, null);
                if (value != null)
                {
                    IDataParameter dbParam = dbCommand.CreateParameter();
                    switch (propertyInfo.PropertyType.ToString())
                    {
                        case "System.DateTime":
                            if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                            {
                                dbParam = dbCommand.CreateParameter();
                                columnName = ColumnName(propertyInfo);
                                dbParam.ParameterName = Prefix(dbProvider) + columnName;
                                dbParam.Value = dbProvider == 6 ? ((DateTime)propertyInfo.GetValue(input, null)).ToString("G") : (propertyInfo.GetValue(input, null));
                                dbParam.DbType = DbType.DateTime;
                                dbCommand.Parameters.Add(dbParam);
                                whereClause += String.IsNullOrEmpty(whereClause) ? whereClause : " AND ";
                                whereClause += columnName + "=" + dbParam.ParameterName;
                            }
                            break;
                        default:
                            dbParam = dbCommand.CreateParameter();
                            columnName = ColumnName(propertyInfo);
                            dbParam.ParameterName = Prefix(dbProvider) + columnName;
                            dbParam.Value = propertyInfo.GetValue(input, null);
                            dbCommand.Parameters.Add(dbParam);
                            whereClause += String.IsNullOrEmpty(whereClause) ? whereClause : " AND ";
                            whereClause += columnName + "=" + dbParam.ParameterName;
                            break;
                    }
                }
            }
            columnName = string.Empty;
            dbCommand.CommandText = !string.IsNullOrEmpty(whereClause) ? string.Format("DELETE FROM {0} WHERE {1}",
                            GetTableName<T>(),
                            whereClause
                            ) : string.Empty;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        
        /// <summary>   Convert to. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="reader">       . </param>
        /// <param name="columnName">   . </param>
        /// <param name="input">        . </param>
        ///
        /// <returns>   to converted. </returns>
        

        internal static T ConvertTo<T>(IDataReader reader, string columnName, T input)
        {
            T t;
            try
            {
                if (reader[columnName] == null)
                {
                    t = input;
                }
                else
                {
                    t = (!Convert.IsDBNull(reader[columnName]) ? (T)Convert.ChangeType(reader[columnName], input.GetType()) : input);
                }
            }
            catch
            {
                t = input;
            }
            return t;
        }

        
        /// <summary>   Column name. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="propertyInfo"> . </param>
        ///
        /// <returns>   A string. </returns>
        

        internal static string ColumnName(PropertyInfo propertyInfo)
        {

            return ((CustomAttribute)propertyInfo.GetCustomAttribute(typeof(CustomAttribute), true)) == null ? propertyInfo.Name : ((CustomAttribute)propertyInfo.GetCustomAttribute(typeof(CustomAttribute), true)).Name ?? propertyInfo.Name;
        }

        
        /// <summary>   Query if 'propertyInfo' is primary. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="propertyInfo"> . </param>
        ///
        /// <returns>   True if primary, false if not. </returns>
        

        internal static bool IsPrimary(PropertyInfo propertyInfo)
        {
            return ((CustomAttribute)propertyInfo.GetCustomAttribute(typeof(CustomAttribute), true)) == null ? false : ((CustomAttribute)propertyInfo.GetCustomAttribute(typeof(CustomAttribute), true)).IsPrimary;
        }

        
        /// <summary>   Convert to. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        /// <param name="reader">   . </param>
        ///
        /// <returns>   to converted. </returns>
        

        internal static T ConvertTo<T>(IDataReader reader)
        {
            T t = Activator.CreateInstance<T>();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetMethod.IsVirtual == false))
            {
                try
                {
                    propertyInfo.SetValue(t, (Convert.IsDBNull(reader[ColumnName(propertyInfo)]) ? null : reader[ColumnName(propertyInfo)]), null);

                }
                catch
                {

                }
            }
            return t;
        }

        
        /// <summary>   Gets the attribute. </summary>
        ///
        /// <value> The t attribute. </value>
        ///
        /// ### <typeparam name="T">    . </typeparam>
        ///
        /// ### <typeparam name="TOut">         . </typeparam>
        /// ### <typeparam name="TAttribute">   . </typeparam>
        /// ### <typeparam name="TValue">       . </typeparam>
        /// ### <param name="propertyExpression">   . </param>
        /// ### <param name="valueSelector">        . </param>


        public static TValue GetPropertyAttributeValue<T, TOut, TAttribute, TValue>(Expression<Func<T, TOut>> propertyExpression, Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {

            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var att = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return att != null ? valueSelector(att) : default(TValue);
        }

        
        /// <summary>   Gets table name. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        ///
        /// <returns>   The table name. </returns>
        

        public static string GetTableName<T>()
        {
            var dnAttribute = typeof(T).GetCustomAttributes(typeof(CustomAttribute), true).FirstOrDefault() as CustomAttribute;
            return dnAttribute != null ? dnAttribute.Name ?? typeof(T).Name : typeof(T).Name;
        }

        
        /// <summary>   Gets primary column. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        ///
        /// <returns>   The primary column. </returns>


        public static string GetPrimaryColumn<T>()
        {
            string result = string.Empty;
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetMethod.IsVirtual == false))
            {
                if (!String.IsNullOrEmpty(ColumnName(propertyInfo)))
                {
                    if (IsPrimary(propertyInfo))
                    {
                        result = ((CustomAttribute)propertyInfo.GetCustomAttribute(typeof(CustomAttribute), true)).IsPrimary == true ? ((CustomAttribute)propertyInfo.GetCustomAttribute(typeof(CustomAttribute), true)).Name ?? propertyInfo.Name : propertyInfo.Name;
                        break;
                    }
                    else
                    {
                        result = propertyInfo.Name;
                        break;
                    }
                }

            }
            return result;
        }

        
       
        
      

        
        /// <summary>   Converts an inTable to a recordset. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="inTable">  . </param>
        ///
        /// <returns>   InTable as an ADODB.Recordset. </returns>
        

        internal static ADODB.Recordset ToRecordset(DataTable inTable)
        {
            ADODB.Recordset result = new ADODB.Recordset();
            result.CursorLocation = ADODB.CursorLocationEnum.adUseClient;

            ADODB.Fields resultFields = result.Fields;
            System.Data.DataColumnCollection inColumns = inTable.Columns;

            foreach (DataColumn inColumn in inColumns)
            {
                resultFields.Append(inColumn.ColumnName
                    , TranslateType(inColumn.DataType)
                    , inColumn.MaxLength
                    , inColumn.AllowDBNull ? ADODB.FieldAttributeEnum.adFldIsNullable :
                                             ADODB.FieldAttributeEnum.adFldUnspecified
                    , null);
            }

            result.Open(System.Reflection.Missing.Value
                    , System.Reflection.Missing.Value
                    , ADODB.CursorTypeEnum.adOpenStatic
                    , ADODB.LockTypeEnum.adLockOptimistic, 0);

            foreach (DataRow dr in inTable.Rows)
            {
                result.AddNew(System.Reflection.Missing.Value,
                              System.Reflection.Missing.Value);

                for (int columnIndex = 0; columnIndex < inColumns.Count; columnIndex++)
                {
                    resultFields[columnIndex].Value = dr[columnIndex];
                }
            }

            return result;
        }

        
        /// <summary>   Translate type. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="columnType">   . </param>
        ///
        /// <returns>   An ADODB.DataTypeEnum. </returns>
        

        internal static ADODB.DataTypeEnum TranslateType(Type columnType)
        {
            switch (columnType.UnderlyingSystemType.ToString())
            {
                case "System.Boolean":
                    return ADODB.DataTypeEnum.adBoolean;

                case "System.Byte":
                    return ADODB.DataTypeEnum.adUnsignedTinyInt;

                case "System.Char":
                    return ADODB.DataTypeEnum.adChar;

                case "System.DateTime":
                    return ADODB.DataTypeEnum.adDate;

                case "System.Decimal":
                    return ADODB.DataTypeEnum.adCurrency;

                case "System.Double":
                    return ADODB.DataTypeEnum.adDouble;

                case "System.Int16":
                    return ADODB.DataTypeEnum.adSmallInt;

                case "System.Int32":
                    return ADODB.DataTypeEnum.adInteger;

                case "System.Int64":
                    return ADODB.DataTypeEnum.adBigInt;

                case "System.SByte":
                    return ADODB.DataTypeEnum.adTinyInt;

                case "System.Single":
                    return ADODB.DataTypeEnum.adSingle;

                case "System.UInt16":
                    return ADODB.DataTypeEnum.adUnsignedSmallInt;

                case "System.UInt32":
                    return ADODB.DataTypeEnum.adUnsignedInt;

                case "System.UInt64":
                    return ADODB.DataTypeEnum.adUnsignedBigInt;

                case "System.String":
                default:
                    return ADODB.DataTypeEnum.adVarChar;
            }
        }

        
        /// <summary>   Convert expression to string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="body"> . </param>
        ///
        /// <returns>   The expression converted to string. </returns>
        

        internal static string ConvertExpressionToString(Expression body)
        {
            if (body == null)
            {
                return string.Empty;
            }
            if (body is ConstantExpression)
            {
                return ValueToString(((ConstantExpression)body).Value);
            }
           
            if (body is MemberExpression)
            {
               
                var member = ((MemberExpression)body);
                var value = GetValueOfMemberExpression(member);
                if (value == null)
                {
                    if (member.Member.MemberType == MemberTypes.Property)
                    {
                        return HelperUtility.ColumnName((PropertyInfo)member.Member);
                    }
                }
                return ValueToString(value);
            }
            if (body is UnaryExpression)
            {
                return ConvertExpressionToString(((UnaryExpression)body).Operand);
            }
            if (body is BinaryExpression)
            {
                var binary = body as BinaryExpression;
                return string.Format("({0}{1}{2})", ConvertExpressionToString(binary.Left),
                    ConvertExpressionTypeToString(binary.NodeType),
                    ConvertExpressionToString(binary.Right));
            }
            if (body is MethodCallExpression)
            {
                var method = body as MethodCallExpression;
                return string.Format("({0} IN ({1}))", ConvertExpressionToString(method.Arguments[0]),
                    ConvertExpressionToString(method.Object));
            }
            if (body is LambdaExpression)
            {
                return ConvertExpressionToString(((LambdaExpression)body).Body);
            }
            return "";
        }

        
        /// <summary>   Convert expression to string with provider. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="body">         . </param>
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   The expression converted to string with provider. </returns>

        internal static string ConvertExpressionToStringWithProvider(Expression body,int dbProvider)
        {
            if (body == null)
            {
                return string.Empty;
            }
            if (body is ConstantExpression)
            {
                return ValueToString(((ConstantExpression)body).Value);
            }
            if (body is MemberExpression)
            {
                var member = ((MemberExpression)body);
                if (member.Member.MemberType == MemberTypes.Property)
                {
                    return Prefix(dbProvider)+ HelperUtility.ColumnName((PropertyInfo)member.Member);
                }
                var value = GetValueOfMemberExpression(member);
                //if (value is IEnumerable)
                //{
                //    var sb = new StringBuilder();
                //    foreach (var item in value as IEnumerable)
                //    {
                //        sb.AppendFormat("{0},", ValueToString(item));
                //    }
                //    return sb.Remove(sb.Length - 1, 1).ToString();
                //}
                return ValueToString(value);
            }
            if (body is UnaryExpression)
            {
                return ConvertExpressionToStringWithProvider(((UnaryExpression)body).Operand,dbProvider);
            }
            if (body is BinaryExpression)
            {
                var binary = body as BinaryExpression;
                return string.Format("({0}{1}{2})", ConvertExpressionToString(binary.Left),
                    ConvertExpressionTypeToString(binary.NodeType),
                    ConvertExpressionToStringWithProvider(binary.Right, dbProvider));
            }
            if (body is MethodCallExpression)
            {
                var method = body as MethodCallExpression;
                return string.Format("({0} IN ({1}))", ConvertExpressionToString(method.Arguments[0]),
                    ConvertExpressionToStringWithProvider(method.Object, dbProvider));
            }
            if (body is LambdaExpression)
            {
                return ConvertExpressionToStringWithProvider(((LambdaExpression)body).Body, dbProvider);
            }
            return "";
        }

        
        /// <summary>   Value to string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="value">    . </param>
        ///
        /// <returns>   A string. </returns>
        

        internal static string ValueToString(object value)
        {
           
            if (value is string || value is Guid)
            {
                return string.Format("'{0}'", value);
            }
            if (value is DateTime)
            {
                return string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", value);
            }
            if (value is bool)
            {
                return ((bool)value ? "1" : "0");
            }
            //if (value is Guid)
            //{
            //    return string.Format("'{0}'", value.ToString());
            //}
            return value.ToString();
        }

        
        /// <summary>   Gets value of member expression. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="member">   . </param>
        ///
        /// <returns>   The value of member expression. </returns>
        

        internal static object GetValueOfMemberExpression(MemberExpression member)
        {
            object result = null;
            try
            {
                var objectMember = Expression.Convert(member, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                result= getter();
            }
            catch { 
            
            }
            return result;
        }

        
        /// <summary>   Convert expression type to string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="nodeType"> . </param>
        ///
        /// <returns>   The expression converted type to string. </returns>
        

        internal static string ConvertExpressionTypeToString(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.And:
                    return " AND ";
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Or:
                    return " OR ";
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                default:
                    return "";
            }
        }

        
        /// <summary>   Queries the builder. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        ///
        /// <returns>   The builder. </returns>
        

        internal static WD.DataAccess.QueryProviders.QueryBuilder<T> QueryBuilder<T>()
        {
            return new QueryProviders.QueryBuilder<T>();

        }

        
        /// <summary>   Gets the columns. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="T">    . </typeparam>
        ///
        /// <returns>   The columns. </returns>
        

        public static Dictionary<string, string> GetColumns<T>()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetMethod.IsVirtual == false).ToList<PropertyInfo>().Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                if (!String.IsNullOrEmpty(ColumnName(propertyInfo)))
                {
                    result.Add(ColumnName(propertyInfo), ColumnName(propertyInfo));
                }
            }
            return result;
        }

        
        /// <summary>   Gets table name. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="type"> The type. </param>
        ///
        /// <returns>   The table name. </returns>


        public static string GetTableName(Type T)
        {

            var dnAttribute = T.GetCustomAttributes(typeof(CustomAttribute), true).FirstOrDefault() as CustomAttribute;
            return dnAttribute != null ? dnAttribute.Name ?? T.Name : T.Name;
        }

        
        /// <summary>   Gets database provider. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   The database provider. </returns>
        

        public static int GetDbProvider(string dbProvider)
        {
            int result = DBProvider.Sql;
            switch (dbProvider.ToLower())
            {
                case "sql": result = DBProvider.Sql; break;
                case "db2": result = DBProvider.Db2; break;
                case "oracle": result = DBProvider.Oracle; break;
                case "oracle2": result = DBProvider.Oracle2; break;
                case "mysql": result = DBProvider.MySql; break;
                case "postgresql": result = DBProvider.PostgreSQL; break;
                default: result = DBProvider.TeraData; break;
            }
            return result;
        }

        
        /// <summary>   Gets database provider name. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   The database provider name. </returns>
        

        public static string GetDbProviderName(int dbProvider)
        {
            string result = "Sql";
            switch (dbProvider)
            {
                case DBProvider.Sql: result = "sql"; break;
                case DBProvider.Db2: result = "db2"; break;
                case DBProvider.Oracle:result = "oracle"; break;
                case DBProvider.Oracle2: result = "oracle2"; break;
                case DBProvider.MySql: result = "mysql"; break;
                case DBProvider.PostgreSQL: result = "postrgresql"; break;
                case DBProvider.TeraData: result = "teradata"; break;
                default: result = "sql"; break;
            }
            return result;
        }

        
        /// <summary>   Decrypts. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="strInCode">    The in code. </param>
        ///
        /// <returns>   A string. </returns>
        

        internal static string Decrypt(string strInCode)
        {
            string text = string.Empty;
            string text2 = string.Empty;
            strInCode = strInCode.Substring(2);
            checked
            {
                int num = (int)Math.Round(unchecked((double)strInCode.Length / 2.0 + 1.0));
                int arg_3E_0 = 2;
                int num2 = num;
                int num3 = arg_3E_0;
                while (true)
                {
                    int arg_AB_0 = num3;
                    int num4 = num2;
                    if (arg_AB_0 > num4)
                    {
                        break;
                    }
                    text = strInCode.Substring(0, 2);
                    strInCode = strInCode.Substring(2);
                    text = ConvertTo<string>(decimal.Subtract(Convert.ToDecimal(Convert.ToInt32(text, 16)), new decimal(num3 * 3)), string.Empty);
                    text = ConvertTo<string>(Convert.ToChar(Convert.ToUInt16(text)), string.Empty);
                    text2 += text;
                    num3++;
                }
                return text2;
            }
        }

        
        /// <summary>   Gets connection string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="list"> The list. </param>
        ///
        /// <returns>   The connection string. </returns>
        

        internal static string GetConnectionString(List<WDDBConfig> list)
        {
            string conString = string.Empty;
            try
            {
                WDDBConfig current = (from w in list
                                      where w.ActiveFlag == true
                                      select w
                                      ).FirstOrDefault();
                if (current != null)
                {
                    conString = new Connections(current.ServerName, current.DatabaseName, Helpers.HelperUtility.Decrypt(current.Userid), Helpers.HelperUtility.Decrypt(current.Password)).ConnectionString();
                }
            }
            catch
            {
                throw;
            }
            return conString;
        }

        
        /// <summary>   Move file to temporary directory. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="filePath"> . </param>
        ///
        /// <returns>   A string. </returns>
        

        public static string MoveToTemporaryDirectory(string filePath)
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            tempDirectory = Path.Combine(tempDirectory, Path.GetFileName(filePath));
            File.Move(filePath, tempDirectory);
            return tempDirectory;
        }

        
        /// <summary>   Move file to directory. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="filePath"> . </param>
        /// <param name="folder">   . </param>
        ///
        /// <returns>   A string. </returns>
        

        public static string MoveToDirectory(string filePath, string folder)
        {
            string tempDirectory = Path.Combine(folder, Guid.NewGuid().ToString());
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            tempDirectory = Path.Combine(tempDirectory, Path.GetFileName(filePath));
            File.Move(filePath, tempDirectory);
            return tempDirectory;
        }

        
        /// <summary>   Copies to temporary directory described by filePath. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="filePath"> . </param>
        ///
        /// <returns>   A string. </returns>
        

        public static string CopyToTemporaryDirectory(string filePath)
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            tempDirectory = Path.Combine(tempDirectory, Path.GetFileName(filePath));
            File.Copy(filePath, tempDirectory, true);
            return tempDirectory;
        }

        
        /// <summary>   Enumerates distinct by in this collection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="TSource">  . </typeparam>
        /// <typeparam name="TKey">     . </typeparam>
        /// <param name="source">       . </param>
        /// <param name="keySelector">  . </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process distinct by in this collection.
        /// </returns>
        

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }

        
        /// <summary>   Creates schema initialize. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="sdef"> . </param>
        ///
        /// <returns>   The new schema initialize. </returns>
        

        public static string CreateSchemaIni(SchemeDef sdef)
        {
            string schema = string.Empty;
            try
            {
                // define a new schema definition and populate it from the 
                // application properties
                // start a string builder to hold the contents of the schema file as it is construction
                StringBuilder sb = new StringBuilder();
                sb.Append("ColNameHeader=False" + Environment.NewLine);
                sb.Append("Format=Delimited(,)" + Environment.NewLine);
                sb.Append(@"DateTimeFormat=""dd-mmm-yyyy hh:nn:ss""" + Environment.NewLine);
                sb.Append(@"CharacterSet=ANSI" + Environment.NewLine);
                // next each column number, name and data type is added to the schema file
                foreach (var c in sdef.ColumnDefinition)
                {
                    string tmp = "Col" + c.ColumnNumber.ToString() + @"=""" + c.Name + @""" " + c.TypeData;
                    if (c.ColumnWidth > 0)
                        tmp += " Width " + c.ColumnWidth.ToString();
                    sb.AppendLine(tmp);
                }
                schema = sb.ToString();
            }
            catch (Exception ex)
            {
                schema = ex.ToString();
            }
            return schema;
        }



        /// <summary>
        /// /
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public static string GetDate(int dbProvider) {


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
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public static string DbDateTime(DateTime value, string format, int dbProvider)
        {
            string returnValue = string.Empty;
            switch (dbProvider)
            {
                case DBProvider.Oracle:
                case DBProvider.Oracle2:
                    returnValue = string.Format(format, value);
                    returnValue = "to_date('" + returnValue + "','MM/dd/yyyy HH24:mi:ss')";
                    break;
                case DBProvider.MySql:
                    returnValue = string.Format("{0:MM/dd/yyyy}", value);
                    break;
                default:
                    returnValue = "'" + string.Format(format, value) + "'";
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
                    returnValue = "Convert(getdate(),'" + format + "')";
                    break;
            }
            return returnValue;
        }
    }
}
