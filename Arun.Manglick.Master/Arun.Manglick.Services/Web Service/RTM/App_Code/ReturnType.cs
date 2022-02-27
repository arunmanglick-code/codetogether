using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections;
using Arun.Manglick.BL;

namespace Arun.Manglick.WebService
{
    /// <summary>
    /// Summary description for ReturnType
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ReturnType : System.Web.Services.WebService
    {
        #region Constructor

        public ReturnType()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        #endregion

        #region Possible Return Types Method

        [WebMethod]
        public DataTable GetEmptyDataTable()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Table1.xml");
                DataTable dt = ds.Tables[0];
                ds.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [WebMethod]
        public Employee GetEmployeeObject()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Table1.xml");
                DataTable dt = ds.Tables[0];
                ds.Dispose();

                Employee emp = new Employee();
                emp.Profile = dt;

                return emp;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [WebMethod]
        public ArrayList GetEmployeeArrayList()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Table1.xml");
                DataTable dt = ds.Tables[0];
                ds.Dispose();

                ArrayList arr = new ArrayList();
                Employee emp = null;
                foreach (DataRow dr in dt.Rows)
                {
                    emp = new Employee();
                    emp.Id = Convert.ToInt32(dr[0]);
                    emp.FirstName = dr[3].ToString();
                    emp.LastName = dr[4].ToString();

                    arr.Add(emp);
                }

                return arr;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [WebMethod]
        public List<Employee> GetEmployeeGenericList()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Table1.xml");
                DataTable dt = ds.Tables[0];
                ds.Dispose();

                List<Employee> arr = new List<Employee>();
                Employee emp = null;
                foreach (DataRow dr in dt.Rows)
                {
                    emp = new Employee();
                    emp.Id = Convert.ToInt32(dr[0]);
                    emp.FirstName = dr[3].ToString();
                    emp.LastName = dr[4].ToString();

                    arr.Add(emp);
                }

                return arr;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // Returning HashTable is not allowed 
        // In fact it does not allow to add Web Reference in the Client Application

        //[WebMethod]
        //public Hashtable GetEmployeeHashTable()
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Table1.xml");
        //        DataTable dt = ds.Tables[0];
        //        ds.Dispose();

        //        Hashtable arr = new Hashtable();
        //        Employee emp = null;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            emp = new Employee();
        //            emp.Id = Convert.ToInt32(dr[0]);
        //            emp.FirstName = dr[3].ToString();
        //            emp.LastName = dr[4].ToString();

        //            arr.Add(emp.Id,emp);
        //        }

        //        return arr;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        #endregion
    }
}

