//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.Util;
using Emgu.CV;
using Emgu.CV.CvEnum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if __ANDROID__
using Android.Widget;
#elif __IOS__
using UIKit;
using CoreGraphics;
#elif WINDOWS
using Visibility = Microsoft.UI.Xaml.Visibility;
#endif

namespace MauiDemoApp
{
   
    public class CvImageView : Image
    {
#if __MACOS__
        public NSImageView NSImageView { get; set; }
#elif __IOS__
        public UIImageView UIImageView { get; set; }
#elif __ANDROID__
        public ImageView ImageView { get; set; }
#elif WINDOWS
        public Microsoft.UI.Xaml.Controls.Image ImageView { get; set; }
#endif

        

        private VectorOfByte _imageStream = new VectorOfByte();
        private static Mutex _imageStreamMutex = new Mutex();

        private IInputArray _inputArray;

        public CvImageView()
            : base()
        {
            this.HandlerChanged += DisplayImage_HandlerChanged;
        }

        private void DisplayImage_HandlerChanged(object sender, EventArgs e)
        {
            var platformView = this.Handler.PlatformView;

#if __ANDROID__
            this.ImageView = platformView as ImageView;
#elif WINDOWS
            this.ImageView = platformView as Microsoft.UI.Xaml.Controls.Image;
#endif
            SetImage(_inputArray);

        }

        public virtual void SetImage(IInputArray image)
        {
            _inputArray = image;
#if __IOS__
         UIImage uiimage;
         if (image == null)
            uiimage = null;
         else
            uiimage = image.ToUIImage ();
         Device.BeginInvokeOnMainThread (
            () => {
               UIImage oldImage = UIImageView.Image;
               UIImageView.Image = uiimage;
               if (oldImage != null)
                  oldImage.Dispose ();
               if ((uiimage != null) && (UIImageView.Frame.Size != uiimage.Size))
                  UIImageView.Frame = new CGRect (CGPoint.Empty, uiimage.Size);
               UIImageView.Hidden = false;
               this.IsVisible = false;
            });

#elif __ANDROID__

            if (this.ImageView != null)
            {
                Android.Graphics.Bitmap bitmap;
                if (image == null)
                    bitmap = null;
                else
                    bitmap = image.ToBitmap();

                this.Dispatcher.Dispatch(
                    () =>
                    {
                        this.ImageView.SetImageBitmap(bitmap);

                        if (bitmap != null)
                            bitmap.Dispose();

                    });
            }
#elif __MACOS__
            
            NSImage nsimage;
            if (image == null)
                nsimage = null;
            else
                nsimage = image.ToNSImage();
            Device.BeginInvokeOnMainThread(
               () => {

                   NSImage oldImage = NSImageView.Image;
                   NSImageView.Image = nsimage;
                   if (oldImage != null)
                       oldImage.Dispose();
                   if ((nsimage != null) && (NSImageView.Frame.Size != nsimage.Size))
                       NSImageView.Frame = new CGRect(CGPoint.Empty, nsimage.Size);
                   NSImageView.Hidden = false;
                   DisplayImage.IsVisible = false;
               });
#elif WINDOWS
            if (this.ImageView != null)
            {
            this.Dispatcher.Dispatch(
                () => {
               if (image == null)
               {
                  this.ImageView.Source = null;
                  this.ImageView.Visibility = Visibility.Collapsed;
               }
               else
               {

                  this.ImageView.Source = image.ToWritableBitmap();
                  //this.ImageView.Source = image.ToBitmapSource();
                  this.ImageView.Visibility = Visibility.Visible;

               }
            });
            }
#else
            if (image == null)
            {
                this.Dispatcher.Dispatch(
                    () =>
                    {
                        this.Source = null;
                        this.IsVisible = false;
                    });
                return;
            }

            int width = 0;
            int height = 0;
            using (InputArray iaImage = image.GetInputArray())
            {
                System.Drawing.Size s = iaImage.GetSize();
                width = s.Width;
                height = s.Height;
            }

            _imageStreamMutex.WaitOne();
            CvInvoke.Imencode(
                ".png",
                image,
                _imageStream,
                new KeyValuePair<ImwriteFlags, int>(ImwriteFlags.PngCompression, 0));

            _imageStreamMutex.ReleaseMutex();
            
            
            this.Dispatcher.Dispatch(
            //this.Dispatcher.Dispatch(
                //Device.BeginInvokeOnMainThread(
                () =>
                {

                    this.IsVisible = true;
                    this.Aspect = Aspect.Center;
                    this.WidthRequest = Math.Min(this.Width, width);
                    this.HeightRequest = height;

                    this.Source = ImageSource.FromStream(() =>
                    {
                        MemoryStream ms = new MemoryStream();
                        _imageStreamMutex.WaitOne();
                        _imageStream.Position = 0;
                        _imageStream.CopyTo(ms);
                        _imageStreamMutex.ReleaseMutex();
                        //var data = ms.ToArray();
                        //File.WriteAllBytes("D:\\tmp\\out.png", data);
                        return ms;
                    });
                    //this.InvalidateMeasure();
                    //this.DisplayImage.


                });
            //}
#endif
            //Microsoft.UI.Xamal.Media.Imaging
            //this.DisplayImage.
        }
    }
}
