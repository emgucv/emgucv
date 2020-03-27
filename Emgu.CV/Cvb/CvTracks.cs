//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Cvb
{
   /// <summary>
   /// Blobs tracking
   /// </summary>
   /// <remarks>
   /// Tracking based on:
   /// A. Senior, A. Hampapur, Y-L Tian, L. Brown, S. Pankanti, R. Bolle. Appearance Models for
   /// Occlusion Handling. Second International workshop on Performance Evaluation of Tracking and
   /// Surveillance Systems &amp; CVPR'01. December, 2001.
   /// (http://www.research.ibm.com/peoplevision/PETS2001.pdf)
   ///</remarks>
   public class CvTracks : UnmanagedObject, IDictionary<uint, CvTrack>
   {
      static CvTracks()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a new CvTracks
      /// </summary>
      public CvTracks()
      {
         _ptr = cvbCvTracksCreate();
      }

      /// <summary>
      /// Release all the unmanaged resources used by this CvBlobs
      /// </summary>
      protected override void DisposeObject()
      {
         cvbCvTracksRelease(ref _ptr);
      }

      /// <summary>
      /// Updates list of tracks based on current blobs.
      /// </summary>
      /// <param name="blobs">List of blobs</param>
      /// <param name="thDistance">Distance Max distance to determine when a track and a blob match</param>
      /// <param name="thInactive">Inactive Max number of frames a track can be inactive</param>
      /// <param name="thActive">Active If a track becomes inactive but it has been active less than thActive frames, the track will be deleted.</param>
      public void Update(CvBlobs blobs, double thDistance, uint thInactive, uint thActive)
      {
         cvbCvUpdateTracks(blobs, _ptr, thDistance, thInactive, thActive);
      }

      #region IDictionary<uint,CvBlob> Members
      /// <summary>
      /// Adds the specified id and track to the dictionary.
      /// </summary>
      /// <param name="id">The id of the track</param>
      /// <param name="track">The track</param>
      public void Add(uint id, CvTrack track)
      {
         bool success = cvbCvTracksAdd(_ptr, id, ref track);
         if (!success) throw new ArgumentException(String.Format("The item with id {0} already exist in the Tracks.", id));
      }

      /// <summary>
      /// Determines whether the CvTracks contains the specified id.
      /// </summary>
      /// <param name="id">The id (key) to be located</param>
      /// <returns>True if the CvTracks contains an element with the specific id</returns>
      public bool ContainsKey(uint id)
      {
         return IntPtr.Zero != cvbCvTracksFind(_ptr, id);
      }

      /// <summary>
      /// Get a collection containing the ids in the CvTracks.
      /// </summary>
      public ICollection<uint> Keys
      {
         get
         {
            uint[] ids;
            CvTrack[] tracks;
            GetTracks(out ids, out tracks);
            return ids;
         }
      }

      /// <summary>
      /// Removes the track with the specific id
      /// </summary>
      /// <param name="id">The id of the track</param>
      /// <returns>True if the element is successfully found and removed; otherwise, false.</returns>
      public bool Remove(uint id)
      {
         return cvbCvTracksRemove(_ptr, id);
      }

      /// <summary>
      /// Gets the track associated with the specified id.
      /// </summary>
      /// <param name="id">The track id</param>
      /// <param name="track">When this method returns, contains the track associated with the specified id, if the id is found; otherwise, an empty track. This parameter is passed uninitialized.</param>
      /// <returns>True if the tracks contains a track with the specific id; otherwise, false</returns>
      public bool TryGetValue(uint id, out CvTrack track)
      {
         IntPtr trackPtr = cvbCvTracksFind(_ptr, id);
         if (IntPtr.Zero == trackPtr)
         {
            track = new CvTrack();
            return false;
         }
         else
         {
            track = (CvTrack)Marshal.PtrToStructure(trackPtr, typeof(CvTrack));
            return true;
         }
      }

      private void GetTracks(out uint[] ids, out CvTrack[] tracks)
      {
         int count = this.Count;
         tracks = new CvTrack[count];
         ids = new uint[count];

         GCHandle idsHandle = GCHandle.Alloc(ids, GCHandleType.Pinned);
         GCHandle tracksHandle = GCHandle.Alloc(tracks, GCHandleType.Pinned);
         cvbCvTracksGetTracks(_ptr, idsHandle.AddrOfPinnedObject(), tracksHandle.AddrOfPinnedObject());
         idsHandle.Free();
         tracksHandle.Free();
      }

      /// <summary>
      /// Get a collection containing the tracks in the CvTracks.
      /// </summary>
      public ICollection<CvTrack> Values
      {
         get
         {
            uint[] ids;
            CvTrack[] tracks;
            GetTracks(out ids, out tracks);
            return tracks;
         }
      }

      /// <summary>
      /// Get or Set the Track with the specific id.
      /// </summary>
      /// <param name="id">The id of the Track</param>
      public CvTrack this[uint id]
      {
         get
         {
            CvTrack value;
            bool found = TryGetValue(id, out value);
            if (!found)
               throw new KeyNotFoundException(String.Format("The specific item with key {0} cannot be found", id));
            return value;
         }
         set
         {
            cvbCvTracksSetTrack(Ptr, id, ref value);
         }
      }

      #endregion

      #region ICollection<KeyValuePair<uint,CvBlob>> Members
      /// <summary>
      /// Adds the specified id and track to the CvTracks.
      /// </summary>
      ///<param name="item">The structure representing the id and track to add to the CvTracks</param>
      public void Add(KeyValuePair<uint, CvTrack> item)
      {
         Add(item.Key, item.Value);
      }

      /// <summary>
      /// Removes all keys and values
      /// </summary>
      public void Clear()
      {
         cvbCvTracksClear(_ptr);
      }

      /// <summary>
      /// Determines whether the CvTracks contains a specific id and CvTrack.
      /// </summary>
      /// <param name="item">The id and CvTrack to be located</param>
      /// <returns>True if the <paramref name="item"/> is found in the CvTracks; otherwise, false.</returns>
      public bool Contains(KeyValuePair<uint, CvTrack> item)
      {
         CvTrack track;
         if (!TryGetValue(item.Key, out track))
            return false;
         return track.Equals(item.Value);
      }

      /// <summary>
      /// Copies the elements to the <paramref name="array"/>, starting at the specific arrayIndex.
      /// </summary>
      /// <param name="array">The one-dimensional array that is the defination of the elements copied from the CvTracks. The array must have zero-base indexing.</param>
      /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
      public void CopyTo(KeyValuePair<uint, CvTrack>[] array, int arrayIndex)
      {
         foreach (KeyValuePair<uint, CvTrack> item in this)
            array[arrayIndex++] = item;
      }

      /// <summary>
      /// Gets the number of id/track pairs contained in the collection.
      /// </summary>
      public int Count
      {
         get { return cvbCvTracksGetSize(_ptr); }
      }

      /// <summary>
      /// Always false.
      /// </summary>
      public bool IsReadOnly
      {
         get { return false; }
      }


      /// <summary>
      /// Removes a key and value from the dictionary.
      /// </summary>
      /// <param name="item">The structure representing the key and value to be removed</param>
      /// <returns>True if the key are value is sucessfully found and removed; otherwise false.</returns>
      public bool Remove(KeyValuePair<uint, CvTrack> item)
      {
         bool found = Contains(item);
         if (found)
            Remove(item.Key);
         return found;
      }

      #endregion

      #region IEnumerable<KeyValuePair<uint,CvTrack>> Members

      /// <summary>
      /// Returns an enumerator that iterates through the collection.
      /// </summary>
      /// <returns>An enumerator that can be used to iterate through the collection</returns>
      public IEnumerator<KeyValuePair<uint, CvTrack>> GetEnumerator()
      {
         uint[] ids;
         CvTrack[] tracks;
         GetTracks(out ids, out tracks);
         for (int i = 0; i < ids.Length; i++)
         {
            yield return new KeyValuePair<uint, CvTrack>(ids[i], tracks[i]);
         }
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      #endregion

      #region PInvoke
      /// <summary>
      /// Returns a pointer to CvBlobs
      /// </summary>
      /// <returns>Pointer to CvBlobs</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvbCvTracksCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvTracksRelease(ref IntPtr tracks);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvbCvTracksGetSize(IntPtr tracks);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvTracksClear(IntPtr tracks);

      //return true if this is a new label. False if the label already exist and the value in the map will NOT be modified.
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvbCvTracksAdd(IntPtr tracks, uint id, ref Cvb.CvTrack track);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvbCvTracksFind(IntPtr tracks, uint id);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvTracksGetTracks(IntPtr Tracks, IntPtr idsArray, IntPtr tracksArray);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvTracksSetTrack(IntPtr tracks, uint id, ref Cvb.CvTrack track);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvUpdateTracks(IntPtr blobs, IntPtr tracks, double thDistance, uint thInactive, uint thActive);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvbCvTracksRemove(IntPtr tracks, uint id);
      #endregion
   }
}

*/