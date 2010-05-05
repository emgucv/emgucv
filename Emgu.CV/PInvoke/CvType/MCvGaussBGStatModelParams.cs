using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper to the CvBGStatModel
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvGaussBGStatModelParams
   {
      /// <summary>
      /// Learning rate; alpha = 1/win_size. (default = 200)
      /// </summary>
      public int win_size;
      /// <summary>
      /// = K = number of Gaussians in mixture. (default = 5, Max = 500)
      /// </summary>
      public int n_gauss;
      /// <summary>
      /// threshold sum of weights for background test. (default = 0.7)
      /// </summary>
      public double bg_threshold;
      /// <summary>
      /// lambda=2.5 is 99% (default = 2.5)
      /// </summary>
      public double std_threshold;
      /// <summary>
      /// Discard foreground blobs whose bounding box is smaller than this threshold. (default = 15.f)
      /// </summary>
      public double minArea;
      /// <summary>
      /// (default = 0.05)
      /// </summary>
      public double weight_init;
      /// <summary>
      /// (default = 30*30 -&gt; CV_BGFG_MOG_SIGMA_INIT*CV_BGFG_MOG_SIGMA_INIT)
      /// </summary>
      /// <remarks></remarks>
      public double variance_init;
   }
}