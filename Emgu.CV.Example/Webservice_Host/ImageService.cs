using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Emgu.CV;

namespace Webservice_Host
{
    public class ImageService : IImageService
    {
        public Image<Bgr, Byte> GrabFrame()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 300, new Bgr(255, 255, 255));
            
            Font f = new Font(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0, 1.0);
            String str = DateTime.Now.Ticks.ToString();
            img.Draw(str, f, new Point2D<int>(50, 150), new Bgr(0, 0, 0));
            return img;
        }
    }
}
