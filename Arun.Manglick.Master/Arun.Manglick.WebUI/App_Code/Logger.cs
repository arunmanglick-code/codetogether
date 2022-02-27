using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Web;
using System.Configuration;
using System.IO;


namespace Arun.Manglick.UI
{
    public class Logger
    {
        #region Local Variables

        private static Logger logger = new Logger();
        private ArrayList logProducerList;  // Buffer used by the producers to store the log messages
        private ArrayList loggerConsumerList; // Buffer used by the consumer logger thread
        
        private string logFilePath;
        private volatile bool ApplicationEnd; // Boolean value indicating whether the application end event has got fired
        private int GlobalLogLevel;        // Log level set in the configuration file for the Logger
        private StreamWriter streamWriter; // Log messages will be written to the file stream


        private const int DefaultMaximumLogSizeInKB = 1024000;   // Default maximum log file size in kilo bytes (1 MB)
        private const int DefaultBufferSize = 20;
        private int BufferSizeThreshold = 0;
        private int MaximumLogFileSize = 0;        // Maximum size of log file
        private string formatMessage = string.Empty;

        private EventWaitHandle wh = new AutoResetEvent(false);


        #endregion

        #region SingleTon Approach

        private Logger()
        {
            logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"Log\\Logs.txt";
            logProducerList = new ArrayList();
            loggerConsumerList = new ArrayList();

            try
            {
                GlobalLogLevel = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LogLevel"));
            }
            catch (Exception exp)
            {
                GlobalLogLevel = Convert.ToInt32(LogLevel.INFO);
            }

            try
            {
                MaximumLogFileSize = Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxLogFileSizeInKB"));
            }
            catch (Exception exp)
            {
                MaximumLogFileSize = DefaultMaximumLogSizeInKB;
            }
            try
            {
                BufferSizeThreshold = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LogBufferSizeThreshold"));
            }
            catch (Exception exp)
            {
                BufferSizeThreshold = DefaultBufferSize;
            }

            OpenLoggerFileStream();
            CreateThread(); // Create the logger thread, which will be responsible for writing log messages from the buffer to the file
        }

        public static Logger CreateInstance()
        {
            return logger;
        }
               
        public void SetApplicationEnd()
        {
            // Method to set a flag indicating that the application has been terminated or has ended. 
            // This is an indication that the Logger Thread should be terminated.
            ApplicationEnd = true;
        }

        #endregion

        #region Private Methods

        #region Background Thread Methods

        private void OpenLoggerFileStream()
        {
            try
            {
                FileStream stream;

                if (!File.Exists(logFilePath))
                {
                    stream = File.Create(logFilePath);
                }
                else
                {
                    stream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
                }

                streamWriter = new StreamWriter(stream);
            }
            catch (Exception ex)
            {
                // This exception can occur when there is no access to the log folder.
            }
        }

        private void CreateThread()
        {
            Thread thrd = new Thread(new ThreadStart(LoggerRun));
            thrd.Name = "Logger Thread";
            thrd.Priority = ThreadPriority.BelowNormal;
            try
            {
                thrd.Start();
            }
            catch (Exception exp)
            {}
        }

        private void LoggerRun()
        {
            LogData logData;
            
            while (!ApplicationEnd)   // Loop till the application is alive
            {
                int loggerConsumerListCount = 0;
                lock (loggerConsumerList.SyncRoot)
                {
                    loggerConsumerListCount = loggerConsumerList.Count;
                }

                // Log the messages until the buffer is not empty
                while (loggerConsumerListCount > 0)
                {
                    lock (loggerConsumerList.SyncRoot)
                    {
                        logData = loggerConsumerList[0] as LogData;
                    }

                    // Write the message to the file
                    PostLog(logData);

                    lock (loggerConsumerList.SyncRoot)
                    {
                        loggerConsumerList.RemoveAt(0);
                    }
                    loggerConsumerListCount--;
                }
                                
                wh.WaitOne();// Stop the Thread & Wait till the log message buffer is full
            }

            // Application End Event has fired, close the thread
            CleanUp();
        }

