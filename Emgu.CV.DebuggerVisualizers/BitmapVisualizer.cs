//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.Diagnostics;

[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.BitmapVisualizer),
Target = typeof(Bitmap))]


namespace Emgu.CV.DebuggerVisualizers
{
   public sealed class BitmapVisualizer : DialogDebuggerVisualizer
   {
      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         Bitmap image = objectProvider.GetObject() as Bitmap;
         if (image != null)
         {
            using (ImageViewer viewer = new ImageViewer())
            {
               viewer.Image = new Image<Bgr, Byte>(image);
               windowService.ShowDialog(viewer);
            }
         }
      }
   }
}