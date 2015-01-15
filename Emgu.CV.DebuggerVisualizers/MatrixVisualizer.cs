//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using System.Diagnostics;

[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.MatrixVisualizer),
Target = typeof(Matrix<>))]
namespace Emgu.CV.DebuggerVisualizers
{
   public sealed class MatrixVisualizer : DialogDebuggerVisualizer
   {
      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         UnmanagedObject mat = objectProvider.GetObject() as UnmanagedObject;
         if (mat != null)
         {
            using (MatrixViewer viewer = new MatrixViewer())
            {
               viewer.Matrix = mat;
               windowService.ShowDialog(viewer);
            }
         }
      }
   }
}
