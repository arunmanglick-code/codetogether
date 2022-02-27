using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for DBImageHandler
    /// </summary>
    public class DBImageHTTPHandler : IHttpHandler
    {
        public DBImageHTTPHandler()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Buffer = false;

            String fileName = VirtualPathUtility.GetFileName(context.Request.Path);
            String conString = ConfigurationManager.ConnectionStrings["Images"].ToString();

            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand("SELECT Image FROM Images WHERE FileName=@FileName", con);
            cmd.Parameters.AddWithValue("@fileName", fileName);
            con.Open();

            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            if (reader.Read())
            {
                int bufferSize = 8040;
                Byte[] chunk = new Byte[bufferSize];

                long retCount = -1;
                long startIndex = -1;
                retCount = reader.GetBytes(0, startIndex, chunk, 0, bufferSize);

                while (retCount == bufferSize)
                {
                    context.Response.BinaryWrite(chunk);
                    startIndex += bufferSize;
                    retCount = reader.GetBytes(0, startIndex, chunk, 0, bufferSize);
                }

                Byte[] actualChunk = new Byte[retCount];

                Buffer.BlockCopy(chunk, 0, actualChunk, 0, Convert.ToInt16(retCount - 1));
                context.Response.BinaryWrite(actualChunk);
            }
        }

        #endregion
    }
}
