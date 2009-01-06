using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Wrapper to the OpenCV MemStorage
   /// </summary>
   public class StereoBMState : UnmanagedObject
   {
      /// <summary>
      /// Create a stereoBMState
      /// </summary>
      /// <param name="type">ID of one of the pre-defined parameter sets. Any of the parameters can be overridden after creating the structure.</param>
      /// <param name="numberOfDisparities">The number of disparities. If the parameter is 0, it is taken from the preset, otherwise the supplied value overrides the one from preset. </param>
      public StereoBMState(CvEnum.STEREO_BM_TYPE type, int numberOfDisparities)
      {
         _ptr = CvInvoke.cvCreateStereoBMState(type, numberOfDisparities);
      }

      #region pre filters (normalize input images):
      /// <summary>
      /// 0 for now
      /// </summary>
      public int PreFilterType
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "preFilterType").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "preFilterType").ToInt32());
         }
      }

      /// <summary>
      /// ~5x5..21x21
      /// </summary>
      public int PreFilterSize
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "preFilterSize").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "preFilterSize").ToInt32());
         }
      }
      /// <summary>
      /// up to ~31
      /// </summary>
      public int PreFilterCap
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "preFilterCap").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "preFilterCap").ToInt32());
         }
      }
      #endregion

      /// <summary>
      /// Could be 5x5..21x21. Correspondence using Sum of Absolute Difference (SAD):
      /// </summary>
      public int SADWindowSize
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "SADWindowSize").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "SADWindowSize").ToInt32());
         }
      }
      /// <summary>
      /// minimum disparity (=0)
      /// </summary>
      public int MinDisparity
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "minDisparity").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "minDisparity").ToInt32());
         }
      }

      /// <summary>
      /// maximum disparity - minimum disparity
      /// </summary>
      public int NumberOfDisparities
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "numberOfDisparities").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "numberOfDisparities").ToInt32());
         }
      }

      #region post filters (knock out bad matches)
      /// <summary>
      /// areas with no texture are ignored
      /// </summary>
      public int TextureThreshold
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "textureThreshold").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "textureThreshold").ToInt32());
         }
      }

      /// <summary>
      /// Filter out pixels if there are other close matches
      /// </summary>
      public float UniquenessRatio
      {
         get
         {
            IntPtr ptr = new IntPtr(_ptr.ToInt64() + Marshal.OffsetOf(typeof(MCvStereoBMState), "uniquenessRatio").ToInt64());
            return (float)Marshal.PtrToStructure(ptr, typeof(float));
         }
         set
         {
            IntPtr ptr = new IntPtr(_ptr.ToInt64() + Marshal.OffsetOf(typeof(MCvStereoBMState), "uniquenessRatio").ToInt64());
            Marshal.StructureToPtr(value, ptr, false);
         }
      }
      #endregion

      /*
      // with different disparity
      /// <summary>
      /// Disparity variation window (not used)
      /// </summary>
      public int SpeckleWindowSize
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "speckleWindowSize").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "speckleWindowSize").ToInt32());
         }
      }
      /// <summary>
      /// Acceptable range of variation in window (not used)
      /// </summary>
      public int SpeckleRange
      {
         get
         {
            return Marshal.ReadInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "speckleRange").ToInt32());
         }
         set
         {
            Marshal.WriteInt32(_ptr, Marshal.OffsetOf(typeof(MCvStereoBMState), "speckleRange").ToInt32());
         }
      }*/

      /// <summary>
      /// Release the stereo state and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseStereoBMState(ref _ptr);
      }
   }
}