        private void PostLog(LogData logData)
        {
            StringBuilder logEntry = new StringBuilder(500);

            logEntry.Append("DateTime: ").Append(logData.LogDateTime);
            logEntry.Append(Environment.NewLine);

            switch (logData.MessageLevel)
            {
                case LogLevel.DEBUG:
                    logEntry.Append("DEBUGGING:- ").Append(logData.LogMessage);
                    break;
                case LogLevel.INFO:
                    logEntry.Append("INFO:- ").Append(logData.LogMessage);
                    break;
                case LogLevel.WARNING:
                    logEntry.Append("WARNING:- ").Append(logData.LogMessage);
                    break;
                case LogLevel.EXCEPTION:
                    logEntry.Append("EXCEPTION:- ").Append(logData.LogMessage);
                    break;
            }
            logEntry.Append(Environment.NewLine);

            #region Write in File

            try
            {
                long fileSizeInKB = streamWriter.BaseStream.Length / 1024;

                // Write to the log file
                if (fileSizeInKB < MaximumLogFileSize)
                {
                    streamWriter.Write(logEntry.ToString());
                    streamWriter.Flush();
                }
                else
                {
                    #region Close & Archive the old file
                    // Log file size has exceeded the maximum allowed file size
                    try
                    {
                        // Close & Archive the old file
                        streamWriter.BaseStream.Flush();
                        streamWriter.BaseStream.Close();
                        File.Move(logFilePath, logFilePath.Substring(0, logFilePath.Length - 4) + DateTime.Now.ToBinary() + ".txt");
                    }
                    catch (Exception ex)
                    {
                        // Ignore these exceptions
                    } 
                    #endregion

                    try
                    {
                        // Archive the old file
                        OpenLoggerFileStream(); // Open a new file
                        streamWriter.Write(logEntry.ToString());
                        streamWriter.Flush();
                    }
                    catch (Exception e)
                    {}
                }
            }
            catch (Exception ex)
            {} 

            #endregion
        }

        #endregion

        #region Other Methods

        private void AddMessageToBufferList(LogData logData)
        {
            if (!ApplicationEnd)
            {
                lock (logProducerList.SyncRoot)
                {
                    logProducerList.Add(logData);

                    if (logProducerList.Count >= BufferSizeThreshold)
                    {
                        lock (loggerConsumerList.SyncRoot)
                        {
                            loggerConsumerList.AddRange(logProducerList.GetRange(0, logProducerList.Count));
                        }
                        logProducerList.RemoveRange(0, logProducerList.Count);

                        wh.Set(); // Wake up the Thread - This will fire 'LoggerRun()' 
                    }
                }
            }
        }
                
