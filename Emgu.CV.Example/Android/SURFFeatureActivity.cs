using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using SURFFeatureExample;

namespace AndroidExamples
{
   [Activity(Label = "SURF Feature")]
   public class SURFFeatureActivity : ButtonMessageImageActivity
   {
      public SURFFeatureActivity()
         : base("Match SURF Features")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         OnButtonClick += delegate
         {
            long time;
            using (Image<Gray, Byte> box = new Image<Gray, byte>(Assets, "box.png"))
            using (Image<Gray, Byte> boxInScene = new Image<Gray, byte>(Assets, "box_in_scene.png"))
            using (Image<Bgr, Byte> result = DrawMatches.Draw(box, boxInScene, out time))
            {
               SetImageBitmap(result.ToBitmap());
               SetMessage(String.Format("Matched in {0} milliseconds.", time));
               /*
               HomographyMatrix homography;
               VectorOfKeyPoint modelKeyPoints;
               VectorOfKeyPoint observedKeyPoints;
               Matrix<int> indices;
               Matrix<byte> mask;

               DrawMatches.FindMatch(box, boxInScene, out time, out modelKeyPoints, out observedKeyPoints, out indices, out mask, out homography);

               #region draw the projected region on the image
               if (homography != null)
               {  //draw a rectangle along the projected model
                  System.Drawing.Rectangle rect = box.ROI;
                  System.Drawing.PointF[] pts = new System.Drawing.PointF[] { 
                     new System.Drawing.PointF(rect.Left, rect.Bottom),
                     new System.Drawing.PointF(rect.Right, rect.Bottom),
                     new System.Drawing.PointF(rect.Right, rect.Top),
                     new System.Drawing.PointF(rect.Left, rect.Top)};
                  homography.ProjectPoints(pts);

                  Bitmap bmp = null;
                  using (Image<Gray, Byte> result = boxInScene.ConcateVertical(box))
                  {
                     bmp = result.ToBitmap();
                  }
                  using (Canvas c = new Canvas(bmp))
                  using (Paint p = new Paint())
                  {
                     p.StrokeWidth = 4;
                     p.Color = Android.Graphics.Color.Red;
                     p.SetStyle(Paint.Style.Stroke);

                     System.Drawing.PointF p0 = pts[pts.Length - 1];
                     for (int i = 0; i < pts.Length; i++)
                     {
                        System.Drawing.PointF p1 = pts[i];
                        c.DrawLine(p0.X, p0.Y, p1.X, p1.Y, p);
                        p0 = p1;
                     }

                     SetMessage(String.Format("Matched in {0} milliseconds.", time));
                     SetImageBitmap(bmp);
                  }
               }
               #endregion
               */

            }
         };
      }
   }
}

