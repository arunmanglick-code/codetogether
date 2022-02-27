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
using Arun.Manglick.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    #region Private Variables
    private TreeNode mSelectNode;
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblLenderName.Text = "Arun"; //  HttpContext.Current.User.Identity.Name;

            if (!String.IsNullOrEmpty(SessionManager.SelectedNode))
            {                
                trvMenu.DataBind();
                mSelectNode = trvMenu.FindNode(SessionManager.SelectedNode.ToString()) as TreeNode;
                mSelectNode.Select();
                ExpandParent(mSelectNode);
            }
        }
        catch (Exception ex)
        {
            //
            // TODO: Logging exception code goes here
            //
        }
    }

    #endregion

    #region Private Method

    /// <summary>
    /// This function Expands all parents recursively
    /// </summary>
    /// <returns>Void</returns>
    /// <history created="Arun M"></history>
    /// <history date="July 01, 2008"></history>
    private void ExpandParent()
    {
        TreeNode node = null;
        String selectedNode = String.Empty;
        SelectedNodes nodesCollection = SessionManager.SelectedNodes;

        for (int iCnt = nodesCollection.NodesSelected.Count - 1; iCnt >= 0; iCnt--)
        {
            selectedNode = nodesCollection.NodesSelected[iCnt];
            node = trvMenu.FindNode(selectedNode) as TreeNode;
            if (node.ChildNodes.Count > 0)
            {
                node.Expand();
            }
        }
    }

    /// <summary>
    /// This function Expands all parents 
    /// </summary>
    /// <returns>Void</returns>
    /// <param name="selectNode"></param>
    private void ExpandParent(TreeNode selectNode)
    {
        while (selectNode.Parent != null)
        {
            selectNode.Parent.Expand();
            selectNode = selectNode.Parent;
        }
    }

    #endregion

    #region Control Events

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Arun.Manglick.AjaxUI/Home.aspx");
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
        SelectedNodes nodes = null;

        try
        {  
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

    #endregion
}
