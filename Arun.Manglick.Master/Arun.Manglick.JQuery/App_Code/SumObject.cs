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
    public class SumObject
    {
        #region Private Variables

        private int number1;
        private int number2;

        #endregion

        #region Public Properties

        public int Number1
        {
            get
            {
                return number1;
            }
            set
            {
                number1 = value;
            }
        }

        public int Number2
        {
            get
            {
                return number2;
            }
            set
            {
                number2 = value;
            }
        }

        #endregion
    }
}
