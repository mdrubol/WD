using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD.DataAccess.Abstract;
using WD.DataAccess.Concrete;

namespace WD.DataAccess.Factory
{
    /// <summary>   A PostgreSQL factory. This class cannot be inherited. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>


    internal sealed class MySqlFactory : FactoryBase
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
            return new MySqlDatabase(connectionString, dbProvider);
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
            return new MySqlDatabase(aConnect, bxConnections, txConnections);
        }
    }
}
