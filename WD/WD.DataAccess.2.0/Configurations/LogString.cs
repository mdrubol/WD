using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WD.DataAcces.Helpers
{
    public class LogString
    {

        #region Internal Class Fields

        private string m_strName;
        private string m_strLog = string.Empty;
        private bool m_bTimestamp = true;
        private bool m_bLineTerminate = true;
        private int m_nMaxChars = 32000;
        private bool m_bReverseOrder = true;

        #endregion

        #region Internal Static Fields

        // Table of LogString instances
        private static Hashtable m_LogsTable = new Hashtable();

        #endregion

        #region Internal Methods

        // Constructor
        private LogString(string name)
        {
            m_strName = name;
            ReadLog();  // Read existing
        }

        // Generate the log file name
        //
        private string FileName { get { return m_strName + ".log"; } }

        // Read the log file if it exists.
        //
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

        // Write the log file. Delete existing first
        //
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

        // Access to named LogString instances.
        //
        public static LogString GetLogString(string name)
        {
            // If it exists, return the existing log.
            if (m_LogsTable.ContainsKey(name)) return (LogString)m_LogsTable[name];
            // Create and return a new log.
            LogString rv = new LogString(name);
            m_LogsTable.Add(name, rv); // add to table
            return rv;
        }

        // Remove a named log
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

        // Save all named logs.
        //
        public static void PersistAll()
        {
            // Get the strongly typed (LogString) values list
            ICollection loglist = m_LogsTable.Values;
            // Persist each one.
            foreach (LogString ls in loglist) ls.Persist();
        }

        // Clear all named logs.
        //
        public static void ClearAll()
        {
            // Get the strongly typed (LogString) values list
            ICollection loglist = m_LogsTable.Values;
            // Persist each one.
            foreach (LogString ls in loglist) ls.Clear();
        }
        #endregion

        #region Public Class Properties

        // Clients can get update callbacks. The LogUpdateDelegate will be 
        // called whenever a new entry is added to a log.
        //
        public delegate void LogUpdateDelegate();
        public event LogUpdateDelegate OnLogUpdate;

        // Option: Add timestamp to each log entry. Default=true
        //
        public bool Timestamp
        {
            get { return m_bTimestamp; }
            set { m_bTimestamp = value; }
        }

        // Option: Terminate each line with CRLF. Default=true
        //
        public bool LineTerminate
        {
            get { return m_bLineTerminate; }
            set { m_bLineTerminate = value; }
        }

        // Option: Add new entries to the start of the log. Default=true
        // This makes viewing real-time updates easier because new items appear
        // at the top while older entries scroll off the bottom. If set to false,
        // new entries are appended to the end of the log text.
        //
        public bool ReverseOrder
        {
            get { return m_bReverseOrder; }
            set { m_bReverseOrder = value; }
        }

        // Option: Maximum number of characters allowed in the log. Default=32000.
        // Once the log string reaches this size the oldest test is removed: From the
        // end if ReverseOrder is true, from the start if ReverseOrder is false.
        //
        public int MaxChars
        {
            get { return m_nMaxChars; }
            set { m_nMaxChars = value; }
        }

        // Get the logging string. This returns the current log string.
        //
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

        // Name of this log.
        public string Name { get { return m_strName; } }

        #endregion

        #region Public Class Methods

        // Add a log entry. Use the options to determine ordering, timestamping, etc.
        //
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

        // Save this log.
        //
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
