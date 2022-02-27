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

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for MyCollectionBase
    /// </summary>
    public class MyCollectionBase : CollectionBase
    {
        public MyCollectionBase()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int Add(UserI objUser)
        {
            return List.Add(objUser);
        }

        public void Remove(UserI objUser)
        {
            List.Remove(objUser);
        }

        //This is defined by me.
        public UserI Item(int i)
        {
            return List[i] as  UserI;
        }
    }
}
