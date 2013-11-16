//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Shape
{
   public abstract class ShapeTransformer : UnmanagedObject
   {
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ShapeInvoke.cvShapeTransformerRelease(ref _ptr);
         }
      }
   }

   public class ThinPlateSplineShapeTransformer : ShapeTransformer
   {
      public ThinPlateSplineShapeTransformer(double regularizationParameter)
      {
         _ptr = ShapeInvoke.cvThinPlateSplineShapeTransformerCreate(regularizationParameter);
      }
   }

   public class AffineTransformer : ShapeTransformer
   {
      public AffineTransformer(bool fullAffine)
      {
         _ptr = ShapeInvoke.cvAffineTransformerCreate(fullAffine);
      }
   }

   public static partial class ShapeInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvThinPlateSplineShapeTransformerCreate(double regularizationParameter);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvAffineTransformerCreate(
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool fullAffine);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvShapeTransformerRelease(ref IntPtr transformer);
   }
}
