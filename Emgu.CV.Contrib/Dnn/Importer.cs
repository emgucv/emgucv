//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE)
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
   /// <summary>
   /// Small interface class for loading trained serialized models of different dnn-frameworks.
   /// </summary>
   public class Importer : UnmanagedObject
   {
      internal Importer(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Creates the importer of Caffe framework network.
      /// </summary>
      /// <param name="prototxt">path to the .prototxt file with text description of the network architecture.</param>
      /// <param name="caffeModel">path to the .caffemodel file with learned network.</param>
      /// <returns>The created importer, NULL in failure cases.</returns>
      public static Importer CreateCaffeImporter(String prototxt, String caffeModel)
      {
         using (CvString prototxtStr = new CvString(prototxt))
         using (CvString caffeModelStr = new CvString(caffeModel))
         {
            IntPtr result = ContribInvoke.cveDnnCreateCaffeImporter(prototxtStr, caffeModelStr);
            return result == IntPtr.Zero ? null : new Importer(result);
         }
      }

      /// <summary>
      /// Adds loaded layers into the <paramref name="net"/> and sets connetions between them.
      /// </summary>
      /// <param name="net">The net model</param>
      public void PopulateNet(Net net)
      {
         ContribInvoke.cveDnnImporterPopulateNet(_ptr, net);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Importer
      /// </summary>
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
#endif