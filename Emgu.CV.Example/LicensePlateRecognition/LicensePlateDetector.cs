//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Util;

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
         _ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED);
         _ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ-1234567890");
      }

      /// <summary>
      /// Compute the white pixel mask for the given image. 
      /// A white pixel is a pixel where:  satuation &lt; 40 AND value &gt; 200
      /// </summary>
      /// <param name="image">The color image to find white mask from</param>
      /// <returns>The white pixel mask</returns>
      private static Image<Gray, Byte> GetWhitePixelMask(Image<Bgr, byte> image)
      {
         using (Image<Hsv, Byte> hsv = image.Convert<Hsv, Byte>())
         {
            Image<Gray, Byte>[] channels = hsv.Split();

            try
            {
               //channels[1] is the mask for satuation less than 40, this is the mask for either white or black pixels
               channels[1]._ThresholdBinaryInv(new Gray(40), new Gray(255));

               //channels[2] is the mask for bright pixels
               channels[2]._ThresholdBinary(new Gray(200), new Gray(255));

               CvInvoke.cvAnd(channels[1], channels[2], channels[0], IntPtr.Zero);
            }
            finally
            {
               channels[1].Dispose();
               channels[2].Dispose();
            }
            return channels[0];
         }
      }

      /// <summary>
      /// Detect license plate from the given image
      /// </summary>
      /// <param name="img">The image to search license plate from</param>
      /// <param name="licensePlateImagesList">A list of images where the detected license plate regions are stored</param>
      /// <param name="filteredLicensePlateImagesList">A list of images where the detected license plate regions (with noise removed) are stored</param>
      /// <param name="detectedLicensePlateRegionList">A list where the regions of license plate (defined by an MCvBox2D) are stored</param>
      /// <returns>The list of words for each license plate</returns>
      public List<String> DetectLicensePlate(
         Image<Bgr, byte> img, 
         List<Image<Gray, Byte>> licensePlateImagesList, 
         List<Image<Gray, Byte>> filteredLicensePlateImagesList, 
         List<MCvBox2D> detectedLicensePlateRegionList)
      {
         List<String> licenses = new List<String>();
         using (Image<Gray, byte> gray = img.Convert<Gray, Byte>())
         //using (Image<Gray, byte> gray = GetWhitePixelMask(img))
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
         List<String> licenses)
      {
         for (; contours != null; contours = contours.HNext)
         {
            int numberOfChildren = GetNumberOfChildren(contours);      
            //if it does not contains any children (charactor), it is not a license plate region
            if (numberOfChildren == 0) continue;

            if (contours.Area > 400)
            {
               if (numberOfChildren < 3) 
               {
                  //If the contour has less than 3 children, it is not a license plate (assuming license plate has at least 3 charactor)
                  //However we should search the children of this contour to see if any of them is a license plate
                  FindLicensePlate(contours.VNext, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
                  continue;
               }

               MCvBox2D box = contours.GetMinAreaRect();
               if (box.angle < -45.0)
               {
                  float tmp = box.size.Width;
                  box.size.Width = box.size.Height;
                  box.size.Height = tmp;
                  box.angle += 90.0f;
               }
               else if (box.angle > 45.0)
               {
                  float tmp = box.size.Width;
                  box.size.Width = box.size.Height;
                  box.size.Height = tmp;
                  box.angle -= 90.0f;
               }

               double whRatio = (double)box.size.Width / box.size.Height;
               if (!(3.0 < whRatio && whRatio < 10.0))
               //if (!(1.0 < whRatio && whRatio < 2.0))
               {  //if the width height ratio is not in the specific range,it is not a license plate 
                  //However we should search the children of this contour to see if any of them is a license plate
                  Contour<Point> child = contours.VNext;
                  if (child != null)
                     FindLicensePlate(child, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
                  continue;
               }

               using (Image<Gray, Byte> tmp1 = gray.Copy(box))
               //resize the license plate such that the front is ~ 10-12. This size of front results in better accuracy from tesseract
               using (Image<Gray, Byte> tmp2 = tmp1.Resize(240, 180, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true))
               {
                  //removes some pixels from the edge
                  int edgePixelSize = 2;
                  tmp2.ROI = new Rectangle(new Point(edgePixelSize, edgePixelSize), tmp2.Size - new Size(2 * edgePixelSize, 2 * edgePixelSize));
                  Image<Gray, Byte> plate = tmp2.Copy();

                  Image<Gray, Byte> filteredPlate = FilterPlate(plate);

                  Tesseract.Charactor[] words;
                  StringBuilder strBuilder = new StringBuilder();
                  using (Image<Gray, Byte> tmp = filteredPlate.Clone())
                  {
                     _ocr.Recognize(tmp);
                     words = _ocr.GetCharactors();
                     
                     if (words.Length == 0) continue;

                     for (int i = 0; i < words.Length; i++)
                     {
                        strBuilder.Append(words[i].Text);
                     }
                  }

                  licenses.Add(strBuilder.ToString());
                  licensePlateImagesList.Add(plate);
                  filteredLicensePlateImagesList.Add(filteredPlate);
                  detectedLicensePlateRegionList.Add(box);

               }
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
