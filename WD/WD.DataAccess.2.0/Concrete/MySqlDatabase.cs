
using System;
using System.Collections.Generic;
using System.Data;
using WD.DataAccess.Abstract;
using WD.DataAccess.Enums;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;
using WD.DataAccess.Parameters;

namespace WD.DataAccess.Concrete
{
    public sealed class MySqlDatabase : ICommands, IDisposable
    {
        #region Constructor


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="dbProvider">   . </param>


        public MySqlDatabase(int dbProvider) :
            base(dbProvider)
        {
        }


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="connectionString"> . </param>
        /// <param name="dbProvider">       . </param>


        public MySqlDatabase(string connectionString, int dbProvider) :
            base(dbProvider, connectionString)
        {

        }


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect"> . </param>


        public MySqlDatabase(Connect aConnect) :
            base(aConnect)
        {

        }


        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="aConnect">         . </param>
        /// <param name="bxConnections">    . </param>
        /// <param name="txConnections">    . </param>


        public MySqlDatabase(WD.DataAccess.Helpers.Connect aConnect,
             List<WD.DataAccess.Mitecs.WDDBConfig> bxConnections, List<WD.DataAccess.Mitecs.WDDBConfig> txConnections) :
            base(aConnect, bxConnections, txConnections)
        {

        }
        #endregion

       
    }
}
