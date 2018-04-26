// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 05-17-2017
//
// Last Modified By : shahid_k
// Last Modified On : 05-17-2017
// ***********************************************************************
// <copyright file="AccessToken.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



// namespace: WD.XLC.Domain.Enums
//
// summary:	.


namespace WD.XLC.Domain.Enums
{
    
    /// <summary>   Values that represent access tokens. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    

    public enum AccessToken
    {

        /// <summary>   An enum constant representing the working= 0 option. </summary>
        Working=0,

        /// <summary>   An enum constant representing the pending= 1 option. </summary>
        Pending=1,

        /// <summary>   An enum constant representing the bad= 2 option. </summary>
        Bad=2,

        /// <summary>   An enum constant representing the process= 3 option. </summary>
        Process=3
    }
}
