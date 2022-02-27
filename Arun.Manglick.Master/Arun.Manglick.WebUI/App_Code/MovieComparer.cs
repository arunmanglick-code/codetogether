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
    public class MovieComparer : System.Collections.Generic.IComparer<Movie>
    {
        #region Private Variable

        private string sortExpression;
        private string sortOrder;

        #endregion

        #region Public Properties

        /// <summary>
        /// Get or set Sort Expression
        /// </summary>
        public string SortExpression
        {
            get { return sortExpression; }
            set { sortExpression = value; }
        }

        /// <summary>
        /// Get or set Sort Order
        /// </summary>
        public string SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        #endregion

        public int Compare(Movie x, Movie y)
        {
            //return obj1.Name.CompareTo(obj2.Name);

            try
            {
                switch (SortOrder)
                {
                    case "asc":
                        if (SortExpression.Equals("Name", StringComparison.OrdinalIgnoreCase))
                            return String.Compare(x.Name, y.Name);
                        else if (SortExpression.Equals("Language", StringComparison.OrdinalIgnoreCase))
                            return String.Compare(x.Language, y.Language);
                        break;

                    case "dsc":
                        if (SortExpression.Equals("Name", StringComparison.OrdinalIgnoreCase))
                            return String.Compare(y.Name, x.Name);
                        else if (SortExpression.Equals("Language", StringComparison.OrdinalIgnoreCase))
                            return String.Compare(y.Language, x.Language);
                        break;

                    default:
                        break;
                }
                return 0;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }
    }
}
