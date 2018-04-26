// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-17-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="Wrapper.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;
using System.Linq;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   Wrapper Class for Web Api. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    [Serializable]
    public class Wrapper
    {
        
        /// <summary>
        /// Collection of SQL Statements with CommandText, CommandType and Collection of Parameters.
        /// </summary>
        ///
        /// <value> the SQL. </value>
        

        public virtual SqlStatement[] TheSql { get; set; }

        
        /// <summary>   Authentication Token. </summary>
        ///
        /// <value> The authentication token. </value>
        

        public virtual string AuthenticationToken { get; set; }

        
        /// <summary>   Connection Class for DbContext Initialization. </summary>
        ///
        /// <value> The connect. </value>
        

        public virtual Connect Connect { get; set; }
    }
   
}
