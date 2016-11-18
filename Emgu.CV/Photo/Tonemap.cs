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
   
   public class Tonemap : UnmanagedObject
   {
      /// <summary>
      /// The pointer to the unmanaged Tonemap object
      /// </summary>
      protected IntPtr _tonemapPtr;

      /// <summary>
      /// Default constructor that creates empty Tonemap
      /// </summary>
      protected Tonemap()
      {
      }
      
      public Tonemap(float gamma)
      {
         _ptr = CvInvoke.cveTonemapCreate(gamma);
         _tonemapPtr = _ptr;
      }
      
      public void Process(IInputArray src, IOutputArray dst)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())         
         {
            CvInvoke.cveTonemapProcess(_tonemapPtr, iaSrc, oaDst);
         }
      }

      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveTonemapRelease(ref _ptr);
         }
         _tonemapPtr = IntPtr.Zero;
      }
   }

   public class TonemapDrago : Tonemap
   {
      public TonemapDrago(float gamma, float saturation, float bias)
      {
         _ptr = CvInvoke.cveTonemapDragoCreate(gamma, saturation, bias, ref _tonemapPtr);
      }

      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveTonemapDragoRelease(ref _ptr);
         }

         _tonemapPtr = IntPtr.Zero;
      }
   }

   public class TonemapDurand : Tonemap
   {
      public TonemapDurand(float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor)
      {
         _ptr = CvInvoke.cveTonemapDurandCreate(gamma, contrast, saturation, sigmaSpace, sigmaColor, ref _tonemapPtr);
      }

      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveTonemapDragoRelease(ref _ptr);
         }

         _tonemapPtr = IntPtr.Zero;
      }
   }

   public class TonemapReinhard : Tonemap
   {
      public TonemapReinhard(float gamma, float intensity, float lightAdapt, float colorAdapt)
      {
         _ptr = CvInvoke.cveTonemapReinhardCreate(gamma, intensity, lightAdapt, colorAdapt, ref _tonemapPtr);
      }

      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveTonemapReinhardRelease(ref _ptr);
         }

         _tonemapPtr = IntPtr.Zero;
      }
   }

   public class TonemapMantiuk : Tonemap
   {
      public TonemapMantiuk(float gamma, float scale, float saturation)
      {
         _ptr = CvInvoke.cveTonemapMantiukCreate(gamma, scale, saturation, ref _tonemapPtr);
      }

      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveTonemapMantiukRelease(ref _ptr);
         }

         _tonemapPtr = IntPtr.Zero;
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void  cveTonemapProcess(IntPtr tonemap, IntPtr src, IntPtr dst);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTonemapCreate(float gamma);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTonemapRelease(ref IntPtr tonemap);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTonemapDragoCreate(float gamma, float saturation, float bias, ref IntPtr tonemap);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTonemapDragoRelease(ref IntPtr tonemap);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTonemapDurandCreate(float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor, ref IntPtr tonemap);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTonemapDurandRelease(ref IntPtr tonemap);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTonemapReinhardCreate(float gamma, float intensity, float lightAdapt, float colorAdapt, ref IntPtr tonemap);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTonemapReinhardRelease(ref IntPtr tonemap);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTonemapMantiukCreate(float gamma, float scale, float saturation, ref IntPtr tonemap);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTonemapMantiukRelease(ref IntPtr tonemap);
   }
}