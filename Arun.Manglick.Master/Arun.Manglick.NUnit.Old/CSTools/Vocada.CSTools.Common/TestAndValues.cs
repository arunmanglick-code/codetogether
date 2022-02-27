#region File History
/******************************File History***************************
 * File Name        : TestAndValues.cs
 * Author           : Jeeshan K
 * Created Date     : 24-01-2007
 * Purpose          : Test Results Definition for Lab Setup.
 *                  : 
 *                  :
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 
 */

#endregion

#region using
using System;
using System.Text;
using System.Collections.Generic;
#endregion

namespace Vocada.CSTools.Common
{
    public class TestAndValues
    {
        #region Class Level Variables

        private int labTestID = 0;
        private int testID = 0;
        private int groupID;
        private string fullTestName = "";
        private string shortTestName = "";
        private string testArea = "";
        private float highestValue = -1;
        private float lowestValue = -1;
        private int resultTypeID = 0;
        private int measurementID = 0;
        private int findingID = 0;
        private string grammar = "";
        private string testVoiceUrl = "";
        private bool isActive = true;


        #endregion

        #region Class Properties

        /// <summary>
        /// Property to store Lab Test ID for the Test Result
        /// </summary>
        public int LabTestID
        {
            get
            {
                return labTestID;
            }
            set
            {
                labTestID = value;
            }
        }
        /// <summary>
        /// Property to store Test ID for the Test Result
        /// </summary>
        public int TestID
        {
            get
            {
                return testID;
            }
            set
            {
                testID = value;
            }
        }
        /// <summary>
        /// Property to store Group ID for the Test Result
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
        /// Property to store Full Test Name for the Test Result
        /// </summary>
        public string FullTestName
        {
            get
            {
                return fullTestName;
            }
            set
            {
                fullTestName = value;
            }
        }

        /// <summary>
        /// Property to store Short Test Name for the Test Result
        /// </summary>
        public string ShortTestName
        {
            get
            {
                return shortTestName;
            }
            set
            {
                shortTestName = value;
            }
        }

        /// <summary>
        /// Property to store Test Area
        /// </summary>
        public string TestArea
        {
            get
            {
                return testArea;
            }
            set
            {
                testArea = value;
            }
        }

        /// <summary>
        /// Property to store Highest Possible Value
        /// </summary>
        public float HighestValue
        {
            get
            {
                return highestValue;
            }
            set
            {
                highestValue = value;
            }
        }

        /// <summary>
        /// Property to store Lowest Possible Value
        /// </summary>
        public float LowestValue
        {
            get
            {
                return lowestValue;
            }
            set
            {
                lowestValue = value;
            }
        }

        /// <summary>
        /// Property to store Result Type ID
        /// </summary>
        public int ResultTypeID
        {
            get
            {
                return resultTypeID;
            }
            set
            {
                resultTypeID = value;
            }
        }

        /// <summary>
        /// Property to store Measurement ID
        /// </summary>
        public int MeasurementID
        {
            get
            {
                return measurementID;
            }
            set
            {
                measurementID = value;
            }
        }

        /// <summary>
        /// Property to store Finding ID
        /// </summary>
        public int FindingID
        {
            get
            {
                return findingID;
            }
            set
            {
                findingID = value;
            }
        }
        /// <summary>
        /// Property to store Grammar for the Test Result
        /// </summary>
        public string Grammer
        {
            get
            {
                return grammar;
            }
            set
            {
                grammar = value;
            }
        }
        /// <summary>
        /// Property to store Test Voice URL for the Test Result
        /// </summary>
        public string TestVoiceURL
        {
            get
            {
                return testVoiceUrl;
            }
            set
            {
                testVoiceUrl = value;
            }
        }
        /// <summary>
        /// Property to store Test Voice URL for the Test Result
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive ;
            }
            set
            {
                isActive = value;
            }
        }
        #endregion
    }
}