using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ReadXMLFile : System.Web.UI.Page
{

    #region Private Variables

    String XmlFileName1 = "~\\XML\\LINQToXML1.xml";
    String XmlFileName2 = "~\\XML\\LINQToXML2.xml";
    String Namespace = "http://purl.org/rss/1.0/modules/slash/";
    String RSSFeed = @"http://weblogs.asp.net/scottgu/rss.aspx";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods

    private void ReadXML()
    {
        XDocument tableXML = XDocument.Load(Server.MapPath(XmlFileName1));
        var employees = from employee in tableXML.Descendants("Employee")
                        where employee.Attribute("status").Value == "Y"
                        select new
                        {
                            CourseID = employee.Element("CourseId").Value,
                            Sequence = employee.Element("Sequence").Value,
                            YearOfPassing = employee.Element("YearOfPassing").Value,
                            Institution = employee.Element("Institution").Value,
                            Course = employee.Element("Course").Value,
                            Average = (int?) employee.Element("Average") ?? 0
                        };

        GridView1.DataSource = employees.ToList();
        GridView1.DataBind();   
        
    }

    private void ReadHierarchichalXML()
    {
        XDocument tableXML = XDocument.Load(Server.MapPath(XmlFileName2));
        var employees = from employee in tableXML.Descendants("Employee")
                        where employee.Attribute("status").Value == "Y"
                        select new 
                        {
                            CourseID = employee.Element("CourseId").Value,
                            Sequence = employee.Element("Sequence").Value,
                            YearOfPassing = employee.Element("YearOfPassing").Value,
                            Institution = employee.Element("Institution").Value,
                            Course = employee.Element("Course").Value,
                            Average = (int?)employee.Element("Average") ?? 0,
                            Subjects = (from subject in employee.Elements("Subject")
                                        orderby subject.Value
                                        select subject.Value).ToList()
                        };


        // Note: The Subjects is not visible in the Grid. Not Sure why?
        GridView1.DataSource = employees.ToList();        
        GridView1.DataBind();
    }

    private void ReadRemoteRSS()
    {
        XNamespace slashNamespace = Namespace;
        XDocument tableXML = XDocument.Load(RSSFeed);
        var posts = from item in tableXML.Descendants("item")                        
                        select new
                        {
                            Title = item.Element("title").Value,
                            Published = DateTime.Parse(item.Element("pubDate").Value),
                            Url = item.Element("link").Value,
                            NumComments = item.Element(slashNamespace + "comments").Value,                          
                        };

        var newposts = from item in posts
                       where (DateTime.Now - item.Published).Days < 7
                       select item;

        GridView1.DataSource = posts.ToList();
        GridView1.DataBind();
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnQuery1_Click(object sender, EventArgs e)
    {
        ReadXML();
    }

    protected void btnQuery2_Click(object sender, EventArgs e)
    {
        ReadHierarchichalXML();
    }

    protected void btnQuery3_Click(object sender, EventArgs e)
    {
        ReadRemoteRSS();
    }    

    #endregion

}
