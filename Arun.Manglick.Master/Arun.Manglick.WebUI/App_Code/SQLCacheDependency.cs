using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for SQLCacheDependency
    /// </summary>
    public class MySQLCacheDependencyOld
    {
        public MySQLCacheDependencyOld()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable GetData()
        {
            DataTable dt = HttpContext.Current.Cache["HashData"] as DataTable;
            if (dt == null)
            {
                string cn = ConfigurationManager.ConnectionStrings["Pubs"].ConnectionString;
                SqlConnection cnn = new SqlConnection(cn);
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM EMPLOYEE where pub_id < 800", cnn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];

                CacheItemRemovedCallback onremove = new CacheItemRemovedCallback(ItemRemoved);
                SqlCacheDependency myDependency = new SqlCacheDependency("Pubs", "Employee");
                //HttpContext.Current.Cache.Insert("HashKey", dt, myDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, onremove);
                HttpContext.Current.Cache.Insert("HashKey", dt, myDependency);
            }

            return dt;
        }

        public void ItemRemoved(string itemKey, object itemValue, CacheItemRemovedReason  removedReason)
        {

        }

    }
}
