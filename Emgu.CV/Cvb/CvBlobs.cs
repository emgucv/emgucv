//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Cvb
{
   /// <summary>
   /// CvBlobs
   /// </summary>
   public class CvBlobs : UnmanagedObject, IDictionary<uint, CvBlob>
   {
      static CvBlobs()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a new CvBlobs
      /// </summary>
      public CvBlobs()
      {
         _ptr = cvbCvBlobsCreate();
      }

      /// <summary>
      /// Release all the unmanaged resources used by this CvBlobs
      /// </summary>
      protected override void DisposeObject()
      {
         cvbCvBlobsRelease(ref _ptr);
      }

      /// <summary>
      /// Filter blobs by area. Those blobs whose areas are not in range will be erased from the input list of blobs.
      /// </summary>
      /// <param name="minArea">Minimun area</param>
      /// <param name="maxArea">Maximun area</param>
      public void FilterByArea(int minArea, int maxArea)
      {
         cvbCvFilterByArea(_ptr, (uint)minArea, (uint)maxArea);
      }

      #region IDictionary<uint,CvBlob> Members
      /// <summary>
      /// Adds the specified label and blob to the dictionary.
      /// </summary>
      /// <param name="label">The label of the blob</param>
      /// <param name="blob">The blob</param>
      public void Add(uint label, CvBlob blob)
      {
         bool success = cvbCvBlobsAdd(_ptr, label, blob);
         if (!success) throw new ArgumentException(String.Format("The item with label {0} already exist in the Blobs.", label));
      }

      /// <summary>
      /// Determines whether the CvBlobs contains the specified label.
      /// </summary>
      /// <param name="label">The label (key) to be located</param>
      /// <returns>True if the CvBlobs contains an element with the specific label</returns>
      public bool ContainsKey(uint label)
      {
         return IntPtr.Zero != cvbCvBlobsFind(_ptr, label);
      }

      /// <summary>
      /// Get a collection containing the labels in the CvBlobs
      /// </summary>
      public ICollection<uint> Keys
      {
         get
         {
            uint[] labels;
            CvBlob[] blobs;
            GetBlobs(out labels, out blobs);
            return labels;
         }
      }

      /// <summary>
      /// Removes the blob with the specific label
      /// </summary>
      /// <param name="label">The label of the blob</param>
      /// <returns>True if the element is successfully found and removed; otherwise, false.</returns>
      public bool Remove(uint label)
      {
         return cvbCvBlobsRemove(_ptr, label);
      }

      /// <summary>
      /// Gets the blob associated with the specified label.
      /// </summary>
      /// <param name="label">The blob label</param>
      /// <param name="blob">When this method returns, contains the blob associated with the specified labe, if the label is found; otherwise, null. This parameter is passed uninitialized.</param>
      /// <returns>True if the blobs contains a blob with the specific label; otherwise, false</returns>
      public bool TryGetValue(uint label, out CvBlob blob)
      {
         IntPtr blobPtr = cvbCvBlobsFind(_ptr, label);
         if (IntPtr.Zero == blobPtr)
         {
            blob = null;
            return false;
         }
         else
         {
            blob = new CvBlob(blobPtr);
            return true;
         }
      }

      private void GetBlobs(out uint[] labels, out CvBlob[] blobs)
      {
         int count = this.Count;
         blobs = new CvBlob[count];
         labels = new uint[count];

         IntPtr[] ptrs = new IntPtr[count];
         GCHandle labelsHandle = GCHandle.Alloc(labels, GCHandleType.Pinned);
         GCHandle ptrsHandle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
         cvbCvBlobsGetBlobs(_ptr, labelsHandle.AddrOfPinnedObject(), ptrsHandle.AddrOfPinnedObject());
         labelsHandle.Free();
         ptrsHandle.Free();
         for (int i = 0; i < blobs.Length; i++)
         {
            blobs[i] = new CvBlob(ptrs[i]);
         }
      }

      /// <summary>
      /// Get a collection containing the blobs in the CvBlobs.
      /// </summary>
      public ICollection<CvBlob> Values
      {
         get
         {
            uint[] labels;
            CvBlob[] blobs;
            GetBlobs(out labels, out blobs);
            return blobs;
         }
      }

      /// <summary>
      /// Get the blob with the speicific label. Set function is not implemented
      /// </summary>
      /// <param name="label">The label for the blob</param>
      public CvBlob this[uint label]
      {
         get
         {
            CvBlob value;
            bool found = TryGetValue(label, out value);
            if (!found)
               throw new KeyNotFoundException(String.Format("The specific item with key {0} cannot be found", label));
            return value;
         }
         set
         {
            throw new NotImplementedException();
         }
      }

      #endregion

      #region ICollection<KeyValuePair<uint,CvBlob>> Members
      /// <summary>
      /// Adds the specified label and blob to the CvBlobs.
      /// </summary>
      ///<param name="item">The structure representing the label and blob to add to the CvBlobs</param>
      public void Add(KeyValuePair<uint, CvBlob> item)
      {
         Add(item.Key, item.Value);
      }

      /// <summary>
      /// Removes all keys and values
      /// </summary>
      public void Clear()
      {
         cvbCvBlobsClear(_ptr);
      }

      /// <summary>
      /// Determines whether the CvBlobs contains a specific label and CvBlob.
      /// </summary>
      /// <param name="item">The label and blob to be located</param>
      /// <returns>True if the specific label and blob is found in the CvBlobs; otherwise, false.</returns>
      public bool Contains(KeyValuePair<uint, CvBlob> item)
      {
         CvBlob blob;
         if (!TryGetValue(item.Key, out blob))
            return false;
         return blob.Ptr.Equals(item.Value.Ptr);
      }

      /// <summary>
      /// Copies the elements to the <paramref name="array"/>, starting at the specific arrayIndex.
      /// </summary>
      /// <param name="array">The one-dimensional array that is the defination of the elements copied from the CvBlobs. The array must have zero-base indexing.</param>
      /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
      public void CopyTo(KeyValuePair<uint, CvBlob>[] array, int arrayIndex)
      {
         foreach (KeyValuePair<uint, CvBlob> item in this)
            array[arrayIndex++] = item;
      }

      /// <summary>
      /// Gets the number of label/Blob pairs contained in the collection
      /// </summary>
      public int Count
      {
         get { return cvbCvBlobsGetSize(_ptr); }
      }

      /// <summary>
      /// Always false
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
      public bool Remove(KeyValuePair<uint, CvBlob> item)
      {
         bool found = Contains(item);
         if (found)
            Remove(item.Key);
         return found;
      }

      #endregion

      #region IEnumerable<KeyValuePair<uint,CvBlob>> Members
      /// <summary>
      /// Returns an enumerator that iterates through the collection.
      /// </summary>
      /// <returns>An enumerator that can be used to iterate through the collection</returns>
      public IEnumerator<KeyValuePair<uint, CvBlob>> GetEnumerator()
      {
         uint[] labels;
         CvBlob[] blobs;
         GetBlobs(out labels, out blobs);
         for (int i = 0; i < labels.Length; i++)
         {
            yield return new KeyValuePair<uint, CvBlob>(labels[i], blobs[i]);
         }
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      #endregion

      /// <summary>
      /// Returns a pointer to CvBlobs
      /// </summary>
      /// <returns>Pointer to CvBlobs</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvbCvBlobsCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvBlobsRelease(ref IntPtr blobs);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvbCvBlobsGetSize(IntPtr blobs);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvBlobsClear(IntPtr blobs);

      //return true if this is a new label. False if the label already exist and the value in the map will NOT be modified.
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvbCvBlobsAdd(IntPtr blobs, uint label, IntPtr blob);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvbCvBlobsFind(IntPtr blobs, uint label);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvBlobsGetBlobs(IntPtr blobs, IntPtr labelsArray, IntPtr blobsArray);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvFilterByArea(IntPtr blobs, uint minArea, uint maxArea);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvbCvBlobsRemove(IntPtr blobs, uint label);
   }
}

