//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.CodeDom;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.UI;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Windows.Forms;
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
    TargetTypeName = "Emgu.CV.Mat, Emgu.CV.Platform.NetStandard, Version=4.3.0.3890, Culture=neutral, PublicKeyToken=7281126722ab4438",
    Description = "Mat debugger visualizer")]
[assembly: DebuggerVisualizer(
    typeof(Emgu.CV.DebuggerVisualizers.UMatVisualizer),
    typeof(VisualizerObjectSource),
    //Target = typeof(UMat),a
    TargetTypeName = "Emgu.CV.UMat, Emgu.CV.Platform.NetStandard",
    Description = "UMat debugger visualizer")]

namespace Emgu.CV.DebuggerVisualizers
{
    public sealed class ImageVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MessageBox.Show(String.Format("Unable to convert {0} to {1}", objectProvider.GetObject().GetType(), typeof(IInputArray)));
        }
    }

    public sealed class MatVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MessageBox.Show(String.Format("Unable to convert {0} to {1}", objectProvider.GetObject().GetType(), typeof(IInputArray)));
        }
    }

    public sealed class UMatVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            using (var s = objectProvider.GetData())
            {
                //System.Runtime.Serialization
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Binder = new UmatDeserializationBinder();
                IInputArray image = formatter.Deserialize(s) as IInputArray;

                if (image != null)
                {
                    using (ImageViewer viewer = new ImageViewer())
                    {
                        viewer.Image = image;
                        windowService.ShowDialog(viewer);
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("Unable to convert {0} to {1}", objectProvider.GetObject().GetType(), typeof(IInputArray)));
                }
            }
        }
    }

    /*
    public class BaseImageVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MessageBox.Show(String.Format("Unable to convert {0} to {1}", objectProvider.GetObject().GetType(), typeof(IInputArray)));
        }

        
        //public static void TestShowVisualizer(object objectToVisualize)
       // {
        //    VisualizerDevelopmentHost myHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(BaseImageVisualizer));
         //   myHost.ShowVisualizer();
        //}
    }*/

    sealed class UmatDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return typeof(Emgu.CV.UMat);
            /*
            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            String assemVer1 = "Emgu.CV.Platform.NetStandard";
            String typeVer1 = "Emgu.CV.UMat";

            if (assemblyName == assemVer1 && typeName == typeVer1)
            {
                // To use a type from a different assembly version,
                // change the version number.
                // To do this, uncomment the following line of code.
                // assemblyName = assemblyName.Replace("1.0.0.0", "2.0.0.0");

                // To use a different type from the same assembly,
                // change the type name.
                typeName = "Emgu.CV.UMat";
            }

            // The following line of code returns the type.
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                typeName, assemblyName));

            return typeToDeserialize;*/
        }
    }
}
