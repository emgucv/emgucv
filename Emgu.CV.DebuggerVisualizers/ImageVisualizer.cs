//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.CodeDom;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.UI;
using System.Diagnostics;
using System.Security.Cryptography;
using Emgu.CV.DebuggerVisualizers;

[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.ImageVisualizer),
    typeof(VisualizerObjectSource),
    //Target = typeof(Image<,>), 
    TargetTypeName = "Emgu.CV.Image<,>, Emgu.CV.Platform.NetStandard",
    Description = "Image<,> debugger visualizer")]
[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.MatVisualizer),
    typeof(VisualizerObjectSource),
    //Target = typeof(Mat),
    TargetTypeName = "Emgu.CV.Mat, Emgu.CV.Platform.NetStandard",
    Description = "Mat debugger visualizer")]
[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.UMatVisualizer),
    typeof(VisualizerObjectSource),
    //Target = typeof(UMat),
    TargetTypeName = "Emgu.CV.UMat, Emgu.CV.Platform.NetStandard",
    Description = "UMat debugger visualizer")]

namespace Emgu.CV.DebuggerVisualizers
{
    public sealed class ImageVisualizer : BaseImageVisualizer
    {
    }

    public sealed class MatVisualizer : BaseImageVisualizer
    {
    }

    public sealed class UMatVisualizer : BaseImageVisualizer
    {
    }

    public class BaseImageVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            IInputArray image = objectProvider.GetObject() as IInputArray;
            if (image != null)
            {
                using (ImageViewer viewer = new ImageViewer())
                {
                    viewer.Image = image;
                    windowService.ShowDialog(viewer);
                }
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost myHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(BaseImageVisualizer));
            myHost.ShowVisualizer();
        }
    }
}
