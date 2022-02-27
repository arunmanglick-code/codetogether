using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace Arun.Manglick.BL
{
    public class SerializeClass
    {
        #region Private Variables

        private string mFirstName;
        private string mLastName;
        private int mAge; 
        
        #endregion

        #region Public Properties

        public string FirstName
        {
            get { return mFirstName; }
            set { mFirstName = value; }
        }

        public string LastName
        {
            get { return mLastName; }
            set { mLastName = value; }
        }

        public int Age
        {
            get { return mAge; }
            set { mAge = value; }
        }
        
        #endregion

        #region Public Methods

        public string ConcatName()
        {
            return FirstName + ":" + LastName;
        }
        
        #endregion
    }
}
