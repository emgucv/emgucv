//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   public class OclProgramSource : UnmanagedObject
   {
      private CvString _programSource;
      public OclProgramSource(String source)
      {
         _programSource = new CvString(source);
         _ptr = OclInvoke.oclProgramSourceCreate(_programSource);
         
      }

      public String Source
      {
         get
         {
            using (CvString s = new CvString(OclInvoke.oclProgramSourceGetSource(_ptr), false))
               return s.ToString();
         }
      }

      protected override void DisposeObject()
      {
         OclInvoke.oclProgramSourceRelease(ref _ptr);
         _programSource.Dispose();
      }
   }

   /// <summary>
   /// Class that contains ocl functions.
   /// </summary>
   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclProgramSourceCreate(IntPtr source);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclProgramSourceRelease(ref IntPtr oclProgramSource);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclProgramSourceGetSource(IntPtr programSource);
   }
}