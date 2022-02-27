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
using System.Collections.Generic;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for MoveCollection
    /// </summary>
    public class MoveiCollection
    {
        public MoveiCollection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<Movie> GetEmptyDataTable()
        {
            try
            {                                
                DataSet ds = new DataSet();
                ds.ReadXml(HttpContext.Current.Server.MapPath("~\\XML\\Movie.xml"));
                DataTable dt = ds.Tables[0];
                ds.Dispose();

                List<Movie> list = new List<Movie>();
                
                foreach (DataRow drow in dt.Rows)
                {
                    list.Add(new Movie(drow[0].ToString(),drow[1].ToString(),drow[2].ToString()));
                    
                }
                
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Movie> GetMovies()
        {
            List<Movie> list = new List<Movie>();
            Movie movie = new Movie();
            movie.Name = "True Lies";
            movie.Director = "William Jones";
            movie.Language = "English";
            list.Add(movie);

            movie.Name = "Robotics";
            movie.Director = "Wills Smith";
            movie.Language = "English";
            list.Add(movie);

            movie.Name = "Spider Man";
            movie.Director = "Rozer Willy";
            movie.Language = "English";
            list.Add(movie);

            movie.Name = "Krish";
            movie.Director = "Hrithik";
            movie.Language = "Hindi";
            list.Add(movie);

            movie.Name = "SuperMan";
            movie.Director = "William Jones";
            movie.Language = "English";
            list.Add(movie);

            MovieComparer movieComparer = new MovieComparer();
            movieComparer.SortExpression = "asc"; // ViewState[SORT_COLUMN_NAME].ToString();
            movieComparer.SortOrder = "Name"; // ViewState[ViewStateConstant.SORTING_ORDER].ToString();
            //ViewState[SORT_COLUMN_NAME] = null;
            list.Sort(movieComparer);

            return list;
        }
        
    }
}
