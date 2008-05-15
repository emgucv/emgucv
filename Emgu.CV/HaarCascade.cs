using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Emgu.CV
{
    ///<summary> The HaarCascade class for object detection</summary>
    public class HaarCascade : UnmanagedObject
    {
        ///<summary> Create a HaarCascade object from the specific file</summary>
        ///<param name="fileName"> The name of the file that contains the HaarCascade object</param>
        public HaarCascade(String fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File Not Found", fileName);
            
            _ptr = CvInvoke.cvLoad(fileName, IntPtr.Zero, null, IntPtr.Zero);

            if (_ptr == IntPtr.Zero)
            {
                Emgu.PrioritizedException e = new Emgu.PrioritizedException(Emgu.ExceptionLevel.Critical,
                    String.Format("Fail to create HaarCascade object: {0}", fileName));
                e.Alert(true);
                throw e;
            }
        }

        /// <summary>
        /// Release the HaarCascade Object and all the memory associate with it
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            CvInvoke.cvReleaseHaarClassifierCascade(ref _ptr);
        }
    };
}
