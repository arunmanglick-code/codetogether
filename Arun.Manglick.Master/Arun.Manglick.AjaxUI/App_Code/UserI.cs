using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for UserI
    /// </summary>
    public class UserI
    {
        #region Private Variables

        private string _au_id;
        private string _au_fname;
        private string _phone;
        private string _address;
        private string _city;

        #endregion

        #region Public Properties

        public string Id
        {
            get
            {
                return _au_id;
            }
            set
            {
                _au_id = value;
            }
        }
        public string Fname
        {
            get
            {
                return _au_fname;
            }
            set
            {
                _au_fname = value;
            }
        }
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        #endregion
    }
}
