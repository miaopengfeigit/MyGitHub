using NLog;
using System;

namespace Common
{
    public class NLog
    {
        static private Logger log = LogManager.GetCurrentClassLogger();
        static NLog()
        {
            //XmlConfigurator.Configure(new FileInfo(string.Format("{0}configs\\log4net.xml", AppDomain.CurrentDomain.BaseDirectory)));
        }

        public static void Error(string logName, string msg, Exception ex)
        {
            //var log = LogManager.GetLogger(logName);
            if (log != null && log.IsErrorEnabled)
                log.ErrorException(msg, ex);
        }

        public static void Warn(string logName, string msg, params object[] args)
        {
            //var log = LogManager.GetLogger(logName);
            if (log != null && log.IsWarnEnabled)
                log.Warn(msg, args);
        }

        public static void Info(string logName, string msg, params object[] args)
        {
            //var log = LogManager.GetLogger(logName);
            if (log != null && log.IsInfoEnabled)
                log.Info(msg, args);
        }

        public static void Debug(string logName, string msg, params object[] args)
        {
            //var log = LogManager.GetLogger(logName);
            if (log != null && log.IsDebugEnabled)
                log.Debug(msg, args);
        }

        public static void Fatal(string logName, string msg, Exception ex)
        {
            //var log = LogManager.GetLogger(logName);
            if (log != null && log.IsFatalEnabled)
                log.FatalException(msg, ex);
        }
    }
}
