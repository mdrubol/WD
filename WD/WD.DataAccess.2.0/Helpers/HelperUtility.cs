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

using WD.DataAccess.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;


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

        
        /// <summary>   Creates data adapter. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        ///
        /// <returns>   The new data adapter. </returns>
        

      

        
        
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
        
        /// <summary>   Value to string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="value">    . </param>
        ///
        /// <returns>   A string. </returns>
        

        internal static string ValueToString(object value)
        {
           
            if (value is string)
            {
                return string.Format("'{0}'", value);
            }
            if (value is DateTime)
            {
                return string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", value);
            }
            return value.ToString();
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
                case DBProvider.Oracle: result = "oracle"; break;
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
        
    }
}