        private void CleanUp()
        {
            // Flush the contents of the Logger message buffer. Since it is possible that the consumer thread has not run at all
            // Hence there could be messages in the buffer. Log the messages until the buffer is not empty
            int noOfLogMessages = loggerConsumerList.Count;
            LogData logData;
            for (int i = 0; i < noOfLogMessages; i++)
            {
                logData = loggerConsumerList[i] as LogData;
                PostLog(logData);
            }
            // Clear the log message buffer
            loggerConsumerList.Clear();

            noOfLogMessages = logProducerList.Count;
            for (int i = 0; i < noOfLogMessages; i++)
            {
                logData = logProducerList[i] as LogData;
                // Write the message to the file
                PostLog(logData);
            }
            // Clear the log message buffer
            logProducerList.Clear();

            // Write the message that the logger thread has terminated
            // This message is written at debug level
            logData = new LogData("** Logger Thread has closed", LogLevel.DEBUG, DateTime.Now.ToString());
            PostLog(logData);

            // Close the FileStream
            if (streamWriter != null)
            {
                try
                {
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                catch (Exception ex)
                {
                    // Ignore these exceptions
                }
            }
        }

        #endregion

        #region Format Methods

        private string FormatMessage(string message)
        {
            if (message != null && message.Length > 0)
                message = "Message: " + message + Environment.NewLine;
            return message;
        }

        private string FormatMessage(string className, string methodName, string message)
        {
            if (message != null && message.Length > 0)
            {
                StringBuilder msgLog = new StringBuilder(300);
                msgLog.Append("Class Name: ").Append(className);
                msgLog.Append(Environment.NewLine);
                msgLog.Append("Method Name: ").Append(methodName);
                msgLog.Append(Environment.NewLine);
                msgLog.Append("Message: ").Append(message);
                msgLog.Append(Environment.NewLine);
                return msgLog.ToString();
            }
            return null;
        }

        private string FormatMessage(string className, string methodName, string message, Object[] argumentList)
        {
            if (message != null && message.Length > 0)
            {
                try
                {
                    message = String.Format(message, argumentList);
                }
                catch (Exception exp)
                {
                    // Only catching the exception so that it is not propagated
                    // Ignore the exception
                }
                StringBuilder msgLog = new StringBuilder(300);
                msgLog.Append("Class Name: ").Append(className);
                msgLog.Append(Environment.NewLine);
                msgLog.Append("Method Name: ").Append(methodName);
                msgLog.Append(Environment.NewLine);
                msgLog.Append("Message: ").Append(message);
                msgLog.Append(Environment.NewLine);
                return msgLog.ToString();
            }

            return null;
        }

        private string FormatExceptionMessage(string exMessage, string stackTrace)
        {
            if (exMessage != null && exMessage.Length > 0)
                exMessage = "Message: " + exMessage + Environment.NewLine;
            else
                exMessage = "";
            if (stackTrace != null && stackTrace.Length > 0)
                exMessage += "Stack Trace: " + stackTrace + Environment.NewLine;
            return exMessage;
        }

        private string FormatExceptionMessage(string className, string methodName, string exMessage, string stackTrace)
        {
            StringBuilder msgLog = new StringBuilder(1000);

            msgLog.Append("Class Name: ").Append(className); ;
            msgLog.Append(Environment.NewLine);
            msgLog.Append("Method Name: ").Append(methodName);
            msgLog.Append(Environment.NewLine);
            if (exMessage != null && exMessage.Length > 0)
                msgLog.Append("Message: ").Append(exMessage);
            msgLog.Append(Environment.NewLine);
            if (stackTrace != null && stackTrace.Length > 0)
                msgLog.Append("Stack Trace: ").Append(stackTrace);
            msgLog.Append(Environment.NewLine);

            return msgLog.ToString();
        }

        #endregion

        #endregion

        #region Public Methods

        public void WriteLog(string className, string methodName, string messageKey, LogLevel logLevel, DateTime dateTime)
        {
            int localLogLevel = Convert.ToInt32(logLevel);
            LogData logData = null;
            if (GlobalLogLevel <= localLogLevel)
            {
                try
                {
                    formatMessage = FormatMessage(className, methodName, Resources.Message.ResourceManager.GetString(messageKey).ToString());
                    logData = new LogData(formatMessage, logLevel, dateTime.ToString());
                    AddMessageToBufferList(logData);
                }
                catch
                {
                }
            }
        }
        
        public void WriteHardLog(string message, LogLevel logLevel, DateTime dateTime)
        {
            //// Log level of the message
            //int localLogLevel = Convert.ToInt32(logLevel);
            //// Log the message only if the log level of the message is >= the global setting for the log level
            //if (GlobalLogLevel <= localLogLevel)
            //{
            //    try
            //    {

            //        AddMessageToBufferList(new LogData(FormatMessage(message), logLevel, dateTime));
            //    }
            //    catch
            //    {
            //        // Catching the exception so that it is not propagated further
            //    }
            //}
        }
        public void WriteHardLog(string className, string methodName, string message, LogLevel logLevel, DateTime dateTime)
        {
            //// Log level of the message
            //int localLogLevel = Convert.ToInt32(logLevel);
            //// Log the message only if the log level of the message is >= the global setting for the log level
            //if (GlobalLogLevel <= localLogLevel)
            //{
            //    try
            //    {

            //        AddMessageToBufferList(new LogData(FormatMessage(className, methodName, message), logLevel, dateTime));
            //    }
            //    catch
            //    {
            //        // Catching the exception so that it is not propagated further
            //    }
            //}
        }

        public void WriteLog(string className, string methodName, string messageKey, Object[] argumentList, LogLevel logLevel, DateTime dateTime)
        {
            //// Log level of the message
            //int localLogLevel = Convert.ToInt32(logLevel);
            //// Log the message only if the log level of the message is >= the global setting for the log level
            //if (GlobalLogLevel <= localLogLevel)
            //{
            //    try
            //    {

            //        AddMessageToBufferList(new LogData(FormatMessage(className, methodName, Resources.Message.ResourceManager.GetString(messageKey), argumentList), logLevel, dateTime));
            //    }
            //    catch
            //    {
            //        // Catching the exception so that it is not propagated further
            //    }
            //}
        }
        public void WriteLog(string messageKey, LogLevel logLevel, DateTime dateTime)
        {
            //// Log level of the message
            //int localLogLevel = Convert.ToInt32(logLevel);
            //// Log the message only if the log level of the message is >= the global setting for the log level
            //if (GlobalLogLevel <= localLogLevel)
            //{
            //    try
            //    {

            //        AddMessageToBufferList(new LogData(FormatMessage(Resources.Message.ResourceManager.GetString(messageKey)), logLevel, dateTime));
            //    }
            //    catch
            //    {
            //        // Catching the exception so that it is not propagated further
            //    }
            //}
        }

        public void WriteLog(Exception ex, LogLevel logLevel, DateTime dateTime)
        {
            //// Log level of the message
            //int localLogLevel = Convert.ToInt32(logLevel);
            //// Log the message only if the log level of the message is >= the global setting for the log level
            //if (GlobalLogLevel <= localLogLevel)
            //{
            //    try
            //    {
            //        if (ex is BaseTyreLinkException)
            //        {
            //            BaseTyreLinkException baseExp = ex as BaseTyreLinkException;
            //            if (!baseExp.IsLogged)
            //            {

            //                AddMessageToBufferList(new LogData(FormatExceptionMessage(Resources.Exception.ResourceManager.GetString(baseExp.ExceptionCode), baseExp.StackTrace), logLevel, dateTime));

            //                // Mark the Exception as logged.
            //                baseExp.IsLogged = true;

            //            }
            //        }
            //        else
            //        {

            //            AddMessageToBufferList(new LogData(FormatExceptionMessage(ex.Message, ex.StackTrace), logLevel, dateTime));
            //        }
            //    }
            //    catch
            //    {
            //        // Catching the exception so that it is not propagated further
            //    }
            //}
        }
        public void WriteLog(string className, string methodName, Exception ex, LogLevel logLevel, DateTime dateTime)
        {
            //// Log level of the message
            //int localLogLevel = Convert.ToInt32(logLevel);
            //// Log the message only if the log level of the message is >= the global setting for the log level
            //if (GlobalLogLevel <= localLogLevel)
            //{
            //    try
            //    {
            //        if (ex is BaseTyreLinkException)
            //        {
            //            BaseTyreLinkException baseExp = ex as BaseTyreLinkException;
            //            if (!baseExp.IsLogged)
            //            {

            //                AddMessageToBufferList(new LogData(FormatExceptionMessage(className, methodName, Resources.Exception.ResourceManager.GetString(baseExp.ExceptionCode), baseExp.StackTrace), logLevel, dateTime));

            //                // Mark the Exception as logged.
            //                baseExp.IsLogged = true;

            //            }
            //        }
            //        else
            //        {

            //            AddMessageToBufferList(new LogData(FormatExceptionMessage(className, methodName, ex.Message, ex.StackTrace), logLevel, dateTime));
            //        }
            //    }
            //    catch
            //    {
            //        // Catching the exception so that it is not propagated further
            //    }
            //}

        }

        #endregion
    }
}
