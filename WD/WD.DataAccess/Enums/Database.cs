// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 03-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-28-2017
// ***********************************************************************
// <copyright file="Database.cs" company="Western Digital">
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
    
    /// <summary>   A databases. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public class Databases
    {
       
       /// <summary>    Default constructor. </summary>
       ///
       /// <remarks>    Shahid Kochak, 7/20/2017. </remarks>
       

       public Databases() { }
       /// <summary>    The BR flag. </summary>
       public const int BR = 1;
       /// <summary>    The TX flag. </summary>
       public const int TX = 2;
    }
}
