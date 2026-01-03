//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
using Android.Graphics;
using Android.Widget;
#elif __MACCATALYST__
using AppKit;
using UIKit;
using CoreGraphics;
#elif __IOS__ 
using UIKit;
using CoreGraphics;
#elif WINDOWS
using Visibility = Microsoft.UI.Xaml.Visibility;
#endif

namespace Emgu.CV.Platform.Maui.UI
{

    /// <summary>
    /// View that holds an Emgu CV IInputArray
    /// </summary>
    public class CvImageView : Image
    {
        /// <summary>
        /// Get the native ImageView
        /// </summary>
#if __MACCATALYST__
        public UIImageView ImageView { get; set; }
#elif __IOS__
        public UIImageView ImageView { get; set; }
#elif __ANDROID__
        public ImageView ImageView { get; set; }
#elif WINDOWS
        public Microsoft.UI.Xaml.Controls.Image ImageView { get; set; }
#endif

#if __ANDROID__
        private Bitmap[] _renderBuffer = new Bitmap[2];
        private int _renderBufferIdx = 0;
#endif

        private VectorOfByte _imageStream = new VectorOfByte();
        private static Mutex _imageStreamMutex = new Mutex();

        private IInputArray _inputArray;

        /// <summary>
        /// Create an image viewer for Emgu CV image.
        /// </summary>
        public CvImageView()
            : base()
        {
            this.HandlerChanged += DisplayImage_HandlerChanged;
        }

        private void DisplayImage_HandlerChanged(object sender, EventArgs e)
        {
            if (this.Handler == null)
                //Don't do anything if Handler is not available
                return;

            var platformView = this.Handler.PlatformView;

#if __ANDROID__
            this.ImageView = platformView as ImageView;
#elif __MACCATALYST__
            this.ImageView = platformView as UIImageView;
#elif __IOS__
            this.ImageView = platformView as UIImageView;
#elif WINDOWS
            this.ImageView = platformView as Microsoft.UI.Xaml.Controls.Image;
#endif
            SetImage(_inputArray);
        }

        /// <summary>
        /// Set the image to be rendered
        /// </summary>
        /// <param name="image">The image to be rendered</param>
        public virtual void SetImage(IInputArray image)
        {
            _inputArray = image;

#if __MACCATALYST__
            if (this.ImageView != null)
            {
                 UIImage uiimage;
                 if (image == null)
                    uiimage = null;
                 else
                {
                    uiimage = image.ToUIImage ();
                }
                 this.Dispatcher.Dispatch(
                    () => {
                       UIImage oldImage = ImageView.Image;
                       ImageView.Image = uiimage;
                       if (oldImage != null)
                          oldImage.Dispose ();
                       if ((uiimage != null) && (ImageView.Frame.Size != uiimage.Size))
                          ImageView.Frame = new CGRect (CGPoint.Empty, uiimage.Size);
                    });
            }

#elif __IOS__
            if (this.ImageView != null)
            {
                UIImage uiimage;
                if (image == null)
                    uiimage = null;
                else
                {
                    uiimage = image.ToUIImage();
                }
                this.Dispatcher.Dispatch(
                   () =>
                   {
                       UIImage oldImage = ImageView.Image;
                       ImageView.Image = uiimage;
                       if (oldImage != null)
                           oldImage.Dispose();
                       if ((uiimage != null) && (ImageView.Frame.Size != uiimage.Size))
                           ImageView.Frame = new CGRect(CGPoint.Empty, uiimage.Size);
                   });
            }
#elif __ANDROID__

            if (this.ImageView != null)
            {
                if (image == null)
                {
                    this.Dispatcher.Dispatch(
                        () =>
                        {
                            ImageView?.SetImageBitmap(null);
                        });
                    return;
                }

                int bufferIdx = _renderBufferIdx;
                Bitmap buffer;
                _renderBufferIdx = (_renderBufferIdx + 1) % _renderBuffer.Length;

                using (InputArray iaImage = image.GetInputArray())
                using (Mat mat = iaImage.GetMat())
                {
                    if (_renderBuffer[bufferIdx] == null)
                    {
                        buffer = mat.ToBitmap();
                        _renderBuffer[bufferIdx] = buffer;
                    }
                    else
                    {
                        var size = iaImage.GetSize();
                        buffer = _renderBuffer[bufferIdx];
                        if (buffer.Width != size.Width || buffer.Height != size.Height)
                        {
                            buffer.Dispose();
                            buffer = mat.ToBitmap();
                            _renderBuffer[bufferIdx] = buffer;
                        }
                        else
                        {
                            mat.ToBitmap(buffer);
                        }
                    }
                }

                this.Dispatcher.Dispatch(
                    () =>
                    {
                        ImageView?.SetImageBitmap(buffer);
                    });
                /*
                var bitmap = image?.ToBitmap();

                this.Dispatcher.Dispatch(
                    () =>
                    {
                        this.ImageView.SetImageBitmap(bitmap);
                        this.ImageView.SetScaleType(ImageView.ScaleType.FitStart);
                        
                        if (bitmap != null)
                            bitmap.Dispose();

                    });*/
            }

#elif WINDOWS
            if (this.ImageView != null)
            {
                this.Dispatcher.Dispatch(
                    () =>
                    {
                        if (image == null)
                        {
                            this.ImageView.Source = null;
                            this.ImageView.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            var bmp = image?.ToWritableBitmap();
                            this.ImageView.Source = bmp;
                            this.ImageView.Visibility = Visibility.Visible;
                            this.WidthRequest = Math.Min(this.Width, bmp.PixelWidth);
                            this.HeightRequest = bmp.PixelHeight;
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

        }
    }
}
