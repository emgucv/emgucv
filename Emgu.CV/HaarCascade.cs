using System;
using System.IO;
using Emgu.Util;

namespace Emgu.CV
{
   ///<summary> 
   /// HaarCascade for object detection
   /// </summary>
   public class HaarCascade : UnmanagedObject
   {
      ///<summary> Create a HaarCascade object from the specific file</summary>
      ///<param name="fileName"> The name of the file that contains the HaarCascade object</param>
      public HaarCascade(String fileName)
      {
         FileInfo file = new FileInfo(fileName);
         if (!file.Exists)
            throw new FileNotFoundException(Properties.StringTable.FileNotFound, file.FullName);

         _ptr = CvInvoke.cvLoad(file.FullName, IntPtr.Zero, null, IntPtr.Zero);

         if (_ptr == IntPtr.Zero)
         {
            throw new NullReferenceException(String.Format(Properties.StringTable.FailToCreateHaarCascade, file.FullName));
         }
      }

      /// <summary>
      /// Release the HaarCascade Object and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseHaarClassifierCascade(ref _ptr);
      }
   }
}
