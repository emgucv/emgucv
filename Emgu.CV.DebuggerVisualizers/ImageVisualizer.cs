//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.UI;
using System.Diagnostics;

[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.ImageVisualizer),
Target = typeof(Image<,>))]


namespace Emgu.CV.DebuggerVisualizers
{
   public sealed class ImageVisualizer : DialogDebuggerVisualizer
   {
      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         IImage image = objectProvider.GetObject() as IImage;
         if (image != null)
         {
            using (ImageViewer viewer = new ImageViewer())
            {
               viewer.Image = image;
               windowService.ShowDialog(viewer);
            }
         }
      }
   }
}
