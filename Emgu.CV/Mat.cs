//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   internal class Mat : UnmanagedObject
   {
      public Mat()
      {
         _ptr = CvInvoke.cvMatCreate();
      }

      public Size Size
      {
         get
         {
            return CvInvoke.cvMatGetSize(_ptr);
         }
      }

      protected override void DisposeObject()
      {
         CvInvoke.cvMatRelease(ref _ptr);
      }
   }

   public partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreate();
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cvMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatCopyToCvArr(IntPtr mat, IntPtr cvArray);
   }
}
