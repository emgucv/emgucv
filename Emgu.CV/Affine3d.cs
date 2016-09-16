//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;


namespace Emgu.CV
{
#if !NETFX_CORE
   [DebuggerTypeProxy(typeof(Affine3d.DebuggerProxy))]
#endif
   public class Affine3d : UnmanagedObject
   {
      public Affine3d()
      {
         _ptr = CvInvoke.cveAffine3dCreate();
      }

      public static Affine3d Identity()
      {
         return new Affine3d(CvInvoke.cveAffine3dGetIdentity());
      }

      private Affine3d(IntPtr ptr)
      {
         _ptr = ptr;
      }

      public Affine3d Rotate(double r0, double r1, double r2)
      {
         return new Affine3d(CvInvoke.cveAffine3dRotate(_ptr, r0, r1, r2));
      }

      public Affine3d Translate(double t0, double t1, double t2)
      {
         return new Affine3d(CvInvoke.cveAffine3dTranslate(_ptr, t0, t1, t2));
      }

      public double[] GetValues()
      {
         double[] v = new double[9];
         GCHandle handle = GCHandle.Alloc(v, GCHandleType.Pinned);
         CvInvoke.cveAffine3dGetValues(_ptr, handle.AddrOfPinnedObject());
         handle.Free();
         return v;
      }

      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveAffine3dRelease(ref _ptr);
         }
      }

      internal class DebuggerProxy
      {
         private Affine3d _v;

         public DebuggerProxy(Affine3d v)
         {
            _v = v;
         }

         public double[] Values
         {
            get { return _v.GetValues(); }
         }
      }

   }

   public static partial class CvInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveAffine3dCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveAffine3dGetIdentity();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveAffine3dRotate(IntPtr affine, double r0, double r1, double r2);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveAffine3dTranslate(IntPtr affine, double t0, double t1, double t2);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveAffine3dGetValues(IntPtr affine, IntPtr values);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveAffine3dRelease(ref IntPtr affine);

   }
}
