//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
        private byte[] _data_buffer = null;
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
                using (GMat tMat = GapiInvoke.Transpose(rgbMat))
                using (GMat rMat = GapiInvoke.Flip(tMat, FlipType.Horizontal))
                {
                    _gComputation = new GComputation(inYuvMat, rMat);
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
                int pixelCounts = image.Width * image.Height;
                for (int i = 0; i < planes.Length; i++)
                {
                    if (i == 0)
                    {
                        //Y plane
                        totalLength += pixelCounts;
                    }
                    else
                    {
                        //U, V plane
                        totalLength += pixelCounts / 4;
                    }
                    //Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                    //totalLength += buffer.Remaining();
                }

                if (_data == null || _data.Length != totalLength)
                {
                    _data = new byte[totalLength];
                    _data_buffer = new byte[(image.Height / 2) * planes[1].RowStride];
                }

                int offset = 0;
                for (int i = 0; i < planes.Length; i++)
                {
                    if (i == 0)
                    {
                        Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                        int length = buffer.Remaining();

                        buffer.Get(_data, offset, length);
                        offset += pixelCounts;
                    }
                    else if (planes[i].PixelStride == 1)
                    {
                        Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                        int length = buffer.Remaining();

                        buffer.Get(_data, offset, length);
                        offset += pixelCounts / 4;
                    }
                    else if (planes[i].PixelStride <= 4) // && pixel Stride > 1
                    {
                        Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                        int length = buffer.Remaining();
                        buffer.Get(_data_buffer, 0, length);
                        // Need to handle data stride
                        GCHandle bufferHandler = GCHandle.Alloc(_data_buffer, GCHandleType.Pinned);
                        GCHandle dataHandler = GCHandle.Alloc(_data, GCHandleType.Pinned);
                        try
                        {
                            using (Mat bufferMat = new Mat(
                                       image.Height / 2,
                                       image.Width / 2,
                                       DepthType.Cv8U,
                                       planes[i].PixelStride,
                                       bufferHandler.AddrOfPinnedObject(),
                                       planes[i].RowStride))
                            using (Mat dataMat = new Mat(
                                       image.Height / 2,
                                       image.Width / 2,
                                       DepthType.Cv8U,
                                       1,
                                       new IntPtr(dataHandler.AddrOfPinnedObject().ToInt64() + offset),
                                       image.Width / 2))
                            {
                                CvInvoke.ExtractChannel(bufferMat, dataMat, 0);
                            }
                        }
                        finally
                        {
                            bufferHandler.Free();
                            dataHandler.Free();
                        }

                        offset += pixelCounts / 4;
                    }
                    else
                    {
                        throw new NotImplementedException("Pixel stride larger than 4 is not supported.");
                    }
                }

                //System.Diagnostics.Debug.Assert(offset == _data.Length, "Offset != _data.Length");
                System.Diagnostics.Debug.Assert(image.Width * (image.Height + image.Height / 2) == _data.Length, "_data.Length != image pixel count");

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