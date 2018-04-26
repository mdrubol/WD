// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-19-2017
// ***********************************************************************
// <copyright file="FactoryBase.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************


using WD.DataAccess.Abstract;
using System;
using WD.DataAccess.Mitecs;
using System.Collections.Generic;




// namespace: WD.DataAccess.Factory
//
// summary:	.


namespace WD.DataAccess.Factory
{
    
    /// <summary>   A factory base. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    internal abstract class FactoryBase
    {
        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public FactoryBase() { }

        
        /// <summary>   Gets data layer. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="dbProvider">       The database provider. </param>
        ///
        /// <returns>   The data layer. </returns>
        

        public abstract ICommands GetDataLayer(string connectionString, int dbProvider);

        
        /// <summary>   Gets data layer. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect">         The connect. </param>
        /// <param name="bxConnections">    The bx connections. </param>
        /// <param name="txConnections">    The transmit connections. </param>
        ///
        /// <returns>   The data layer. </returns>
        

        public abstract ICommands GetDataLayer(WD.DataAccess.Helpers.Connect aConnect,
           List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections);
        
    }
}
