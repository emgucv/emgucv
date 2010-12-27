using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Similar to CvArray but use GPU for processing
   /// </summary>
   /// <typeparam name="TDepth">The type of element in the matrix</typeparam>
   public class GpuMat<TDepth> : UnmanagedObject
      where TDepth : new()
   {
      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="channels">The number of channels</param>
      public GpuMat(int rows, int cols, int channels)
      {
         _ptr = GpuInvoke.gpuMatCreate( rows, cols, CvInvoke.CV_MAKETYPE((int)Util.GetMatrixDepth(typeof(TDepth)), channels));
      }

      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="size">The size of the GpuMat</param>
      /// <param name="channels">The number of channels</param>
      public GpuMat(Size size, int channels)
         : this(size.Height, size.Width, channels)
      { 
      }

      /// <summary>
      /// Create a GpuMat from an CvArray of the same depth type
      /// </summary>
      /// <param name="arr">The CvArry to be converted to GpuMat</param>
      public GpuMat(CvArray<TDepth> arr)
      {
         _ptr = GpuInvoke.gpuMatCreateFromArr(arr);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this GpuMat
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuMatRelease(ref _ptr);
      }

      /// <summary>
      /// Get the GpuMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      public Size Size
      {
         get { return GpuInvoke.gpuMatGetSize(_ptr); }
      }

      /// <summary>
      /// Get the number of channels in the GpuMat
      /// </summary>
      public int NumberOfChannels
      {
         get { return GpuInvoke.gpuMatGetChannels(_ptr); }
      }

      /// <summary>
      /// Pefroms blocking upload data to GpuMat
      /// </summary>
      /// <param name="arr">The CvArray to be uploaded to GpuMat</param>
      public void Upload(CvArray<TDepth> arr)
      {
         GpuInvoke.gpuMatUpload(_ptr, arr);
      }

      /// <summary>
      /// Downloads data from device to host memory. Blocking calls
      /// </summary>
      /// <param name="arr">The destination CvArray where the GpuMat data will be downloaded to.</param>
      public void Download(CvArray<TDepth> arr)
      {
         GpuInvoke.gpuMatDownload(_ptr, arr);
      }
   }
}
