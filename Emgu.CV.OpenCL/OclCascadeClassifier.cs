//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// Cascade Classifier for object detection using OpenCL
   /// </summary>
   public class OclCascadeClassifier : CascadeClassifier
   {
      /// <summary>
      /// Create a OpenCL cascade classifier using the specific file
      /// </summary>
      /// <param name="fileName">The file to create the classifier from</param>
      public OclCascadeClassifier(String fileName)
         : base()
      {
#if !NETFX_CORE
         FileInfo file = new FileInfo(fileName);
         if (!file.Exists)
            throw new FileNotFoundException("File not found", file.FullName);
#endif

         _ptr = OclInvoke.oclCascadeClassifierCreate(fileName);

         if (_ptr == IntPtr.Zero)
         {
            throw new NullReferenceException(String.Format("Fail to create OpenCL HaarCascade object: {0}", fileName));
         }
      }

      /// <summary>
      /// Release all unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.oclCascadeClassifierRelease(ref _ptr);
      }

   }

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr oclCascadeClassifierCreate(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String filename);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclCascadeClassifierRelease(ref IntPtr classified);
   }
}
