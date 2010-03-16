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
   /// <summary>
   /// A simple license plate detector
   /// </summary>
   public class LicensePlateDetector : DisposableObject
   {
      /// <summary>
      /// The OCR engine
      /// </summary>
      private Tesseract _ocr;

      /// <summary>
      /// Create a license plate detector
      /// </summary>
      public LicensePlateDetector()
      {
         //create OCR engine
         _ocr = new Tesseract();

         //You can download more language definition data from
         //http://code.google.com/p/tesseract-ocr/downloads/list
         //Languages supported includes:
         //Dutch, Spanish, German, Italian, French and English
         _ocr.Init(null, "eng", false);
      }

      /// <summary>
      /// Detect license plate from the given image
      /// </summary>
      /// <param name="img">The image to search license plate from</param>
      /// <param name="licensePlateImagesList">A list of images where the detected license plate region is stored</param>
      /// <param name="filteredLicensePlateImagesList">A list of images where the detected license plate region with noise removed is stored</param>
      /// <param name="detectedLicensePlateRegionList">A list where the region of license plate, defined by an MCvBox2D is stored</param>
      /// <returns>The list of words for each license plate</returns>
      public List<List<Word>> DetectLicensePlate(
         Image<Bgr, byte> img, 
         List<Image<Gray, Byte>> licensePlateImagesList, 
         List<Image<Gray, Byte>> filteredLicensePlateImagesList, 
         List<MCvBox2D> detectedLicensePlateRegionList)
      {
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
            FindLicensePlate(contours, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
         }
         return licenses;
      }

      private static int GetNumberOfChildren(Contour<Point> contours)
      {
         Contour<Point> child = contours.VNext;
         if (child == null) return 0;
         int count = 0;
         while (child != null)
         {
            count++;
            child = child.HNext;
         }
         return count;
      }

      private void FindLicensePlate(
         Contour<Point> contours, Image<Gray, Byte> gray, Image<Gray, Byte> canny,
         List<Image<Gray, Byte>> licensePlateImagesList, List<Image<Gray, Byte>> filteredLicensePlateImagesList, List<MCvBox2D> detectedLicensePlateRegionList,
         List<List<Word>> licenses)
      {
         for (; contours != null; contours = contours.HNext)
         {
            int numberOfChildren = GetNumberOfChildren(contours);      
            //if it does not contains any children (charactor), it is not a license plate region
            if (numberOfChildren == 0) continue;

            if (contours.Area > 100)
            {
               if (numberOfChildren < 3) 
               {
                  //If the contour has less than 3 children, it is not a license plate (assuming license plate has at least 3 charactor)
                  //However we should search the children of this contour to see if any of them is a license plate
                  FindLicensePlate(contours.VNext, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
                  continue;
               }

               MCvBox2D box = contours.GetMinAreaRect();
               double whRatio = (double)box.size.Width / box.size.Height;
               if (!(3.0 < whRatio && whRatio < 10.0))
               {  //if the width height ratio is not in the specific range,it is not a license plate 
                  //However we should search the children of this contour to see if any of them is a license plate
                  Contour<Point> child = contours.VNext;
                  if (child != null)
                     FindLicensePlate(child, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
                  continue;
               }
               box.size.Width -= 2;
               box.size.Height -= 2;
               Image<Gray, Byte> plate = gray.Copy(box);
               Image<Gray, Byte> filteredPlate = FilterPlate(plate);

               List<Word> words;
               using (Bitmap bmp = filteredPlate.Bitmap)
                  words = _ocr.DoOCR(bmp, filteredPlate.ROI);

               licenses.Add(words);
               licensePlateImagesList.Add(plate);
               filteredLicensePlateImagesList.Add(filteredPlate);
               detectedLicensePlateRegionList.Add(box);
            }
         }
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
               contours != null; 
               contours = contours.HNext)
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
