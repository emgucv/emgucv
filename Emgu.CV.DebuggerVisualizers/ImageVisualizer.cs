using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.ImageVisualizer),
Target = typeof(Emgu.CV.Image<,>))]

/*
[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.HistogramVisualizer),
Target = typeof(Emgu.CV.Histogram))]
*/

namespace Emgu.CV.DebuggerVisualizers
{
   public sealed class ImageVisualizer : DialogDebuggerVisualizer
   {
      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         IImage image = objectProvider.GetObject() as IImage;
         if (image != null)
         {
            ImageViewer viewer = new ImageViewer();
            viewer.Image = image;
            windowService.ShowDialog(viewer);
         }
      }
   }

   /*
   public sealed class HistogramVisualizer : DialogDebuggerVisualizer
   {
      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         Histogram hist = objectProvider.GetObject() as Histogram;
         if (hist != null)
         {
            HistogramViewer viewer = new HistogramViewer();
            viewer.HistogramCtrl.AddHistogram("Histogram", System.Drawing.Color.Black, hist);
            viewer.HistogramCtrl.Refresh();
            windowService.ShowDialog(viewer);
         }
      }
   }*/
}
