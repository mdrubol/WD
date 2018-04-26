// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-19-2017
// ***********************************************************************
// <copyright file="SqlFactory.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using WD.DataAccess.Abstract;
using WD.DataAccess.Concrete;
using System;
using System.Collections.Generic;




// namespace: WD.DataAccess.Factory
//
// summary:	.


namespace WD.DataAccess.Factory
{
    
    /// <summary>   A SQL factory. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    internal sealed class SqlFactory : FactoryBase
    {
        
        /// <summary>   Gets data layer. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="dbProvider">       The database provider. </param>
        ///
        /// <returns>   The data layer. </returns>
        

        public override ICommands GetDataLayer(string connectionString, int dbProvider)
        {
            return new SqlDatabase(connectionString, dbProvider);
        }

        
        /// <summary>   Gets data layer. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect">         The connect. </param>
        /// <param name="bxConnections">    The bx connections. </param>
        /// <param name="txConnections">    The transmit connections. </param>
        ///
        /// <returns>   The data layer. </returns>
        

        public override ICommands GetDataLayer(WD.DataAccess.Helpers.Connect aConnect,
                 List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections)


        {
            return new SqlDatabase(aConnect, bxConnections,txConnections);
        }
    } 
}
