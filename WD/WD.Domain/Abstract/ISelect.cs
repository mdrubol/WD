using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using WD.DataAccess.Abstract;
using System.Collections.Generic;
using WD.DataAccess.Parameters;

namespace WD.Domain.Abstract
{
    public interface ISelect 
    {
        // DataTable
        DataTable Select_Generic_Table(string query, ref string errorNumber, ref string errorMessage);
        DataTable Select_Generic_Table(string table, string where, ref string errorNumber, ref string errorMessage);
        DataTable Select_Generic_Table(string table, string where, string columns, ref string errorNumber, ref string errorMessage);
        DataTable Select_Generic_Table(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage);
        
        // DataSet
        DataSet Select_Generic_DataSet(string query, ref string errorNumber, ref string errorMessage);
        DataSet Select_Generic_DataSet(string table, string where, ref string errorNumber, ref string errorMessage);
        DataSet Select_Generic_DataSet(string table, string where, string columns, ref string errorNumber, ref string errorMessage);
        DataSet Select_Generic_DataSet(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage);

        // Return List<T> Object
        List<T> Select_Generic_Object<T>(string query, ref string errorNumber, ref string errorMessage);
        List<T> Select_Generic_Object<T>(string table, string where, ref string errorNumber, ref string errorMessage);
        List<T> Select_Generic_Object<T>(string table, string where, string columns, ref string errorNumber, ref string errorMessage);
        List<T> Select_Generic_Object<T>(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage);

        // Return JSon String
        string Select_Generic_JSon(string query, ref string errorNumber, ref string errorMessage);
        string Select_Generic_JSon(string table, string where, ref string errorNumber, ref string errorMessage);
        string Select_Generic_JSon(string table, string where, string columns, ref string errorNumber, ref string errorMessage);
        string Select_Generic_JSon(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage);

        // Return XML String
        string Select_Generic_XML(string query, ref string errorNumber, ref string errorMessage);
        string Select_Generic_XML(string table, string where, ref string errorNumber, ref string errorMessage);
        string Select_Generic_XML(string table, string where, string columns, ref string errorNumber, ref string errorMessage);
        string Select_Generic_XML(string table, string where, string columns, string orderBy, ref string errorNumber, ref string errorMessage);

        // Serial Number
        DataTable Select_Generic_TableBySN(string table, string SERIAL_NUMBER, string columns, string orderBy, ref string errorNumber, ref string errorMessage);

        //Stored Procedure
        int Execute_SP(string spName, DBParameter[] parm, ref string errorNumber, ref string errorMessage);
    }
}
