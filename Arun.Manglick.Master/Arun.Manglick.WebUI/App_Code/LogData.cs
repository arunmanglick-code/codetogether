using System;
using System.Collections.Generic;
using System.Text;

namespace Arun.Manglick.UI
{
    public class LogData
    {
        #region Member Variables

        private string m_LogMessage;
        private LogLevel m_LogLevel;
        private string m_LogDateTime;

        #endregion

        #region Properties
               
        public string LogMessage
        {
            get { return m_LogMessage; }
            set { m_LogMessage = value; }
        }

        public LogLevel MessageLevel
        {
            get { return m_LogLevel; }
            set { m_LogLevel = value; }
        }

        public string LogDateTime
        {
            get { return m_LogDateTime; }
            set { m_LogDateTime = value; }
        }

        #endregion

        #region Constructor

        public LogData(string logMessage, LogLevel logLevel, string logDateTime)
        {
            this.m_LogMessage = logMessage;
            this.m_LogLevel = logLevel;
            this.m_LogDateTime = logDateTime;
        }

        #endregion
    }
}
