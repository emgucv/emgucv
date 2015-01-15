//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Cvb
{
   /// <summary>
   /// CvTrack
   /// </summary>
   public struct CvTrack : IEquatable<CvTrack>
   {
      static CvTrack()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Track identification number
      /// </summary>
      public uint Id;

      /// <summary>
      /// Label assigned to the blob related to this track
      /// </summary>
      public uint BlobLabel;

      /// <summary>
      /// X min
      /// </summary>
      public uint MinX;
      /// <summary>
      /// X max
      /// </summary>
      public uint MaxX;
      /// <summary>
      /// Y min
      /// </summary>
      public uint MinY;
      /// <summary>
      /// y max
      /// </summary>
      public uint MaxY;

      /// <summary>
      /// Get the minimun bounding rectanble for this track 
      /// </summary>
      public Rectangle BoundingBox
      {
         get
         {
            return new Rectangle((int)MinX, (int)MinY, (int)(MaxX - MinX + 1), (int)(MaxY - MinY + 1));
         }
      }

      /// <summary>
      ///  Centroid
      /// </summary>
      public MCvPoint2D64f Centroid;

      /// <summary>
      /// Indicates how much frames the object has been in scene
      /// </summary>
      public uint Lifetime;

      /// <summary>
      /// Indicates number of frames that has been active from last inactive period.
      /// </summary>
      public uint Active;

      /// <summary>
      /// Indicates number of frames that has been missing.
      /// </summary>
      public uint Inactive;

      /// <summary>
      /// Compares CvTrack for equality
      /// </summary>
      /// <param name="other">The other track to compares with</param>
      /// <returns>True if the two CvTrack are equal; otherwise false.</returns>
      public bool Equals(CvTrack other)
      {
         return cvbCvTrackEquals(ref this, ref other);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvbCvTrackEquals(ref Cvb.CvTrack track1, ref Cvb.CvTrack track2);
   }
}
