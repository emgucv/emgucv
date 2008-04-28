using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> The HaarCascade class for object detection</summary>
    public class HaarCascade : UnmanagedObject
    {
        ///<summary> Create a HaarCascade object from the specific file</summary>
        ///<param name="fileName"> The name of the file that contains the HaarCascade object</param>
        public HaarCascade(String fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                Emgu.Exception e = new Emgu.Exception(Emgu.ExceptionHeader.CriticalException,
                    String.Format("HaarCascade file {0} do not exist", fileName));
                e.Alert(true);
                throw e;
            }

            m_ptr = CvInvoke.cvLoad(fileName, IntPtr.Zero, null, IntPtr.Zero);

            if (m_ptr == IntPtr.Zero)
            {
                Emgu.Exception e = new Emgu.Exception(Emgu.ExceptionHeader.CriticalException,
                    String.Format("Fail to create HaarCascade object: {0}", fileName));
                e.Alert(true);
                throw e;
            }
        }

        ///<summary> A pointer to the internal CvHaarClassifierCascade structure </summary>
        public IntPtr Ptr { get { return m_ptr; } }

        /// <summary>
        /// Release the HaarCascade Object and all the memory associate with it
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            CvInvoke.cvReleaseHaarClassifierCascade(ref m_ptr);
        }
    };
}
