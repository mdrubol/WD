// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-18-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="SqlStatement.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;

using WD.DataAccess.Parameters;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   (Serializable) a SQL statement. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    [Serializable]
    public class SqlStatement
    {
        
        /// <summary>   Open Sql Statement or Procedure Name. </summary>
        ///
        /// <value> The command text. </value>
        

        public virtual string CommandText { get; set; }

        
        /// <summary>   CommandType for Text or StoredProcedure (1 or 4) </summary>
        ///
        /// <value> The type of the command. </value>
        

        public virtual CommandType CommandType { get; set; }

        
        /// <summary>   Collection of Parameters. </summary>
        ///
        /// <value> The parameters. </value>
        

        public virtual DBParameter[] Params { get; set; }
    }
}
