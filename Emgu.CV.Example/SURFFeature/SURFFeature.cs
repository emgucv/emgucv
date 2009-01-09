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
         Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png");

         #region extract features from the object image
         MCvSURFParams param1 = new MCvSURFParams(500, false);
         SURFFeature[] modelFeatures = modelImage.ExtractSURF(ref param1);
         SURFFeature[] modelFeaturesPositiveLaplacian = Array.FindAll<SURFFeature>(modelFeatures, delegate(SURFFeature f) { return f.Point.laplacian >= 0; });
         SURFFeature[] modelFeaturesNegativeLaplacian = Array.FindAll<SURFFeature>(modelFeatures, delegate(SURFFeature f) { return f.Point.laplacian < 0; });

         //Create feature trees for the given features
         FeatureTree featureTreePositiveLaplacian = new FeatureTree(
            Array.ConvertAll<SURFFeature, Matrix<float>>(
               modelFeaturesPositiveLaplacian,
               delegate(SURFFeature f) { return f.Descriptor; }));
         FeatureTree featureTreeNegativeLaplacian = new FeatureTree(
            Array.ConvertAll<SURFFeature, Matrix<float>>(
               modelFeaturesNegativeLaplacian,
               delegate(SURFFeature f) { return f.Descriptor; }));
         #endregion

         Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");

         #region extract features from the observed image
         MCvSURFParams param2 = new MCvSURFParams(500, false);
         SURFFeature[] imageFeatures = observedImage.ExtractSURF(ref param2);
         SURFFeature[] imageFeaturesPositiveLaplacian = Array.FindAll<SURFFeature>(imageFeatures, delegate(SURFFeature f) { return f.Point.laplacian >= 0; });
         SURFFeature[] imageFeaturesNegativeLaplacian = Array.FindAll<SURFFeature>(imageFeatures, delegate(SURFFeature f) { return f.Point.laplacian < 0; });
         #endregion

         #region Merge the object image and the observed image into one image for display
         Image<Gray, Byte> res = new Image<Gray, byte>(Math.Max(modelImage.Width, observedImage.Width), modelImage.Height + observedImage.Height);
         res.ROI = new System.Drawing.Rectangle(0, 0, modelImage.Width, modelImage.Height);
         modelImage.Copy(res, null);
         res.ROI = new System.Drawing.Rectangle(0, modelImage.Height, observedImage.Width, observedImage.Height);
         observedImage.Copy(res, null);
         res.ROI = Rectangle.Empty;
         #endregion

         double matchDistanceRatio = 0.8;
         List<PointF> modelPoints = new List<PointF>();
         List<PointF> observePoints = new List<PointF>();

         #region using Feature Tree to match feature
         Matrix<float>[] imageFeatureDescriptorsPositiveLaplacian = Array.ConvertAll<SURFFeature, Matrix<float>>(
            imageFeaturesPositiveLaplacian,
            delegate(SURFFeature f) { return f.Descriptor; });
         Matrix<float>[] imageFeatureDescriptorsNegativeLaplacian = Array.ConvertAll<SURFFeature, Matrix<float>>(
            imageFeaturesNegativeLaplacian,
            delegate(SURFFeature f) { return f.Descriptor; });
         Matrix<Int32> result1;
         Matrix<double> dist1;

         featureTreePositiveLaplacian.FindFeatures(imageFeatureDescriptorsPositiveLaplacian, out result1, out dist1, 2, 20);
         MatchSURFFeatureWithFeatureTree(
           modelFeaturesPositiveLaplacian,
           imageFeaturesPositiveLaplacian,
           matchDistanceRatio, result1.Data, dist1.Data, modelPoints, observePoints);

         featureTreeNegativeLaplacian.FindFeatures(imageFeatureDescriptorsNegativeLaplacian, out result1, out dist1, 2, 20);
         MatchSURFFeatureWithFeatureTree(
              modelFeaturesNegativeLaplacian,
              imageFeaturesNegativeLaplacian,
              matchDistanceRatio, result1.Data, dist1.Data, modelPoints, observePoints);
         #endregion

         Matrix<float> homographyMatrix = CameraCalibration.FindHomography(
            modelPoints.ToArray(), //points on the object image
            observePoints.ToArray(), //points on the observed image
            HOMOGRAPHY_METHOD.RANSAC,
            3).Convert<float>();

         #region draw the projected object in observed image
         for (int i = 0; i < modelPoints.Count; i++)
         {
            PointF p = observePoints[i];
            p.Y += modelImage.Height;
            res.Draw(new LineSegment2DF(modelPoints[i], p), new Gray(0), 1);
         }

         System.Drawing.Rectangle rect = modelImage.ROI;
         Matrix<float> orginalCornerCoordinate = new Matrix<float>(new float[,] 
            {{  rect.Left, rect.Bottom, 1.0f},
               { rect.Right, rect.Bottom, 1.0f},
               { rect.Right, rect.Top, 1.0f},
               { rect.Left, rect.Top, 1.0f}});

         Matrix<float> destCornerCoordinate = homographyMatrix * orginalCornerCoordinate.Transpose();
         float[,] destCornerCoordinateArray = destCornerCoordinate.Data;

         Point[] destCornerPoints = new Point[4];
         for (int i = 0; i < destCornerPoints.Length; i++)
         {
            float denominator = destCornerCoordinateArray[2, i];
            destCornerPoints[i] = new Point(
               (int)(destCornerCoordinateArray[0, i] / denominator),
               (int)(destCornerCoordinateArray[1, i] / denominator) + modelImage.Height);
         }

         res.DrawPolyline(destCornerPoints, true, new Gray(255.0), 5);
         #endregion

         ImageViewer.Show(res);
      }

      private static void MatchSURFFeatureWithFeatureTree(SURFFeature[] modelFeatures, SURFFeature[] imageFeatures, double matchDistanceRatio, int[,] result1, double[,] dist1, List<PointF> modelPointList, List<PointF> imagePointList)
      {
         for (int i = 0; i < result1.GetLength(0); i++)
         {
            int bestMatchedIndex = dist1[i, 0] < dist1[i, 1] ? result1[i, 0] : result1[i, 1];
            int secondBestMatchedIndex = dist1[i, 0] < dist1[i, 1] ? result1[i, 1] : result1[i, 0];

            SURFFeature bestMatchedModelPoint = bestMatchedIndex >= 0 ? modelFeatures[bestMatchedIndex] : null;
            SURFFeature secondBestMatchedModelPoint = secondBestMatchedIndex > 0 ? modelFeatures[secondBestMatchedIndex] : null;
            if (bestMatchedModelPoint != null)
            {
               double distanceRatio = dist1[i, 0] / dist1[i, 1];
               if (secondBestMatchedModelPoint == null || distanceRatio <= matchDistanceRatio || distanceRatio >= (1.0 / matchDistanceRatio))
               {  //this is a unique / almost unique match
                  modelPointList.Add(bestMatchedModelPoint.Point.pt);
                  imagePointList.Add(imageFeatures[i].Point.pt);
               }
            }
         }
      }
   }
}
