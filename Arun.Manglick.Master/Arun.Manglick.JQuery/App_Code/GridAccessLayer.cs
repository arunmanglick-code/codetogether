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
using Arun.Manglick.BL;

namespace Arun.Manglick.UI
{
    public class GridAccessLayer
    {
        public GridAccessLayer()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable GridAuditReflectionTable()
        {
            if (HttpContext.Current.Session["GridTable"] == null)
            {
                Employee employee = null;
                employee = GetEmployeeData();
                HttpContext.Current.Session["GridTable"] = employee.Profile;
            }

            DataView dv = (HttpContext.Current.Session["GridTable"] as DataTable).DefaultView;
            dv.Sort = "Sequence";
            DataTable dTable = dv.ToTable();
            HttpContext.Current.Session["GridTable"] = dTable;
            return dTable;
        }

        private static Employee GetEmployeeData()
        {
            Employee employee = null;

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath("~\\XML\\AuditXML.xml"));
                
                employee = new Employee();
                employee.Id = 2;
                employee.FirstName = "John";
                employee.LastName = "Deer";
                employee.Profile = ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            return employee;
        }
    }
}
