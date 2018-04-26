// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 02-20-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-19-2017
// ***********************************************************************
// <copyright file="TeraFactory.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using WD.DataAccess.Abstract;
using WD.DataAccess.Concrete;



// namespace: WD.DataAccess.Factory
//
// summary:	.


namespace WD.DataAccess.Factory
{
    
    /// <summary>   A tera factory. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    internal sealed class TeraFactory : FactoryBase
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
            return new TeraDatabase(connectionString, dbProvider);
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
            return new TeraDatabase(aConnect, bxConnections,txConnections);
        }
    } 
}
