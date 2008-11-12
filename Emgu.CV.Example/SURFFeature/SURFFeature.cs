using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;

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
         res.ROI = new Rectangle<double>(new MCvRect(0, 0, objectImage.Width, objectImage.Height));
         objectImage.Copy(res, null);
         res.ROI = new Rectangle<double>(new MCvRect(0, objectImage.Height, image.Width, image.Height) );
         image.Copy(res, null);
         res.ROI = null;

         t1 = DateTime.Now;
         List<Point2D<float>> list1 = new List<Point2D<float>>();
         List<Point2D<float>> list2 = new List<Point2D<float>>();
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
               Point2D<float> p1 = new Point2D<float>((float)f.Point.pt.x, (float)f.Point.pt.y);
               SURFFeature match = imageFeatures[closestIndex];
               Point2D<float> p2 = new Point2D<float>((float)match.Point.pt.x, (float)match.Point.pt.y);
               list1.Add(p1);
               list2.Add(p2);

               Point2D<float> p = p2.Convert<float>();
               p.Y += objectImage.Height;
               res.Draw(new LineSegment2D<int>(p1.Convert<int>(), p.Convert<int>()), new Gray(0), 1);
            }
         }

         Matrix<float> homographyMatrix = CameraCalibration.FindHomography(list1.ToArray(), list2.ToArray(), HOMOGRAPHY_METHOD.RANSAC, 3);
         Rectangle<double> rect = objectImage.ROI;

         Point2D<double>[] pts = new Point2D<double>[]
         {
            HomographyTransform( rect.BottomLeft, homographyMatrix ),
            HomographyTransform( rect.BottomRight, homographyMatrix ),
            HomographyTransform( rect.TopRight, homographyMatrix ),
            HomographyTransform( rect.TopLeft, homographyMatrix )
         };

         foreach (Point2D<double> p in pts)
         {
            p.Y += objectImage.Height;
         }

         res.DrawPolyline(pts, true, new Gray(255.0), 5);

         Application.Run(new ImageViewer(res));
      }

      private static Point2D<double> HomographyTransform(Point2D<double> p, Matrix<float> homographyMatrix)
      {
         Matrix<float> pMat = new Matrix<float>(p.Convert<float>().Resize(3).Coordinate);
         pMat[2, 0] = 1.0f;
         pMat = homographyMatrix * pMat;
         pMat = pMat / (double)pMat[2, 0];
         return new Point2D<double>((double)pMat[0, 0], (double)pMat[1, 0]);
      }
   }
}
