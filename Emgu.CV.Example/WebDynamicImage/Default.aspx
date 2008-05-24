<%@ Page Language="C#" AutoEventWireup="true"  %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Emgu.CV" %>

<%
    Response.Clear();
    Response.ContentType = "image/jpeg";

    using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 40, new Bgr()))
    {
        img.Draw(
            DateTime.Now.Ticks.ToString(), 
            new Font( Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 2.0, 2.0), 
            new Point2D<int>(5, img.Height - 5), 
            new Bgr(255.0, 255.0, 255.0));
        img._Dilate(1);

        img._Not();
        
        img.Bitmap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
    }
    
    Response.End();
%>
