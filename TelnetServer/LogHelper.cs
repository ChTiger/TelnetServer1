using System;
using log4net;
using System.IO;

namespace TelnetServer
{
    public static class LogHelper
    {
        private const string SError = "Error";
        private const string SDebug = "Debug";
        private const string DefaultName = "Business";

        static LogHelper()
        {
            //#if DEBUG   
            //            var path = "E:\\project\\TelnetServer\\TelnetServer\\log4net.config";
            //#endif
            //#if !DEBUG   
            //            string path = AppDomain.CurrentDomain.BaseDirectory + @"\log4net.config";
            //#endif
            string path = "D:\\project\\log4net.config";
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
        }

        public static ILog GetLog(string logName)
        {
            var log = LogManager.GetLogger(logName);
            return log;
        }

        public static void Debug(string message)
        {
            var log = LogManager.GetLogger(SDebug);
            if (log.IsDebugEnabled)
                log.Debug(message);
        }

        public static void Debug(string message, Exception ex)
        {
            var log = LogManager.GetLogger(SDebug);
            if (log.IsDebugEnabled)
                log.Debug(message, ex);
        }

        public static void Error(string message)
        {
            var log = log4net.LogManager.GetLogger(SError);
            if (log.IsErrorEnabled)
                log.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            var log = log4net.LogManager.GetLogger(SError);
            if (log.IsErrorEnabled)
                log.Error(message, ex);
        }

        public static void Fatal(string message)
        {
            var log = log4net.LogManager.GetLogger(DefaultName);
            if (log.IsFatalEnabled)
                log.Fatal(message);
        }

        public static void Info(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(DefaultName);
            if (log.IsInfoEnabled)
                log.Info(message);
        }

        public static void Warn(string message)
        {
            var log = log4net.LogManager.GetLogger(DefaultName);
            if (log.IsWarnEnabled)
                log.Warn(message);
        }
    }
}