using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using tessnet2;

namespace LicensePlateRecognition
{
   public class StopSignDetector
   {
      private Tesseract _ocr;

      public StopSignDetector()
      {
         //create OCR
         _ocr = new Tesseract();

         //You can download more language definition data from
         //http://code.google.com/p/tesseract-ocr/downloads/list
         //Languages supported includes:
         //Dutch, Spanish, German, Italian, French and English
         _ocr.Init("eng", false);
      }

      public void DetectStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Image<Gray, Byte>> filteredStopSignList, List<MCvBox2D> boxList)
      {
         Image<Hsv, Byte> hsv = img.Convert<Hsv, Byte>();

         Image<Gray, Byte> h = new Image<Gray, byte>(hsv.Size);
         CvInvoke.cvSplit(hsv, h, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

         Image<Gray, Byte> redMask = h.InRange(new Gray(30), new Gray(150));
         redMask._Not();

         Image<Gray, Byte> canny = redMask.Canny(new Gray(100), new Gray(50));

         using (MemStorage stor = new MemStorage())
         {
            for (
               Contour<Point> contours = canny.FindContours(
                  Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                  Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST,
                  stor);
               contours != null;
               contours = contours.HNext)
            {
                Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.01, stor);
               
                //img.Draw(currentContour, new Bgr(Color.Green), new Bgr(Color.Green),0, 2);
                if (currentContour.Area > 100 && currentContour.Total == 8)
                {
                   MCvBox2D box = currentContour.GetMinAreaRect();
                   
                   Image<Gray, Byte> contourMask = new Image<Gray, Byte>(canny.Size);
                   contourMask.Draw(currentContour, new Gray(255.0), new Gray(255.0), 0, -1);
                   Image<Gray, Byte> stopSignMask = redMask.Copy(contourMask);
                   Image<Gray, Byte> stopSign = stopSignMask.Copy(box);
                   Image<Gray, Byte> filteredStopSign = FilterStopSign(stopSign);
                   if (filteredStopSign == null) continue;

                   List<Word> wordList = _ocr.DoOCR(filteredStopSign.Not().Bitmap, filteredStopSign.ROI);
                   String words = String.Join(" ", wordList.ConvertAll<String>(delegate(Word w) { return w.Text; }).ToArray());

                   if (words.Contains( "STOP") || words.Contains("STUP") || words.Contains( "ST0P") )
                   {
                      boxList.Add(box);
                      stopSignList.Add(stopSign);
                      filteredStopSignList.Add(filteredStopSign);
                   }
                }
            }
         }
      }

      
      /// <summary>
      /// Filter the license plate to remove noise
      /// </summary>
      /// <param name="plate">The license plate image</param>
      /// <returns>License plate image without the noise</returns>
      private Image<Gray, Byte> FilterStopSign(Image<Gray, Byte> stopSign)
      {
         Image<Gray, Byte> result = stopSign.Not();
         using (Image<Gray, Byte> cornerFilledStopSign = stopSign.Copy())
         {
            #region flood fill the four corners
            MCvConnectedComp comp;
            CvInvoke.cvFloodFill(cornerFilledStopSign, Point.Empty, new MCvScalar(255.0), new MCvScalar(), new MCvScalar(), out comp, 8, IntPtr.Zero);
            CvInvoke.cvFloodFill(cornerFilledStopSign, new Point(0, cornerFilledStopSign.Height - 1), new MCvScalar(255.0), new MCvScalar(), new MCvScalar(), out comp, 8, IntPtr.Zero);
            CvInvoke.cvFloodFill(cornerFilledStopSign, new Point(cornerFilledStopSign.Width - 1, cornerFilledStopSign.Height - 1), new MCvScalar(255.0), new MCvScalar(), new MCvScalar(), out comp, 8, IntPtr.Zero);
            CvInvoke.cvFloodFill(cornerFilledStopSign, new Point(cornerFilledStopSign.Width - 1, 0), new MCvScalar(255.0), new MCvScalar(), new MCvScalar(), out comp, 8, IntPtr.Zero);
            cornerFilledStopSign._Dilate(1);
            cornerFilledStopSign._Erode(1);
            #endregion

            List<PointF> points = new List<PointF>();

            using (Image<Gray, Byte> charactorMask = new Image<Gray, byte>(stopSign.Size))
            using (Image<Gray, Byte> canny = cornerFilledStopSign.Canny(new Gray(100), new Gray(50)))
            using (MemStorage stor = new MemStorage())
            {
               for (
                  Contour<Point> contours = canny.FindContours(
                     Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                     Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL,
                     stor);
                  contours != null; contours = contours.HNext)
               {
                  Rectangle rect = contours.BoundingRectangle;
                  if (rect.Height < (stopSign.Height >> 1) && rect.Height > (stopSign.Height >> 4))
                  {
                     rect.X -= 1; rect.Y -= 1; rect.Width += 2; rect.Height += 2;
                     rect.Intersect(stopSign.ROI);
                     points.Add(rect.Location);
                     Point p2 = rect.Location;
                     p2.Offset(rect.Width, rect.Height);
                     points.Add(p2);
                     charactorMask.Draw(rect, new Gray(255.0), -1);
                  }
               }

               charactorMask._Not();
               result.SetValue(new Gray(0), charactorMask);

               if (points.Count == 0) return result;
               return result.Copy(PointCollection.BoundingRectangle(points.ToArray()));
            }
         }

         //return result;
      }
   }
}
