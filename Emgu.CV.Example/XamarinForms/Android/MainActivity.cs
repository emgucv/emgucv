using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;

using Emgu.CV.Structure;
using Android.Content;
using System.Threading.Tasks;
using Android.Graphics;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.CurrentActivity;

namespace Emgu.CV.XamarinForms.Droid
{
    [Activity(Label = "Emgu.CV.XamarinForms", Icon = "@drawable/ic_launcher", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {

        private Emgu.CV.XamarinForms.App _app;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CrossCurrentActivity.Current.Init(this, bundle);

            CheckAppPermissions();

            _app = new Emgu.CV.XamarinForms.App();
            LoadApplication(_app);

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

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.Camera, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera };
                    RequestPermissions(permissions, 1);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}



