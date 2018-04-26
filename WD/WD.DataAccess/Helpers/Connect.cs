// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-18-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-20-2017
// ***********************************************************************
// <copyright file="Connect.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using WD.DataAccess.Enums;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
   
   /// <summary>    This class acts as Wrapper for Initializing DbContext. </summary>
   ///
   /// <remarks>    Shahid Kochak, 7/20/2017. </remarks>
   

   public class Connect
    {
        
        /// <summary>
        /// Connection Name Stored in Connection Settings of Client Application Web.Config or App.Config
        /// File.
        /// </summary>
        ///
        /// <value> The name of the connection. </value>
        

        public virtual string ConnectionName { get; set; }

        
        /// <summary>   Full Connection String for Client Application. </summary>
        ///
        /// <value> The connection string. </value>
        

        public virtual string ConnectionString { get; set; }

        
        /// <summary>   Provider Type SQL, Db2, Oracle, Oracle2 or TeraData. </summary>
        ///
        /// <value> The database provider. </value>
        

        public virtual int  DbProvider { get; set; }
    }
}
