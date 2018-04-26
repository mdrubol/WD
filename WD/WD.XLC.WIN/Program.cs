using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Configuration;
using System.IO;
using WD.XLC.Domain.Entities;
using WD.XLC.Domain.Helpers;



// namespace: WD.XLC.WIN
//
// summary:	.


namespace WD.XLC.WIN
{
    
    /// <summary>   A program. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    static class Program
    {
        /// <summary>   The main entry point for the application. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        [STAThread]
        static void Main(string[] args)
        {
            if (!IsApplicationAlreadyRunning())
            {
                Utility.ResetConfig();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DashboardForm df = new DashboardForm(Utility.InitiateConfig(), ConfigurationManager.AppSettings["IsRunning"].ToLower() == "true" ? true : false);
            df.ChangeTab(4);
            Application.Run(df);
            df.Dispose();
            df = null;
        }
        /// <summary>   Running instance. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <returns>   The Process. </returns>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
           
            //Loop through the running processes in with the same name 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file. 
                    if (System.Reflection.Assembly.GetExecutingAssembly().Location.
                         Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.  
                        return process;

                    }
                }
            }
            //No other instance was found, return null.  
            return null;
        }
        /// <summary>   Query if this object is application already running. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <returns>   True if application already running, false if not. </returns>
        static bool IsApplicationAlreadyRunning()
        {
            string proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 1)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
