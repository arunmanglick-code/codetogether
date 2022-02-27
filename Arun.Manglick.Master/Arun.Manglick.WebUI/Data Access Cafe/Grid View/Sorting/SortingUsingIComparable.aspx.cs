using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Arun.Manglick.UI;

public partial class SortingUsingIComparable : System.Web.UI.Page
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
        Session["Data"] = GetMovieList();
        GridView1.DataSource = Session["Data"] as List<Movie>;
        GridView1.DataBind();
    }

    private List<Movie> GetMovieList()
    {
        try
        {
            List<Movie> list = new List<Movie>();
            Movie movie = new Movie();
            movie.Name = "True Lies";
            movie.Director = "William Jones";
            movie.Language = "English";
            list.Add(movie);

            movie = new Movie();
            movie.Name = "Robotics";
            movie.Director = "Wills Smith";
            movie.Language = "English";
            list.Add(movie);

            movie = new Movie();
            movie.Name = "Spider Man";
            movie.Director = "Rozer Willy";
            movie.Language = "English";
            list.Add(movie);

            movie = new Movie();
            movie.Name = "Krish";
            movie.Director = "Hrithik";
            movie.Language = "Hindi";
            list.Add(movie);

            movie = new Movie();
            movie.Name = "SuperMan";
            movie.Director = "William Jones";
            movie.Language = "English";
            list.Add(movie);

            return list;
           
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
        List<Movie> list = Session["Data"] as List<Movie>;

        MovieComparer movieComparer = new MovieComparer();
        movieComparer.SortExpression = sortExpression;
        movieComparer.SortOrder = direction;
        list.Sort(movieComparer);

        Session["Data"] = list;
        GridView1.DataSource = Session["Data"] as List<Movie>;
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
