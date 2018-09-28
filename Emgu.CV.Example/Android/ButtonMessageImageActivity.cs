//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Emgu.CV;
using Emgu.CV.Structure;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;


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

        public ButtonMessageImageActivity(String buttonText)
            : base()
        {
            _buttonText = buttonText;

            //dummy code to load the opencv libraries
            bool loaded = CvInvoke.CheckLibraryLoaded();
        
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

        public async void PickImage(String defaultImageName)
        {
            await CrossMedia.Current.Initialize();
            String pickImgString = "Use Image from";

            bool haveCameraOption =
                (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported);
            bool havePickImgOption =
                CrossMedia.Current.IsPickVideoSupported;

            String action;
            if (haveCameraOption & havePickImgOption)
            {
                int result = GetUserResponse(this, pickImgString, "Default", "Photo Library", "Camera");
                if (result == 1)
                    action = "Default";
                else if (result == 0)
                    action = "Photo Library";
                else
                {
                    action = "Camera";
                }

            }
            else if (havePickImgOption)
            {
                int result = GetUserResponse(this, pickImgString, "Default", "Photo Library", "Cancel");

                if (result == 1)
                    action = "Default";
                else if (result == 0)
                    action = "Photo Library";
                else
                {
                    action = null;
                }
            }
            else
            {
                action = "Default";
            }


            if (action.Equals("Default"))
            {
#if __ANDROID__
                Mat m = new Mat(this.Assets, defaultImageName);
#else
                Mat m = CvInvoke.Imread(defaultImageName);            
#endif
                OnImagePicked(this, m);
            }
            else if (action.Equals("Photo Library"))
            {
                var photoResult = await CrossMedia.Current.PickPhotoAsync();
                if (photoResult == null) //cancelled
                    return;
                Mat m = CvInvoke.Imread(photoResult.Path);
                //mats[i] = photoResult.Path;
                OnImagePicked(this, m);
            }
            else if (action.Equals("Camera"))
            {
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Emgu",
                    Name = $"{DateTime.UtcNow}.jpg"
                };
                var takePhotoResult = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                if (takePhotoResult == null) //cancelled
                    return;
                Mat m = new Mat(takePhotoResult.Path);
                OnImagePicked(this, m);
            }
        }

        private static int GetUserResponse(Activity activity, String title, String positive, string neutral,
            string negative)
        {
            AutoResetEvent e = new AutoResetEvent(false);
            int value = 0;
            activity.RunOnUiThread(delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                builder.SetTitle(title);
                builder.SetPositiveButton(positive, (s, er) =>
                {
                    value = 1;
                    e.Set();
                });
                builder.SetNeutralButton(neutral, (s, er) =>
                {
                    value = 0;
                    e.Set();
                });
                builder.SetNegativeButton(negative, (s, er) =>
                {
                    value = -1;
                    e.Set();
                });
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

        protected void SetProgressMessage(String message)
        {
            if (_progress != null)
            {
                RunOnUiThread(() =>
                {
                    _progress.SetMessage(message);
                });
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.button_message_image_layout);

            _clickButton = FindViewById<Button>(Resource.Id.button_message_image_layout_button);
            _clickButton.Text = _buttonText;
            _messageText = FindViewById<TextView>(Resource.Id.button_message_image_layout_message);
            _messageText.Text = String.Empty;
            _imageView = FindViewById<ImageView>(Resource.Id.button_message_image_layout_image);
            _progress = new ProgressDialog(this) { Indeterminate = true };
            _progress.SetTitle("Processing");
            _progress.SetMessage("Please wait...");

            _clickButton.Click += delegate (Object sender, EventArgs e)
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
                                RunOnUiThread(_progress.Hide);
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

        private Bitmap[] _renderBuffer = new Bitmap[2];
        private int _renderBufferIdx = 0;

        public void SetImage(Mat image)
        {
            RunOnUiThread(() =>
            {
                if (_renderBuffer[_renderBufferIdx] == null)
                {
                    _renderBuffer[_renderBufferIdx] = image.ToBitmap();
                }
                else
                {
                    Bitmap buffer = _renderBuffer[_renderBufferIdx];
                    if (buffer.Width != image.Width || buffer.Height != image.Height)
                    {
                        buffer.Dispose();
                        _renderBuffer[_renderBufferIdx] = image.ToBitmap();
                    }
                    else
                    {
                        image.ToBitmap(buffer);
                    }

                }

                _imageView.SetImageBitmap(_renderBuffer[_renderBufferIdx]);
            });
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}