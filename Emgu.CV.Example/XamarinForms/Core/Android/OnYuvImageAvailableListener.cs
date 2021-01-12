//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XamarinForms
{
    class OnYuvImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        private byte[] _data = null;
        private Mat _bgrMat = new Mat();
        private Mat _rotatedMat = new Mat();

        private YUV420Converter _yuv420Converter;
        private Bitmap[] _bitmapSrcBuffer = new Bitmap[2];
        private int _bitmapBufferIdx = 0;

        public EventHandler<Mat> OnImageProcessed;

        public void OnImageAvailable(ImageReader reader)
        {
            Image image = reader.AcquireLatestImage();
            if (image == null)
                return;

            Image.Plane[] planes = image.GetPlanes();
            int totalLength = 0;
            for (int i = 0; i < planes.Length; i++)
            {
                Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                totalLength += buffer.Remaining();
            }

            if (_data == null || _data.Length != totalLength)
            {
                _data = new byte[totalLength];
            }
            int offset = 0;
            for (int i = 0; i < planes.Length; i++)
            {
                Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                int length = buffer.Remaining();

                buffer.Get(_data, offset, length);
                offset += length;
            }

            if (_yuv420Converter == null)
                _yuv420Converter = new YUV420Converter(Android.App.Application.Context);

            if (_bitmapSrcBuffer[_bitmapBufferIdx] == null || image.Width != (_bitmapSrcBuffer[_bitmapBufferIdx].Width) || (image.Height != _bitmapSrcBuffer[_bitmapBufferIdx].Height))
            {
                _bitmapSrcBuffer[_bitmapBufferIdx] = Bitmap.CreateBitmap(image.Width, image.Height, Bitmap.Config.Argb8888);
            }
            Bitmap bmpSrc = _bitmapSrcBuffer[_bitmapBufferIdx];

            _yuv420Converter.YUV_420_888_toRGBIntrinsics(image.Width, image.Height, _data, bmpSrc);

            using (Mat m = new Mat(bmpSrc.Height, bmpSrc.Width, DepthType.Cv8U, 4, bmpSrc.LockPixels(),
                bmpSrc.Width * 4))
            {
                bmpSrc.UnlockPixels();
                CvInvoke.CvtColor(m, _bgrMat, ColorConversion.Bgra2Bgr);

                //Rotate 90 degree by transpose and flip
                CvInvoke.Transpose(_bgrMat, _rotatedMat);
                CvInvoke.Flip(_rotatedMat, _rotatedMat, FlipType.Horizontal);

                //apply a simple invert filter
                //CvInvoke.BitwiseNot(_rotatedMat, _rotatedMat);
            }

            if (OnImageProcessed != null)
            {
                OnImageProcessed(reader, _rotatedMat);
            }

            _bitmapBufferIdx = (_bitmapBufferIdx + 1) % _bitmapSrcBuffer.Length;


            image.Close();
            //image.Dispose();
        }
    }
}

#endif