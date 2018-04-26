// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-17-2017
// ***********************************************************************
// <copyright file="ILogger.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;



// namespace: WD.DataAccess.Logger
//
// summary:	.


namespace WD.DataAccess.Logger
{
    
    /// <summary>   A logger. 
    /// Used to log activities, exceptions, information and errors.
    /// It uses Log4Net library for logging purpose.
    /// This class cannot be inherited.  
    /// </summary>
    ///
    /// <remarks>   Shahid Kochak, Asim, 7/21/2017. </remarks>
    

    public sealed class ILogger
    {

        /// <summary>   Zero-based index of the log. </summary>
        private readonly ILog iLog;

        
        /// <summary>   Default constructor. 
        /// Reads the WDlog.config file to get the Logfile name and initialize the log configurations.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, Asim, 7/21/2017. </remarks>
        

        public ILogger()
        {
            try
            {
                string logFile = System.Configuration.ConfigurationManager.AppSettings["wdlog.Config"];
                if (!string.IsNullOrEmpty(logFile))
                {
                    if (File.Exists(logFile))
                    {
                        log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logFile));
                    }
                }
                logFile = string.Empty;
            }
            catch { }
            iLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        
        /// <summary>   Logs an object with the log4net.Core.Level.Error level.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///     //some code
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Error(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        ///     This method first checks if this logger is ERROR enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Error level. If this logger
        ///     is ERROR enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Error(object,Exception) form instead.
        /// Shahid Kochak, Asim, 7/21/2017.
        /// <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="ERRORLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="ERROR"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinError.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="ERROR" /> //
        ///        <levelMax value="ERROR" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="ERROR" /> //
        ///      <appender-ref ref="ERRORLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>  
        ///    </remarks>
        /// <param name="exc">  . </param>
        

        public static void Error(Exception exc)
        {
            
            new ILogger().iLog.Error(exc);
        }

        
        /// <summary>   Logs a message object with the log4net.Core.Level.Error level.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///     //some code
        ///       WD.DataAccess.Logger.ILogger.ILog.Error("some text or object");
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Error(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is ERROR enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Error level. If this logger
        ///     is ERROR enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Error(object,Exception) form instead.
        /// Shahid Kochak, Asim, 7/21/2017. 
        /// <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="ERRORLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="ERROR"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinError.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="ERROR" /> //
        ///        <levelMax value="ERROR" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="ERROR" /> //
        ///      <appender-ref ref="ERRORLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// </remarks>
        ///
        /// <param name="item"> . </param>
        

        public static void Error(object item)
        {
            new ILogger().iLog.Error(item);
        }

        
        /// <summary>   Log a message object with the log4net.Core.Level.Error level including the
        ///     stack trace of the System.Exception passed as a parameter.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///     //some code
        ///       WD.DataAccess.Logger.ILogger.ILog.Error("some text or object");
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Error("some text or object",exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>
        /// <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="ERRORLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="ERROR"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinError.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="ERROR" /> //
        ///        <levelMax value="ERROR" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="ERROR" /> //
        ///      <appender-ref ref="ERRORLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// </remarks>
        /// <param name="item"> . </param>
        /// <param name="exc">  . </param>
        

        public static void Error(object item, Exception exc)
        {
            new ILogger().iLog.Error(item, exc);
        }

        
        /// <summary>   Logs an object with the log4net.Core.Level.Info level.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///     //some code
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Info(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is INFO enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Info level. If this logger
        ///     is INFO enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Info(object,Exception) form instead.
        /// Shahid Kochak, Asim, 7/21/2017. 
        ///  <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="INFOLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="INFO"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinInfo.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="INFO" /> //
        ///        <levelMax value="INFO" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="INFO" /> //
        ///      <appender-ref ref="INFOLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// </remarks>
        ///
        /// <param name="exc">  . </param>
        

        public static void Info(Exception exc)
        {
            new ILogger().iLog.Info(exc);
        }

        
        /// <summary>  Logs a message object with the log4net.Core.Level.Info level.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///    WD.DataAccess.Logger.ILogger.ILog.Info("some text or some object");
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Info(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is INFO enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Info level. If this logger
        ///     is INFO enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Info(object,Exception) form instead.
        /// Shahid Kochak, Asim, 7/21/2017.
        ///   <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="INFOLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="INFO"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinInfo.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="INFO" /> //
        ///        <levelMax value="INFO" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="INFO" /> //
        ///      <appender-ref ref="INFOLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// </remarks>
        ///  
        ///    </remarks>
        ///
        /// <param name="item"> . </param>
        

        public static void Info(object item)
        {
            new ILogger().iLog.Info(item);
        }

        
        /// <summary>   Logs a message object with the INFO level including the stack trace of the
        ///     System.Exception passed as a parameter.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///    //some code
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Info("some text or some object",exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>    
        ///                <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="INFOLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="INFO"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinInfo.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="INFO" /> //
        ///        <levelMax value="INFO" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="INFO" /> //
        ///      <appender-ref ref="INFOLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// </remarks></remarks>
        ///
        /// <param name="item"> . </param>
        /// <param name="exc">  . </param>
        

        public static void Info(object item, Exception exc)
        {
            new ILogger().iLog.Info(item, exc);
        }

        
        /// <summary>   Debugs the given exc. 
        ///Log the contents of Exception object as Debug Level.
        ///</summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///    //some code
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Debug(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        ///                <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="DEBUGLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="DEBUG"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinDebug.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="DEBUG" /> //
        ///        <levelMax value="DEBUG" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="INFO" /> //
        ///      <appender-ref ref="INFOLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        ///             </remarks>
        ///
        /// <param name="exc">  . </param>
        

        public static void Debug(Exception exc)
        {
            new ILogger().iLog.Debug(exc);
        }

        
        /// <summary>   Debugs the given item. 
        /// Log the message object as Debug level.
        /// </summary>
        ///
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///     WD.DataAccess.Logger.ILogger.ILog.Debug("some message or object");
        ///    
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Debug(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        ///                <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="DEBUGLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="DEBUG"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinDebug.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="DEBUG" /> //
        ///        <levelMax value="DEBUG" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="INFO" /> //
        ///      <appender-ref ref="INFOLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        ///             </remarks>
        ///
        /// <param name="item"> . </param>
        

        public static void Debug(object item)
        {
            new ILogger().iLog.Debug(item);
        }

        
        /// <summary>   Debugs. 
        /// Logs the message object as Debug level, including the stacktrace information from Exception parameter.
        /// </summary>
        ///
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///    //some code
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Debug("some message or object,exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        ///                <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="DEBUGLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="DEBUG"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinDebug.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="DEBUG" /> //
        ///        <levelMax value="DEBUG" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="INFO" /> //
        ///      <appender-ref ref="INFOLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        ///             </remarks>
        ///
        /// <param name="item"> . </param>
        /// <param name="exc">  . </param>
        

        public static void Debug(object item, Exception exc)
        {
            new ILogger().iLog.Debug(item, exc);
        }

        
        /// <summary>   Log a message object with the log4net.Core.Level.Fatal level. </summary>
        ///
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///    //some code
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Fatal(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is FATAL enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Fatal level. If this logger
        ///     is FATAL enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Fatal(object,Exception) form instead.  
        ///                <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="FatalLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="FATAL"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinFatal.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="FATAL" /> //
        ///        <levelMax value="FATAL" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="FATAL" /> //
        ///      <appender-ref ref="FatalLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        ///             </remarks>
        ///
        /// <param name="exc">  . </param>
        

        public static void Fatal(Exception exc)
        {
            new ILogger().iLog.Fatal(exc);
        }

        
        /// <summary>   Log a message object with the log4net.Core.Level.Fatal level. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///    WD.DataAccess.Logger.ILogger.ILog.Fatal("some message or object");
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Fatal(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is FATAL enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Fatal level. If this logger
        ///     is FATAL enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Fatal(object,Exception) form instead.
        ///                <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="FatalLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="FATAL"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinFatal.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="FATAL" /> //
        ///        <levelMax value="FATAL" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="FATAL" /> //
        ///      <appender-ref ref="FatalLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// 
        /// </remarks>
        ///
        /// <param name="item"> . </param>
        

        public static void Fatal(object item)
        {
            new ILogger().iLog.Fatal(item);
        }

        
        /// <summary>  Log a message object with the log4net.Core.Level.Fatal level including the
        ///     stack trace of the System.Exception passed as a parameter. 
        ///     </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///   //some code here
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Fatal("some message or object",exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>             
        ///                <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="FatalLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="FATAL"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinFatal.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="FATAL" /> //
        ///        <levelMax value="FATAL" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="FATAL" /> //
        ///      <appender-ref ref="FatalLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// </remarks>
        /// <param name="item"> . </param>
        /// <param name="exc">  . </param>
        

        public static void Fatal(object item, Exception exc)
        {
            new ILogger().iLog.Fatal(item, exc);
        }

        
        /// <summary>  Log an object with the log4net.Core.Level.Warn level.
        /// </summary>
         ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///   //some code here
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Fatal(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is WARN enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Warn level. If this logger
        ///     is WARN enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Warn(object,Exception) form instead.
        /// 
        ///                 <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="WarnLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="WARN"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinWarn.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="FATAL" /> //
        ///        <levelMax value="FATAL" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="WARN" /> //
        ///      <appender-ref ref="WarnLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        ///       </remarks>
        ///
        /// <param name="exc">  . </param>
        

        public static void Warn(Exception exc)
        {
            new ILogger().iLog.Warn(exc);
        }

        
        /// <summary>   Log a message object with the log4net.Core.Level.Warn level.
        /// </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///    WD.DataAccess.Logger.ILogger.ILog.Fatal("some message or object");
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Fatal(exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is WARN enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Warn level. If this logger
        ///     is WARN enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///     WARNING Note that passing an System.Exception to this method will print the
        ///     name of the System.Exception but no stack trace. To print a stack trace use
        ///     the Warn(object,Exception) form instead.
        ///                      <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="WarnLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="WARN"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinWarn.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="FATAL" /> //
        ///        <levelMax value="FATAL" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="WARN" /> //
        ///      <appender-ref ref="WarnLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        ///</remarks>
        ///
        /// <param name="item"> . </param>
        

        public static void Warn(object item)
        {
            new ILogger().iLog.Warn(item);
        }

        
        /// <summary>  Log a message object with the log4net.Core.Level.Warn level including the
        ///     stack trace of the System.Exception passed as a parameter. </summary>
        ///<example>
        /// <code>
        /// class TestClass
        /// {
        /// static void Main()
        /// {
        /// try{
        ///   //some code here
        ///       
        /// }
        /// catch(Exception exc){
        ///     WD.DataAccess.Logger.ILogger.ILog.Warn("some message or object",exc);
        /// }
        /// }
        /// }
        /// </code>
        ///</example>
        /// <remarks>   
        /// This method first checks if this logger is WARN enabled by comparing the
        ///     level of this logger with the log4net.Core.Level.Warn level. If this logger
        ///     is WARN enabled, then it converts the message object (passed as parameter)
        ///     to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        ///     It then proceeds to call all the registered appenders in this logger and
        ///     also higher in the hierarchy depending on the value of the additivity flag.
        ///                 <code>
        ///        <configuration>
        ///  <configSections>
        ///    // Level 1 //
        ///    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/> //
        ///    // Level 2 //
        ///  </configSections>
        ///  <log4net>
        ///    <appender name="WarnLogFileAppender" type="log4net.Appender.RollingFileAppender">
        ///      <threshold value="WARN"></threshold>
        ///      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" /> //
        ///      <file value="E:\Logs\WinWarn.txt" /> //
        ///      <appendToFile value="true" /> //
        ///      <rollingStyle value="date" /> //
        ///      <datePattern value="yyyyMMdd" /> //
        ///      <maxSizeRollBackups value="10" /> //
        ///      <maximumFileSize value="1000KB" /> //
        ///      <layout type="log4net.Layout.PatternLayout">
        ///        <conversionPattern value="%date [%thread] %5level %logger %message%newline  %Exception" /> //
        ///      </layout>
        ///  <filter type="log4net.Filter.LevelRangeFilter">
        ///        <levelMin value="FATAL" /> //
        ///        <levelMax value="FATAL" /> //
        ///  </filter>
        ///    </appender>
        ///    <root>
        ///      <level value="WARN" /> //
        ///      <appender-ref ref="WarnLogFileAppender" /> //
        ///    </root>
        ///  </log4net>
        /// </configuration>
        ///       </code>
        /// </remarks>  
        ///
        /// <param name="item"> . </param>
        /// <param name="exc">  . </param>
        

        public static void Warn(object item, Exception exc)
        {
            new ILogger().iLog.Warn(item, exc);
        }
    }
}


