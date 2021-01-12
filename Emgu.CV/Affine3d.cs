//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
   /// <summary>
   /// The Affine3 matrix, double precision. 
   /// </summary>
   [DebuggerTypeProxy(typeof(Affine3d.DebuggerProxy))]
   public class Affine3d : UnmanagedObject
   {
      /// <summary>
      /// Create an empty Affine3, double precision matrix
      /// </summary>
      public Affine3d()
      {
         _ptr = CvInvoke.cveAffine3dCreate();
      }

      /// <summary>
      /// Create a new identity matrix
      /// </summary>
      /// <returns>The identity affine 3d matrix</returns>
      public static Affine3d Identity()
      {
         return new Affine3d(CvInvoke.cveAffine3dGetIdentity());
      }

      private Affine3d(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Rotate the Affine3 matrix by a Rodrigues vector
      /// </summary>
      /// <param name="r0">Value of the Rodrigues vector</param>
      /// <param name="r1">Value of the Rodrigues vector</param>
      /// <param name="r2">Value of the Rodrigues vector</param>
      /// <returns>The rotated Affine3 matrix</returns>
      public Affine3d Rotate(double r0, double r1, double r2)
      {
         return new Affine3d(CvInvoke.cveAffine3dRotate(_ptr, r0, r1, r2));
      }

      /// <summary>
      /// Translate the Affine3 matrix by the given value
      /// </summary>
      /// <param name="t0">Value of the translation vector</param>
      /// <param name="t1">Value of the translation vector</param>
      /// <param name="t2">Value of the translation vector</param>
      /// <returns>The translated Affine3 matrix</returns>
      public Affine3d Translate(double t0, double t1, double t2)
      {
         return new Affine3d(CvInvoke.cveAffine3dTranslate(_ptr, t0, t1, t2));
      }

      /// <summary>
      /// Get the 3x3 matrix's value as a double vector (of size 9)
      /// </summary>
      /// <returns>The 3x3 matrix's value as a double vector (of size 9)</returns>
      public double[] GetValues()
      {
         double[] v = new double[9];
         GCHandle handle = GCHandle.Alloc(v, GCHandleType.Pinned);
         CvInvoke.cveAffine3dGetValues(_ptr, handle.AddrOfPinnedObject());
         handle.Free();
         return v;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Affine3 model
      /// </summary>
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
