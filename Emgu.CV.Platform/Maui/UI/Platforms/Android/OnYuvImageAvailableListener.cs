//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace Emgu.CV.Platform.Maui.UI
{
    class OnYuvImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        private byte[] _data = null;
        private Mat _bgrMat = null;
        private Mat _rotatedMat = new Mat();

        private GComputation _gComputation = null;
        public OnYuvImageAvailableListener()
        {
            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveGapi = (openCVConfigDict["HAVE_OPENCV_GAPI"] != 0);

            if (haveGapi)
            {
                using (GMat inYuvMat = new GMat())
                using (GMat rgbMat = GapiInvoke.I4202BGR(inYuvMat))
                    //The following two step will rotate the image 90 degrees
                using (GMat transposeMat = GapiInvoke.Transpose(rgbMat))
                using (GMat rotatedMat = GapiInvoke.Flip(transposeMat, FlipType.Horizontal))
                {
                    _gComputation = new GComputation(inYuvMat, rotatedMat);
                }
            }
            else
            {
                _bgrMat = new Mat();
            }
        }

        public EventHandler<Mat> OnImageProcessed;

        public void OnImageAvailable(ImageReader reader)
        {
            if (OnImageProcessed != null)
            {
                var image = reader.AcquireLatestImage();
                if (image == null)
                    return;

                var planes = image.GetPlanes();
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

                GCHandle handle = GCHandle.Alloc(_data, GCHandleType.Pinned);
                using (Mat m = new Mat(
                           new System.Drawing.Size(image.Width, image.Height + image.Height / 2),
                           DepthType.Cv8U,
                           1,
                           handle.AddrOfPinnedObject(),
                           image.Width))
                {
                    if (_gComputation != null)
                    {
                        _gComputation.Apply(m, _rotatedMat);
                    }
                    else
                    {
                        CvInvoke.CvtColor(m, _bgrMat, ColorConversion.Yuv2RgbYv12);
                        //Rotate 90 degree by transpose and flip
                        CvInvoke.Transpose(_bgrMat, _rotatedMat);
                        CvInvoke.Flip(_rotatedMat, _rotatedMat, FlipType.Horizontal);
                    }

                }
                handle.Free();

                OnImageProcessed(reader, _rotatedMat);

                image.Close();
            }

        }
    }
}

#endif