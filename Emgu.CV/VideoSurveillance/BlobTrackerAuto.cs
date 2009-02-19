using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A blob tracker auto
   /// </summary>
   public class BlobTrackerAuto :UnmanagedObject, IEnumerable<MCvBlob>
   {
      private BlobTrackerAutoParam _param;

      /// <summary>
      /// Create a auto blob tracker using the specific parameters
      /// </summary>
      /// <param name="param">The parameters for this blob tracker auto</param>
      public BlobTrackerAuto(BlobTrackerAutoParam param)
      {
         _param = param;
         MCvBlobTrackerAutoParam1 p = _param.MCvBlobTrackerAutoParam1;
         _ptr = CvInvoke.CvCreateBlobTrackerAuto1(ref p);
      }

      /// <summary>
      /// Create a default auto blob tracker 
      /// </summary>
      public BlobTrackerAuto()
      {
         BlobTrackerAutoParam param = new BlobTrackerAutoParam();
         param.ForgroundDetector = new ForgroundDetector(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
         _param = param;
         MCvBlobTrackerAutoParam1 p = _param.MCvBlobTrackerAutoParam1;
         _ptr = CvInvoke.CvCreateBlobTrackerAuto1(ref p);
      }

      /// <summary>
      /// Process a frame
      /// </summary>
      /// <param name="currentFrame">The frame to be processed</param>
      public void Process(IImage currentFrame)
      {
         CvInvoke.CvBlobTrackerAutoProcess(_ptr, currentFrame.Ptr, IntPtr.Zero);
      }

      /// <summary>
      /// Release the blob tracker auto
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobTrackerAutoRelease(_ptr);
      }

      /// <summary>
      /// Get the number of blobs in this tracker
      /// </summary>
      public int Count
      {
         get
         {
            return CvInvoke.CvBlobTrackerAutoGetBlobNum(_ptr);
         }
      }
      
      /// <summary>
      /// Get the forground mask
      /// </summary>
      /// <returns>The forground mask</returns>
      public Image<Gray, Byte> GetForgroundMask()
      {
         IntPtr forground = CvInvoke.CvBlobTrackerAutoGetFGMask(_ptr);
         if (forground == IntPtr.Zero) return null;
         Image<Gray, Byte> res = new Image<Gray, byte>(CvInvoke.cvGetSize(forground));
         CvInvoke.cvCopy(forground, res.Ptr, IntPtr.Zero);
         return res;
      }

      /// <summary>
      /// Return the blob given the specific index
      /// </summary>
      /// <param name="i">The index of the blob</param>
      /// <returns>The blob in the specific index</returns>
      public MCvBlob this[int i]
      {
         get
         {
            return (MCvBlob) Marshal.PtrToStructure(CvInvoke.CvBlobTrackerAutoGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }

      /// <summary>
      /// The parameters for this blob tracker auto
      /// </summary>
      public BlobTrackerAutoParam Param
      {
         get
         {
            return _param;
         }
         set
         {
            _param = value;
         }
      }

      #region IEnumerable<MCvBlob> Members
      /// <summary>
      /// Get an enumerator of all the blobs tracked by this tracker.
      /// </summary>
      /// <returns></returns>
      public IEnumerator<MCvBlob> GetEnumerator()
      {
         for (int i = 0; i < Count; i++)
            yield return this[i];
      }
      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      #endregion
   }
}
