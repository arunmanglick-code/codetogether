using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Web;
using System.Configuration;
using System.IO;

namespace Arun.Manglick.UI
{
    public class LoggerOld
    {
        #region Local Variables

        private static LoggerOld logger;        
        private static ArrayList logList = null;
        private string formatMessage = string.Empty;
        
        private string logFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\" + ConfigurationManager.AppSettings.Get("LogPath").ToString();
        #endregion

        #region SingleTon Approach

        private LoggerOld()
        {
        }
        public static LoggerOld CreateInstance()
        {
            if (logger == null)
            {
                logger = new LoggerOld();
                logList = new ArrayList();
                logList = ArrayList.Synchronized(logList);

                logger.CreateThread();
            }
            return logger;
        }

        #endregion

        #region Private Methods

        private void CreateThread()
        {
            Thread thrd = new Thread(new ThreadStart(Run));
            thrd.Name = "Logger Thread";
            thrd.Priority = ThreadPriority.Normal;
            thrd.Start();
        }

        private void Run()
        {
            while (true)
            {
                PostLog();
            }
        }       
                
        private string FormatMessage(string message)
        {
            StringBuilder msgLog = new StringBuilder();
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "Message: " + message);
            msgLog.Append(Environment.NewLine);

            return msgLog.ToString();
        }
        private string FormatMessage(string className, string methodName, string message)
        {
            StringBuilder msgLog = new StringBuilder();
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "ClassName: " + className);
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "MethodName: " + methodName);
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "Message: " + message);
            msgLog.Append(Environment.NewLine);

            return msgLog.ToString();
        }

        private string FormatExceptionMessage(string exMessage, string stackTrace)
        {
            StringBuilder msgLog = new StringBuilder();

            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "Message: " + exMessage + "; ");
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "StackTrace: " + stackTrace + "; ");
            msgLog.Append(Environment.NewLine);

            return msgLog.ToString();
        }
        private string FormatExceptionMessage(string className, string methodName, string exMessage, string stackTrace)
        {
            StringBuilder msgLog = new StringBuilder();

            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "ClassName: " + className + "; ");
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "MethodName: " + methodName);
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "Message: " + exMessage + "; ");
            msgLog.Append(Environment.NewLine);
            msgLog.Append("\t" + "StackTrace: " + stackTrace + "; ");
            msgLog.Append(Environment.NewLine);

            return msgLog.ToString();
        }

        private void PostLog()
        {
            StringBuilder logEntry = new StringBuilder();            
            LogData logData;
            int globalLogLevel;
            int localLogLevel;

            if (logList.Count > 0)
            {
                for (int i = 0; i < logList.Count; i++)
                {
                    logData = logList[i] as LogData;
                    globalLogLevel = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LogLevel"));
                    localLogLevel = Convert.ToInt32(logData.MessageLevel);
                    if (globalLogLevel <= localLogLevel)
                    {
                        logEntry.Append("DateTime: " + logData.LogDateTime);
                        logEntry.Append(Environment.NewLine);

                        switch (logData.MessageLevel)
                        {

                            case LogLevel.DEBUG:
                                logEntry.Append("DEBUGGING:- " + logData.LogMessage);
                                logEntry.Append(Environment.NewLine);
                                logEntry.Append(Environment.NewLine);
                                break;
                            case LogLevel.INFO:
                                logEntry.Append("MESSAGE:- ");
                                logEntry.Append(logData.LogMessage);
                                logEntry.Append(Environment.NewLine);
                                logEntry.Append(Environment.NewLine);
                                break;
                            case LogLevel.WARNING:
                                logEntry.Append("WARNING:- " + logData.LogMessage);
                                logEntry.Append(Environment.NewLine);
                                logEntry.Append(Environment.NewLine);
                                break;
                            case LogLevel.EXCEPTION:
                                logEntry.Append("EXCEPTION:- " + logData.LogMessage);
                                logEntry.Append(Environment.NewLine);
                                logEntry.Append(Environment.NewLine);
                                break;
                        }

                    }
                         logList.Remove(logList[i--]);
               }

               
               FileStream strm = null;
               StreamWriter strmWrite = null;
               try
               {
                   strm = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
                   strmWrite = new StreamWriter(strm);
                   strmWrite.Write(logEntry.ToString());
               }
               catch (Exception ex)
               {
               }
               finally
               {
                   strmWrite.Close();
               }
            }
        }

        #endregion

        #region Public Methods

        public void WriteHardLog(string message, LogLevel logLevel, string dateTime)
        {
            try
            {
                formatMessage = FormatMessage(message);
                logList.Add(new LogData(formatMessage, logLevel, dateTime));
            }
            catch
            {
            }
            
        }
        public void WriteHardLog(string className, string methodName, string message, LogLevel logLevel, string dateTime)
        {
            try
            {
                formatMessage = FormatMessage(className, methodName, message);
                logList.Add(new LogData(formatMessage, logLevel, dateTime));
            }
            catch 
            {
            }
        }

        public void WriteLog(string messageKey, LogLevel logLevel, string dateTime)
        {
            try
            {
                formatMessage = FormatMessage(Resources.Message.ResourceManager.GetString(messageKey).ToString());
                logList.Add(new LogData(formatMessage, logLevel, dateTime));
            }
            catch 
            {
            }
        }
        public void WriteLog(string className, string methodName, string messageKey, LogLevel logLevel, string dateTime)
        {
            try
            {
                formatMessage = FormatMessage(className, methodName, Resources.Message.ResourceManager.GetString(messageKey).ToString());
                logList.Add(new LogData(formatMessage, logLevel, dateTime));
            }
            catch
            {
            }
        }

        public void WriteLog(Exception ex, LogLevel logLevel, string dateTime)
        {
            //try
            //{
            //    if (ex is Exceptions.BaseTyreLinkException)
            //    {
            //        if (!(ex as Exceptions.BaseTyreLinkException).IsLogged)
            //        {
            //            formatMessage = FormatExceptionMessage(Resources.Exception.ResourceManager.GetString((ex as Exceptions.BaseTyreLinkException).ExceptionCode).ToString(), ex.StackTrace);
            //            logList.Add(new LogData(formatMessage, logLevel, dateTime));

            //            // Mark the Exception as logged.
            //            (ex as Exceptions.BaseTyreLinkException).IsLogged = true;
            //        }
            //    }
            //    else if (ex is SystemException)
            //    {
            //        formatMessage = FormatExceptionMessage(ex.Message, ex.StackTrace);
            //        logList.Add(new LogData(formatMessage, logLevel, dateTime));
            //    }
            //}
            //catch 
            //{
            //}
        }
        public void WriteLog(string className, string methodName, Exception ex, LogLevel logLevel, string dateTime)
        {
            //try
            //{
            //    if (ex is Exceptions.BaseTyreLinkException)
            //    {
            //        if (!(ex as Exceptions.BaseTyreLinkException).IsLogged)
            //        {
            //            formatMessage = FormatExceptionMessage(className, methodName, Resources.Exception.ResourceManager.GetString((ex as Exceptions.BaseTyreLinkException).ExceptionCode).ToString(), ex.StackTrace);
            //            logList.Add(new LogData(formatMessage, logLevel, dateTime));

            //            // Mark the Exception as logged.
            //            (ex as Exceptions.BaseTyreLinkException).IsLogged = true;
            //        }
            //    }
            //    else if (ex is System.Exception)
            //    {
            //        formatMessage = FormatExceptionMessage(className, methodName, ex.Message, ex.StackTrace);
            //        logList.Add(new LogData(formatMessage, logLevel, dateTime));

            //    }
            //}
            //catch 
            //{
            //}

        }
        
        #endregion
    }
}
