//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   public abstract class ImageFilter : Emgu.Util.DisposableObject, ICloneable
   {
      protected ImageBufferFactory<Mat> _bgrBuffers;
      protected ImageBufferFactory<Mat> _grayBuffers;
      protected ImageBufferFactory<Mat> _bgrSingleBuffers;
      protected bool _inplaceCapable = false;

      public ImageFilter()
      {
      }

      /// <summary>
      /// If true, the sourceImage and destImage in ProcessData function can be the same
      /// </summary>
      public bool InplaceCapable
      {
         get
         {
            return _inplaceCapable;
         }
      }

      public abstract void ProcessData(Mat sourceImage, Mat destImage);

      public Mat GetBufferBgrSingle(Size size, int index)
      {
         if (_bgrSingleBuffers == null)
            _bgrSingleBuffers = new ImageBufferFactory<Mat>(s => new Mat(s.Height, s.Width, DepthType.Cv8U, 3));
         return _bgrSingleBuffers.GetBuffer(size, index);
      }

      public Mat GetBufferBgr(Size size, int index)
      {
         if (_bgrBuffers == null)
            _bgrBuffers = new ImageBufferFactory<Mat>( s => new Mat(s.Height, s.Width, DepthType.Cv8U, 3));
         return _bgrBuffers.GetBuffer(size, index);
      }

      public Mat GetBufferGray(Size size, int index)
      {
         if (_grayBuffers == null)
            _grayBuffers = new ImageBufferFactory<Mat>( s => new Mat(s.Height, s.Width, DepthType.Cv8U, 1));
         return _grayBuffers.GetBuffer(size, index);
      }

      protected override void DisposeObject()
      {
         if (_bgrBuffers != null)
         {
            _bgrBuffers.Dispose();
            _bgrBuffers = null;
         }
         if (_grayBuffers != null)
         {
            _grayBuffers.Dispose();
            _grayBuffers = null;
         }
         if (_bgrSingleBuffers != null)
         {
            _bgrSingleBuffers.Dispose();
            _bgrSingleBuffers = null;
         }
      }

      public abstract Object Clone();
   }

   public class CannyFilter : ImageFilter
   {
      private double _thresh;
      private double _threshLinking;
      private int _apertureSize;

      public CannyFilter(double thresh, double threshLinking, int apertureSize)
      {
         _thresh = thresh;
         _threshLinking = threshLinking;
         _apertureSize = apertureSize;

         _inplaceCapable = true;
      }

      public override void ProcessData(Mat sourceImage, Mat destImage)
      {
         Size size = sourceImage.Size;

         Mat i0 = GetBufferGray(size, 0);
         Mat i1 = GetBufferGray(size, 1);
         Mat i2 = GetBufferGray(size, 2);
         Mat i3 = GetBufferGray(size, 3);
         CvInvoke.ExtractChannel(sourceImage, i0, 0);
         CvInvoke.Canny(i0, i1, _thresh, _threshLinking, _apertureSize );
         CvInvoke.ExtractChannel(sourceImage, i0, 1);
         CvInvoke.Canny(i0, i2, _thresh, _threshLinking, _apertureSize);
         CvInvoke.ExtractChannel(sourceImage, i0, 2);
         CvInvoke.Canny(i0, i3, _thresh, _threshLinking, _apertureSize);
         using (VectorOfMat vm = new VectorOfMat(i1, i2, i3))
         {
            CvInvoke.Merge(vm, destImage );
         }

      }

      public override object Clone()
      {
         return new CannyFilter(_thresh, _threshLinking, _apertureSize);
      }
   }

   public class ColorMapFilter : ImageFilter
   {
      private Emgu.CV.CvEnum.ColorMapType _colorMapType;

      public ColorMapFilter(Emgu.CV.CvEnum.ColorMapType type)
      {
         _colorMapType = type;
         _inplaceCapable = true;
      }

      public override void ProcessData(Mat sourceImage, Mat destImage)
      {
         CvInvoke.ApplyColorMap(sourceImage, destImage, _colorMapType);
      }

      public override object Clone()
      {
         return new ColorMapFilter(_colorMapType);
      }
   }

   public class DistorFilter : ImageFilter
   {
      private double _centerX;
      private double _centerY;
      private double _distorCoeff;

      private Mat _mapX;
      private Mat _mapY;

      private Size _size;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="centerWidth">A value between 0 and 1.0, if 0, the center is on the left side of the image, if 1, the center is on the right side of the image</param>
      /// <param name="centerHeight">A value between 0 and 1.0, if 0, the center is on the top of the image, if 1, the center is on the bottom of the image</param>
      /// <param name="distorCoeff"></param>
      public DistorFilter(double centerWidth, double centerHeight, double distorCoeff)
      {
         if (centerWidth < 0 || centerWidth > 1.0 || centerHeight < 0 || centerHeight > 1.0)
         {
            throw new ArgumentException("CenterWidth and CenterHeight must be a number >= 0 and <= 1.0");
         }
         _centerX = centerWidth;
         _centerY = centerHeight;
         _distorCoeff = distorCoeff;
      }

      public override void ProcessData(Mat sourceImage, Mat destImage)
      {
         if (!sourceImage.Size.Equals(_size))
         {
            if (_mapX != null)
            {
               _mapX.Dispose();
               _mapX = null;
            }
            if (_mapY != null)
            {
               _mapY.Dispose();
               _mapY = null;
            }

            _size = sourceImage.Size;
         }

         if (_mapX == null || _mapY == null)
         {
            Mat intrinsicMat = new Mat(new Size(3,3),DepthType.Cv64F, 1);
            int centerY = (int)(_size.Width * _centerY);
            int centerX = (int)(_size.Height * _centerX);
            double[] intrinsicVal = new double[9]
            {
               1, 0, centerY,
               0, 1, centerX,
               0, 0, 1
            };
            intrinsicMat.SetTo(intrinsicVal);

            Mat distortionMat = new Mat(new Size(1, 5), DepthType.Cv64F, 1 );
            double[] distortionVal = new double[5]
            {
               _distorCoeff/(_size.Width*_size.Height), 0, 0, 0, 0
            };
            distortionMat.SetTo(distortionVal);
            CvInvoke.InitUndistortRectifyMap(intrinsicMat, distortionMat, null, intrinsicMat, _size, CvEnum.DepthType.Cv32F, _mapX, _mapY);
         }

         CvInvoke.Remap(sourceImage, destImage, _mapX, _mapY, Emgu.CV.CvEnum.Inter.Linear);
      }

      public override object Clone()
      {
         return new DistorFilter(_centerX, _centerY, _distorCoeff);
      }
   }
}