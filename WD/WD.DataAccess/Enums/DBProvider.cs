// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-19-2017
// ***********************************************************************
// <copyright file="DBProvider.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;




// namespace: WD.DataAccess.Enums
//
// summary:	.


namespace WD.DataAccess.Enums
{
    
    /// <summary>   Defines different Set of databases. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public class DBProvider 
    {
        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public DBProvider() { }
        /// <summary>   SQL Database Provider. </summary>
        public const int Sql=1;
        /// <summary>   IBM DB2 Database Provider. </summary>
        public const int Db2=2;
        /// <summary>   Oracle Client Database Provider. </summary>
        public const int Oracle = 3;
        /// <summary>   Oracle Managed Data Access Provider. </summary>
        public const int Oracle2 = 4;
        /// <summary>   TeraData Database Provider. </summary>
        public const int TeraData = 5;
        /// <summary>
        /// Access database provider
        /// </summary>
        public const int Access = 6;
        /// <summary>
        /// PostgreSQL Database provider
        /// </summary>
        public const int PostgreSQL = 7;

        /// <summary>
        /// PostgreSQL Database provider
        /// </summary>
        public const int MySql = 8;
      
    }
}
