// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 05-17-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="DataTableExtension.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   A data table extension. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public static class DataTableExtension
    {
        
        /// <summary>   A DataTable extension method that writes to CSV file. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dataTable">    The dataTable to act on. </param>
        /// <param name="filePath">     Full pathname of the file. </param>
        

        public static void WriteToCsvFile(this DataTable dataTable, string filePath)
        {
            StringBuilder fileContent = new StringBuilder();

            foreach (var col in dataTable.Columns)
            {
                fileContent.Append(col.ToString() + ",");
            }

            fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);



            foreach (DataRow dr in dataTable.Rows)
            {

                foreach (var column in dr.ItemArray)
                {
                    fileContent.Append("\"" + column.ToString() + "\",");
                }

                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
            }

            System.IO.File.WriteAllText(filePath, fileContent.ToString());

        }
    }
}
