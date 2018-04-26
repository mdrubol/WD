// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 03-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="Connections.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;



// namespace: WD.DataAccess.Mitecs
//
// summary:	.


namespace WD.DataAccess.Mitecs
{
    
    /// <summary>   A connections. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public class Connections
    {
        /// <summary>   The connection string. </summary>
        private string connectionString;

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="servername">   . </param>
        /// <param name="databaseName"> . </param>
        

        public Connections(string servername, string databaseName)
        {
            this.connectionString = string.Empty;
            this.connectionString = string.Concat(new string[]
			{
				"Database=",
				databaseName,
				";Server=",
				servername,
				";User Id=;Password=;Connect Timeout=30;"
			});
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="servername">   . </param>
        /// <param name="databaseName"> . </param>
        /// <param name="UserID">       . </param>
        /// <param name="Password">     . </param>
        

        public Connections(string servername, string databaseName, string UserID, string Password)
        {
            this.connectionString = string.Empty;
            this.connectionString = string.Concat(new string[]
			{
				"Database=",
				databaseName,
				";Server=",
				servername,
				";User Id=",
				UserID,
				";Password=",
				Password,
				";Connect Timeout=30;"
			});
        }

        
        /// <summary>   Connection string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string ConnectionString()
        {
            return connectionString;
        }
    }
}
