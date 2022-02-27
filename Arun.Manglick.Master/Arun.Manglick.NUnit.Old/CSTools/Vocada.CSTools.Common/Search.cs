/******************************File History***************************
 * File Name        : Search.cs
 * Author           : Prerak Shah.
 * Created Date     : 26-02-2007
 * Purpose          : This Class will provide properties of Message Search Criteria.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * -------------------------------------------------------------------- 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    public class Search
    {
        #region private Members
        private int groupType;
        private int groupID;
        private int messageStatus;
        private string ocName;
        private string rcName;
        private string nurseName;
        private string unitName;
        private string findingName;
        private string mrn;
        private string accession;
        private string dob;
        private string fromDate;
        private string toDate;
        private string ctName;
        
        #endregion

        #region Property
        /// <summary>
        /// Get group Type
        /// </summary>
        public int GroupType
        {
            get
            {
                return groupType;
            }
            set
            {
                groupType = value;
            }
        }
        /// <summary>
        /// Get group ID 
        /// </summary>
        public int GroupID
        {
            get
            {
                return groupID;
            }
            set
            {
                groupID = value;
            }
        }
        /// <summary>
        /// Get Message Status
        /// </summary>
        public int MessageStatus
        {
            get
            {
                return messageStatus;
            }
            set
            {
                messageStatus = value;
            }
        }
        /// <summary>
        /// Get Ordering Clinician Name
        /// </summary>
        public string OCName
        {
            get
            {
                return ocName;
            }
            set
            {
                ocName = value;
            }
        }
        /// <summary>
        /// Get Reporting Clinician Name 
        /// </summary>
        public string RCName
        {
            get
            {
                return rcName;
            }
            set
            {
                rcName = value;
            }
        }
        /// <summary>
        /// Get Nurse Name 
        /// </summary>
        public string NurseName
        {
            get
            {
                return nurseName;
            }
            set
            {
                nurseName = value;
            }
        }
        /// <summary>
        /// Get Unit Name 
        /// </summary>
        public string UnitName
        {
            get
            {
                return unitName;
            }
            set
            {
                unitName = value;
            }
        }
        /// <summary>
        /// Get Finding Name
        /// </summary>
        public string FindingName
        {
            get
            {
                return findingName;
            }
            set
            {
                findingName = value;
            }
        }
        /// <summary>
        /// Get MRN Name
        /// </summary>
        public string MRN
        {
            get
            {
                return mrn;
            }
            set
            {
                mrn = value;
            }
        }
        /// <summary>
        /// Get Accession 
        /// </summary>
        public string Accession
        {
            get
            {
                return accession;
            }
            set
            {
                accession = value;
            }
        }
        /// <summary>
        /// Get Date of Birth
        /// </summary>
        public string DOB
        {
            get
            {
                return dob;
            }
            set
            {
                dob = value;
            }
        }
        /// <summary>
        /// Get From Date
        /// </summary>
        public string FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                fromDate = value;
            }
        }
        /// <summary>
        /// Get To Date
        /// </summary>
        public string ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                toDate = value;
            }
        }

        /// <summary>
        /// Get Clinician Team Name
        /// </summary>
        public string CTName
        {
            get
            {
                return ctName;
            }
            set
            {
                ctName = value;
            }
        }

        #endregion
    }
}
