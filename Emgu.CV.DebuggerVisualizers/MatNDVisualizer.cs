//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[assembly: DebuggerVisualizer(
typeof(Emgu.CV.DebuggerVisualizers.MatNDVisualizer),
Target = typeof(MatND<>))]
namespace Emgu.CV.DebuggerVisualizers
{
   public sealed class MatNDVisualizer : DialogDebuggerVisualizer
   {
#if DEBUG
      private static void DebugVisualizer()
      {
         using (MatND<float> mat = new MatND<float>(3, 5, 1))
         {
            mat.SetRandNormal(new MCvScalar(), new MCvScalar(255));
            VisualizerDevelopmentHost myHost = 
               new VisualizerDevelopmentHost(mat, typeof(MatNDVisualizer));
            myHost.ShowVisualizer();
         }
      }
#endif

      protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
      {
         UnmanagedObject matND = objectProvider.GetObject() as UnmanagedObject;
         if (matND != null)
         {
            MCvMatND cvMatND = (MCvMatND)Marshal.PtrToStructure(matND.Ptr, typeof(MCvMatND));
            if (cvMatND.dims > 3 || (cvMatND.dims == 3 && cvMatND.dim[2].Size > 4))
            {
               MessageBox.Show("MatND of dimension > 3 is not supported for debugger visualizer");
               return;
            }

            UnmanagedObject matrix = null;
            int rows = cvMatND.dim[0].Size;
            int cols = cvMatND.dims >= 2 ? cvMatND.dim[1].Size : 1;
            int channels = cvMatND.dims >= 3 ? cvMatND.dim[2].Size : 1;
            if (matND is MatND<float>)
               matrix = new Matrix<float>(rows, cols, channels);
            else if (matND is MatND<int>)
               matrix = new Matrix<int>(rows, cols, channels);
            else if (matND is MatND<double>)
               matrix = new Matrix<double>(rows, cols, channels);

            if (matrix == null)
            {
               MessageBox.Show(String.Format("{0} is not supported", cvMatND.type.ToString()));
               return;
            }

            CvInvoke.cvCopy(matND.Ptr, matrix.Ptr, IntPtr.Zero);
            
            using (MatrixViewer viewer = new MatrixViewer())
            {
               viewer.Matrix = matrix;
               windowService.ShowDialog(viewer);
            }
         }
      }
   }
}
