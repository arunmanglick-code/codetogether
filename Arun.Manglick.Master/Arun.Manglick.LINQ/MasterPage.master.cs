using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Arun.Manglick.LINQ;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (SessionManager.SelectedNode != string.Empty)
            {
                trvMenu.DataBind();
                TreeNode selectNode;
                selectNode = (TreeNode)trvMenu.FindNode(SessionManager.SelectedNode.ToString());
                selectNode.Select();
                ExpandParent(selectNode);
            }
            lblLenderName.Text = "Arun"; //  HttpContext.Current.User.Identity.Name;
        }
        catch (Exception ex)
        {
            //
            // TODO: Logging exception code goes here
            //
        }
    }

    /// <summary>
    /// Clear the breadcrumb trail after selecting the tree node
    /// </summary>
    /// <returns>Void</returns>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history created="Paresh B"></history>
    /// <history date="Nov 07, 2007"></history>
    protected void trvMenu_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            SessionManager.SelectedNode = trvMenu.SelectedNode.ValuePath;

            if (trvMenu.SelectedNode.Target != string.Empty)
            {
                Response.Redirect(trvMenu.SelectedNode.Target, false);
            }
            else
            {
                Response.Redirect(Resources.AppLinks.Home, false);
            }
        }
        catch (Exception ex)
        {
            //
            // TODO: Logging exception code goes here
            //
        }
    }

    /// <summary>
    /// This function Expands all parents 
    /// </summary>
    /// <returns>Void</returns>
    /// <param name="SelectedTreeNode"></param>
    private void ExpandParent(TreeNode selectNode)
    {
        while (selectNode.Parent != null)
        {
            selectNode.Parent.Expand();
            selectNode = selectNode.Parent;
        }
    }
}
