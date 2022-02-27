using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numeric;

public partial class HTTP_Runtime_Cafe_HTTP_Handlers_Handlers_ImageGenerator : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //String text = HttpContext.Current.Request.QueryString["text"];
        //String font = HttpContext.Current.Request.QueryString["font"];
        //String size = HttpContext.Current.Request.QueryString["size"];

        //Font fntText = new Font(font, Single.Parse(size));
        //Bitmap bmp = new Bitmap(10, 10);
        //Graphics g = Graphics.FromImage(bmp);

        //SizeF bmpSize = g.MeasureString(text, fntText);
        //int width = Convert.ToInt16(Math.Ceiling(bmpSize.Width));
        //int height = Convert.ToInt16(Math.Ceiling(bmpSize.Height));
        //bmp = new Bitmap(bmp, width, height);
        //g.Dispose();

        //g = Graphics.FromImage(bmp);
        //g.Clear(Color.White);
        //g.DrawString(text, fntText, Brushes.Black, new PointF(0, 0));
        //g.Dispose();

        //bmp.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Gif);
    }
}
