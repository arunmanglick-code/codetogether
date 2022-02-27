<%@ WebHandler Language="C#" Class="TextToImageHandler" %>

using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numeric;


public class TextToImageHandler : IHttpHandler
{    
    public void ProcessRequest (HttpContext context) 
    {
        String text = context.Request.QueryString["text"];
        String font = context.Request.QueryString["font"];
        String size = context.Request.QueryString["size"];
        
        Font fntText = new Font(font,Single.Parse(size));
        Bitmap bmp = new Bitmap(10,10);
        Graphics g = Graphics.FromImage(bmp);
        
        SizeF bmpSize = g.MeasureString(text, fntText);
        int width = 20; // Convert.ToInt16(Math.Ceiling(bmpSize.Width));
        int height = 20; // Convert.ToInt16(Math.Ceiling(bmpSize.Height));
        bmp = new Bitmap(bmp, width, height);        
        g.Dispose();
        
        g = Graphics.FromImage(bmp);
        g.Clear(Color.White);
        g.DrawString(text, fntText, Brushes.Black, new PointF(0, 0));
        g.Dispose();

        bmp.Save(context.Response.OutputStream, ImageFormat.Gif);
    }
 
    public bool IsReusable {
        get 
        {
            return true;
        }
    }

}