using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Arun.Manglick.LINQ
{
    #region Session Manager Class

    /// <summary>
    /// This class is used as a framework to access the Session Variables
    /// </summary>
    /// <history created=”Arun M”></history>
    /// <history date=”Nov 22, 2007”></history>
    public sealed class SessionManager
    {
        #region Constructor

        /// <summary>
        /// FxCop guideline - StaticHolderTypesShouldNotHaveConstructors
        /// </summary>
        /// <history created=”Arun M”></history>
        /// <history date=”Nov 22, 2007”></history>
        private SessionManager()
        { }
        #endregion

        #region Public Methods

        /// <summary>
        /// This Method Clears the Session
        /// </summary>
        /// <returns></returns>
        /// <history created=”Mayur P”></history>
        /// <history date=”Dec 12, 2007”></history>
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
 
        #endregion

        #region Public Properties

        /// <summary>
        /// Used to maintain the BreadCrumb
        /// Keeps track of all the pages (Nodes) visited so far.
        /// </summary>
        /// <returns>SiteMapNodeCollection</returns>
        /// <history created=”Arun M”></history>
        /// <history date=”Nov 22, 2007”></history>
        public static SiteMapNodeCollection NodeCollection
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey.NodeCollection]!= null)
                {
                    return HttpContext.Current.Session[SessionKey.NodeCollection] as SiteMapNodeCollection;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[SessionKey.NodeCollection] = value;
            }
        }

        /// <summary>
        /// Used to hold the new page(node) visited in Breadcrumb.
        /// </summary>
        /// <returns>SiteMapNode</returns>
        /// <history created=”Arun M”></history>
        /// <history date=”Nov 22, 2007”></history>
        public static SiteMapNode NewNode
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey.NewNode] != null)
                {
                    return HttpContext.Current.Session[SessionKey.NewNode] as SiteMapNode;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[SessionKey.NewNode] = value;
            }
        }

        /// <summary>
        /// Used to hold the new SelectedNode in TreeView.
        /// </summary>
        /// <returns>string</returns>
        /// <history created=”Paresh B”></history>
        /// <history date=”Nov 22, 2007”></history>
        public static string SelectedNode
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey.SelectedNode] != null)
                {
                    return HttpContext.Current.Session[SessionKey.SelectedNode].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[SessionKey.SelectedNode] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NotePadText
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey.NotePadText] != null && HttpContext.Current.Session[SessionKey.NotePadText.ToString()] != String.Empty)
                {
                    return HttpContext.Current.Session[SessionKey.NotePadText].ToString();
                }
                return "Nothing to Read!";
            }
            set
            {
                HttpContext.Current.Session[SessionKey.NotePadText] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DiagramPath
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey.DiagramPath] != null && HttpContext.Current.Session[SessionKey.DiagramPath.ToString()] != String.Empty)
                {
                    return HttpContext.Current.Session[SessionKey.DiagramPath].ToString();
                }
                return "Nothing to Draw!";
            }
            set
            {
                HttpContext.Current.Session[SessionKey.DiagramPath] = value;
            }
        }

        #endregion
    } 

    #endregion
}
