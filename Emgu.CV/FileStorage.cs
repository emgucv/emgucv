//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// XML/YAML file storage class that encapsulates all the information necessary for writing or reading data to/from a file.
   /// </summary>
   public class FileStorage : UnmanagedObject
   {
      /// <summary>
      /// File storage mode
      /// </summary>
      [Flags]
      public enum Mode
      {
         /// <summary>
         /// Open the file for reading
         /// </summary>
         Read = 0,
         /// <summary>
         /// Open the file for writing
         /// </summary>
         Write = 1,
         /// <summary>
         /// Open the file for appending
         /// </summary>
         Append = 2,
         /// <summary>
         /// ReadMat data from source or write data to the internal buffer
         /// </summary>
         Memory = 4,

         /// <summary>
         /// Mask for format flags
         /// </summary>
         FormatMask = (7 << 3),
         /// <summary>
         /// Auto format
         /// </summary>
         FormatAuto = 0,
         /// <summary>
         /// XML format
         /// </summary>
         FormatXml = (1 << 3),
         /// <summary>
         /// YAML format
         /// </summary>
         FormatYaml = (2 << 3)
      };

      /// <summary>
      /// Initializes a new instance of the <see cref="FileStorage"/> class.
      /// </summary>
      /// <param name="source">Name of the file to open or the text string to read the data from. Extension of the
      /// file (.xml or .yml/.yaml) determines its format (XML or YAML respectively). Also you can append .gz
      /// to work with compressed files, for example myHugeMatrix.xml.gz. If both FileStorage::WRITE and
      /// FileStorage::MEMORY flags are specified, source is used just to specify the output file format (e.g.
      /// mydata.xml, .yml etc.).</param>
      /// <param name="flags">Mode of operation.</param>
      /// <param name="encoding">Encoding of the file. Note that UTF-16 XML encoding is not supported currently and
      /// you should use 8-bit encoding instead of it.</param>
      public FileStorage(String source, Mode flags, String encoding = null)
      {
         using (CvString enc = new CvString(encoding))
         using (CvString src = new CvString(source))
         {
            _ptr = CvInvoke.cveFileStorageCreate(src, flags, enc);
         }
      }

      /// <summary>
      /// Writes the specified Mat to the node with the specific <param Name="nodeName"></param>
      /// </summary>
      /// <param name="m">The Mat to be written to the file storage</param>
      /// <param name="nodeName">The name of the node.</param>
      public void Write(Mat m, String nodeName = null)
      {
         using (CvString cs = new CvString(nodeName))
            CvInvoke.cveFileStorageWriteMat(_ptr, cs, m);
      }

      /// <summary>
      /// Writes the specified Mat to the node with the specific <param Name="nodeName"></param>
      /// </summary>
      /// <param name="value">The value to be written to the file storage</param>
      /// <param name="nodeName">The name of the node.</param>
      public void Write(int value, String nodeName = null)
      {
         using (CvString cs = new CvString(nodeName))
            CvInvoke.cveFileStorageWriteInt(_ptr, cs, value);
      }

      /// <summary>
      /// Writes the specified Mat to the node with the specific <param Name="nodeName"></param>
      /// </summary>
      /// <param name="value">The value to be written to the file storage</param>
      /// <param name="nodeName">The name of the node.</param>
      public void Write(float value, String nodeName = null)
      {
         using (CvString cs = new CvString(nodeName))
            CvInvoke.cveFileStorageWriteFloat(_ptr, cs, value);
      }

      /// <summary>
      /// Writes the specified Mat to the node with the specific <param Name="nodeName"></param>
      /// </summary>
      /// <param name="value">The value to be written to the file storage</param>
      /// <param name="nodeName">The name of the node.</param>
      public void Write(double value, String nodeName = null)
      {
         using (CvString cs = new CvString(nodeName))
            CvInvoke.cveFileStorageWriteDouble(_ptr, cs, value);
      }

      /// <summary>
      /// Writes the specified Mat to the node with the specific <param Name="nodeName"></param>
      /// </summary>
      /// <param name="value">The value to be written to the file storage</param>
      /// <param name="nodeName">The name of the node.</param>
      public void Write(String value, String nodeName = null)
      {
         using (CvString cs = new CvString(nodeName))
         using (CvString vs = new CvString(value))
            CvInvoke.cveFileStorageWriteString(_ptr, cs, vs);
      }

      /// <summary>
      /// Gets a value indicating whether this instance is opened.
      /// </summary>
      /// <value>
      ///   <c>true</c> if the object is associated with the current file; otherwise, <c>false</c>.
      /// </value>
      public bool IsOpened
      {
         get { return CvInvoke.cveFileStorageIsOpened(_ptr); }
      }

      /// <summary>
      /// Closes the file and releases all the memory buffers
      /// Call this method after all I/O operations with the storage are finished. If the storage was
      /// opened for writing data and FileStorage.Mode.Write was specified
      /// </summary>
      /// <returns>The string that represent the text in the FileStorage</returns>
      public String ReleaseAndGetString()
      {
         using (CvString s = new CvString())
         {
            CvInvoke.cveFileStorageReleaseAndGetString(_ptr, s);
            return s.ToString();
         }
      }

      /// <summary>
      /// Gets the top-level mapping.
      /// </summary>
      /// <param name="streamIdx">Zero-based index of the stream. In most cases there is only one stream in the file.
      /// However, YAML supports multiple streams and so there can be several.</param>
      /// <returns> The top-level mapping</returns>
      public FileNode GetRoot(int streamIdx = 0)
      {
         return new FileNode(CvInvoke.cveFileStorageRoot(_ptr, streamIdx));
      }

      /// <summary>
      /// Gets the first element of the top-level mapping.
      /// </summary>
      /// <returns>The first element of the top-level mapping.</returns>
      public FileNode GetFirstTopLevelNode()
      {
         return new FileNode(CvInvoke.cveFileStorageGetFirstTopLevelNode(_ptr));
      }

      /// <summary>
      /// Gets the specified element of the top-level mapping.
      /// </summary>
      /// <param name="nodeName">Name of the node.</param>
      /// <returns>The specified element of the top-level mapping.</returns>
      public FileNode GetNode(String nodeName)
      {
         using (CvString nn = new CvString(nodeName))
         {
            return new FileNode(CvInvoke.cveFileStorageGetNode(_ptr, nn));
         }
      }

      /// <summary>
      /// Gets the <see cref="FileNode"/> with the specified node name.
      /// </summary>
      /// <value>
      /// The <see cref="FileNode"/>.
      /// </value>
      /// <param name="nodeName">Name of the node.</param>
      /// <returns></returns>
      public FileNode this[String nodeName]
      {
         get { return GetNode(nodeName); }
      }

      /// <summary>
      /// Release the unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveFileStorageRelease(ref _ptr);
      }
   }

   public static partial class CvInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern IntPtr cveFileStorageCreate(IntPtr source, FileStorage.Mode flags, IntPtr encoding);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      static internal extern bool cveFileStorageIsOpened(IntPtr storage);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern void cveFileStorageReleaseAndGetString(IntPtr storage, IntPtr result);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern void cveFileStorageRelease(ref IntPtr storage);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern void cveFileStorageWriteMat(IntPtr fs, IntPtr name, IntPtr value);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern void cveFileStorageWriteInt(IntPtr fs, IntPtr name, int value);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern void cveFileStorageWriteFloat(IntPtr fs, IntPtr name, float value);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern void cveFileStorageWriteDouble(IntPtr fs, IntPtr name, double value);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern void cveFileStorageWriteString(IntPtr fs, IntPtr name, IntPtr value);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern IntPtr cveFileStorageRoot(IntPtr fs, int streamIdx);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern IntPtr cveFileStorageGetFirstTopLevelNode(IntPtr fs);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      static internal extern IntPtr cveFileStorageGetNode(IntPtr fs, IntPtr nodeName);
   }

}
