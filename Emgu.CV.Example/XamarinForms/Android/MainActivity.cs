using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Emgu.CV.Structure;
using Xamarin.Media;
using Android.Content;
using System.Threading.Tasks;
using Android.Graphics;

namespace Emgu.CV.XamarinForms.Droid
{
   [Activity(Label = "Emgu.CV.XamarinForms", Icon = "@drawable/ic_launcher", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
   public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
   {

      private Emgu.CV.XamarinForms.App _app;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         global::Xamarin.Forms.Forms.Init(this, bundle);
         _app = new Emgu.CV.XamarinForms.App();
         LoadApplication(_app);


      }

      protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
      {
         if (requestCode == ButtonTextImagePage.PickImageRequestCode)
         {
            var p = _app.CurrentPage as ButtonTextImagePage;
            if (p != null)
            {
               if (resultCode != Result.Canceled)
               {
                  p.MatHandle = GetImageFromTask(data.GetMediaFileExtraAsync(this), 800, 800);
                  p.Continute();
               }
               else
               {
                  //cancelled
                  p.MatHandle = null;
                  p.Continute();
               }
            }
         }
         base.OnActivityResult(requestCode, resultCode, data);
      }

      private static TResult GetResultFromTask<TResult>(Task<TResult> task) where TResult : class
      {
         if (task == null)
            return null;

         task.Wait();

         if (task.Status != TaskStatus.Canceled && task.Status != TaskStatus.Faulted)
            return task.Result;

         return null;
      }

      private static Mat GetImageFromTask(Task<MediaFile> task, int maxWidth, int maxHeight)
      {
         MediaFile file = GetResultFromTask(task);
         if (file == null)
            return null;

         int rotation = 0;
         Android.Media.ExifInterface exif = new Android.Media.ExifInterface(file.Path);
         int orientation = exif.GetAttributeInt(Android.Media.ExifInterface.TagOrientation, int.MinValue);
         switch (orientation)
         {
            case (int)Android.Media.Orientation.Rotate270:
               rotation = 270;
               break;
            case (int)Android.Media.Orientation.Rotate180:
               rotation = 180;
               break;
            case (int)Android.Media.Orientation.Rotate90:
               rotation = 90;
               break;
         }

         using (Bitmap bmp = BitmapFactory.DecodeFile(file.Path))
         {
            if (bmp.Width <= maxWidth && bmp.Height <= maxHeight && rotation == 0)
            {
               return new Mat(bmp);
            }
            else
            {
               using (Matrix matrix = new Matrix())
               {
                  if (bmp.Width > maxWidth || bmp.Height > maxHeight)
                  {
                     double scale = Math.Min((double)maxWidth / bmp.Width, (double)maxHeight / bmp.Height);
                     matrix.PostScale((float)scale, (float)scale);
                  }
                  if (rotation != 0)
                     matrix.PostRotate(rotation);

                  using (Bitmap scaled = Bitmap.CreateBitmap(bmp, 0, 0, bmp.Width, bmp.Height, matrix, true))
                  {
                     return new Mat(scaled);
                  }
               }
            }
         }
      }
   }
}

