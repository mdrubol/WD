// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 03-24-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="Transaction.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;



// namespace: WD.DataAccess.Enums
//
// summary:	.


namespace WD.DataAccess.Enums
{
    
    /// <summary>   A transaction. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public class Transaction 
    {
        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public Transaction() { }
      /// <summary> No Transaction for Database. </summary>
      public const int  None = 1;
      /// <summary> Transaction for Connections Separately. </summary>
      public const int Yes = 2;
       /// <summary>    One Transaction for All Connections without connection Transaction. </summary>
       public const int  TS = 3;
        /// <summary>   One Transaction for All Connections with connection Transaction. </summary>
        public const int TSNT = 4;
    }
}
