using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Data transfer objet for Vehicle Valuation page
    /// </summary>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 04, 2007"></history>
    public class EmployeeCaption
    {
        #region Private Variables

        private string mEmployeeId;
        private string mFirstName;
        private string mLastName;
        private string mAge;
        private StringDictionary mProfile;

        #endregion

        #region Constructor

        /// <summary>
        /// Get or Set employee id
        /// </summary>
        /// <returns>int</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public EmployeeCaption()
        {
            mEmployeeId = "";
            mFirstName = "";
            mLastName = "";
            mProfile = new StringDictionary();
            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set employee id
        /// </summary>
        /// <returns>int</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public string Id
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
        /// Get or Set employee age
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public string Age
        {
            get
            {
                return mAge;
            }
            set
            {
                mAge = value;
            }
        }

        /// <summary>
        /// Get or Set employee profile
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public StringDictionary Profile
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

        

        #endregion
    }
}
