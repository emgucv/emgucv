using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      #region Foreground detector
      /// <summary>
      /// Create a simple foreground detector
      /// </summary>
      /// <param name="type">The type of the detector</param>
      /// <param name="param">Pointer to the parameters of the detector</param>
      /// <returns>Pointer the to foreground detector</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateFGDetectorBase(CvEnum.FORGROUND_DETECTOR_TYPE type, IntPtr param);

      /// <summary>
      /// Get the foreground mask from the foreground detector
      /// </summary>
      /// <param name="detector">The foreground detector</param>
      /// <returns>The foreground mask</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvFGDetectorGetMask(IntPtr detector);

      /// <summary>
      /// Update the FGDetector with new image
      /// </summary>
      /// <param name="detector">The foreground detector</param>
      /// <param name="image">The image which will be used to update the FGDetector</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvFGDetectorProcess(IntPtr detector, IntPtr image);

      /// <summary>
      /// Create a simple foreground detector
      /// </summary>
      /// <param name="type">The type of the detector</param>
      /// <param name="param">The parameters of the detector</param>
      /// <returns>Pointer the to foreground detector</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateFGDetectorBase(CvEnum.FORGROUND_DETECTOR_TYPE type, ref MCvFGDStatModelParams param);

      /// <summary>
      /// Release the foreground detector
      /// </summary>
      /// <param name="detector">The foreground detector to be released</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvFGDetectorRelease(IntPtr detector);
      #endregion

      #region BlobSeq
      /// <summary>
      /// Create a BlobSeq
      /// </summary>
      /// <param name="blobSize">The size of the blob in bytes</param>
      /// <returns>Pointer to the BlobSeq</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobSeqCreate(int blobSize);

      /// <summary>
      /// Release the blob sequence
      /// </summary>
      /// <param name="blobSeq">The BlobSeq to be released</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobSeqRelease(IntPtr blobSeq);

      /// <summary>
      /// Get the specific blob from the blob sequence
      /// </summary>
      /// <param name="blobSeq">the blob sequence</param>
      /// <param name="blobIndex">the index of the blob to be retrieved</param>
      /// <returns>Pointer to the specific blob</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobSeqGetBlob(IntPtr blobSeq, int blobIndex);

      /// <summary>
      /// Get the specific blob from the blob sequence
      /// </summary>
      /// <param name="blobSeq">the blob sequence</param>
      /// <param name="blobIndex">the index of the blob to be retrieved</param>
      /// <returns>Pointer to the specific blob</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobSeqGetBlobByID(IntPtr blobSeq, int blobIndex);

      /// <summary>
      /// Get the number of blob in the blob sequence
      /// </summary>
      /// <param name="blobSeq">The blob sequence</param>
      /// <returns>The number of blob in the blob sequence</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int CvBlobSeqGetBlobNum(IntPtr blobSeq);
      #endregion

      #region BlobDetector
      /// <summary>
      /// Release the blob detector
      /// </summary>
      /// <param name="detector">the detector to be released</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobDetectorRelease(IntPtr detector);

      /// <summary>
      /// Detect new blobs.
      /// </summary>
      /// <param name="detector">The blob detector</param>
      /// <param name="img">The image</param>
      /// <param name="imgFG">The foreground mask</param>
      /// <param name="newBlobList">The new blob list</param>
      /// <param name="oldBlobList">The old blob list</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int CvBlobDetectorDetectNewBlob(IntPtr detector, IntPtr img, IntPtr imgFG, IntPtr newBlobList, IntPtr oldBlobList);

      /// <summary>
      /// Get a simple blob detector 
      /// </summary>
      /// <returns>Pointer to the blob detector</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobDetectorSimple();

      /// <summary>
      /// Get a CC blob detector 
      /// </summary>
      /// <returns>Pointer to the blob detector</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobDetectorCC();
      #endregion

      #region BlobTracker
      /// <summary>
      /// Simple blob tracker based on connected component tracking
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobTrackerCC();

      /// <summary>
      /// Connected component tracking and mean-shift particle filter collion-resolver
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobTrackerCCMSPF();

      /// <summary>
      /// Blob tracker that integrates meanshift and connected components
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobTrackerMSFG();

      /// <summary>
      /// Blob tracker that integrates meanshift and connected components:
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobTrackerMSFGS();

      /// <summary>
      /// Meanshift without connected-components
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobTrackerMS();

      /// <summary>
      /// Particle filtering via Bhattacharya coefficient, which
      /// is roughly the dot-product of two probability densities.
      /// </summary>
      /// <remarks>See: Real-Time Tracking of Non-Rigid Objects using Mean Shift Comanicius, Ramesh, Meer, 2000, 8p</remarks>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobTrackerMSPF();

      /// <summary>
      /// Release the blob tracker
      /// </summary>
      /// <param name="tracker">The tracker to be released</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobTrackerRealease(IntPtr tracker);

      /// <summary>
      /// Return number of currently tracked blobs
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <returns>Number of currently tracked blobs</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int CvBlobTrackerGetBlobNum(IntPtr tracker);

      /// <summary>
      /// Return pointer to specified by index blob
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobIndex">The index of the blob</param>
      /// <returns>Pointer to the blob with the specific index</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobTrackerGetBlob(IntPtr tracker, int blobIndex);

      /// <summary>
      /// Return pointer to specified by index blob
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobId">The id of the blob</param>
      /// <returns>Pointer to the blob with specific id</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobTrackerGetBlobByID(IntPtr tracker, int blobId);

      /// <summary>
      /// Delete blob by its index
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobIndex">The index of the blob</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobTrackerDelBlob(IntPtr tracker, int blobIndex);

      /// <summary>
      /// Add new blob to track it and assign to this blob personal ID
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blob">pointer to structure with blob parameters (ID is ignored)</param>
      /// <param name="currentImage">current image</param>
      /// <param name="currentForegroundMask">current foreground mask</param>
      /// <returns>Pointer to new added blob</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobTrackerAddBlob(IntPtr tracker, ref MCvBlob blob, IntPtr currentImage, IntPtr currentForegroundMask);

      #endregion

      #region BlobTrackPostProc
      /// <summary>
      /// Returns a Kalman blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcKalman();

      /// <summary>
      /// Returns a TimeAverRect blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverRect();

      /// <summary>
      /// Returns a TimeAverExp blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverExp();

      /// <summary>
      /// Release the blob tracking post process module
      /// </summary>
      /// <param name="postProc">The post process module to be released</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobTrackPostProcRelease(IntPtr postProc);
      #endregion

      #region BlobTrackerAuto
      /// <summary>
      /// Create blob tracker auto ver1
      /// </summary>
      /// <param name="param">The parameters for the tracker</param>
      /// <returns>Pointer to the blob tracker auto</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateBlobTrackerAuto1(ref MCvBlobTrackerAutoParam1 param);

      /// <summary>
      /// Release the blob tracker auto
      /// </summary>
      /// <param name="tracker">The tracker to be released</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobTrackerAutoRelease(IntPtr tracker);

      /// <summary>
      /// Get the blob of specific index from the auto blob tracker
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="index">The index of the blob</param>
      /// <returns>Pointer to the the blob</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobTrackerAutoGetBlob(IntPtr tracker, int index);
      /// <summary>
      /// Get the blob of specific id from the auto blob tracker
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>Pointer to the blob of specific ID</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobTrackerAutoGetBlobByID(IntPtr tracker, int blobID);
      /// <summary>
      /// Get the number of blobs in the auto blob tracker 
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <returns>The number of blobs</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int CvBlobTrackerAutoGetBlobNum(IntPtr tracker);

      /// <summary>
      /// Process a image frame
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="pImg">The frame to process</param>
      /// <param name="pMask">The foreground mask, can be IntPtr.Zero if not needed</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobTrackerAutoProcess(IntPtr tracker, IntPtr pImg, IntPtr pMask);

      /// <summary>
      /// Get the foreground mask
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <returns>Pointer to the foreground mask</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobTrackerAutoGetFGMask(IntPtr tracker);
      #endregion

      /// <summary>
      /// The grab cut algorithm for segmentation
      /// </summary>
      /// <param name="img">The image to be segmented</param>
      /// <param name="mask">The mask to initialize segmentation</param>
      /// <param name="rect">The rectangle to initialize the segmentation</param>
      /// <param name="bgdModel">The background model</param>
      /// <param name="fgdModel">The foreground model</param>
      /// <param name="iterCount">The number of iternations</param>
      /// <param name="type">The initilization type</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvGrabCut(
         IntPtr img,
         IntPtr mask,
         ref Rectangle rect,
         IntPtr bgdModel,
         IntPtr fgdModel,
         int iterCount,
         CvEnum.GRABCUT_INIT_TYPE type);


      /// <summary>
      /// Check that every array element is neither NaN nor +- inf. The functions also check that each value
      /// is between minVal and maxVal. in the case of multi-channel arrays each channel is processed
      /// independently. If some values are out of range, position of the first outlier is stored in pos, 
      /// and then the functions either return false (when quiet=true) or throw an exception.
      /// </summary>
      /// <param name="arr">The array to check</param>
      /// <param name="quiet">The flag indicating whether the functions quietly return false when the array elements are
      /// out of range, or they throw an exception</param>
      /// <param name="pos">This will be filled with the position of the first outlier</param>
      /// <param name="minVal">The inclusive lower boundary of valid values range</param>
      /// <param name="maxVal">The exclusive upper boundary of valid values range</param>
      /// <returns>If quiet, return true if all values are in range</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvCheckRange(
         IntPtr arr,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool quiet,
         ref Point pos,
         double minVal,
         double maxVal);
   }
}
