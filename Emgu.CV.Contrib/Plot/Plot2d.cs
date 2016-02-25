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
using System.Drawing;

namespace Emgu.CV.Plot
{
   public partial class Plot2d : UnmanagedObject
   {
      public Plot2d(Mat data)
      {
         _ptr = PlotInvoke.cvePlot2dCreateFromX(data);
      }

      public Plot2d(Mat dataX, Mat dataY)
      {
         _ptr = PlotInvoke.cvePlot2dCreateFromXY(dataX, dataY);
      }

      public void Render(Mat result)
      {
         PlotInvoke.cvePlot2dRender(_ptr, result);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            PlotInvoke.cvePlot2dRelease(ref _ptr);
      }
   }

   public static partial class PlotInvoke
   {
      static PlotInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvePlot2dCreateFromX(IntPtr data);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvePlot2dCreateFromXY(IntPtr dataX, IntPtr dataY);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dRender(IntPtr plot, IntPtr result);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dRelease(ref IntPtr plot);
   }
}