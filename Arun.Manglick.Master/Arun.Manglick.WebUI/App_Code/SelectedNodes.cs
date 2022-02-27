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
using System.Collections;
using System.Collections.Specialized;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// SelectedNodes Class
    /// </summary>
    /// <history created="Arun M"></history>
    /// <history date="Feb 27, 2008"></history>
    public class SelectedNodes 
    {
        #region Private Variables

        private StringCollection mSelectedNodes;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor - SelectedNodes
        /// </summary>
        /// <history created="Arun M"></history>
        /// <history date="Feb 27, 2008"></history>
        public SelectedNodes()
        {
            mSelectedNodes = new StringCollection();
        }        

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set NodesSelected
        /// </summary>
        /// <returns>StringCollection</returns>
        /// <history created="Arun M"></history>
        /// <history date="Feb 27, 2008"></history>
        public StringCollection NodesSelected
        {
            get { return mSelectedNodes; }
            set { mSelectedNodes = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To add the Selected Nodes valuepath Key-Value pair
        /// </summary>
        /// <param name="valuePath">string</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Feb 27, 2008"></history>
        public void Add(string valuePath)
        {
            if (!mSelectedNodes.Contains(valuePath))
            {
                mSelectedNodes.Add(valuePath);           
            }            
        }

        /// <summary>
        /// To removes Selected Nodes valuepath Key-Value pair
        /// </summary>
        /// <param name="valuePath">string</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Feb 27, 2008"></history>
        public void Remove(string valuePath)
        {
            if (mSelectedNodes.Contains(valuePath))
            {
                mSelectedNodes.Remove(valuePath);
            }
        }

        /// <summary>
        /// return count of stringcollection
        /// </summary>
        /// <returns>int</returns>
        /// <history created="Arun M"></history>
        /// <history date="Feb 27, 2008"></history>
        public int Count()
        {
            return mSelectedNodes.Count;
        }

        /// <summary>
        /// returns StringCollection
        /// </summary>
        /// <returns>StringEnumerator</returns>
        /// <history created="Arun M"></history>
        /// <history date="Feb 27, 2008"></history>
        public System.Collections.Specialized.StringEnumerator GetEnumerator()
        {
            return mSelectedNodes.GetEnumerator();
        }
                
        #endregion                
    }
}
