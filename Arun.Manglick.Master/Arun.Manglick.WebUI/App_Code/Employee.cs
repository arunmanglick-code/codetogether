using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Data transfer objet for Vehicle Valuation page
    /// </summary>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 04, 2007"></history>
    public class Employee
    {
        #region Private Variables

        private int mEmployeeId;
        private string mFirstName;
        private string mLastName;
        private int mAge;
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
        /// Get or Set employee age
        /// </summary>
        /// <returns>string</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public int Age
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

        #region Public Methods  

        /// <summary>
        /// Method to get the data of audit trail
        /// </summary>
        /// <returns>Employee DTO of the field</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 04, 2007"></history>
        public static Employee GetEmployeeData()
        {
            Employee employee = null;
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath("~\\XML\\AuditXML.xml"));

                employee = new Employee();
                employee.Id = 2;
                employee.FirstName = "John";
                employee.LastName = "Deer";
                employee.Age = 30;
                employee.Profile = ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            return employee;
        }

        /// <summary>
        /// Method to save the employee dto object
        /// </summary>
        /// <param name="employee">Employee DTO</param>
        /// <returns>bool</returns>
        /// <history created="Paresh B"></history>
        /// <history date="Dec 06, 2007"></history>
        public static bool SaveEmployeeData(Employee employee)
        {
            return true;
        }

        #endregion
    }
}
