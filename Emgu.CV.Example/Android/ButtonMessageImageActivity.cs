//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Emgu.CV;
using Emgu.CV.Structure;
using Xamarin.Media;

namespace AndroidExamples
{
   public class ButtonMessageImageActivity : Activity
   {
      private Button _clickButton;
      private TextView _messageText;
      private ImageView _imageView;
      private String _buttonText;
      private ProgressDialog _progress;
      //private Image<Bgr, Byte> _defaultImage;
      private MediaPicker _mediaPicker;

      public ButtonMessageImageActivity(String buttonText)
         : base()
      {
         _buttonText = buttonText;

         //dummy code to load the opencv libraries
         bool loaded = CvInvoke.CheckLibraryLoaded();
         //CvInvoke.CV_FOURCC('m', 'j', 'p', 'g');
      }

      /*
      private Image<Bgr, Byte> EnforceMaxSize(Image<Bgr, Byte> image, int maxWidth, int maxHeight)
      {
         if (image.Width > maxWidth || image.Height > maxHeight)
         {
            Image<Bgr, Byte> result = image.Resize(maxWidth, maxHeight, Emgu.CV.CvEnum.INTER.CV_INTER_NN, true);
            image.Dispose();
            return result;
         }
         else
            return image;
      }*/

      protected event EventHandler<Mat> OnImagePicked;

      private const int _pickImageRequestCode = 1000;
    
      public void PickImage(String defaultImageName)
      {
         if (_mediaPicker == null)
         {
            _mediaPicker = new MediaPicker(this);
         }
         String negative = _mediaPicker.IsCameraAvailable ? "Camera" : "Cancel";
         int result = GetUserResponse(this, "Use Image from", "Default", "Photo Library", negative);
         if (result > 0)
         {
            OnImagePicked(this, new Mat(Assets, defaultImageName));
         }
         else if (result == 0)
         {
            Intent intent = _mediaPicker.GetPickPhotoUI();
            StartActivityForResult(intent, _pickImageRequestCode);
         }
         else if (_mediaPicker.IsCameraAvailable)
         {
            Intent intent = _mediaPicker.GetTakePhotoUI(new StoreCameraMediaOptions());
            StartActivityForResult(intent, _pickImageRequestCode);
         }
      }

      protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
      {
         if (resultCode != Result.Canceled && requestCode == _pickImageRequestCode && OnImagePicked != null)
         {
            Mat image = GetImageFromTask(data.GetMediaFileExtraAsync(this), 800, 800);
            OnImagePicked(this, image);
         }
         base.OnActivityResult(requestCode, resultCode, data);
      }

      private static int GetUserResponse(Activity activity, String title, String positive, string neutral, string negative)
      {
         AutoResetEvent e = new AutoResetEvent(false);
         int value = 0;
         activity.RunOnUiThread(delegate
         {
            AlertDialog.Builder builder = new AlertDialog.Builder(activity);
            builder.SetTitle(title);
            builder.SetPositiveButton(positive, (s, er) => { value = 1; e.Set(); });
            builder.SetNeutralButton(neutral, (s, er) => { value = 0; e.Set(); });
            builder.SetNegativeButton(negative, (s, er) => { value = -1; e.Set(); });
            builder.Show();
         });
         e.WaitOne();
         return value;
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

      private static TResult GetResultFromTask<TResult>(Task<TResult> task)
         where TResult : class
      {
         if (task == null)
            return null;

         task.Wait();

         if (task.Status != TaskStatus.Canceled && task.Status != TaskStatus.Faulted)
            return task.Result;

         return null;
      }

      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);

         _clickButton = new Button(this);
         _clickButton.Text = _buttonText;
         _clickButton.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.FillParent,
            ViewGroup.LayoutParams.WrapContent);

         _messageText = new TextView(this);
         _messageText.SetTextAppearance(this, Android.Resource.Attribute.TextAppearanceSmall);
         _imageView = new ImageView(this);
         _imageView.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.FillParent,
            ViewGroup.LayoutParams.MatchParent);

         LinearLayout layout = new LinearLayout(this);
         layout.Orientation = Orientation.Vertical;
         layout.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.FillParent,
            ViewGroup.LayoutParams.FillParent);

         layout.AddView(_clickButton);
         layout.AddView(_messageText);
         layout.AddView(_imageView);

         ScrollView scrollView = new ScrollView(this);
         scrollView.AddView(layout);
         SetContentView(scrollView);

         _progress = new ProgressDialog(this) { Indeterminate = true };
         _progress.SetTitle("Processing");
         _progress.SetMessage("Please wait...");

         _clickButton.Click += delegate(Object sender, EventArgs e)
         {
            if (OnButtonClick != null)
            {
               _progress.Show();

               ThreadPool.QueueUserWorkItem(delegate
               {
                  try
                  {
                     OnButtonClick(sender, e);
                  }
#if !DEBUG
                  catch (Exception excpt)
                  {
                     while (excpt.InnerException != null)
                     {
                        excpt = excpt.InnerException;
                     }

                     RunOnUiThread(() =>
                     {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Error");
                        alert.SetMessage(excpt.Message);
                        alert.SetPositiveButton("OK", (s, er) => { });
                        alert.Show();
                     });
                  }
#endif
                  finally
                  {
                     RunOnUiThread( _progress.Hide);
                  }
               }
               );
            }
         };
      }

      public EventHandler<EventArgs> OnButtonClick;

      public void SetMessage(String message)
      {
         RunOnUiThread(() => { _messageText.Text = message; });
      }

      public void SetImageBitmap(Bitmap image)
      {
         RunOnUiThread(() => { _imageView.SetImageBitmap(image); });
      }
   }
}