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
    public enum MoveRow
    {
        MoveUp,
        MoveDown
    }

    public enum LogLevel
    {
        DEBUG = 1,
        INFO = 2,
        WARNING = 3,
        EXCEPTION = 4
    }
}
