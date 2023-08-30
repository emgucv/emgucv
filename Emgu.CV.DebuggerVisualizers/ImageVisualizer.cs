//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.UI;
using System.Diagnostics;

/*
[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.ImageVisualizer), 
    typeof(VisualizerObjectSource), 
    Target = typeof(Image<,>))]*/
[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.MatVisualizer), 
    typeof(VisualizerObjectSource), 
    Target = typeof(Mat))]
[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.UMatVisualizer), 
    typeof(VisualizerObjectSource), 
    Target = typeof(UMat))]

namespace Emgu.CV.DebuggerVisualizers
{
    /*
    public sealed class ImageVisualizer : BaseImageVisualizer<Image>
    {
    }*/

    public sealed class MatVisualizer : BaseImageVisualizer<Mat>
    {
    }

    public sealed class UMatVisualizer : BaseImageVisualizer<UMat>
    {
    }

    public class BaseImageVisualizer<T> : DialogDebuggerVisualizer where T : IInputArray
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            IInputArray image;
            if (objectProvider is IVisualizerObjectProvider3)
            {
                IVisualizerObjectProvider3 objectProvider3 = objectProvider as IVisualizerObjectProvider3;
                image = objectProvider3.GetObject<T>();
            }
            else if (objectProvider is IVisualizerObjectProvider2)
            {
                IVisualizerObjectProvider2 objectProvider2 = objectProvider as IVisualizerObjectProvider2;
                var deserializableObject = objectProvider2.GetDeserializableObject();
                image = deserializableObject.ToObject<T>();
            }
            else
            {
                image = objectProvider.GetObject() as IInputArray;
            }
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
            VisualizerDevelopmentHost myHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(BaseImageVisualizer<T>));
            myHost.ShowVisualizer();
        }
    }
}
