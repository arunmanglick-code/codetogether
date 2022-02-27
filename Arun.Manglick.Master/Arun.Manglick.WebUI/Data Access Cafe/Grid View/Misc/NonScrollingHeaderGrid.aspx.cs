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
using System.IO;

public partial class NonScrollingHeaderGrid : System.Web.UI.Page
{
    #region Private Variables

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private const string Direction = "SortDirection";
    private String sortExpression = String.Empty;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }
    
    #endregion

    #region Public Properties

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState[Direction] == null)
            {
                ViewState[Direction] = SortDirection.Ascending;
            }

            return (SortDirection)ViewState[Direction];
        }
        set { ViewState[Direction] = value; }
    }

    #endregion

    #region Private Methods

    private void BindGrid()
    {
        Session["Data"] = GetEmptyDataTable();
        DataTable dt = Session["Data"] as DataTable;
        dt.Rows.Clear();

        if (dt.Rows.Count == 0)
        {
            dt.Rows.Add(dt.NewRow());
           
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        GridView1.Rows[0].Cells.Clear();
    }

    private DataTable GetEmptyDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~\\XML\\Empty.xml"));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public int GetSortColumnIndex(GridView gridView, String sortExpression)
    {
        foreach (DataControlField field in gridView.Columns)
        {
            if (!String.IsNullOrEmpty(sortExpression))
            {
                if (field.SortExpression == sortExpression)
                {
                    return gridView.Columns.IndexOf(field);
                }
            }
        }
        return -1;
    }

    #endregion

    #region Control Events

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int temp;
        if (int.TryParse(txtTitle.Text, out temp))
        {
            DataTable dt = GetEmptyDataTable();
            DataTable dtOrig = dt.Copy();
            DataTable dtNew = dtOrig.Clone();
            DataRow drow;
            int count = Convert.ToInt32(txtTitle.Text);
            if (dt != null)
            {
                for (int i = 0; i < count; i++)
                {
                    drow = dtNew.NewRow();
                    drow.ItemArray = dtOrig.Rows[i].ItemArray;
                    dtNew.Rows.Add(drow);
                }
            }

            GridView1.DataSource = dtNew;
            GridView1.DataBind();
        }

    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);            
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }       
    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = Session["Data"] as DataTable;
        DataView dv = dt.DefaultView;
        dv.Sort = sortExpression + direction;

        GridView1.DataSource = dv;
        GridView1.DataBind();
    }
    
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int i = GetSortColumnIndex(GridView1, sortExpression);

            if (i != -1)
            {
                LinkButton sortLink = e.Row.Cells[i].Controls[0] as LinkButton;
                if (sortLink != null)
                {
                    if (GridViewSortDirection == SortDirection.Ascending)
                    {
                        sortLink.Text += " <img src='../../../Images/SortUp.gif' border ='0' align='absmiddle' alt='ASC' />";
                    }
                    else
                    {
                        sortLink.Text += " <img src='../../../Images/SortDown.gif' border ='0' align='absmiddle' alt='ASC' />"; ;
                    }
                }
            }
        }
    }

    #endregion

}
