using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using tessnet2;
using System.Diagnostics;

namespace LicensePlateRecognition
{
   public class LicensePlateDetector : DisposableObject
   {
      private Tesseract _ocr;

      public LicensePlateDetector()
      {
         //create OCR
         _ocr = new Tesseract();

         //You can download more language definition data from
         //http://code.google.com/p/tesseract-ocr/downloads/list
         //Languages supported includes:
         //Dutch, Spanish, German, Italian, French and English
         _ocr.Init("eng", false);
      }

      public List<List<Word>> DetectLicensePlate(Image<Bgr, byte> img, List<Image<Gray, Byte>> licensePlateList, List<Image<Gray, Byte>> filteredLicensePlateList, List<MCvBox2D> boxList)
      {
         //Stopwatch w = Stopwatch.StartNew();
         List<List<Word>> licenses = new List<List<Word>>();
         using (Image<Gray, byte> gray = img.Convert<Gray, Byte>())
         using (Image<Gray, Byte> canny = new Image<Gray, byte>(gray.Size))
         using (MemStorage stor = new MemStorage())
         {
            CvInvoke.cvCanny(gray, canny, 100, 50, 3);

            Contour<Point> contours = canny.FindContours(
                 Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                 Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                 stor);
            FindLicensePlate(contours, gray, canny, licensePlateList, filteredLicensePlateList, boxList, licenses);
         }
         //w.Stop();
         return licenses;
      }

      private void FindLicensePlate(
         Contour<Point> contours, Image<Gray, Byte> gray, Image<Gray, Byte> canny,
         List<Image<Gray, Byte>> licensePlateList, List<Image<Gray, Byte>> filteredLicensePlateList, List<MCvBox2D> boxList,
         List<List<Word>> licenses)
      {
         for (; contours != null; contours = contours.HNext)
         {
            Contour<Point> approxContour = contours.ApproxPoly(contours.Perimeter * 0.05, contours.Storage);

            if (approxContour.Area > 100 && approxContour.Total == 4)
            {
               //img.Draw(contours, new Bgr(Color.Red), 1);
               if (!IsParallelogram(approxContour.ToArray()))
               {
                  Contour<Point> child = contours.VNext;
                  if (child != null)
                     FindLicensePlate(child, gray, canny, licensePlateList, filteredLicensePlateList, boxList, licenses);
                  continue;
               }

               MCvBox2D box = approxContour.GetMinAreaRect();

               double whRatio = (double)box.size.Width / box.size.Height;
               if (!(3.0 < whRatio && whRatio < 8.0))
               {
                  Contour<Point> child = contours.VNext;
                  if (child != null)
                     FindLicensePlate(child, gray, canny, licensePlateList, filteredLicensePlateList, boxList, licenses);
                  continue;
               }

               Image<Gray, Byte> plate = gray.Copy(box);
               Image<Gray, Byte> filteredPlate = FilterPlate(plate);

               List<Word> words;
               using (Bitmap bmp = filteredPlate.Bitmap)
                  words = _ocr.DoOCR(bmp, filteredPlate.ROI);

               licenses.Add(words);
               licensePlateList.Add(plate);
               filteredLicensePlateList.Add(filteredPlate);
               boxList.Add(box);
            }
         }
      }

      private static bool IsParallelogram(Point[] pts)
      {
         LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

         double diff1 = Math.Abs(edges[0].Length - edges[2].Length);
         double diff2 = Math.Abs(edges[1].Length - edges[3].Length);
         if (diff1 / edges[0].Length <= 0.05 && diff1 / edges[2].Length <= 0.05
            && diff2 / edges[1].Length <= 0.05 && diff2 / edges[3].Length <= 0.05)
         {
            return true;
         }
         return false;
      }

      /// <summary>
      /// Filter the license plate to remove noise
      /// </summary>
      /// <param name="plate">The license plate image</param>
      /// <returns>License plate image without the noise</returns>
      private static Image<Gray, Byte> FilterPlate(Image<Gray, Byte> plate)
      {
         Image<Gray, Byte> thresh = plate.ThresholdBinaryInv(new Gray(120), new Gray(255));

         using (Image<Gray, Byte> plateMask = new Image<Gray, byte>(plate.Size))
         using (Image<Gray, Byte> plateCanny = plate.Canny(new Gray(100), new Gray(50)))
         using (MemStorage stor = new MemStorage())
         {
            plateMask.SetValue(255.0);
            for (
               Contour<Point> contours = plateCanny.FindContours(
                  Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                  Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL,
                  stor);
               contours != null; contours = contours.HNext)
            {
               Rectangle rect = contours.BoundingRectangle;
               if (rect.Height > (plate.Height >> 1))
               {
                  rect.X -= 1; rect.Y -= 1; rect.Width += 2; rect.Height += 2;
                  rect.Intersect(plate.ROI);

                  plateMask.Draw(rect, new Gray(0.0), -1);
               }
            }

            thresh.SetValue(0, plateMask);
         }

         thresh._Erode(1);
         thresh._Dilate(1);

         return thresh;
      }

      protected override void DisposeObject()
      {
         _ocr.Dispose();
      }
   }
}
