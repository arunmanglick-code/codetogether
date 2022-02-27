using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Arun.Manglick.UI
{

    /// <summary>
    /// Summary description for Movie
    /// </summary>
    public class Movie
    {
        private string m_Name;
        private string m_Director;
        private string m_Language;

        public Movie()
        {
        }

        public Movie(string name, string director, string language)
        {
            m_Name = name;
            m_Director = director;
            m_Language = language;
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public string Director
        {
            get { return m_Director; }
            set { m_Director = value; }
        }
        public string Language
        {
            get { return m_Language; }
            set { m_Language = value; }
        }
    }

}
