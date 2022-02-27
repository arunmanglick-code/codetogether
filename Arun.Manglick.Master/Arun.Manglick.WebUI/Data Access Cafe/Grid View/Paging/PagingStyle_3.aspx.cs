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

public partial class PagingStyle_3 : System.Web.UI.Page
{
    private const string StartIndex = "startIndex";
    private const string EndIndex = "endIndex";
    private int startIndex;
    private int endIndex;
    int totalPageCount;
    int average;
    int gridPageCount;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[StartIndex] == null)
        {
            startIndex = 1;
            Session[StartIndex] = startIndex;
        }

        if (Session[EndIndex] == null)
        {
            endIndex = 3;
            Session[EndIndex] = endIndex;
        }
        
        if (!IsPostBack)
        {
            BindGrid();
        }
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                if (cell.Controls.Count != 0)
                {
                    LinkButton lnkButton = cell.Controls[0] as LinkButton;
                    if (lnkButton != null)
                    {
                        lnkButton.ToolTip = "Sort By " + "'" + lnkButton.Text + "'";
                    }
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text != string.Empty)
        {
            GridView1.EmptyDataText = "Sorry";
            srcMovies.FilterExpression = "Title='" + txtTitle.Text + "'";
        }
        else
        {
            srcMovies.FilterExpression = "Title like '%'";
        }
    }

    protected void mnuPage_MenuItemClick(object sender, MenuEventArgs e)
    {
        GridView1.PageIndex = Convert.ToInt16(e.Item.Text) - 1;
        BindGrid();
    }
    protected void GridView1_DataBoundOld(object sender, EventArgs e)
    {
        //Menu mnuPage = GridView1.BottomPagerRow.FindControl("mnuPage") as Menu;

        //ImageButton imgPrev = new ImageButton();
        //imgPrev.ImageUrl = "../../Images/Left1.GIF";
        //imgPrev.CommandName = "Page";
        //imgPrev.CommandArgument = "Prev";
        //TableCell cellImgPrev = new TableCell();
        //cellImgPrev.Controls.Add(imgPrev);

        //Label lblPage = new Label();
        //lblPage.Text = "Page";
        //TableCell cellLblPage = new TableCell();
        //cellLblPage.Controls.Add(lblPage);

        //GridView1.BottomPagerRow.Controls.Add(cellImgPrev);
        //GridView1.BottomPagerRow.Controls.Add(cellLblPage);

        
        //MenuItem mnuItem = null;
        //int totalPageCount = GridView1.PageCount * GridView1.PageSize;
        //int average = 5;
        //int gridPageCount = totalPageCount / average;

        //for (int i = 1; i <= average; i++)
        //{
        //    mnuItem = new MenuItem();
        //    mnuItem.Text = i.ToString();            

        //    if (GridView1.PageIndex == i - 1)
        //    {
        //        mnuItem.Selected = true;               
        //    }
        //    mnuPage.Items.Add(mnuItem);
        //}

        //if (average < totalPageCount)
        //{
        //    GridView1.BottomPagerRow.Controls.Add(lblPage);

        //    ImageButton imgNext = new ImageButton();
        //    imgNext.ImageUrl = "../../Images/Right1.GIF";
        //    imgNext.CommandName = "Page";
        //    imgNext.CommandArgument = "Next";
        //    GridView1.BottomPagerRow.Controls.Add(imgPrev);

        //    Label lblOf = new Label();
        //    lblPage.Text = "of";
        //    GridView1.BottomPagerRow.Controls.Add(lblPage);

        //    LinkButton lnkLastCount = new LinkButton();
        //    lnkLastCount.Command = "Page";
        //    lnkLastCount.CommandArgument = totalPageCount;
        //    lnkLastCount.Text = totalPageCount.ToString();
        //    GridView1.BottomPagerRow.Controls.Add(lnkLastCount);
        //}
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        Menu mnuPage = GridView1.BottomPagerRow.FindControl("mnuPage") as Menu;
       
        MenuItem mnuItem = null;
        totalPageCount = GridView1.PageCount * GridView1.PageSize;
        average = 3;
        gridPageCount = totalPageCount / average;

        for (int i = Convert.ToInt16(Session[StartIndex]); i <= Convert.ToInt16(Session[EndIndex]); i++)
        {
            mnuItem = new MenuItem();
            mnuItem.Text = i.ToString();

            if (GridView1.PageIndex == i - 1)
            {
                //mnuItem.Selected = true;
                mnuItem.Selectable = false;
            }
            mnuPage.Items.Add(mnuItem);

            mnuItem = new MenuItem();
            mnuItem.Text = "|";
            mnuItem.Selectable = false;
            mnuPage.Items.Add(mnuItem);
        }

        LinkButton lnkLast = GridView1.BottomPagerRow.FindControl("lnkLast") as LinkButton;
        lnkLast.Text = gridPageCount.ToString();
        lnkLast.CommandArgument = gridPageCount.ToString();

        if (Convert.ToInt16(Session[EndIndex]) <= average)
        {           
            LinkButton lnkFirst = GridView1.BottomPagerRow.FindControl("lnkFirst") as LinkButton;
            lnkFirst.Visible = false;
        }

    }

    private void BindGrid()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~\\XML\\Paging3.xml"));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            dt= dt.Copy();

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void GridView1_PageIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        totalPageCount = GridView1.PageCount * GridView1.PageSize;
        average = 3;
        gridPageCount = totalPageCount / average;
        if (e.NewPageIndex >= Convert.ToInt16(Session[EndIndex]))
        {
            Session[StartIndex] = e.NewPageIndex + 1;
            Session[EndIndex] = Convert.ToInt16(Session[StartIndex]) + 3;

            if (gridPageCount < Convert.ToInt16(Session[EndIndex]))
            {
                Session[EndIndex] = Convert.ToInt16(Session[EndIndex]) - gridPageCount;
                Session[EndIndex] = Convert.ToInt16(Session[EndIndex]) + Convert.ToInt16(Session[StartIndex]) - 1;
            }
            LinkButton lnkFirst = GridView1.BottomPagerRow.FindControl("lnkFirst") as LinkButton;
            lnkFirst.Visible = true;

        }

        if (e.NewPageIndex + 1 == GridView1.PageCount)
        {
            int i = (e.NewPageIndex + 1) % average;
            Session[EndIndex] = e.NewPageIndex + 1;
            Session[StartIndex] = Convert.ToInt16(Session[EndIndex]) - i;
        }

        if (e.NewPageIndex < average)
        {
            Session[StartIndex] = 1;
            Session[EndIndex]=5;
            LinkButton lnkFirst = GridView1.BottomPagerRow.FindControl("lnkFirst") as LinkButton;
            lnkFirst.Visible = false;
        }
    }    
}
