//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.UI;
using System.Diagnostics;

[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.DenseHistogramVisualizer),
Target = typeof(Emgu.CV.DenseHistogram))]

namespace Emgu.CV.DebuggerVisualizers
{
   public sealed class DenseHistogramVisualizer : DialogDebuggerVisualizer
   {
      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         DenseHistogram hist = objectProvider.GetObject() as DenseHistogram;
         if (hist.Dimension > 1)
         {
            MessageBox.Show("Only 1-D histogram visualization is supported");
            return;
         }

         if (hist != null)
         {
            using (HistogramViewer viewer = new HistogramViewer())
            {
               viewer.HistogramCtrl.AddHistogram("Histogram", System.Drawing.Color.Black, hist);
               viewer.HistogramCtrl.Refresh();
               windowService.ShowDialog(viewer);
            }
         }
      }
   }
}
