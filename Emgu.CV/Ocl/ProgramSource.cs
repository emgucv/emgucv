//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace Emgu.CV.Ocl
{
   /// <summary>
   /// Open CL kernel program source code
   /// </summary>
   public class ProgramSource : UnmanagedObject
   {
      private CvString _programSource;

      /// <summary>
      /// Create OpenCL program source code
      /// </summary>
      /// <param name="source">The source code</param>
      public ProgramSource(String source)
      {
         _programSource = new CvString(source);
         _ptr = OclInvoke.oclProgramSourceCreate(_programSource);
         
      }

      /// <summary>
      /// Get the source code as String
      /// </summary>
      public String Source
      {
         get
         {
            using (CvString s = new CvString(OclInvoke.oclProgramSourceGetSource(_ptr), false))
               return s.ToString();
         }
      }
      
      /// <summary>
      /// Release the unmanaged memory associated with this object
      /// </summary>
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