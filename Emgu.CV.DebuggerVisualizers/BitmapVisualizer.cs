//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.Diagnostics;
using System.Security.Cryptography;

[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.BitmapVisualizer),
    typeof(VisualizerObjectSource),
    Target = typeof(Bitmap),
    //TargetTypeName = "System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0",
    //TargetTypeName = "System.Drawing.Bitmap, System.Drawing",
    Description = "Bitmap Debugger Visualizer")]

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
                    viewer.Image = image.ToImage<Bgr, Byte>();
                    windowService.ShowDialog(viewer);
                }
            }
        }
    }
}