using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace Arun.Manglick.UI
{
    #region AuditAction Class

    /// <summary>
    /// Summary description for AuditTrail
    /// This class is used as a Row Object in the AuditTrail Datatable
    /// </summary>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2008"></history>
    public class AuditAction
    {
        #region Private Variables

        private string mPage;
        private int mActionNumber;
        private DateTime mDateTime;
        private string mUser;
        private string mSourceLabel;
        private string mAction;
        private int mPosition;
        private string mColumn;
        private string mOldValue;
        private string mNewValue;

        #endregion

        #region Constructor
        public AuditAction()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Get or Set value for ActionNumber
        /// </summary>
        /// <returns>int</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public int ActionNumber
        {
            get { return mActionNumber; }
            set { mActionNumber = value; }
        }

        /// <summary>
        /// Get or Set value for DateTime
        /// </summary>
        /// <returns>DateTime</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public DateTime DateTime
        {
            get { return mDateTime; }
            set { mDateTime = value; }
        }

        /// <summary>
        /// Get or Set value for User
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public string User
        {
            get { return mUser; }
            set { mUser = value; }
        }

        /// <summary>
        /// Get or Set value for SourceLabel
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public string SourceLabel
        {
            get { return mSourceLabel; }
            set { mSourceLabel = value; }
        }

        /// <summary>
        /// Get or Set value for Action
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public string Action
        {
            get { return mAction; }
            set { mAction = value; }
        }

        /// <summary>
        /// Get or Set value for Position
        /// </summary>
        /// <returns>int</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public int Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        /// <summary>
        /// Get or Set value for Column
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public string Column
        {
            get { return mColumn; }
            set { mColumn = value; }
        }

        /// <summary>
        /// Get or Set value for NewValue
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public string NewValue
        {
            get { return mNewValue; }
            set { mNewValue = value; }
        }

        /// <summary>
        /// Get or Set value for OldValue
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2008"></history>
        public string OldValue
        {
            get { return mOldValue; }
            set { mOldValue = value; }
        }
        
        /// <summary>
        /// Get or Set value for Page
        /// </summary>
        public string Page
        {
            get { return mPage; }
            set { mPage = value; }
        }

        #endregion
    }

    #endregion
}
