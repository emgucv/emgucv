using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace SURFFeatureExample
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Run();
      }

      static void Run()
      {
         Image<Gray, Byte> objectImage = new Image<Gray, byte>("box.png");
         //objectImage = objectImage.Resize(400, 400, true);
         DateTime t1 = DateTime.Now;
         MCvSURFParams param1 = new MCvSURFParams(500, false);
         SURFFeature[] objectFeatures = objectImage.ExtractSURF(ref param1);
         
         Image<Gray, Byte> image = new Image<Gray, byte>("box_in_scene.png");
         //image = image.Resize(400, 400, true);
         t1 = DateTime.Now;
         MCvSURFParams param2 = new MCvSURFParams(500, false);
         SURFFeature[] imageFeatures = image.ExtractSURF(ref param2);
         
         Image<Gray, Byte> res = new Image<Gray, byte>(Math.Max(objectImage.Width, image.Width), objectImage.Height + image.Height);
         res.ROI = new System.Drawing.Rectangle(0, 0, objectImage.Width, objectImage.Height);
         objectImage.Copy(res, null);
         res.ROI = new System.Drawing.Rectangle(0, objectImage.Height, image.Width, image.Height);
         image.Copy(res, null);
         res.ROI = System.Drawing.Rectangle.Empty;

         t1 = DateTime.Now;
         List<PointF> list1 = new List<PointF>();
         List<PointF> list2 = new List<PointF>();
         foreach (SURFFeature f in objectFeatures)
         {
            double[] distance = Array.ConvertAll<SURFFeature, double>(imageFeatures,
               delegate(SURFFeature imgFeature)
               {
                  if (imgFeature.Point.laplacian != f.Point.laplacian)
                     return -1;
                  return CvInvoke.cvNorm(imgFeature.Descriptor, f.Descriptor, Emgu.CV.CvEnum.NORM_TYPE.CV_L2, IntPtr.Zero);
               });

            int closestIndex = 0;
            int secondClosestIndex = 0;

            for (int i = 0; i < distance.Length; i++)
            {
               if (distance[i] >= 0)
               {
                  if (distance[i] < distance[closestIndex] || distance[closestIndex] == -1)
                  {
                     secondClosestIndex = closestIndex;
                     closestIndex = i;
                  }
               }
            }
            if (distance[closestIndex] < 0.6 * distance[secondClosestIndex])
            { //If this is almost a unique match
               SURFFeature match = imageFeatures[closestIndex];
               list1.Add(f.Point.pt);
               list2.Add(match.Point.pt);

               res.Draw(new LineSegment2D(
                  new Point( (int)f.Point.pt.X, (int)f.Point.pt.Y),  
                  new Point( (int)match.Point.pt.X, (int)match.Point.pt.Y + objectImage.Height)), 
                  new Gray(0), 1);
            }
         }

         Matrix<float> homographyMatrix = CameraCalibration.FindHomography(list1.ToArray(), list2.ToArray(), HOMOGRAPHY_METHOD.RANSAC, 3);

         System.Drawing.Rectangle rect = objectImage.ROI;
         Matrix<float> orginalCornerCoordinate = new Matrix<float>(new float[,] 
            {{  rect.Left, rect.Bottom, 1.0f},
               { rect.Right, rect.Bottom, 1.0f},
               { rect.Right, rect.Top, 1.0f},
               { rect.Left, rect.Top, 1.0f}});

         Matrix<float> destCornerCoordinate = homographyMatrix * orginalCornerCoordinate.Transpose();
         Point[] destCornerPoints = new Point[4];
         float[,] destCornerCoordinateArray = destCornerCoordinate.Data;
         for (int i = 0; i < destCornerPoints.Length; i++)
         {
            float denominator = destCornerCoordinateArray[2, i];
            destCornerPoints[i] = new Point(
               (int)(destCornerCoordinateArray[0, i] / denominator),
               (int)(destCornerCoordinateArray[1, i] / denominator) + objectImage.Height);
         }

         res.DrawPolyline(destCornerPoints, true, new Gray(255.0), 5);

         Application.Run(new ImageViewer(res));
      }


   }
}
