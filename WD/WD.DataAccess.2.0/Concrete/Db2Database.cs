// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 06-05-2017
// ***********************************************************************
// <copyright file="Db2Database.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using WD.DataAccess.Abstract;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using System.Data.Common;
using WD.DataAccess.Parameters;
using WD.DataAccess.Logger;



// namespace: WD.DataAccess.Concrete
//
// summary:	.


namespace WD.DataAccess.Concrete
{
    
    /// <summary>   A database 2 database. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public sealed class Db2Database : ICommands, IDisposable
    {

        #region Constructor

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>
        

        public Db2Database(int  dbProvider) :
            base(dbProvider)
        {
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> . </param>
        /// <param name="dbProvider">       . </param>
        

        public Db2Database(string connectionString, int  dbProvider) :
            base(dbProvider, connectionString)
        {
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect"> . </param>
        

        public Db2Database(Connect aConnect) :
            base(aConnect)
        {

        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect">         . </param>
        /// <param name="bxConnections">    . </param>
        /// <param name="txConnections">    The transmit connections. </param>
        

        public Db2Database(Connect aConnect,
            List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections) :
            base(aConnect, bxConnections, txConnections)
        {

        }
        #endregion

        

    }


}
