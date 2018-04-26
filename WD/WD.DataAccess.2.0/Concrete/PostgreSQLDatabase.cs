using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Text;

using WD.DataAccess.Abstract;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;

namespace WD.DataAccess.Concrete
{
    public sealed class PostgreSQLDatabase : ICommands, IDisposable
    {
        #region Constructor


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>


        public PostgreSQLDatabase(int dbProvider) :
            base(dbProvider)
        {
        }


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> . </param>
        /// <param name="dbProvider">       . </param>


        public PostgreSQLDatabase(string connectionString, int dbProvider) :
            base(dbProvider, connectionString)
        {
        }


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect"> . </param>


        public PostgreSQLDatabase(Connect aConnect) :
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


        public PostgreSQLDatabase(Connect aConnect,
             List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections) :
            base(aConnect, bxConnections, txConnections)
        {

        }
        #endregion



    }
}
