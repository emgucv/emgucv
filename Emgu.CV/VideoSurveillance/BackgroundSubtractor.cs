//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// An abstract class that can be use the perform background / forground detection.
   /// </summary>
   public abstract class BackgroundSubtractor : UnmanagedObject, IBGFGDetector<Bgr>
   {
      private Image<Gray, Byte> _fgMask;
      private Image<Gray, Byte> _bgMask;

      /// <summary>
      /// Update the background model
      /// </summary>
      /// <param name="image">The image that is used to update the background model</param>
      /// <param name="learningRate">Use -1 for default</param>
      public void Update(Image<Bgr, Byte> image, double learningRate)
      {
         if (_fgMask == null)
            _fgMask = new Image<Gray, byte>(image.Size);
         CvInvoke.CvBackgroundSubtractorUpdate(_ptr, image, _fgMask, learningRate);
      }

      /// <summary>
      /// Update the background model
      /// </summary>
      /// <param name="image">The image that is used to update the background model</param>
      public void Update(Image<Bgr, byte> image)
      {
         Update(image, -1);
      }

      /// <summary>
      /// Get the mask of the forground
      /// </summary>
      public Image<Gray, byte> ForegroundMask
      {
         get { return _fgMask; }
      }

      /// <summary>
      /// Get the mask of the background
      /// </summary>
      public Image<Gray, byte> BackgroundMask
      {
         get
         {
            if (_bgMask == null)
            {
               if (_fgMask == null)
                  return null;
               _bgMask = new Image<Gray, byte>(_fgMask.Size);
            }
            CvInvoke.cvNot(_fgMask, _bgMask);
            return _bgMask;
         }
      }

      /// <summary>
      /// Release all managed resources associated with this background model.
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         if (_fgMask != null)
            _fgMask.Dispose();
         if (_bgMask != null)
            _bgMask.Dispose();

         base.ReleaseManagedResources();
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvBackgroundSubtractorUpdate(IntPtr bgSubstractor, IntPtr image, IntPtr fgmask, double learningRate);
   }
}