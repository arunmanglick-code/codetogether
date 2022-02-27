using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Arun.Manglick.BL
{    
    public class Employee 
    {
        #region Private Variables

        private int mEmployeeId;
        private string mFirstName;
        private string mLastName;
        private DataTable mProfile;
        private DataTable mAuditTrailLog;

        #endregion

        #region Constructor

        /// <summary>
        /// Get or Set employee id
        /// </summary>
        /// <returns>int</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public Employee()
        {
            mEmployeeId = 0;
            mFirstName = "";
            mLastName = "";
            mProfile = new DataTable();
            mAuditTrailLog = new DataTable();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set employee id
        /// </summary>
        /// <returns>int</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public int Id
        {
            get
            {
                return mEmployeeId;
            }
            set 
            {
                mEmployeeId = value;
            }
        }

        /// <summary>
        /// Get or Set employee first name
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public string FirstName
        {
            get
            {
                return mFirstName;
            }
            set
            {
                mFirstName = value;
            }
        }

        /// <summary>
        /// Get or Set employee last name
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public string LastName
        {
            get
            {
                return mLastName;
            }
            set
            {
                mLastName = value;
            }
        }

        /// <summary>
        /// Get or Set employee profile
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public DataTable Profile
        {
            get
            {
                return mProfile;
            }
            set
            {
                mProfile = value;
            }
        }

        /// <summary>
        /// Get or Set audit trail log
        /// </summary>
        /// <returns>DataTable</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 05, 2007"></history>
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
