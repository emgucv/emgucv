using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using System.Diagnostics;

[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.MatrixVisualizer),
Target = typeof(Emgu.CV.Matrix<>))]
namespace Emgu.CV.DebuggerVisualizers
{
   public sealed class MatrixVisualizer : DialogDebuggerVisualizer
   {
      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         UnmanagedObject mat = objectProvider.GetObject() as UnmanagedObject;
         if (mat != null)
         {
            MatrixViewer viewer = new MatrixViewer();
            viewer.Matrix = mat;
            windowService.ShowDialog(viewer);
         }
      }
   }
}
