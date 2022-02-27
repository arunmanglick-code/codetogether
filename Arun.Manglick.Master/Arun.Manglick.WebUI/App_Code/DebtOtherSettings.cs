using System;
using System.Data;

namespace Arun.Manglick.UI
{
    public class DebtOtherSettings 
    {

        #region Private Variables

        //Deal Settings
        private string mIncludeAccountsCreditLine;          // (Y)es, (N)o
        private string mIncludeAccountsOpen;                // (Y)es, (N)o
        private string mIncludeAccountsChildSupport;        // (Y)es, (N)o
        private string mIncludeAccountsUnknown;             // (Y)es, (N)o

        private string mCreditLineType;                     // use (M)onthly Payment, (C)alculate
        private string mOpenType;                           // use (M)onthly Payment, (C)alculate
        private string mChildSupportType;                   // use (M)onthly Payment, (C)alculate
        private string mUnknownType;                        // use (M)onthly Payment, (C)alculate

        private double mCreditLinePercent;
        private double mOpenPercent;
        private double mChildSupportPercent;
        private double mUnknownPercent;
        
        //Audit Trail
        private DataTable mAuditTrailLog;

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiate Debt Other Settings ‘Data Transfer Object’ class
        /// </summary>
        /// <returns>Void</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public DebtOtherSettings()
        {
            mIncludeAccountsCreditLine = string.Empty;
            mIncludeAccountsOpen = string.Empty;
            mIncludeAccountsChildSupport = string.Empty;
            mIncludeAccountsUnknown = string.Empty;

            mCreditLineType = string.Empty;
            mOpenType = string.Empty;
            mChildSupportType = string.Empty;
            mUnknownType = string.Empty;

            mAuditTrailLog = new DataTable();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Get or Set value for Include Accounts Credit Line
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string IncludeAccountsCreditLine
        {
            get
            {
                return mIncludeAccountsCreditLine;
            }
            set
            {
                mIncludeAccountsCreditLine = value;
            }
        }

        /// <summary>
        /// Get or Set value for Include Accounts Open
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string IncludeAccountsOpen
        {
            get
            {
                return mIncludeAccountsOpen;
            }
            set
            {
                mIncludeAccountsOpen = value;
            }
        }

        /// <summary>
        /// Get or Set value for Include Accounts Child Support
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string IncludeAccountsChildSupport
        {
            get
            {
                return mIncludeAccountsChildSupport;
            }
            set
            {
                mIncludeAccountsChildSupport = value;
            }
        }

        /// <summary>
        /// Get or Set value for Include Accounts Unknown
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string IncludeAccountsUnknown
        {
            get
            {
                return mIncludeAccountsUnknown;
            }
            set
            {
                mIncludeAccountsUnknown = value;
            }
        }

        /// <summary>
        /// Get or Set value for Credit Line Type
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string CreditLineType
        {
            get
            {
                return mCreditLineType;
            }
            set
            {
                mCreditLineType = value;
            }
        }

        /// <summary>
        /// Get or Set value for Open Type
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string OpenType
        {
            get
            {
                return mOpenType;
            }
            set
            {
                mOpenType = value;
            }
        }

        /// <summary>
        /// Get or Set value for Child Support Type
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string ChildSupportType
        {
            get
            {
                return mChildSupportType;
            }
            set
            {
                mChildSupportType = value;
            }
        }

        /// <summary>
        /// Get or Set value for Unknown Type
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public string UnknownType
        {
            get
            {
                return mUnknownType;
            }
            set
            {
                mUnknownType = value;
            }
        }

        /// <summary>
        /// Get or Set value for Credit Line Percent
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public double CreditLinePercent
        {
            get
            {
                return mCreditLinePercent;
            }
            set
            {
                mCreditLinePercent = value;
            }
        }

        /// <summary>
        /// Get or Set value for Open Percent
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public double OpenPercent
        {
            get
            {
                return mOpenPercent;
            }
            set
            {
                mOpenPercent = value;
            }
        }

        /// <summary>
        /// Get or Set value for Child Support Percent
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public double ChildSupportPercent
        {
            get
            {
                return mChildSupportPercent;
            }
            set
            {
                mChildSupportPercent = value;
            }
        }

        /// <summary>
        /// Get or Set value for Unknown Percent
        /// </summary>
        /// <returns>String</returns>
        /// <history created="JAV"></history>
        /// <history date="Nov 03, 2008"></history>
        public double UnknownPercent
        {
            get
            {
                return mUnknownPercent;
            }
            set
            {
                mUnknownPercent = value;
            }
        }

        /// <summary>
        /// Get or Set audit trail log
        /// </summary>
        /// <returns>DataTable</returns>
        /// <history created="JAV"></history>
        /// <history date="Aug 19, 2008"></history>
        public DataTable AuditTrailLog
        {
            get
            {
                return mAuditTrailLog;
            }
            set
            {
                mAuditTrailLog = value;
            }
        }

        #endregion

    }
}
