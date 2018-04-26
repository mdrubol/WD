// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 05-17-2017
//
// Last Modified By : shahid_k
// Last Modified On : 05-18-2017
// ***********************************************************************
// <copyright file="SQLHelpers.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



// namespace: WD.XLC.Domain.Helpers
//
// summary:	.


namespace WD.XLC.Domain.Helpers
{
   

    public class SQLHelpers
    {
     

        
        public static string GetDataType(string p)
        {
          
            string type = "string";
            switch (p.ToLower().Replace("system.",""))
            {
                case "string":
                case "varchar":
                case "varchar2":
                case "nvarchar2":
                case "clob":
                case "nclob":
                case "char":
                case "nchar":
                    type = "string";
                    break;
                case "date":
                case "datetime":
                case "timestamp":
                    type = "datetime";
                    break;
                case "number":
                    type = "int"; break;
                case "raw":
                    type = "byte"; break;
                case "long":
                    type = "long"; break;
                case "rowid":
                case "urowid":
                    type = "Int64"; break;
                case "blob":
                case "bfile":
                    type = "byte"; break;
                case "float":
                case "binary_float":
                    type = "float"; break;
                case "double":
                case "binary_double":
                    type = "double"; break;
                default: type = "int"; break;
            }
            return type;
        }

       
    }
}
