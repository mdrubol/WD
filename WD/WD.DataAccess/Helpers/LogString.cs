// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 06-14-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="LogString.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   A log string. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public class LogString
    {

        #region Internal Class Fields
        /// <summary>   The name. </summary>
        private string m_strName;
        /// <summary>   The log. </summary>
        private string m_strLog = string.Empty;
        /// <summary>   True to timestamp. </summary>
        private bool m_bTimestamp = true;
        /// <summary>   True to line terminate. </summary>
        private bool m_bLineTerminate = true;
        /// <summary>   The maximum characters. </summary>
        private int m_nMaxChars = 32000;
        /// <summary>   True to reverse order. </summary>
        private bool m_bReverseOrder = true;
        #endregion

        #region Internal Static Fields

       
        /// <summary>   The logs table. </summary>
        private static Hashtable m_LogsTable = new Hashtable();

        #endregion

        #region Internal Methods

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="name"> The name. </param>
        

        public LogString(string name)
        {
            m_strName = name;
            ReadLog();  // Read existing
        }

        
        /// <summary>   Gets the filename of the file. </summary>
        ///
        /// <value> The name of the file. </value>
        

        private string FileName { get { return m_strName + ".log"; } }

        
        /// <summary>   Reads the log. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        private void ReadLog()
        {
            if (!File.Exists(FileName)) return;
            lock (m_strLog)          // lock resource
            {
                using (StreamReader sr = File.OpenText(FileName))
                {
                    m_strLog = sr.ReadToEnd();  // read entire file
                    sr.Close();
                }
            }
        }

        
        /// <summary>   Writes the log. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        private void WriteLog()
        {
            if (File.Exists(FileName)) File.Delete(FileName); // remove existing
            // Don't create file if log is empty.
            if (m_strLog.Length == 0) return;
            lock (m_strLog)        // lock resource
            {
                using (StreamWriter sw = File.CreateText(FileName))
                {
                    sw.Write(m_strLog);         // write entire contents
                    sw.Close();
                }
            }
        }
        #endregion

        #region Public Static Methods

        
        /// <summary>   Gets log string. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="name"> The name. </param>
        ///
        /// <returns>   The log string. </returns>
        

        public static LogString GetLogString(string name)
        {
            // If it exists, return the existing log.
            if (m_LogsTable.ContainsKey(name)) return (LogString)m_LogsTable[name];
            // Create and return a new log.
            LogString rv = new LogString(name);
            m_LogsTable.Add(name, rv); // add to table
            return rv;
        }

        
        /// <summary>   Removes the log string described by name. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="name"> The name. </param>
        

        public static void RemoveLogString(string name)
        {
            // If must exist
            if (m_LogsTable.ContainsKey(name))
            {
                LogString l = (LogString)m_LogsTable[name];
                l.Clear(); // remove file
                m_LogsTable.Remove(name); // remove from table
            }
        }

        
        /// <summary>   Persist all. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public static void PersistAll()
        {
            // Get the strongly typed (LogString) values list
            ICollection loglist = m_LogsTable.Values;
            // Persist each one.
            foreach (LogString ls in loglist) ls.Persist();
        }

        
        /// <summary>   Clears all. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public static void ClearAll()
        {
            // Get the strongly typed (LogString) values list
            ICollection loglist = m_LogsTable.Values;
            // Persist each one.
            foreach (LogString ls in loglist) ls.Clear();
        }
        #endregion

        #region Public Class Properties

        
        /// <summary>   Logs update delegate. </summary>
        /// Clients can get update callbacks. The LogUpdateDelegate will be 
        /// called whenever a new entry is added to a log.
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public delegate void LogUpdateDelegate();

        /// <summary>   Event queue for all listeners interested in OnLogUpdate events. </summary>
        public event LogUpdateDelegate OnLogUpdate;

        
        /// <summary>   Gets or sets a value indicating whether the timestamp. </summary>
        /// Option: Add timestamp to each log entry. Default=true
        /// <value> True if timestamp, false if not. </value>
        

        public bool Timestamp
        {
            get { return m_bTimestamp; }
            set { m_bTimestamp = value; }
        }

        
        /// <summary>   Gets or sets a value indicating whether the line terminate. </summary>
        /// Option: Terminate each line with CRLF. Default=true
        /// <value> True if line terminate, false if not. </value>
        

        public bool LineTerminate
        {
            get { return m_bLineTerminate; }
            set { m_bLineTerminate = value; }
        }

        
        /// <summary>   Gets or sets a value indicating whether the reverse order. </summary>
        /// Option: Add new entries to the start of the log. Default=true
        /// This makes viewing real-time updates easier because new items appear
        /// at the top while older entries scroll off the bottom. If set to false,
        /// new entries are appended to the end of the log text.
        ///
        /// <value> True if reverse order, false if not. </value>
        

        public bool ReverseOrder
        {
            get { return m_bReverseOrder; }
            set { m_bReverseOrder = value; }
        }

        
        /// <summary>   Gets or sets the maximum characters. </summary>
        /// Option: Maximum number of characters allowed in the log. Default=32000.
        /// Once the log string reaches this size the oldest test is removed: From the
        /// end if ReverseOrder is true, from the start if ReverseOrder is false.
        /// <value> The maximum characters. </value>
        

        public int MaxChars
        {
            get { return m_nMaxChars; }
            set { m_nMaxChars = value; }
        }

        
        /// <summary>   Gets the log. </summary>
        /// Get the logging string. This returns the current log string.
        /// <value> The log. </value>
        

        public string Log
        {
            get
            {
                lock (m_strLog)  // lock the resource
                {
                    return m_strLog;
                }
            }
        }

        
        /// <summary>   Gets the name. </summary>
        /// Name of this log.
        /// <value> The name. </value>
        

        public string Name { get { return m_strName; } }

        #endregion

        #region Public Class Methods

        
        /// <summary>   Adds str. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="str">  The String to add. </param>
        

        public void Add(string str)
        {
            string toadd = "";
            if (m_bTimestamp)
            {
                DateTime dt = DateTime.Now;
                // TBD: Allow user supplied timestamp formatting.
                toadd += dt.ToString() + ": ";
            }

            // Add the string
            toadd += str;

            // Add termination
            if (m_bLineTerminate) toadd += "\r\n";

            lock (m_strLog) // lock resource 
            {
                if (m_bReverseOrder)
                {
                    m_strLog = toadd + m_strLog;    // new entry on top
                }
                else
                {
                    m_strLog += toadd;              // new entry at the end
                }

                // Limit string length
                if (m_strLog.Length > m_nMaxChars)
                {
                    if (m_bReverseOrder)
                        m_strLog = m_strLog.Substring(0, m_nMaxChars); // preserve first N chars
                    else
                        m_strLog = m_strLog.Substring(m_strLog.Length - m_nMaxChars); // preserve last N chars
                }
            }
            // Notify listeners of the update
            if (OnLogUpdate != null) OnLogUpdate();
        }

        
        /// <summary>   Persists this object. </summary>
        /// Save this log.
        ///
        

        public void Persist()
        {
            WriteLog();
        }

        // Clear log contents and remove the log file
        //
        public void Clear()
        {
            lock (m_strLog) // lock resource 
            {
                m_strLog = string.Empty;
            }
            WriteLog(); // This will remove the log file
            // Notify listeners of the update
            if (OnLogUpdate != null) OnLogUpdate();
        }
        #endregion
    }
}
