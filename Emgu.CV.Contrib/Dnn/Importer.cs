//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
   public class Importer : UnmanagedObject
   {
      internal Importer(IntPtr ptr)
      {
         _ptr = ptr;
      }

      public static Importer CreateCaffeImporter(String prototxt, String caffeModel)
      {
         using (CvString prototxtStr = new CvString(prototxt))
         using (CvString caffeModelStr = new CvString(caffeModel))
         {
            return new Importer(ContribInvoke.cveDnnCreateCaffeImporter(prototxtStr, caffeModelStr));
         }
      }

      public void PopulateNet(Net net)
      {
         ContribInvoke.cveDnnImporterPopulateNet(_ptr, net);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ContribInvoke.cveDnnImporterRelease(ref _ptr);
         }
      }
   }
}

namespace Emgu.CV
{
   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDnnCreateCaffeImporter(IntPtr prototxt, IntPtr caffeModel);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnImporterRelease(ref IntPtr importer);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnImporterPopulateNet(IntPtr importer, IntPtr net);
   }
}
