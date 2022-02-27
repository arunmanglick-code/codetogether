using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace Arun.Manglick.BL
{
    public class SerializeNestedClass
    {
        #region Private Variables

        private string mFirstName;
        private string mLastName;
        private int mAge;
        private Address mAddress;
        
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

        public Address Address
        {
            get { return mAddress; }
            set { mAddress = value; }
        }

        #endregion

        #region Public Methods

        public string ConcatName()
        {
            return FirstName + ":" + LastName;
        }
        
        #endregion
    }

    public class Address
    {
        #region Private Variables

        private string mAddress1;
        private string mAddress2;
        private string mCity;
        private string mState;
        private string mCountry;

        #endregion

        #region Public Properties

        public string Address1
        {
            get { return mAddress1; }
            set { mAddress1 = value; }
        }

        public string Address2
        {
            get { return mAddress2; }
            set { mAddress2 = value; }
        }

        public string City
        {
            get { return mCity; }
            set { mCity = value; }
        }

        public string State
        {
            get { return mState; }
            set { mState = value; }
        }

        public string Country
        {
            get { return mCountry; }
            set { mCountry = value; }
        }

        #endregion
    }
}
