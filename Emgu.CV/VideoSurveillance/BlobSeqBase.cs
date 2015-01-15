/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Emgu.CV.Structure;
using Emgu.Util;
namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// An abstract class that server as a base class for Blob sequence
   /// </summary>
   public abstract class BlobSeqBase : UnmanagedObject, IEnumerable<MCvBlob>
   {
      /// <summary>
      /// The number of blobs in this sequence
      /// </summary>
      public abstract int Count
      {
         get;
      }

      /// <summary>
      /// Return the blob given the specific index
      /// </summary>
      /// <param name="index">The index of the blob</param>
      /// <returns>The blob in the specific index</returns>
      public abstract MCvBlob this[int index]
      {
         get;
      }

      /// <summary>
      /// Get the blob with the specific id
      /// </summary>
      /// <param name="blobId">The id of the blob</param>
      /// <returns>The blob of the specific id, if do not exist, MCvBlob.Empty is returned</returns>
      public abstract MCvBlob GetBlobByID(int blobId);

      #region IEnumerable<MCvBlob> Members
      /// <summary>
      /// Get an enumerator of all the blobs in this blob sequence.
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
*/