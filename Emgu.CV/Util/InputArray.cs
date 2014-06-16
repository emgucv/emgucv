//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV
{
   public class InputArray : UnmanagedObject
   {
      private InputArray()
      {        
      }

      public InputArray(IntPtr inputArrayPtr)
      {
         _ptr = inputArrayPtr;
      }

      public static InputArray GetEmpty()
      {
         return new InputArray();
      }

      public Size GetSize(int idx = -1)
      {
         Size s = new Size();
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveInputArrayGetSize(_ptr, ref s, idx);
         return s;
      }

      public bool IsEmpty()
      {
         if (_ptr == IntPtr.Zero)
            return true;
         return CvInvoke.cveInputArrayIsEmpty(_ptr);
      }

      public DepthType GetDepth(int idx = -1)
      {
         if (_ptr == IntPtr.Zero)
            return DepthType.Default;
         return CvInvoke.cveInputArrayGetDepth(_ptr, idx);
      }

      public int GetChannels(int idx = -1)
      {
         if (_ptr == IntPtr.Zero)
            return 0;
         return CvInvoke.cveInputArrayGetChannels(_ptr, idx);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveInputArrayRelease(ref _ptr);
      }
   }

   public partial class CvInvoke
   {
      /// <summary>
      /// Release the InputArray
      /// </summary>
      /// <param name="arr">Pointer to the input array</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveInputArrayRelease(ref IntPtr arr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveInputArrayGetSize(IntPtr ia, ref System.Drawing.Size size, int idx);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern DepthType cveInputArrayGetDepth(IntPtr ia, int idx);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveInputArrayGetChannels(IntPtr ia, int idx);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(BoolMarshalType)]
      internal static extern bool cveInputArrayIsEmpty(IntPtr ia);
   }
}