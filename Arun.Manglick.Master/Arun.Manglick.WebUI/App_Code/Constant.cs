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
    /// This class is used to keep all the constants representing Session Keys.
    /// The constants defined here is then used by the 'SessionManager' Class.
    /// </summary>
    /// <history created="Arun M"></history>
    /// <history date="Nov 22, 2007"></history>
    public class SessionKey
    {
        public const string NodeCollection = "NodeCollection";
        public const string NewNode = "NewNode";
        public const string SelectedNode = "SelectedNode";
        public const string SelectedNodes = "SelectedNodes";
        public const string NotePadText = "NotePadText";
        public const string DiagramPath = "DiagramPath";
        public const string IsDirty = "IsDirty";
        
        
    }
}
