// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 03-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-19-2017
// ***********************************************************************
// <copyright file="WDDBConfig.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WD.DataAccess.Enums;



// namespace: WD.DataAccess.Mitecs
//
// summary:	.


namespace WD.DataAccess.Mitecs
{
    
    /// <summary>   A WD configuration. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public class WDDBConfig
    {
        /// <summary>
        /// The index
        /// </summary>
        private readonly int _index;
        /// <summary>   The database provider. </summary>
        private readonly int _dbProvider;
        /// <summary>   True to recovery flag. </summary>
        private readonly bool _RecoveryFlag;

        /// <summary>   True to active flag. </summary>
        private readonly bool _ActiveFlag;

        /// <summary>   Name of the server. </summary>
        private readonly string _ServerName;

        /// <summary>   Name of the database. </summary>
        private readonly string _DatabaseName;


        private IDbTransaction _Transaction;
        public int Index {

            get { return _index; }
        }
        
        /// <summary>   Gets or sets the userid. </summary>
        ///
        /// <value> The userid. </value>
        

        public string Userid
        {
            get;
            set;
        }

        
        /// <summary>   Gets or sets the password. </summary>
        ///
        /// <value> The password. </value>
        

        public string Password
        {
            get;
            set;
        }

        
        /// <summary>   Gets or sets a value indicating whether the recovery flag. </summary>
        ///
        /// <value> True if recovery flag, false if not. </value>
        

        public bool RecoveryFlag
        {
            get
            {
                return this._RecoveryFlag;
            }
        }

        
        /// <summary>   Gets or sets a value indicating whether the active flag. </summary>
        ///
        /// <value> True if active flag, false if not. </value>
        

        public bool ActiveFlag
        {
            get
            {
                return this._ActiveFlag;
            }
        }

        
        /// <summary>   Gets or sets the name of the server. </summary>
        ///
        /// <value> The name of the server. </value>
        

        public string ServerName
        {
            get
            {
                return this._ServerName;
            }
        }

        
        /// <summary>   Gets or sets the name of the database. </summary>
        ///
        /// <value> The name of the database. </value>
        

        public string DatabaseName
        {
            get
            {
                return this._DatabaseName;
            }
        }

        
        /// <summary>   Gets or sets the error messages. </summary>
        ///
        /// <value> The error messages. </value>
        

        public string ErrorMessages
        {
            get;
            set;
        }

        
        /// <summary>   Gets or sets the database transaction. </summary>
        ///
        /// <value> The database transaction. </value>
        

        public IDbTransaction DatabaseTransaction
        {
            get
            {
                return this._Transaction;
            }
            set
            {
                this._Transaction = value;
            }
        }

        
        /// <summary>   Gets or sets the database provider. </summary>
        ///
        /// <value> The database provider. </value>
        

        public int dbProvider
        {
            get
            {
                return this._dbProvider;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="WDDBConfig"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="activeFlag">if set to <c>true</c> [active flag].</param>
        /// <param name="dbProvider">The database provider.</param>
        public WDDBConfig(int index,string serverName, string databaseName, bool activeFlag, int dbProvider)
        {
            this._RecoveryFlag = false;
            this._ActiveFlag = false;
            this._ServerName = string.Empty;
            this._DatabaseName = string.Empty;
            this._ActiveFlag = activeFlag;
            this._ServerName = serverName;
            this._DatabaseName = databaseName;
            this._dbProvider = dbProvider;
            this._index = index;
        }
    }
}
