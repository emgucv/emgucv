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
   /// This class allows to create and manipulate comprehensive artificial neural networks.
   /// </summary>
   public class Net : UnmanagedObject
   {
      /// <summary>
      /// Default constructor.
      /// </summary>
      public Net()
      {
         _ptr = ContribInvoke.cveDnnNetCreate();
      }

      /// <summary>
      /// Sets the new value for the layer output blob.
      /// </summary>
      /// <param name="outputName">Descriptor of the updating layer output blob.</param>
      /// <param name="blob">New blob</param>
      public void SetBlob(String outputName, Blob blob)
      {
         using (CvString outputNameStr = new CvString(outputName))
            ContribInvoke.cveDnnNetSetBlob(_ptr, outputNameStr, blob);
      }

      /// <summary>
      /// Returns the layer output blob.
      /// </summary>
      /// <param name="outputName">the descriptor of the returning layer output blob.</param>
      /// <returns>The layer output blob.</returns>
      public Blob GetBlob(String outputName)
      {
         using (CvString outputNameStr = new CvString(outputName))
         {
            return new Blob(ContribInvoke.cveDnnNetGetBlob(_ptr, outputNameStr));
         }
      }

      /// <summary>
      /// Runs forward pass for the whole network.
      /// </summary>
      public void Forward()
      {
         ContribInvoke.cveDnnNetForward(_ptr);
      }

      /// <summary>
      /// Release the memory associated with this network.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ContribInvoke.cveDnnNetRelease(ref _ptr);
         }
      }
   }
}

namespace Emgu.CV
{
   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDnnNetCreate();
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnNetSetBlob(IntPtr net, IntPtr outputName, IntPtr blob);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDnnNetGetBlob(IntPtr net, IntPtr outputName);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnNetForward(IntPtr net);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnNetRelease(ref IntPtr net);
   }
}
#endif