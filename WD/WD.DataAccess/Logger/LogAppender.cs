// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-24-2017
//
// Last Modified By : shahid_k
// Last Modified On : 04-19-2017
// ***********************************************************************
// <copyright file="LogAppender.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using log4net.Appender;
using log4net.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using WD.DataAccess.Enums;




// namespace: WD.DataAccess.Logger
//
// summary:	.


namespace WD.DataAccess.Logger
{
    
    /// <summary>   A log appender. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public abstract class  LogAppender : IAppender, IBulkAppender, IOptionHandler
    {
        
        /// <summary>   Loggings the given loggin event. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="logginEvent">  The loggin event. </param>
        

        public delegate void Logging(LoggingEvent logginEvent);

        
        /// <summary>   Loggings the given logging events. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="loggingEvents">    The logging events. </param>
        

        public delegate void Loggings(LoggingEvent[] loggingEvents);

        
        /// <summary>   Active options. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public delegate void ActiveOptions();

        
        /// <summary>   Gets the RegEx statement. </summary>
        ///
        /// <value> The RegEx statement. </value>
        

        public virtual System.Text.RegularExpressions.Regex RegexStatement { get { return new System.Text.RegularExpressions.Regex(@"(?m)^SELECT\s+FROM|DELETE\s+FROM\s|UPDATE\s+SET\s|INSERT\s+INTO\s", System.Text.RegularExpressions.RegexOptions.IgnoreCase); } }

        
        /// <summary>   Gets the RegEx select statement. </summary>
        ///
        /// <value> The RegEx select statement. </value>
        

        public virtual System.Text.RegularExpressions.Regex RegexSelectStatement { get { return new System.Text.RegularExpressions.Regex(@"(?m)^SELECT\sFROM\s", System.Text.RegularExpressions.RegexOptions.IgnoreCase); } }

        
        /// <summary>   Gets the RegEx insert statement. </summary>
        ///
        /// <value> The RegEx insert statement. </value>
        

        public virtual System.Text.RegularExpressions.Regex RegexInsertStatement { get { return new System.Text.RegularExpressions.Regex(@"(?m)^((\s*INSERT\s+INTO\s+))", System.Text.RegularExpressions.RegexOptions.IgnoreCase); } }

        
        /// <summary>   Gets the RegEx update statement. </summary>
        ///
        /// <value> The RegEx update statement. </value>
        

        public virtual System.Text.RegularExpressions.Regex RegexUpdateStatement { get { return new System.Text.RegularExpressions.Regex(@"(?m)^((\s*UPDATE\s*(\w+)\s+SET\s+)|(\s*UPDATE\s*(\w+.\w+)\s+SET\s+)|(\s*UPDATE\s*\[\w+\].\[\w+\]\s+SET\s)|(\s*UPDATE\s*\[\w+\]\s+SET\s))", System.Text.RegularExpressions.RegexOptions.IgnoreCase); } }

        
        /// <summary>   Gets the RegEx delete statement. </summary>
        ///
        /// <value> The RegEx delete statement. </value>
        

        public virtual System.Text.RegularExpressions.Regex RegexDeleteStatement { get { return new System.Text.RegularExpressions.Regex(@"(?m)^((\s*DELETE\s+FROM\s+))", System.Text.RegularExpressions.RegexOptions.IgnoreCase); } }

        
        /// <summary>   Gets the RegEx where clause. </summary>
        ///
        /// <value> The RegEx where clause. </value>
        

        public virtual System.Text.RegularExpressions.Regex RegexWhereClause{get{return new System.Text.RegularExpressions.Regex(@"(?m)^*WHERE\s+",System.Text.RegularExpressions.RegexOptions.IgnoreCase);}}
        
        /// <summary>   Event queue for all listeners interested in DoActiveOptions events. </summary>
        public event ActiveOptions DoActiveOptions;
       
        /// <summary>   Event queue for all listeners interested in DoLogging events. </summary>
        public event Logging DoLogging;
       
        /// <summary>   Event queue for all listeners interested in DoLoggings events. </summary>
        public event Loggings DoLoggings;

        /// <summary>   The database provider. </summary>
        private int dbProvider;
        /// <summary>
        /// 
        /// </summary>
        private string name;
        /// <summary>   The connection string. </summary>
        private string connectionString;

        
        /// <summary>   Gets or sets the name of this appender. </summary>
        ///
        /// <remarks>   <para>The name uniquely identifies the appender.</para> </remarks>
        ///
        /// <value> The name of the appender. </value>
        

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        
        /// <summary>   Closes the appender and releases resources. </summary>
        ///
        /// <remarks>
        /// <para>
        /// Releases any resources allocated within the appender such as file handles, network
        /// connections, etc.
        /// </para>
        /// <para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// </remarks>
        

        public void Close()
        {
           
        }

        
        /// <summary>   Log the logging event in Appender specific way. </summary>
        ///
        /// <remarks>
        /// <para>
        /// This method is called to log a message into this appender.
        /// </para>
        /// </remarks>
        ///
        /// <param name="loggingEvent"> The event to log. </param>
        

        public void DoAppend(LoggingEvent loggingEvent)
        {
            if (DoLogging != null)
            {
                DoLogging(loggingEvent);//Raise the event
            }
        }

        
        /// <summary>   Log the array of logging events in Appender specific way. </summary>
        ///
        /// <remarks>
        /// <para>
        /// This method is called to log an array of events into this appender.
        /// </para>
        /// </remarks>
        ///
        /// <param name="loggingEvents">    The events to log. </param>
        

        public void DoAppend(LoggingEvent[] loggingEvents)
        {
            if (DoLoggings != null)
            {
                DoLoggings(loggingEvents);//Raise the event
            }
        }

        
        /// <summary>   Activate the options that were previously set with calls to properties. </summary>
        ///
        /// <remarks>
        /// <para>
        /// This allows an object to defer activation of its options until all options have been set.
        /// This is required for components which have related options that remain ambiguous until all
        /// are set.
        /// </para>
        /// <para>
        /// If a component implements this interface then this method must be called after its properties
        /// have been set before the component can be used.
        /// </para>
        /// </remarks>
        

        public void ActivateOptions()
        {
            if (DoActiveOptions != null)
            {
                DoActiveOptions();//Raise the event
            }
        }
    }
}
