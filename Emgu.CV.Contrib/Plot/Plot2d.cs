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
   /// <summary>
   /// A 2D plot
   /// </summary>
   public partial class Plot2d : UnmanagedObject
   {
      /// <summary>
      /// Create 2D plot from data
      /// </summary>
      /// <param name="data">The data to be plotted</param>
      public Plot2d(Mat data)
      {
         _ptr = PlotInvoke.cvePlot2dCreateFromX(data);
      }

      /// <summary>
      /// Create 2D plot for data
      /// </summary>
      /// <param name="dataX">The data for the X-axis</param>
      /// <param name="dataY">The data for the Y-axis</param>
      public Plot2d(Mat dataX, Mat dataY)
      {
         _ptr = PlotInvoke.cvePlot2dCreateFromXY(dataX, dataY);
      }

      /// <summary>
      /// Render the plot to the resulting Mat
      /// </summary>
      /// <param name="result">The output plot</param>
      public void Render(Mat result)
      {
         PlotInvoke.cvePlot2dRender(_ptr, result);
      }

      /// <summary>
      /// Set the line color
      /// </summary>
      /// <param name="plotLineColor">The plot line color</param>
      public void SetPlotLineColor(MCvScalar plotLineColor)
      {
         PlotInvoke.cvePlot2dSetPlotLineColor(_ptr, ref plotLineColor);
      }

      /// <summary>
      /// Set the background color
      /// </summary>
      /// <param name="plotBackgroundColor">The background color</param>
      public void SetPlotBackgroundColor(MCvScalar plotBackgroundColor)
      {
         PlotInvoke.cvePlot2dSetPlotBackgroundColor(_ptr, ref plotBackgroundColor);
      }

      /// <summary>
      /// Set the axis color
      /// </summary>
      /// <param name="plotAxisColor">the axis color</param>
      public void SetPlotAxisColor(MCvScalar plotAxisColor)
      {
         PlotInvoke.cvePlot2dSetPlotAxisColor(_ptr, ref plotAxisColor);
      }

      /// <summary>
      /// Set the plot grid color
      /// </summary>
      /// <param name="plotGridColor">The plot grid color</param>
      public void SetPlotGridColor(MCvScalar plotGridColor)
      {
         PlotInvoke.cvePlot2dSetPlotGridColor(_ptr, ref plotGridColor);
      }

      /// <summary>
      /// Set the plot text color
      /// </summary>
      /// <param name="plotTextColor">The plot text color</param>
      public void SetPlotTextColor(MCvScalar plotTextColor)
      {
         PlotInvoke.cvePlot2dSetPlotTextColor(_ptr, ref plotTextColor);
      }

      /// <summary>
      /// Set the plot size
      /// </summary>
      /// <param name="width">The width</param>
      /// <param name="height">The height</param>
      public void SetPlotSize(int width, int height)
      {
         PlotInvoke.cvePlot2dSetPlotSize(_ptr, width, height);
      }

      /// <summary>
      /// Release unmanaged memory associated with this plot2d.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            PlotInvoke.cvePlot2dRelease(ref _ptr);
      }
   }

   /// <summary>
   /// Entry points for the cv::plot functions
   /// </summary>
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

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dSetPlotLineColor(IntPtr plot, ref MCvScalar plotLineColor);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dSetPlotBackgroundColor(IntPtr plot, ref MCvScalar plotBackgroundColor);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dSetPlotAxisColor(IntPtr plot, ref MCvScalar plotAxisColor);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dSetPlotGridColor(IntPtr plot, ref MCvScalar plotGridColor);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dSetPlotTextColor(IntPtr plot, ref MCvScalar plotTextColor);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlot2dSetPlotSize(IntPtr plot, int plotSizeWidth, int plotSizeHeight);
   }
}