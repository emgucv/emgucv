<%@ Page Language="C#" AutoEventWireup="true"  %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Emgu.CV.CvEnum" %>
<%@ Import Namespace="Emgu.Util" %>
<%@ Import Namespace="Emgu.CV" %>
<%@ Import Namespace="Emgu.CV.Structure" %>
<%@ Import Namespace="Emgu.CV.OCR" %>
<%@ Import Namespace="System.Drawing" %>

<%
    Response.Clear();

    NameValueCollection nvc = Request.Form;

    //If you want this web page to perform ocr on an image, post the image file to this webpage
    //e.g. Using curl
    //
    //curl -F imageForOcr=@{image_file_name} http://{webpage_url}/Default.aspx
    //
    //Replace {image_file_name} with your image file and replace {webpage_url} with the url of this host
        
    string[] imageFiles = Request.Files.AllKeys;
    if (imageFiles.Length == 1)
    {
       Response.ContentType = "text/plain";
       HttpPostedFile pf = Request.Files.Get(imageFiles[0]);
       using (Stream stream = pf.InputStream)
       using (Bitmap bmp = new Bitmap(stream))
       using (Image<Gray, Byte> img = new Image<Gray,byte>(bmp))
       {
          string appDataPath = Server.MapPath("App_Data");
          if (!appDataPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
              appDataPath += Path.DirectorySeparatorChar.ToString();
          Tesseract ocr = new Tesseract(appDataPath, "eng", Tesseract.OcrEngineMode.OemTesseractCubeCombined);
          ocr.Recognize(img);
          String result = ocr.GetText();
          Response.Write(String.Format("Text recognized: {0}", result));
       }
    }
    else
    {
       Response.ContentType = "image/jpeg";

       using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 40, new Bgr()))
       {
          
          img.Draw(
              DateTime.Now.Ticks.ToString(),
              new Point(5, img.Height - 5),
              Emgu.CV.CvEnum.FontFace.HersheyPlain,
              2.0,
              new Bgr(255.0, 255.0, 255.0),
              1, LineType.EightConnected, false);
          img._Dilate(1);

          img._Not();

          img.Bitmap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
       }
    }
    Response.End();
%>
