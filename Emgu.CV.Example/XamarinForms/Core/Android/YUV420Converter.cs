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
using Android.OS;
using Android.Renderscripts;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Emgu.CV.XamarinForms
{
    public class YUV420Converter : Emgu.Util.DisposableObject
    {
        private RenderScript rs;
        private ScriptIntrinsicYuvToRGB _yuvToRgbIntrinsic;
        private int _yuvLength;
        private Allocation _input;
        private int _width;
        private int _height;
        private Allocation _output;

        public YUV420Converter(Context context)
        {
            rs = RenderScript.Create(context);
            _yuvToRgbIntrinsic = ScriptIntrinsicYuvToRGB.Create(rs, Element.U8_4(rs));
        }

        public void YUV_420_888_toRGBIntrinsics(int width, int height, byte[] yuv, Bitmap bmpOut)
        {
            YUV_420_888_toRGBIntrinsicsProcess(width, height, yuv);
            _output.CopyTo(bmpOut);
        }

        public void YUV_420_888_toRGBIntrinsics(int width, int height, byte[] yuv, byte[] dataOut)
        {
            YUV_420_888_toRGBIntrinsicsProcess(width, height, yuv);
            _output.CopyTo(dataOut);
        }

        private void YUV_420_888_toRGBIntrinsicsProcess(int width, int height, byte[] yuv)
        {
            if (yuv.Length != _yuvLength)
            {

                Android.Renderscripts.Type.Builder yuvType =
                    new Android.Renderscripts.Type.Builder(rs, Element.U8(rs)).SetX(yuv.Length);
                if (_input != null)
                    _input.Destroy();
                _input = Allocation.CreateTyped(rs, yuvType.Create(), AllocationUsage.Script);
                _yuvLength = yuv.Length;
            }

            if (_width != width || _height != height)
            {
                Android.Renderscripts.Type.Builder rgbaType = new Android.Renderscripts.Type.Builder(rs, Element.RGBA_8888(rs)).SetX(width).SetY(height);
                if (_output != null)
                    _output.Destroy();
                _output = Allocation.CreateTyped(rs, rgbaType.Create(), AllocationUsage.Script);
                _width = width;
                _height = height;
            }


            _input.CopyFromUnchecked(yuv);

            _yuvToRgbIntrinsic.SetInput(_input);
            _yuvToRgbIntrinsic.ForEach(_output);

        }

        public Bitmap YUV_420_888_toRGBIntrinsics(int width, int height, byte[] yuv)
        {
            Bitmap bmpOut = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            YUV_420_888_toRGBIntrinsics(width, height, yuv, bmpOut);
            return bmpOut;
        }

        protected override void DisposeObject()
        {
            if (_input != null)
            {
                _input.Destroy();
                _input = null;
            }
            if (_yuvToRgbIntrinsic != null)
            {
                _yuvToRgbIntrinsic.Destroy();
                _yuvToRgbIntrinsic = null;
            }
            if (rs != null)
            {
                rs.Destroy();
                rs = null;
            }
        }
    }
}

#endif